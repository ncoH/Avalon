using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MediaPortal.Configuration;
using Log = MediaPortal.GUI.Library.Log;
using System.Reflection;
using Microsoft.Win32;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Util;
using MediaPortal.Profile;
using System.Diagnostics;
using WindowPlugins.GUITVSeries;
using MediaPortal.GUI.Music;
using MediaPortal.GUI.View;
using OnlineVideos;
using Cornerstone.Database.Tables;
using Cornerstone.GUI.Filtering;
using MediaPortal.Plugins.MovingPictures;
using MediaPortal.Plugins.MovingPictures.Database;
using MyFilmsPlugin.MyFilms;

namespace ProcessPlugins.AvalonEditor
{
    public partial class AvalonConfig : Form
    {
        string configDir;
        string path;
        string WP_Path;
        string myskin = "";
        List<string> ids = new List<string>();
        List<menuItem> AvailmenuItems = new List<menuItem>();
        List<menuItem> menuItems = new List<menuItem>();
        List<menuItem> submenuItems = new List<menuItem>();
        Dictionary<int, int> values = new Dictionary<int, int>();
        List<menuItem> HomemenuItems = new List<menuItem>();
        List<menuItem> PluginmenuItems = new List<menuItem>();
        private ArrayList Parents = new ArrayList();
        private ArrayList Children = new ArrayList();
        private ArrayList newChildren = new ArrayList();
        private ArrayList tempChildren = new ArrayList();
        private ArrayList singleChildren = new ArrayList();
        bool configloaded = false;
        bool tvhasbeenadded = false; // Fix for TV showing up two times in MP module-list. Bug?
        SkinInfo skInfo = new SkinInfo();
        AvalonHelper helper = new AvalonHelper();

        public static List<string> theTVSeriesViews = new List<string>();
        public static List<string> theMusicViews = new List<string>();
        public static List<string> theOnlineVideosViews = new List<string>();

        public static List<KeyValuePair<string, string>> tvseriesViews = new List<KeyValuePair<string, string>>();
        public static List<KeyValuePair<string, string>> musicViews = new List<KeyValuePair<string, string>>();
        public static List<KeyValuePair<string, string>> onlineVideosViews = new List<KeyValuePair<string, string>>();

        public AvalonConfig()
        {
            InitializeComponent();
        }

        #region Parmeter Handling

        /// <summary>
        /// Get list of views in TVseries database
        /// Key: should be used as hyperlinkParameter
        /// Val: can be used as a default display name
        /// </summary>    
        private List<KeyValuePair<string, string>> GetTVSeriesViews()
        {
            // check if we have already got them
            if (tvseriesViews.Count == 0)
                tvseriesViews = DBView.GetSkinViews();

            return tvseriesViews;
        }

        private List<KeyValuePair<string, string>> GetOnlineVideosViews()
        {
            // check if we have already got them
            if (onlineVideosViews.Count == 0)
            {
                // set path of config file, so we load user settings
                OnlineVideoSettings.Instance.ConfigDir = SkinInfo.mpPaths.configBasePath;

                // load list of sites
                OnlineVideoSettings onlineVideos = OnlineVideos.OnlineVideoSettings.Instance;
                onlineVideos.LoadSites();

                foreach (SiteSettings site in onlineVideos.SiteSettingsList)
                {
                    // just get a list of enabled sites
                    if (site.IsEnabled)
                    {
                        KeyValuePair<string, string> view = new KeyValuePair<string, string>(site.Name, site.Name);
                        onlineVideosViews.Add(view);
                    }
                }

            }

            return onlineVideosViews;
        }

        /// <summary>
        /// Get list of views in Music Views database
        /// Key: should be used as hyperlinkParameter
        /// Val: can be used as a default display name
        /// </summary>    
        private List<KeyValuePair<string, string>> GetMusicViews()
        {
            if (musicViews.Count == 0)
            {
                MusicViewHandler MusicViews = new MusicViewHandler();
                foreach (ViewDefinition MusicView in MusicViews.Views)
                {
                    string viewKey = MusicView.Name;
                    string viewValue = MusicView.LocalizedName;
                    KeyValuePair<string, string> skinview = new KeyValuePair<string, string>(viewKey, viewValue);
                    musicViews.Add(skinview);
                }
            }

            return musicViews;
        }

        //MyFilms
        private void cbEditorConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList MyFilmsEditor = BaseMesFilms.GetConfigViewLists();
            tbEditorStartParamsOutput.Text = "";
            cbEditorViews.Items.Clear();
            cbEditorViewValues.Items.Clear();
            cbEditorViews.Text = "";
            cbEditorViewValues.Text = "";

            foreach (BaseMesFilms.MFConfig config in MyFilmsEditor)
            {
                if (config.Name == cbEditorConfigs.Text)
                {
                    foreach (KeyValuePair<string, string> view in config.ViewList)
                    {
                        // key = viewname, value = translated viewname
                        cbEditorViews.Items.Add(view.Value);
                    }
                }
            }
        }

        private void cbEditorViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            string viewCallName = "";
            tbEditorStartParamsOutput.Text = "";
            cbEditorViewValues.Items.Clear();
            cbEditorViewValues.Text = "";
            ArrayList MyFilmsEditor = BaseMesFilms.GetConfigViewLists();
            foreach (BaseMesFilms.MFConfig config in MyFilmsEditor)
            {
                if (config.Name == cbEditorConfigs.Text)
                {
                    foreach (KeyValuePair<string, string> view in config.ViewList)
                    {
                        // key = viewname, value = translated viewname
                        if (view.Value == cbEditorViews.Text)
                        {
                            viewCallName = view.Key;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(cbEditorConfigs.Text) && !string.IsNullOrEmpty(viewCallName))
            {
                List<string> ViewValues = BaseMesFilms.GetViewListValues(cbEditorConfigs.Text, viewCallName);
                if (ViewValues != null)
                    foreach (string value in ViewValues)
                    {
                        cbEditorViewValues.Items.Add(value);
                    }
            }
        }

        private void cbEditorLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEditorLayout.SelectedIndex < 0 || cbEditorLayout.SelectedIndex > 5)
                cbEditorLayout.SelectedIndex = 0;
            if (cbEditorLayout.SelectedIndex == 0)
                cbEditorLayout.Text = "";
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            string startParamOutput = "";
            ArrayList myFilmsEditor = BaseMesFilms.GetConfigViewLists();

            if (!string.IsNullOrEmpty(cbEditorConfigs.Text))
                startParamOutput += "config:" + cbEditorConfigs.Text;
            if (!string.IsNullOrEmpty(cbEditorViews.Text))
            {
                string viewCallName = "";
                if (!string.IsNullOrEmpty(startParamOutput)) startParamOutput += "|";
                foreach (BaseMesFilms.MFConfig config in myFilmsEditor)
                {
                    if (config.Name == cbEditorConfigs.Text)
                    {
                        foreach (KeyValuePair<string, string> view in config.ViewList)
                        {
                            if (view.Value == cbEditorViews.Text)
                                viewCallName = view.Key;
                        }
                    }
                }
                startParamOutput += "view:" + viewCallName;
            }
            if (!string.IsNullOrEmpty(cbEditorViewValues.Text))
            {
                if (!string.IsNullOrEmpty(startParamOutput)) startParamOutput += "|";
                startParamOutput += "viewvalue:" + cbEditorViewValues.Text;
            }
            if (!string.IsNullOrEmpty(cbEditorLayout.Text))
            {
                if (!string.IsNullOrEmpty(startParamOutput)) startParamOutput += "|";
                startParamOutput += "layout:" + cbEditorLayout.SelectedIndex;
            }
            if (!string.IsNullOrEmpty(tbEditorSearchExpression.Text))
            {
                if (!string.IsNullOrEmpty(startParamOutput)) startParamOutput += "|";
                startParamOutput += "search:" + tbEditorSearchExpression.Text;
            }
            tbEditorStartParamsOutput.Text = startParamOutput;

            MessageBox.Show("MyFilms Start Parameter successfully created! \nYou can add the item to the menu now!");
        }

        #endregion

        public class Owner
        {

            public int ID;
            public string Name;
            public int Hyperlink;
            public string property;

            public Owner(int strID, string strName, int intHyperlink, string property)
            {
                this.ID = strID;
                this.Name = strName;
                this.Hyperlink = intHyperlink;
                this.property = property;
            }

            public override string ToString()
            {
                return this.ID + " : " + this.Name;
            }

        }


        public class Child
        {
            public int Owner;
            public int ID;
            public string Name;
            public int Hyperlink;
            public string property;

            public Child(int intOwner, int strID, string strName, int intHyperlink, string property)
            {
                this.Owner = intOwner;
                this.ID = strID;
                this.Name = strName;
                this.Hyperlink = intHyperlink;
                this.property = property;
            }

            public override string ToString()
            {
                return this.ID + " : " + this.Name;
            }

        }



        private void LoadWindowPlugins()
        {
            string directory = Config.GetSubFolder(Config.Dir.Plugins, "windows");
            if (!Directory.Exists(directory)) return;

            using (Settings xmlreader = new Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml")))
            {
                string[] files = Directory.GetFiles(directory, "*.dll");
                foreach (string pluginFile in files)
                {
                    try
                    {
                        Assembly pluginAssembly = Assembly.LoadFrom(pluginFile);

                        if (pluginAssembly != null)
                        {
                            Type[] exportedTypes = pluginAssembly.GetExportedTypes();
                            foreach (Type type in exportedTypes)
                            {
                                // an abstract class cannot be instanciated
                                if (type.IsAbstract) continue;
                                // Try to locate the interface we're interested in
                                if (type.GetInterface("MediaPortal.GUI.Library.ISetupForm") != null)
                                {
                                    try
                                    {
                                        // Create instance of the current type
                                        object pluginObject = Activator.CreateInstance(type);
                                        ISetupForm pluginForm = pluginObject as ISetupForm;

                                        if (pluginForm != null)
                                        {
                                            if (pluginForm.PluginName().Equals("Home")) continue;
                                            if (pluginForm.PluginName().Equals("my Plugins")) continue;
                                            if (pluginForm.PluginName().Equals("Topbar")) continue; //Works?

                                            string enabled = xmlreader.GetValue("plugins", pluginForm.PluginName());
                                            if (enabled.CompareTo("yes") != 0) continue;

                                            string showInHome = xmlreader.GetValue("home", pluginForm.PluginName());

                                            if (showInHome.CompareTo("no") == 0)
                                            {
                                                menuItem item = new menuItem();
                                                item.name = pluginForm.PluginName();
                                                item.hyperlink = pluginForm.GetWindowId();
                                                item.bgImage = "";
                                                item.media = "";
                                                PluginmenuItems.Add(item);
                                            }

                                            if (showInHome.CompareTo("yes") == 0)
                                            {
                                                menuItem item = new menuItem();
                                                item.name = pluginForm.PluginName();
                                                item.hyperlink = pluginForm.GetWindowId();
                                                item.bgImage = "";
                                                item.media = "";



                                                if (item.name == "TV" && tvhasbeenadded != true)  // Fix for TV showing up two times in MP module-list. Bug?
                                                {
                                                    tvhasbeenadded = true;
                                                    HomemenuItems.Add(item);

                                                }
                                                else if (item.name != "TV")
                                                {
                                                    HomemenuItems.Add(item);
                                                }


                                            }
                                        }
                                    }
                                    catch (Exception setupFormException)
                                    {
                                        Log.Info("Avalon plugin: Exception in plugin SetupForm loading :{0}", setupFormException.Message);
                                        Log.Info("Avalon plugin: Current class is :{0}", type.FullName);
                                        Log.Info(setupFormException.StackTrace);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception unknownException)
                    {
                        Log.Info("Avalon plugin: Exception in plugin loading :{0}", unknownException.Message);
                        Log.Info(unknownException.StackTrace);
                    }
                }
            }

            // Manually add the plugins window
            menuItem item2 = new menuItem();
            item2.name = "Plugins";
            item2.hyperlink = 34;
            HomemenuItems.Add(item2);

        }





        public static List<FileInfo> getFilesRecursive(DirectoryInfo inputDir)
        {
            List<FileInfo> fileList = new List<FileInfo>();
            DirectoryInfo[] childDirs = new DirectoryInfo[] { };

            try
            {
                fileList.AddRange(inputDir.GetFiles("*"));
                childDirs = inputDir.GetDirectories();
            }

            catch (Exception e)
            {
                Log.Error("Avalon plugin: Error while retrieving files/directories for: " + inputDir.FullName, e);
                throw e;
            }


            return fileList;
        }









        /// <summary>
        /// Load the Path information from the Config File into the dictionary
        /// </summary>
        /// <returns></returns>
        private string ReadConfig(string configDir)
        {
            // Make sure the file exists before we try to do any processing
            string strFileName = configDir + "\\MediaPortalDirs.xml";
            string strpath = string.Empty;
            Log.Info("Avalon plugin: Look for config in " + strFileName);
            if (File.Exists(strFileName))
            {
                Log.Info("Avalon plugin: Found config");
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(strFileName);
                    if (xml.DocumentElement == null)
                    {
                        return string.Empty;
                    }
                    XmlNodeList dirList = xml.DocumentElement.SelectNodes("/Config/Dir");
                    if (dirList == null || dirList.Count == 0)
                    {
                        return string.Empty;
                    }
                    foreach (XmlNode nodeDir in dirList)
                    {
                        if (nodeDir.Attributes != null)
                        {
                            XmlNode dirId = nodeDir.Attributes.GetNamedItem("id");
                            if (dirId != null && dirId.InnerText != null && dirId.InnerText.Length > 0)
                            {
                                if (dirId.InnerText == "Skin")
                                {
                                    XmlNode path = nodeDir.SelectSingleNode("Path");
                                    if (path != null)
                                    {
                                        string commonData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                                        strpath = path.InnerText.ToString();
                                        strpath = strpath.Replace("%PROGRAMDATA%", commonData);
                                        strpath = strpath.Replace("%ProgramData%", commonData);
                                        //Log.Info("Avalon plugin: " + dirId.InnerText + ":" + strpath);
                                        return strpath;
                                    }
                                }
                            }
                        }
                    }
                    return strpath;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }



        private void frmAvalonEditor_Load(object sender, EventArgs e)
        {
            LoadWindowPlugins();




            configDir = Config.GetFolder(Config.Dir.Base);

            string configDirXML = ReadConfig(configDir);
            //Log.Info("Avalon plugin: Skindir is " + configDirXML);
            configDir = configDirXML;



            // Check to see if Avalon is the selected skin (halt plugin if not)
            using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Base, "MediaPortalDirs.xml")))
            {
                myskin = "Avalon";
            }

            if (configDir == null)
                return;


            // Check to see if Avalon is the selected skin (halt plugin if not)
            using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml")))
            {
                myskin = "Avalon";
            }

            musicViews = GetMusicViews();
            cboParameterViews.Items.Clear();
            theMusicViews.Clear();

            foreach (KeyValuePair<string, string> mvv in musicViews)
            {
                theMusicViews.Add(mvv.Value);
            }

            cboParameterViews.Visible = false;
            lblParameter.Visible = false;
            cB_onlinevideosOption.Visible = false;
            movPicsCategoryCombo.Visible = false;
            groupBox5.Visible = false;

            string filename = Path.Combine(Path.Combine(SkinInfo.mpPaths.pluginPath, "windows"), "MP-TVSeries.dll");
            if (AvalonHelper.IsAssemblyAvailable("MP-TVSeries", new Version(2, 6, 5, 1265), filename))
            {
                tvseriesViews = GetTVSeriesViews();

                cboParameterViews.Items.Clear();
                theTVSeriesViews.Clear();

                foreach (KeyValuePair<string, string> tvv in tvseriesViews)
                {
                    theTVSeriesViews.Add(tvv.Value);
                    //cboParameterViews.Items.Add(tvv.Value);
                }

            }

            filename = Path.Combine(Path.Combine(SkinInfo.mpPaths.pluginPath, "windows\\OnlineVideos"), "OnlineVideos.dll");
            if (AvalonHelper.IsAssemblyAvailable("OnlineVideos", new Version(0, 27, 0, 0), filename))
            {
                onlineVideosViews = GetOnlineVideosViews();

                cboParameterViews.Items.Clear();
                theOnlineVideosViews.Clear();

                foreach (KeyValuePair<string, string> ovv in onlineVideosViews)
                {
                    theOnlineVideosViews.Add(ovv.Value);
                }
            }

            filename = Path.Combine(Path.Combine(SkinInfo.mpPaths.pluginPath, "windows"), "MyFilms.dll");
            if (AvalonHelper.IsAssemblyAvailable("MyFilms", new Version(5, 1, 0, 0), filename))
            {
                tbEditorStartParamsOutput.Text = "";
                cbEditorConfigs.Items.Clear();
                cbEditorViews.Items.Clear();
                cbEditorViewValues.Items.Clear();
                cbEditorConfigs.Text = "";
                cbEditorViews.Text = "";
                cbEditorViewValues.Text = "";

                ArrayList MyFilmsEditor = BaseMesFilms.GetConfigViewLists();
                // cbEditorConfigs.Items.Add("");
                foreach (BaseMesFilms.MFConfig config in MyFilmsEditor)
                {
                    cbEditorConfigs.Items.Add(config.Name);
                }
            }

            filename = Path.Combine(Path.Combine(SkinInfo.mpPaths.pluginPath, "windows"), "MovingPictures.dll");
            if (AvalonHelper.IsAssemblyAvailable("MovingPictures", new Version(1, 1, 0, 0), filename))
            {
                // load categories
                LoadMovingPicturesCategories();
            }

            path = configDir + "\\" + myskin;
            if (System.IO.Directory.Exists(path))
            {

                WP_Path = configDir + "\\" + myskin + "\\Media\\BasicHomeBG";
                if (System.IO.Directory.Exists(WP_Path))
                {
                    string[] fileEntries = System.IO.Directory.GetFiles(WP_Path);
                    int count = 0;
                    foreach (string file in fileEntries)
                    {
                        fileEntries[count] = System.IO.Path.GetFileName(file);
                        count++;
                    }
                    new_bgimage.Items.AddRange(fileEntries);

                    //Log.Info("Avalon plugin: Path " + path + " found");
                    if (loadIDs(true) == true)
                    {
                        avail_list.Enabled = true;
                        //Enable whatever needs to be enabled...
                    }
                }
            }
            else
            {
                Log.Info("Avalon plugin: Path " + path + " not found");
            }

            if (loadMenuIDs(true) == true)
            {
                used_list.Enabled = true;
                used_list_submenu.Enabled = true;
                //Enable whatever needs to be enabled...
            }

        }

        //
        // Key/value read methods TVSeries
        //
        // TVSeries Key
        //
        public string getTVSeriesViewKey(string value)
        {
            foreach (KeyValuePair<string, string> tvv in tvseriesViews)
            {
                if (tvv.Value.ToLower() == value.ToLower())
                    return tvv.Key;
            }
            return "false";
        }
        // TVSeries Value
        //
        public string getTVSeriesViewValue(string key)
        {
            foreach (KeyValuePair<string, string> tvv in tvseriesViews)
            {
                if (tvv.Key.ToLower() == key.ToLower())
                    return tvv.Value;
            }
            return "false";
        }
        //
        // Key/value read methods Music
        //
        // Music key
        //
        public string getMusicViewKey(string value)
        {
            foreach (KeyValuePair<string, string> mvv in musicViews)
            {
                if (mvv.Value.ToLower() == value.ToLower())
                    return mvv.Key;
            }
            return "false";
        }
        //Music Value
        //
        public string getMusicViewValue(string key)
        {
            foreach (KeyValuePair<string, string> mvv in musicViews)
            {
                if (mvv.Key.ToLower() == key.ToLower())
                    return mvv.Value;
            }
            return "false";
        }

        //
        // Key/value read methods OnlineVideos
        //
        //OnlineVideos Key
        //
        public string getOnlineVideosViewKey(string value)
        {
            foreach (KeyValuePair<string, string> mvv in onlineVideosViews)
            {
                if (mvv.Value.ToLower() == value.ToLower())
                    return mvv.Key;
            }
            return "false";
        }
        //OnlineVideos Value
        //
        public string getOnlineVideosViewValue(string key)
        {
            foreach (KeyValuePair<string, string> mvv in onlineVideosViews)
            {
                if (mvv.Key.ToLower() == key.ToLower())
                    return mvv.Value;
            }
            return "false";
        }

        /// <summary>
        /// Loads the Custom Category list into a Cornerstone Filter Combo Box
        /// </summary>
        private void LoadMovingPicturesCategories()
        {
            // initialize filter combo to manage the default filter
            movPicsCategoryCombo.TreePanel.TranslationParser = new TranslationParserDelegate(MediaPortal.Plugins.MovingPictures.MainUI.Translation.ParseString);
            movPicsCategoryCombo.Menu = MovingPicturesCore.Settings.CategoriesMenu;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (new_name.Text != "")
            {
                if (avail_list.SelectedItem != null)
                {
                    toolStripStatusLabel1.Text = avail_list.SelectedItem.ToString() + " added to menu";
                }
                menuItem item = new menuItem();
                item.name = new_name.Text;
                item.identifier = menuItems.Count;

                if (cB_singleImage.Checked)
                {
                    item.bgImage = new_bgimage.Text;
                }
                else if (cB_FanartHandler.Checked)
                {
                    item.bgImage = combo_FanartHandler.Text;
                }

                if (cB_RecentMedia.Checked)
                {

                    if (rB_Movies.Checked)
                    {
                        item.media = "MovingPictures";
                    }
                    else if (rB_Series.Checked)
                    {
                        item.media = "TVSeries";
                    }
                    else if (rB_Music.Checked)
                    {
                        item.media = "Music";
                    }
                    else if (rB_Recordings.Checked)
                    {
                        item.media = "Recordings";
                    }
                    //else if (rB_Pictures.Checked)
                    //{
                    //    item.media = "Pictures";
                    //}
                }
                else
                    item.media = "";

                int num1;
                bool res = int.TryParse(new_windowid.Text, out num1);
                if (res == false)
                {
                    item.hyperlink = -1;
                }
                else
                {
                    if (num1 == 501 && cboParameterViews.Text != "")
                    {
                        item.hyperlink = 504;
                    }
                    else
                    {
                        item.hyperlink = num1;
                    }
                }

                if (cboParameterViews.SelectedIndex != -1 || (movPicsCategoryCombo.Visible && movPicsCategoryCombo.SelectedIndex != -1) || tbEditorStartParamsOutput.Text != string.Empty)
                {
                    if (item.hyperlink == 9811)
                    {
                        item.property = getTVSeriesViewKey(cboParameterViews.SelectedItem.ToString());
                    }

                    if (item.hyperlink == 504)
                    {
                        item.property = getMusicViewKey(cboParameterViews.SelectedItem.ToString());
                    }

                    if (item.hyperlink == 4755)
                    {
                        string OnlineVideosReturn = "";
                        if (cB_onlinevideosOption.Checked)
                            OnlineVideosReturn = "Root";
                        else
                            OnlineVideosReturn = "Locked";

                        item.property = "site:" + getOnlineVideosViewKey(cboParameterViews.SelectedItem.ToString()) + "|return:" + OnlineVideosReturn;
                    }

                    if (item.hyperlink == 96742)
                    {
                        if (movPicsCategoryCombo.SelectedIndex != -1)
                        {

                            int? id = GetMovPicsCategoryNodeID(movPicsCategoryCombo.SelectedNode);
                            if (id != null)
                                item.property = "categoryid:" + id.ToString();

                            ClearMovingPicturesCategories();
                        }
                    }

                    if(item.hyperlink == 7986)
                    {
                        item.property = tbEditorStartParamsOutput.Text;
                    }

                }
                else
                    item.property = "";

                menuItems.Add(item);
                used_list.Items.Add(item.name);

                // Clear items
                new_name.Text = "";
                new_bgimage.Text = "";
                new_windowid.Text = "";
                cboParameterViews.Text = "";
                movPicsCategoryCombo.SelectedIndex = -1;
                cB_RecentMedia.Checked = false;
                if (used_list.Items.Count > 0)
                    avail_list.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Please enter a name for this item", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void btnSubAdd_Click(object sender, EventArgs e)
        {

            if (used_list.SelectedItem == null)
            {
                MessageBox.Show("Please select the Mainmenu this item belongs to", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (avail_list.SelectedItem == null)
            {
                MessageBox.Show("Please select an item from the 'Available modules' list", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (new_windowid.Text != "" && new_name.Text != "")
            {
                used_list_submenu.Enabled = true;
                toolStripStatusLabel1.Text = avail_list.SelectedItem.ToString() + " added to menu";
                menuItem item = new menuItem();
                item.name = new_name.Text;
                item.owner = menuItems[used_list.SelectedIndex].identifier;

                if (Convert.ToInt32(new_windowid.Text) == 501 && cboParameterViews.Text != "")
                {
                    item.hyperlink = Convert.ToInt32(504);
                }
                else
                {
                    item.hyperlink = Convert.ToInt32(new_windowid.Text);
                }


                if (cboParameterViews.SelectedIndex != -1 || (movPicsCategoryCombo.Visible && movPicsCategoryCombo.SelectedIndex != -1) || tbEditorStartParamsOutput.Text != string.Empty)
                {
                    if (item.hyperlink == 9811)
                    {
                        item.property = getTVSeriesViewKey(cboParameterViews.SelectedItem.ToString());
                    }

                    if (item.hyperlink == 504)
                    {
                        item.property = getMusicViewKey(cboParameterViews.SelectedItem.ToString());
                    }

                    if (item.hyperlink == 4755)
                    {
                        string OnlineVideosReturn = "";
                        if (cB_onlinevideosOption.Checked)
                            OnlineVideosReturn = "Root";
                        else
                            OnlineVideosReturn = "Locked";

                        item.property = "site:" + getOnlineVideosViewKey(cboParameterViews.SelectedItem.ToString()) + "|return:" + OnlineVideosReturn;
                    }

                    if (item.hyperlink == 96742)
                    {
                        if (movPicsCategoryCombo.SelectedIndex != -1)
                        {

                            int? id = GetMovPicsCategoryNodeID(movPicsCategoryCombo.SelectedNode);
                            if (id != null)
                                item.property = "categoryid:" + id.ToString();

                            ClearMovingPicturesCategories();
                        }
                    }
                    if (item.hyperlink == 7986)
                    {
                        item.property = tbEditorStartParamsOutput.Text;
                    }

                }
                newChildren.Add(new Child(item.owner, newChildren.Count, new_name.Text, item.hyperlink, item.property));
                //tempChildren.Add(new Child(item.owner, tempChildren.Count, new_name.Text, Convert.ToInt32(new_windowid.Text)));
                used_list_submenu.Items.Add(item.name);


                // Clear items
                new_name.Text = "";
                new_windowid.Text = "";
                cboParameterViews.Text = "";
                movPicsCategoryCombo.SelectedIndex = -1;
                cB_RecentMedia.Checked = false;
                if (used_list_submenu.Items.Count > 0)
                    avail_list.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("All fields must be completed", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            removeToolStripMenuItem_Click(sender, e);
        }

        private void btnSubRemove_Click(object sender, EventArgs e)
        {
            removeToolStripSubMenuItem_Click(sender, e);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (used_list.SelectedItem != null)
            {
                int index = used_list.SelectedIndex;
                int selected = menuItems[used_list.SelectedIndex].identifier;
                menuItem used_item = new menuItem();
                used_item.name = new_name.Text;
                used_item.identifier = selected;

                if (cB_singleImage.Checked)
                {
                    used_item.bgImage = new_bgimage.Text;
                }
                if (cB_FanartHandler.Checked)
                {
                    used_item.bgImage = combo_FanartHandler.Text;
                }

                if (cB_RecentMedia.Checked)
                {

                    if (rB_Movies.Checked)
                    {
                        used_item.media = "MovingPictures";
                    }
                    else if (rB_Series.Checked)
                    {
                        used_item.media = "TVSeries";
                    }
                    else if (rB_Music.Checked)
                    {
                        used_item.media = "Music";
                    }
                    else if (rB_Recordings.Checked)
                    {
                        used_item.media = "Recordings";
                    }
                    //else if (rB_Pictures.Checked)
                    //{
                    //    used_item.media = "Pictures";
                    //}
                }
                else
                    used_item.media = "";

                if (new_windowid.Text.Length > 0)
                {
                    if (Convert.ToInt32(new_windowid.Text) == 501 && cboParameterViews.Text != "")
                    {
                        used_item.hyperlink = Convert.ToInt32(504);
                    }
                    else
                    {
                        used_item.hyperlink = Convert.ToInt32(new_windowid.Text);
                    }

                }
                else
                {
                    used_item.hyperlink = -1;
                }

                if (cboParameterViews.SelectedIndex != -1 || (movPicsCategoryCombo.Visible && movPicsCategoryCombo.SelectedIndex != -1))
                {
                    if (used_item.hyperlink == 9811)
                    {
                        used_item.property = getTVSeriesViewKey(cboParameterViews.SelectedItem.ToString());
                    }

                    if (used_item.hyperlink == 504)
                    {
                        used_item.property = getMusicViewKey(cboParameterViews.SelectedItem.ToString());
                    }

                    if (used_item.hyperlink == 4755)
                    {
                        string OnlineVideosReturn = "";
                        if (cB_onlinevideosOption.Checked)
                            OnlineVideosReturn = "Root";
                        else
                            OnlineVideosReturn = "Locked";

                        used_item.property = "site:" + getOnlineVideosViewKey(cboParameterViews.SelectedItem.ToString()) + "|return:" + OnlineVideosReturn;
                    }

                    if (used_item.hyperlink == 96742)
                    {
                        if (movPicsCategoryCombo.SelectedIndex != -1)
                        {

                            int? id = GetMovPicsCategoryNodeID(movPicsCategoryCombo.SelectedNode);
                            if (id != null)
                                used_item.property = "categoryid:" + id.ToString();

                            ClearMovingPicturesCategories();
                        }
                    }
                }

                used_list.Items.RemoveAt(index);
                used_list.Items.Insert(index, used_item.name);

                menuItems.RemoveAt(index);
                menuItems.Insert(index, used_item);
            }
        }

        private void btnSubUpdate_Click(object sender, EventArgs e)
        {
            if (used_list_submenu.SelectedItem != null)
            {

                int index = used_list_submenu.SelectedIndex;
                int owner = menuItems[used_list.SelectedIndex].identifier;
                int hyperlink = 0;
                string property = "";

                if (new_windowid.Text.Length > 0)
                {
                    if (Convert.ToInt32(new_windowid.Text) == 501 && cboParameterViews.Text != "")
                    {
                        hyperlink = Convert.ToInt32(504);
                    }
                    else
                    {
                        hyperlink = Convert.ToInt32(new_windowid.Text);
                    }
                }
                else
                {
                    hyperlink = -1;
                }

                if (cboParameterViews.SelectedIndex != -1)
                {
                    if (hyperlink == 9811)
                    {
                        property = getTVSeriesViewKey(cboParameterViews.SelectedItem.ToString());
                    }

                    if (hyperlink == 504)
                    {
                        property = getMusicViewKey(cboParameterViews.SelectedItem.ToString());
                    }

                    if (hyperlink == 4755)
                    {
                        string OnlineVideosReturn = "";
                        if (cB_onlinevideosOption.Checked)
                            OnlineVideosReturn = "Root";
                        else
                            OnlineVideosReturn = "Locked";

                        property = "site:" + getOnlineVideosViewKey(cboParameterViews.SelectedItem.ToString()) + "|return:" + OnlineVideosReturn;
                    }

                    if (hyperlink == 96742)
                    {
                        if (movPicsCategoryCombo.SelectedIndex != -1)
                        {

                            int? id = GetMovPicsCategoryNodeID(movPicsCategoryCombo.SelectedNode);
                            if (id != null)
                                property = "categoryid:" + id.ToString();

                            ClearMovingPicturesCategories();
                        }
                    }

                }

                Child newchild = new Child(owner, index, new_name.Text, hyperlink, property);


                used_list_submenu.Items.RemoveAt(index);
                used_list_submenu.Items.Insert(index, new_name.Text);


                int a = 0;
                int itemtoremove = 0;
                foreach (Child mychild in newChildren)
                {
                    if (mychild.Owner == owner && mychild.ID == index)
                    {
                        itemtoremove = a;
                    }
                    a++;
                }

                singleChildren.RemoveAt(index);
                singleChildren.Insert(index, newchild);

                newChildren.RemoveAt(itemtoremove);
                newChildren.Insert(itemtoremove, newchild);


            }
        }

        // Move selected menu item down one position in list. 
        private void btMoveDown_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at bottom
            if (used_list.SelectedItem != null && used_list.SelectedIndex < used_list.Items.Count - 1)
            {
                int index = used_list.SelectedIndex;
                Object listItem = used_list.SelectedItem;
                menuItem mnuItem = menuItems[index];
                used_list.Items.RemoveAt(index);
                menuItems.RemoveAt(index);
                if (index < used_list.Items.Count)
                {
                    used_list.Items.Insert(index + 1, listItem);
                    menuItems.Insert(index + 1, mnuItem);
                }
                else
                {
                    used_list.Items.Add(listItem);
                    menuItems.Add(mnuItem);
                }
                used_list.SelectedIndex = index + 1;
            }
        }

        // Move selected menu item down one position in list. 
        private void btSubMoveDown_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at bottom
            if (used_list_submenu.SelectedItem != null && used_list_submenu.SelectedIndex < used_list_submenu.Items.Count - 1)
            {
                int index = used_list_submenu.SelectedIndex;
                int a = 0;
                int itemID = 0;
                int selID = 0;
                int itemtoremove = 0;
                int selected = menuItems[used_list.SelectedIndex].identifier;
                foreach (Child mychild in singleChildren)
                {
                    if (mychild.Owner == selected && a == used_list_submenu.SelectedIndex)
                    {
                        //Log.Info("fjern elm. " + mychild.Name);
                        itemtoremove = a;
                        itemID = mychild.ID;
                    }
                    a++;
                }

                singleChildren.RemoveAt(index);

                a = 0;
                foreach (Child mychild in newChildren)
                {
                    if (mychild.Owner == selected && mychild.ID == index)
                    {
                        itemtoremove = a;
                        selID = mychild.ID;
                    }
                    a++;
                }

                newChildren.RemoveAt(itemtoremove);

                Child newchild = new Child(selected, selID, new_name.Text, Convert.ToInt32(new_windowid.Text), cboParameterViews.Text);
                Object listItem = used_list_submenu.SelectedItem;
                used_list_submenu.Items.RemoveAt(index);
                if (index < used_list_submenu.Items.Count)
                {
                    used_list_submenu.Items.Insert(index + 1, listItem);
                    newChildren.Insert(itemtoremove + 1, newchild);
                    singleChildren.Insert(index + 1, newchild);
                }
                else
                {
                    used_list_submenu.Items.Add(listItem);
                    newChildren.Add(newchild);
                    singleChildren.Add(newchild);
                }
                used_list_submenu.SelectedIndex = index + 1;
            }
        }

        // Move selected menu item up one position in list. 
        private void btMoveUp_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at top
            if (used_list.SelectedItem != null && used_list.SelectedIndex > 0)
            {
                int index = used_list.SelectedIndex;
                Object listItem = used_list.SelectedItem;
                menuItem mnuItem = menuItems[index];
                used_list.Items.RemoveAt(index);
                menuItems.RemoveAt(index);
                used_list.Items.Insert(index - 1, listItem);
                menuItems.Insert(index - 1, mnuItem);
                used_list.SelectedIndex = index - 1;
            }

        }

        // Move selected menu item up one position in list. 
        private void btSubMoveUp_Click(object sender, EventArgs e)
        {
            // Do nothing if no item is selected or if already at top
            if (used_list_submenu.SelectedItem != null && used_list_submenu.SelectedIndex > 0)
            {

                int index = used_list_submenu.SelectedIndex;
                int selected = menuItems[used_list.SelectedIndex].identifier;
                int a = 0;
                int itemID = 0;
                int place = 0;
                string name = string.Empty;
                string childname = string.Empty;
                int hyper = 0;
                string property = string.Empty;
                Child child = null;

                tempChildren.Clear();

                foreach (Child mychild in newChildren)
                {
                    //MessageBox.Show("ejer:" + selected + "id:" + mychild.ID + "op:" + index + ":" + mychild.Name);
                    if (mychild.Owner == selected && mychild.ID == index)
                    {
                        place = mychild.ID - 1;
                        itemID = mychild.ID;
                        name = mychild.Name;
                        hyper = mychild.Hyperlink;
                        child = new Child(selected, place, name, hyper, property);
                    }

                    else if (mychild.Owner == selected && mychild.ID == index - 1)
                    {
                        place = mychild.ID + 1;
                        itemID = mychild.ID;
                        name = mychild.Name;
                        hyper = mychild.Hyperlink;
                        property = mychild.property;
                        child = new Child(selected, place, name, hyper, property);
                    }
                    else
                    {
                        place = mychild.ID;
                        itemID = mychild.ID;
                        name = mychild.Name;
                        hyper = mychild.Hyperlink;
                        property = mychild.property;
                        child = new Child(mychild.Owner, place, name, hyper, property);
                    }
                    tempChildren.Add(child);
                    a++;
                }
                newChildren.Clear();
                newChildren.AddRange(tempChildren);

                tempChildren.Clear();
                a = 0;
                do
                {



                    foreach (Child mychild in newChildren)
                    {
                        if (mychild.ID == a)
                        {
                            place = mychild.ID;
                            itemID = mychild.ID;
                            name = mychild.Name;
                            hyper = mychild.Hyperlink;
                            property = mychild.property;
                            child = new Child(mychild.Owner, place, name, hyper, property);
                            tempChildren.Add(child);

                        }
                    }

                    a++;
                }
                while (a < newChildren.Count);

                newChildren.Clear();
                newChildren.AddRange(tempChildren);

                Object listItem = used_list_submenu.SelectedItem;
                used_list_submenu.Items.RemoveAt(index);

                used_list_submenu.Items.Insert(index - 1, listItem);

                used_list_submenu.SelectedIndex = index - 1;



                new_windowid.Text = "" + hyper;
                new_name.Text = "" + name;
                toolStripStatusLabel1.Text = "Window ID: " + hyper;

                //foreach (Child mychild in newChildren)
                //{
                //    MessageBox.Show(mychild.ID + " " + mychild.Name);
                //}


            }

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int a = 0;
            if (used_list.SelectedItem != null)
            {
                int owner = menuItems[used_list.SelectedIndex].identifier;
                menuItems.RemoveAt(used_list.SelectedIndex);
                used_list.Items.Remove(used_list.SelectedItem);

                tempChildren.Clear();
                tempChildren.AddRange(newChildren);

                foreach (Child mychild in newChildren)
                {
                    if (mychild.Owner == owner)
                    {
                        //MessageBox.Show("Fandt " + mychild.Name + " der ejes af " + mychild.Owner + " og er på index " + a);
                        tempChildren.RemoveAt(a);
                        a--;
                    }
                    a++;
                }

                newChildren.Clear();
                newChildren.AddRange(tempChildren);
                used_list_submenu.Items.Clear();
                //Save();
            }
            else
                MessageBox.Show("No item is selected for removal", "Remove", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void removeToolStripSubMenuItem_Click(object sender, EventArgs e)
        {
            if (used_list_submenu.SelectedItem != null)
            {
                int a = 0;
                int owner = menuItems[used_list.SelectedIndex].identifier;
                int selected = used_list_submenu.SelectedIndex;

                tempChildren.Clear();
                tempChildren.AddRange(newChildren);

                foreach (Child mychild in newChildren)
                {
                    if (mychild.Owner == owner && a == selected)
                    {
                        //MessageBox.Show(selected + "-fjern " + mychild.Name + " der ejes af " + mychild.Owner);
                        tempChildren.RemoveAt(a);
                    }
                    a++;
                }

                newChildren.Clear();
                newChildren.AddRange(tempChildren);
                used_list_submenu.Items.Remove(used_list_submenu.SelectedItem);
            }
            else
                MessageBox.Show("No item is selected for removal", "Remove", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        int? GetMovPicsCategoryNodeID(IDBNode node)
        {
            return ((DBNode<DBMovieInfo>)node).ID;
        }

        IDBNode GetMovPicsDBNodeFromID(int id)
        {
            return MovingPicturesCore.DatabaseManager.Get<DBNode<DBMovieInfo>>(id);
        }

        private void movPicsCategoryCombo_DropDown(object sender, EventArgs e)
        {
            if (movPicsCategoryCombo.SelectedIndex == -1 && MovingPicturesCore.Settings.CategoriesEnabled)
            {
                movPicsCategoryCombo.SelectedNode = MovingPicturesCore.Settings.CategoriesMenu.RootNodes[0];
            }
        }

        void ClearMovingPicturesCategories()
        {
            movPicsCategoryCombo.Text = string.Empty;
            movPicsCategoryCombo.SelectedNode = null;
            movPicsCategoryCombo.SelectedIndex = -1;
        }

        // Save settings to file
        private void btSave_Click(object sender, EventArgs e)
        {

            using (MediaPortal.Profile.Settings xmlwriter = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "Avalon.xml")))
            {
                if (configloaded)
                {
                    int a = 0;
                    do
                    {
                        xmlwriter.RemoveEntry("AvalonBasicHome", "menuItemName" + a);
                        xmlwriter.RemoveEntry("AvalonBasicHome", "AvalonBasicHomeSubmenu" + a);
                        xmlwriter.RemoveEntry("AvalonBasicHome", "menuItemBgImage" + a);
                        xmlwriter.RemoveEntry("AvalonBasicHome", "menuItemParameter" + a);
                        xmlwriter.RemoveEntry("AvalonBasicHome", "menuItemRecentMedia" + a);

                        int b = 0;
                        do
                        {
                            xmlwriter.RemoveEntry("AvalonBasicHomeSubmenu" + a, "submenuItemName" + b);
                            xmlwriter.RemoveEntry("AvalonBasicHomeSubmenu" + a, "submenuItemHyperlink" + b);
                            xmlwriter.RemoveEntry("AvalonBasicHomeSubmenu" + a, "submenuItemParameter" + b);
                            b++;
                        } while (b < 250);


                        a++;
                    } while (a < 25);
                }





                int mycount = 0;
                //menuItems.Sort(delegate(menuItem li1, menuItem li2) { return li1.identifier.CompareTo(li2.identifier); });

                foreach (menuItem item in menuItems)
                {
                    xmlwriter.SetValue("AvalonBasicHome", "menuItemName" + mycount, item.name);
                    xmlwriter.SetValue("AvalonBasicHome", "menuItemHyperlink" + mycount, item.hyperlink);
                    xmlwriter.SetValue("AvalonBasicHome", "menuItemBgImage" + mycount, item.bgImage);
                    xmlwriter.SetValue("AvalonBasicHome", "menuItemParameter" + mycount, item.property);
                    xmlwriter.SetValue("AvalonBasicHome", "menuItemRecentMedia" + mycount, item.media);

                    int mycount2 = 0;
                    foreach (Child mychild in newChildren)
                    {
                        //Log.Info("tester gem: " + mychild.Name);
                        if (item.identifier == mychild.Owner)
                        {
                            xmlwriter.SetValue("AvalonBasicHomeSubmenu" + mycount, "submenuItemName" + mycount2, mychild.Name);
                            xmlwriter.SetValue("AvalonBasicHomeSubmenu" + mycount, "submenuItemHyperlink" + mycount2, mychild.Hyperlink);
                            xmlwriter.SetValue("AvalonBasicHomeSubmenu" + mycount, "submenuItemParameter" + mycount2, mychild.property);
                            mycount2++;
                        }
                    }
                    mycount++;
                }

            }


            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }






        private bool loadIDs(bool onLoad)
        {
            avail_list.Enabled = true;
            avail_list.Items.Clear();
            string[] files = System.IO.Directory.GetFiles(path);
            foreach (string file in files)
            {
                try
                {
                    if (file.ToLower().StartsWith("common") == false && file.ToLower().Contains("dialog") == false
                        && file.ToLower().Contains("wizard") == false && file.ToLower().Contains("basichome") == false)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(file);
                        XmlNode node = doc.DocumentElement.SelectSingleNode("/window/id");

                        ids.Add(node.InnerText);
                        avail_list.Items.Add(file.Remove(0, file.LastIndexOf(@"\") + 1).Replace(".xml", ""));
                        menuItem avail_item = new menuItem();
                        avail_item.name = file.Remove(0, file.LastIndexOf(@"\") + 1).Replace(".xml", "");
                        avail_item.hyperlink = Convert.ToInt32(node.InnerText);
                        avail_item.bgImage = "";
                        AvailmenuItems.Add(avail_item);
                    }
                }
                catch { }
            }

            if (avail_list.Items.Count > 0)
            {
                //loadSkin("BasicHome.xml");
                avail_list.Items.Clear();
                foreach (menuItem item in AvailmenuItems)
                {
                    avail_list.Items.Add(item.name);
                }
                return true;
            }
            else
            {
                // Dont need to complain when first loading the app as its possible that the skin isnt installed
                if (!onLoad)
                    MessageBox.Show("Error reading directory.");
                return false;

            }

        }


        public bool loadMenuIDs(bool onLoad)
        {

            menuItems.Clear();
            used_list.Enabled = true;
            used_list.Items.Clear();
            used_list_submenu.Items.Clear();


            using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "Avalon.xml")))
            {

                int a = 0;

                do
                {
                    //Read menu
                    if (xmlreader.GetValueAsString("AvalonBasicHome", "menuItemName" + a, "") != "")
                    {
                        Parents.Add(new Owner(a, xmlreader.GetValueAsString("AvalonBasicHome", "menuItemName" + a, ""), xmlreader.GetValueAsInt("AvalonBasicHome", "menuItemName" + a, 0), xmlreader.GetValueAsString("AvalonBasicHome", "menuItemParameter" + a, "")));
                        menuItem used_item = new menuItem();
                        used_item.identifier = a;
                        used_item.name = xmlreader.GetValueAsString("AvalonBasicHome", "menuItemName" + a, "");
                        used_item.hyperlink = xmlreader.GetValueAsInt("AvalonBasicHome", "menuItemHyperlink" + a, 0);
                        used_item.bgImage = xmlreader.GetValueAsString("AvalonBasicHome", "menuItemBgImage" + a, "");
                        used_item.property = xmlreader.GetValueAsString("AvalonBasicHome", "menuItemParameter" + a, "");
                        used_item.media = xmlreader.GetValueAsString("AvalonBasicHome", "menuItemRecentMedia" + a, "");
                        menuItems.Add(used_item);
                        used_list.Items.Add(used_item.name);
                        configloaded = true;


                        int b = 0;
                        do
                        {
                            //Read submenu
                            if (xmlreader.GetValueAsString("AvalonBasicHomeSubmenu" + a, "submenuItemName" + b, "") != "")
                            {
                                //Log.Info("found some submenus, :" + a + ", " + b + "navn: " + xmlreader.GetValueAsString("AvalonBasicHomeSubmenu" + a, "submenuItemName" + b, ""));
                                Children.Add(new Child(a, b, xmlreader.GetValueAsString("AvalonBasicHomeSubmenu" + a, "submenuItemName" + b, ""), xmlreader.GetValueAsInt("AvalonBasicHomeSubmenu" + a, "submenuItemHyperlink" + b, 0), xmlreader.GetValueAsString("AvalonBasicHomeSubmenu" + a, "submenuItemParameter" + b, "")));
                            }
                            b++;
                        } while (b < 250);




                    }
                    a++;
                } while (a < 25);
                newChildren = Children;

                cB_FanartHandler.Checked = false;
                new_bgimage.Visible = false;
                cB_singleImage.Checked = false;
                combo_FanartHandler.Visible = false;
                bt_browse.Visible = false;
                rB_Movies.Visible = false;
                rB_Music.Visible = false;
                rB_Recordings.Visible = false;
                //rB_Pictures.Visible = false;
                rB_Series.Visible = false;

                cB_FanartHandler.Location = new Point(12, 42);
            }


            //return true;
            if (used_list.Items.Count > 0)
            {
                return true;
            }
            else
            {
                // Dont need to complain when first loading the app as its possible that the skin isnt installed
                if (!onLoad)
                    MessageBox.Show("Error reading directory.");
                return false;

            }

            // Remove double entries
            int cnt = used_list.Items.Count;
            for (int i = 1; i < (cnt / 2) + 1; i++)
            {
                used_list.Items.RemoveAt(i);
                menuItems.RemoveAt(i);
            }
            cnt = used_list.Items.Count - 1;
            used_list.Items.RemoveAt(cnt);
            menuItems.RemoveAt(cnt);


        }

        
        public void loadSubMenuIDs(int owner)
        {
        }


        private void avail_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (avail_list.SelectedIndex >= 0)
            {
                if (radioButton1.Checked)
                {
                    new_windowid.Text = "" + AvailmenuItems[avail_list.SelectedIndex].hyperlink;

                    new_name.Text = "" + AvailmenuItems[avail_list.SelectedIndex].name;
                    //cboParameterViews.Text = "";
                    toolStripStatusLabel1.Text = "Window ID: " + AvailmenuItems[avail_list.SelectedIndex].hyperlink;
                    //used_list.SelectedIndex = -1;

                    cB_RecentMedia.Checked = false;
                    rB_Movies.Checked = false;
                    rB_Music.Checked = false;
                    rB_Recordings.Checked = false;
                    //rB_Pictures.Checked = false;
                    rB_Series.Checked = false;
                    cB_FanartHandler.Checked = false;

                    switch (AvailmenuItems[avail_list.SelectedIndex].hyperlink)
                    {
                        case 9811:
                            cboParameterViews.Visible = true;
                            lblParameter.Visible = true;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            cboParameterViews.DataSource = theTVSeriesViews;
                            cboParameterViews.Text = "";
                            cB_onlinevideosOption.Visible = false;
                            break;
                        case 501:
                            cboParameterViews.Visible = true;
                            lblParameter.Visible = true;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            cboParameterViews.DataSource = theMusicViews;
                            cboParameterViews.Text = "";
                            cB_onlinevideosOption.Visible = false;
                            break;
                        case 4755:
                            cboParameterViews.Visible = true;
                            lblParameter.Visible = true;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            cB_onlinevideosOption.Visible = true;
                            cboParameterViews.DataSource = theOnlineVideosViews;
                            cboParameterViews.Text = "";
                            break;
                        case 96742:
                            movPicsCategoryCombo.Visible = true;
                            movPicsCategoryCombo.Location = new Point(70, 81);
                            cboParameterViews.Visible = false;
                            lblParameter.Visible = false;
                            lblMovieCategories.Visible = true;
                            lblMovieCategories.Location = new Point(6, 84);
                            cB_onlinevideosOption.Visible = false;
                            break;
                        case 7986:
                            cboParameterViews.Visible = false;
                            lblParameter.Visible = false;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            cB_onlinevideosOption.Visible = false;
                            groupBox5.Visible = true;
                            groupBox5.Location = new Point(6, 84);
                            groupBox2.Location = new Point(9, 300);
                            cB_RecentMedia.Location = new Point(21, 430);
                            rB_Movies.Location = new Point(39, 450);
                            rB_Music.Location = new Point(39, 474);
                            rB_Series.Location = new Point(109, 450);
                            rB_Recordings.Location = new Point(39, 474);
                            break;
                        default:
                            cboParameterViews.Visible = false;
                            lblParameter.Visible = false;
                            cB_onlinevideosOption.Visible = false;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            groupBox5.Visible = false;
                            groupBox2.Location = new Point(9, 130);
                            cB_RecentMedia.Location = new Point(21, 260);
                            rB_Movies.Location = new Point(39, 280);
                            rB_Music.Location = new Point(39, 304);
                            rB_Series.Location = new Point(109, 280);
                            rB_Recordings.Location = new Point(39, 304);
                            break;
                    }

                }

                if (radioButton2.Checked)
                {
                    new_windowid.Text = "" + HomemenuItems[avail_list.SelectedIndex].hyperlink;
                    new_name.Text = "" + HomemenuItems[avail_list.SelectedIndex].name;
                    toolStripStatusLabel1.Text = "Window ID: " + HomemenuItems[avail_list.SelectedIndex].hyperlink;
                    //cboParameterViews.Text = "";
                    //used_list.SelectedIndex = -1;

                    cB_RecentMedia.Checked = false;
                    rB_Movies.Checked = false;
                    rB_Music.Checked = false;
                    rB_Recordings.Checked = false;
                    //rB_Pictures.Checked = false;
                    rB_Series.Checked = false;
                    cB_FanartHandler.Checked = false;

                    switch (HomemenuItems[avail_list.SelectedIndex].hyperlink)
                    {
                        case 9811:
                            cboParameterViews.Visible = true;
                            lblParameter.Visible = true;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            cboParameterViews.DataSource = theTVSeriesViews;
                            cboParameterViews.Text = "";
                            cB_onlinevideosOption.Visible = false;
                            break;
                        case 501:
                            cboParameterViews.Visible = true;
                            lblParameter.Visible = true;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            cboParameterViews.DataSource = theMusicViews;
                            cboParameterViews.Text = "";
                            cB_onlinevideosOption.Visible = false;
                            break;
                        case 4755:
                            cboParameterViews.Visible = true;
                            lblParameter.Visible = true;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            cB_onlinevideosOption.Visible = true;
                            cboParameterViews.DataSource = theOnlineVideosViews;
                            cboParameterViews.Text = "";
                            break;
                        case 96742:
                            movPicsCategoryCombo.Visible = true;
                            movPicsCategoryCombo.Location = new Point(70, 81);
                            cboParameterViews.Visible = false;
                            lblParameter.Visible = false;
                            lblMovieCategories.Visible = true;
                            lblMovieCategories.Location = new Point(6, 84);
                            cB_onlinevideosOption.Visible = false;
                            break;
                        case 7986:
                            cboParameterViews.Visible = false;
                            lblParameter.Visible = false;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            cB_onlinevideosOption.Visible = false;
                            groupBox5.Visible = true;
                            groupBox5.Location = new Point(6, 84);
                            groupBox2.Location = new Point(9, 300);
                            cB_RecentMedia.Location = new Point(21, 430);
                            rB_Movies.Location = new Point(39, 450);
                            rB_Music.Location = new Point(39, 474);
                            rB_Series.Location = new Point(109, 450);
                            rB_Recordings.Location = new Point(39, 474);
                            break;
                        default:
                            cboParameterViews.Visible = false;
                            lblParameter.Visible = false;
                            cB_onlinevideosOption.Visible = false;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            groupBox5.Visible = false;
                            groupBox2.Location = new Point(9, 130);
                            cB_RecentMedia.Location = new Point(21, 260);
                            rB_Movies.Location = new Point(39, 280);
                            rB_Music.Location = new Point(39, 304);
                            rB_Series.Location = new Point(109, 280);
                            rB_Recordings.Location = new Point(39, 304);
                            break;
                    }

                }

                if (radioButton3.Checked)
                {
                    new_windowid.Text = "" + PluginmenuItems[avail_list.SelectedIndex].hyperlink;
                    new_name.Text = "" + PluginmenuItems[avail_list.SelectedIndex].name;
                    toolStripStatusLabel1.Text = "Window ID: " + PluginmenuItems[avail_list.SelectedIndex].hyperlink;
                    cboParameterViews.Text = "";
                    //used_list.SelectedIndex = -1;

                    cB_RecentMedia.Checked = false;
                    rB_Movies.Checked = false;
                    rB_Music.Checked = false;
                    rB_Recordings.Checked = false;
                    //rB_Pictures.Checked = false;
                    rB_Series.Checked = false;

                    switch (PluginmenuItems[avail_list.SelectedIndex].hyperlink)
                    {
                        case 9811:
                            cboParameterViews.Visible = true;
                            lblParameter.Visible = true;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            cboParameterViews.DataSource = theTVSeriesViews;
                            cboParameterViews.Text = "";
                            cB_onlinevideosOption.Visible = false;
                            break;
                        case 501:
                            cboParameterViews.Visible = true;
                            lblParameter.Visible = true;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            cboParameterViews.DataSource = theMusicViews;
                            cboParameterViews.Text = "";
                            cB_onlinevideosOption.Visible = false;
                            break;
                        case 4755:
                            cboParameterViews.Visible = true;
                            lblParameter.Visible = true;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            cB_onlinevideosOption.Visible = true;
                            cboParameterViews.DataSource = theOnlineVideosViews;
                            cboParameterViews.Text = "";
                            break;
                        case 96742:
                            movPicsCategoryCombo.Visible = true;
                            movPicsCategoryCombo.Location = new Point(70, 81);
                            cboParameterViews.Visible = false;
                            lblParameter.Visible = false;
                            lblMovieCategories.Visible = true;
                            lblMovieCategories.Location = new Point(6, 84);
                            break;
                        case 7986:
                            cboParameterViews.Visible = false;
                            lblParameter.Visible = false;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            cB_onlinevideosOption.Visible = false;
                            groupBox5.Visible = true;
                            groupBox5.Location = new Point(6, 84);
                            groupBox2.Location = new Point(9, 300);
                            cB_RecentMedia.Location = new Point(21, 430);
                            rB_Movies.Location = new Point(39, 450);
                            rB_Music.Location = new Point(39, 474);
                            rB_Series.Location = new Point(109, 450);
                            rB_Recordings.Location = new Point(39, 474);
                            break;
                        default:
                            cboParameterViews.Visible = false;
                            lblParameter.Visible = false;
                            cB_onlinevideosOption.Visible = false;
                            movPicsCategoryCombo.Visible = false;
                            lblMovieCategories.Visible = false;
                            groupBox5.Visible = false;
                            groupBox2.Location = new Point(9, 130);
                            cB_RecentMedia.Location = new Point(21, 260);
                            rB_Movies.Location = new Point(39, 280);
                            rB_Music.Location = new Point(39, 304);
                            rB_Series.Location = new Point(109, 280);
                            rB_Recordings.Location = new Point(39, 304);
                            break;
                    }
                }

            }
            movPicsCategoryCombo.SelectedIndex = -1;
            cboParameterViews.SelectedIndex = -1;
        }

        private void used_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (used_list.SelectedIndex >= 0)
            {
                used_list_submenu.Items.Clear();
                singleChildren.Clear();
                foreach (Child mychild in newChildren)
                {
                    if (mychild.Owner == menuItems[used_list.SelectedIndex].identifier)
                    {
                        singleChildren.Add(mychild);
                        used_list_submenu.Items.Add(mychild.Name);
                    }
                }
                if (menuItems[used_list.SelectedIndex].hyperlink > -1)
                {
                    new_windowid.Text = "" + menuItems[used_list.SelectedIndex].hyperlink;
                }
                else
                {
                    new_windowid.Text = "";
                }
                new_name.Text = "" + menuItems[used_list.SelectedIndex].name;
                toolStripStatusLabel1.Text = "Window ID: " + menuItems[used_list.SelectedIndex].hyperlink;
                new_bgimage.Text = menuItems[used_list.SelectedIndex].bgImage;
                combo_FanartHandler.Text = menuItems[used_list.SelectedIndex].bgImage;
                avail_list.SelectedIndex = -1;

                if (!new_bgimage.Text.Contains(".jpg") || !new_bgimage.Text.Contains(".png"))
                {
                    cB_singleImage.Checked = false;
                    cB_FanartHandler.Checked = true;
                }

                if (combo_FanartHandler.Text.Contains(".jpg") || combo_FanartHandler.Text.Contains(".png"))
                {
                    cB_singleImage.Checked = true;
                    cB_FanartHandler.Checked = false;
                }

                if (new_bgimage.Text == String.Empty && combo_FanartHandler.Text == String.Empty)
                {
                    cB_singleImage.Checked = false;
                    cB_FanartHandler.Checked = false;
                }

                switch (menuItems[used_list.SelectedIndex].media)
                {
                    case "MovingPictures":
                        cB_RecentMedia.Checked = true;
                        rB_Movies.Checked = true;
                        break;
                    case "TVSeries":
                        cB_RecentMedia.Checked = true;
                        rB_Series.Checked = true;
                        break;
                    case "Recordings":
                        cB_RecentMedia.Checked = true;
                        rB_Recordings.Checked = true;
                        break;
                    case "Music":
                        cB_RecentMedia.Checked = true;
                        rB_Music.Checked = true;
                        break;
                    //case "Pictures":
                    //    cB_RecentMedia.Checked = true;
                    //    rB_Pictures.Checked = true;
                    //    break;
                    default:
                        cB_RecentMedia.Checked = false;
                        rB_Movies.Checked = false;
                        rB_Music.Checked = false;
                        rB_Recordings.Checked = false;
                        rB_Series.Checked = false;
                        //    rB_Pictures.Checked = false;
                        break;
                }

                switch (menuItems[used_list.SelectedIndex].hyperlink)
                {
                    case 9811:
                        cboParameterViews.Visible = true;
                        lblParameter.Visible = true;
                        movPicsCategoryCombo.Visible = false;
                        lblMovieCategories.Visible = false;
                        cboParameterViews.DataSource = theTVSeriesViews;
                        cboParameterViews.Text = menuItems[used_list.SelectedIndex].property;
                        cB_onlinevideosOption.Visible = false;
                        break;
                    case 501:
                        cboParameterViews.Visible = true;
                        lblParameter.Visible = true;
                        movPicsCategoryCombo.Visible = false;
                        lblMovieCategories.Visible = false;
                        cboParameterViews.DataSource = theMusicViews;
                        cboParameterViews.Text = menuItems[used_list.SelectedIndex].property;
                        cB_onlinevideosOption.Visible = false;
                        break;
                    case 4755:
                        cboParameterViews.Visible = true;
                        lblParameter.Visible = true;
                        movPicsCategoryCombo.Visible = false;
                        lblMovieCategories.Visible = false;
                        cB_onlinevideosOption.Visible = true;
                        cboParameterViews.DataSource = theOnlineVideosViews;
                        cboParameterViews.Text = menuItems[used_list.SelectedIndex].property;
                        break;
                    case 96742:
                        movPicsCategoryCombo.Visible = true;
                        movPicsCategoryCombo.Location = new Point(70, 81);
                        cboParameterViews.Visible = false;
                        lblParameter.Visible = false;
                        lblMovieCategories.Visible = true;
                        lblMovieCategories.Location = new Point(6, 84);
                        cB_onlinevideosOption.Visible = false;
                        break;
                    default:
                        cboParameterViews.Visible = false;
                        lblParameter.Visible = false;
                        cB_onlinevideosOption.Visible = false;
                        movPicsCategoryCombo.Visible = false;
                        lblMovieCategories.Visible = false;
                        break;
                }
            }
            movPicsCategoryCombo.SelectedIndex = -1;
            cboParameterViews.SelectedIndex = -1;
        }

        private void used_list_submenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selected = used_list_submenu.SelectedIndex;
            int a = 0;
            foreach (Child mychild in singleChildren)
            {
                if (a == selected)
                {
                    if (mychild.Hyperlink > -1)
                    {
                        new_windowid.Text = "" + mychild.Hyperlink;
                    }
                    else
                    {
                        new_windowid.Text = "";
                    }
                    new_name.Text = "" + mychild.Name;
                    cboParameterViews.Text = "" + mychild.property;
                    toolStripStatusLabel1.Text = "Window ID: " + mychild.Hyperlink;
                }
                a++;
            }

            //avail_list.SelectedIndex = -1;
            movPicsCategoryCombo.SelectedIndex = -1;
            cboParameterViews.SelectedIndex = -1;

        }

        private void rBut1_CheckedChanged(object sender, System.EventArgs e)
        {
            avail_list.Items.Clear();
            foreach (menuItem item in AvailmenuItems)
            {
                avail_list.Items.Add(item.name);
            }
        }

        private void rBut2_CheckedChanged(object sender, System.EventArgs e)
        {
            avail_list.Items.Clear();
            foreach (menuItem item in HomemenuItems)
            {
                avail_list.Items.Add(item.name);
            }
        }

        private void rBut3_CheckedChanged(object sender, System.EventArgs e)
        {
            avail_list.Items.Clear();
            foreach (menuItem item in PluginmenuItems)
            {
                avail_list.Items.Add(item.name);
            }
        }

        private void bt_browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            Stream myStream = null;

            openFileDialog1.InitialDirectory = path + "\\media\\BasicHomeBG";
            openFileDialog1.Filter = "PNG files (*.png)|*.png|JPG files (*.jpg)|*.jpg";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            if (System.IO.Directory.Exists(path))
                            {
                                System.IO.File.Copy(openFileDialog1.FileName, path + "\\media\\BasicHomeBG\\" + openFileDialog1.SafeFileName, true);
                            }
                            new_bgimage.Text = openFileDialog1.SafeFileName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }

            }
        }

        private void cB_RecentMedia_CheckedChanged(object sender, EventArgs e)
        {
            if (cB_RecentMedia.Checked == true)
            {
                rB_Movies.Visible = true;
                rB_Music.Visible = true;
                rB_Recordings.Visible = true;
                rB_Series.Visible = true;
                //rB_Pictures.Visible = true;
            }
            else
            {
                rB_Movies.Visible = false;
                rB_Music.Visible = false;
                rB_Recordings.Visible = false;
                rB_Series.Visible = false;
                // rB_Pictures.Visible = false;
            }
        }

        private void cB_FanartHandler_CheckedChanged(object sender, EventArgs e)
        {
            if (cB_FanartHandler.Checked == true)
            {
                combo_FanartHandler.Visible = true;
                cB_singleImage.Checked = false;
                combo_FanartHandler.Location = new Point(12, 65);
            }
            else
            {
                combo_FanartHandler.Visible = false;
            }
        }

        private void cB_singleImage_CheckedChanged(object sender, EventArgs e)
        {
            if (cB_singleImage.Checked == true)
            {
                new_bgimage.Visible = true;
                cB_FanartHandler.Checked = false;
                cB_FanartHandler.Location = new Point(12, 69);
                new_bgimage.Location = new Point(12, 42);
                bt_browse.Visible = true;

            }
            else
            {
                new_bgimage.Visible = false;
                cB_FanartHandler.Location = new Point(12, 42);
                combo_FanartHandler.Location = new Point(12, 65);
                bt_browse.Visible = false;
            }
        }

        public class menuItem
        {
            public int hyperlink;
            public string bgImage;
            public string name;
            public int identifier;
            public int owner;
            public string property;
            public string media;
        }
      }
    }
