using System;
using System.Collections.Generic;
using System.Reflection;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using MediaPortal.Dialogs;
using Microsoft.Win32;
using TraktPlugin.TraktAPI;
using TraktPlugin.TraktAPI.DataStructures;
using TraktPlugin;

namespace AvalonGUIConfig
{
    [PluginIcons("AvalonGUIConfig.Plugin.png", "AvalonGUIConfig.PluginDisabled.png")]
    public class AvalonGUIConfig : GUIWindow, ISetupForm
    {

        #region Enums
        public enum PendingChanges
        {
            ClearCache
        }

        public enum TraktMenuItems
        {
            Main,
            Recommendations,
            Trending,
            WatchList,
            Rate,
            AddToWatchList,
            Shouts,
            AddToList,
            RelatedMovies
        }

        #endregion

        internal static settings ampSettings = new settings();
        SkinInfo skInfo = new SkinInfo();

        public static string CurrentSkin { get { return GUIGraphicsContext.Skin; } }
        public static string PreviousSkin { get; set; }

        public static bool updateAvailable { get; set; }
        public static bool checkOnStart { get; set; }
        public static bool checkForUpdateAt { get; set; }
        public static int checkInterval { get; set; }
        public static double hours { get; set; }
        public static bool manualInstallNeeded { get; set; }
        public static bool patchUtilityRunUnattended = true;
        public static bool patchUtilityRestartMP = true;
        public static bool patchAppliedLastRun { get; set; }
        public static string theRevisions { get; set; }
        public static bool MinimiseOnExit { get; set; }
        public static DateTime checkTime { get; set; }
        public static string nextUpdateCheck { get; set; }
        public static System.Threading.Timer updateChkTimer;
        System.Windows.Forms.Timer mrTimer = new System.Windows.Forms.Timer();
      
        public static List<PendingChanges> PendingRestartChanges = new List<PendingChanges>();

        #region Private Properties

        private static Regex _isNumber = new Regex(@"^\d+$");

        #endregion

        #region Skin Connection
        
        public enum SkinControlIDs
        {
            MovPicsConfig = 50,     // MovingPictures Configuration
            TVConfig = 51,           // TV Configuration
            TVSeriesConfig = 52,           // TV Configuration
            SkinUpdateGUI = 53,           // Skin Update
            MyFilmsConfig = 54,     // MyFilms Configuration
            MiscConfigGUI = 55,     // Misc Configuration
            VideoConfig = 56
        }

        public enum AvalonScreenID
        {
            AvalonMovPicsConfig = 155101,
            AvalonTVConfig = 155102,
            AvalonTVSeriesConfig = 155103,
            AvalonSkinUpdate = 155104,
            AvalonMyFilmsConfig = 155105,
            AvalonMiscConfig = 155106,
            AvalonVideoConfig = 155107
        }

        private enum MediaPortalWindows
        {
            Home = 0,
            HomePlugins = 34,
            BasicHome = 35
        }

        [SkinControl((int)SkinControlIDs.MovPicsConfig)]
        protected GUIButtonControl btMovPicsConfig = null;
        [SkinControl((int)SkinControlIDs.TVConfig)]
        protected GUIButtonControl btTVConfig = null;
        [SkinControl((int)SkinControlIDs.TVSeriesConfig)]
        protected GUIButtonControl btTVSeriesConfig = null;
        [SkinControl((int)SkinControlIDs.SkinUpdateGUI)]
        protected GUIButtonControl btSkinUpdateGUI = null;
        [SkinControl((int)SkinControlIDs.MyFilmsConfig)]
        protected GUIButtonControl btMyFilmsConfig = null;
        [SkinControl((int)SkinControlIDs.MiscConfigGUI)]
        protected GUIButtonControl btMiscConfig = null;
        [SkinControl((int)SkinControlIDs.VideoConfig)]
        protected GUIButtonControl btVideoConfig = null;
        #endregion

        #region Base overrides

        public override bool Init()
        {

            // Check if the skin is Avalon and bail if not.
            if (GUIGraphicsContext.Skin.Contains("Avalon"))
            {
                InitAvalon();
            }
            settings.PreviousSkin = settings.CurrentSkin;
            GUIWindowManager.OnDeActivateWindow += new GUIWindowManager.WindowActivationHandler(GUIWindowManager_OnDeActivateWindow);
            GUIWindowManager.OnNewAction += new OnActionHandler(GUIWindowManager_OnNewAction); 
            return Load(GUIGraphicsContext.Skin + @"\AvalonConfig.xml");
        }

        protected override void OnPageLoad()
        {
            GUIControl.SetControlLabel(GetID, (int)SkinControlIDs.MovPicsConfig, Translation.SkinSettingsMovingPictures);
            GUIControl.SetControlLabel(GetID, (int)SkinControlIDs.TVConfig, Translation.SkinSettingsTV);
            GUIControl.SetControlLabel(GetID, (int)SkinControlIDs.TVSeriesConfig, Translation.SkinSettingsTVSeries);
            GUIControl.SetControlLabel(GetID, (int)SkinControlIDs.SkinUpdateGUI, Translation.SkinSettingsUpdate);
            GUIControl.SetControlLabel(GetID, (int)SkinControlIDs.MyFilmsConfig, Translation.SkinSettingsMyFilms);
            GUIControl.SetControlLabel(GetID, (int)SkinControlIDs.MiscConfigGUI, Translation.SkinSettingsMisc);
            GUIControl.SetControlLabel(GetID, (int)SkinControlIDs.VideoConfig, Translation.SkinSettingsVideo);
        }

        protected override void OnPageDestroy(int new_windowId)
        {
            Log.Info("Avalon: Shutdown of AvalonGUIConfig complete");
        }

        protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
        {
            #region Activate Setting GUI Windows

            if (control == btMovPicsConfig)
            {
                GUIWindowManager.ActivateWindow((int)AvalonScreenID.AvalonMovPicsConfig);
            }

            if (control == btTVConfig)
            {
                GUIWindowManager.ActivateWindow((int)AvalonScreenID.AvalonTVConfig);
            }

            if (control == btTVSeriesConfig)
            {
                GUIWindowManager.ActivateWindow((int)AvalonScreenID.AvalonTVSeriesConfig);
            }

            if (control == btSkinUpdateGUI)
            {
                GUIWindowManager.ActivateWindow((int)AvalonScreenID.AvalonSkinUpdate);
            }

            if (control == btMyFilmsConfig)
            {
                GUIWindowManager.ActivateWindow((int)AvalonScreenID.AvalonMyFilmsConfig);
            }

            if (control == btMiscConfig)
            {
                GUIWindowManager.ActivateWindow((int)AvalonScreenID.AvalonMiscConfig);
            }

            if (control == btVideoConfig)
            {
                GUIWindowManager.ActivateWindow((int)AvalonScreenID.AvalonVideoConfig);
            }

            #endregion

            // Pass it on
            base.OnClicked(controlId, control, actionType);
        }

        #endregion

        #region Public Methods

        internal static void SetProperty(string property, string value)
        {
            if (property == null)
                return;

            //// If the value is empty always add a space
            //// otherwise the property will keep 
            //// displaying it's previous value
            if (String.IsNullOrEmpty(value))
                value = " ";

            GUIPropertyManager.SetProperty(property, value);
        }

        public static DateTime NextAt(TimeSpan time)
        {
            DateTime now = DateTime.Now;
            DateTime result = now.Date + time;

            return (now <= result) ? result : result.AddDays(1);
        }

        public void checkUpdateOnTimer(object source)
        {
            Log.Debug("Avalon: Update timer fired - checking for update");
            if (updateCheck.updateAvailable(false))
            {
                AvalonGUIConfig.updateAvailable = true;

                // This property controls the visibility of the large update icon on the home screens
                AvalonGUIConfig.SetProperty("#Avalon.UpdateAvailable", "true");

                // This property controls the visibility or the indicator icon displyed next to the clock on the home screens
                AvalonGUIConfig.SetProperty("#Avalon.ShowUpdateInd", "true");

                // This property controls the visibility or the indicator icon that is displayed next to the skin item in the settings screens
                AvalonGUIConfig.SetProperty("#Avalon.ShowSettingsUpdateInd", "true");
            }
            else
                AvalonGUIConfig.updateAvailable = false;
        }

        public static TimeSpan nextCheckAt
        {
            get
            {
                switch (AvalonGUIConfig.checkInterval)
                {
                    case 1:
                        AvalonGUIConfig.hours = 24 * 7;
                        break;
                    case 2:
                        AvalonGUIConfig.hours = 24 * 14;
                        break;
                    case 3:
                        AvalonGUIConfig.hours = 24 * 28;
                        break;
                }
                TimeSpan hoursToAdd = TimeSpan.FromHours(AvalonGUIConfig.hours);

                DateTime nextCheck = NextAt(new TimeSpan(AvalonGUIConfig.checkTime.Hour, 0, 0).Add(hoursToAdd));
                Log.Info("Avalon: Next update check at : " + nextCheck.ToLongDateString() + ":" + nextCheck.ToLongTimeString());
                AvalonGUIConfig.nextUpdateCheck = nextCheck.ToString();
                settings.Save(settings.cXMLSectionUpdate);

                TimeSpan setTimerFor = (nextCheck - DateTime.Now).Add(hoursToAdd);
                Log.Info("Avalon: Timer Set for : " + setTimerFor.ToString());

                return setTimerFor;
            }
        }

        public static void checkAndCopy(string destinationPath)
        {
            String[] Files;
            Files = Directory.GetFileSystemEntries(destinationPath);
            foreach (string patchdir in Files)
            {
                if (patchdir.ToLower().Contains("language"))
                    copyDirectory(Path.Combine(destinationPath, "language"), SkinInfo.mpPaths.langBasePath);
                if (patchdir.ToLower().Contains("skin"))
                    copyDirectory(Path.Combine(destinationPath, "skin"), SkinInfo.mpPaths.skinBasePath);
                if (patchdir.ToLower().Contains("thumbs"))
                    copyDirectory(Path.Combine(destinationPath, "thumbs"), SkinInfo.mpPaths.thumbsPath);
                if (patchdir.ToLower().Contains("database"))
                    copyDirectory(Path.Combine(destinationPath, "database"), SkinInfo.mpPaths.databasePath);
            }
        }

        public static void copyDirectory(string patchSource, string patchDestination)
        {
            String[] patchFiles;
            //CheckSum checkSum = new CheckSum();

            if (patchDestination[patchDestination.Length - 1] != Path.DirectorySeparatorChar)
                patchDestination += Path.DirectorySeparatorChar;

            if (!Directory.Exists(patchDestination))
                Directory.CreateDirectory(patchDestination);

            patchFiles = Directory.GetFileSystemEntries(patchSource);

            foreach (string Element in patchFiles)
            {
                if (Directory.Exists(Element))
                    copyDirectory(Element, patchDestination + Path.GetFileName(Element));
                else
                {
                    //if (Path.GetExtension(patchDestination + Path.GetFileName(Element)).ToLower() == ".xml")        // checkSum on xmlrpc files only
                    //    if (checkSum.Compare(patchDestination + Path.GetFileName(Element)))                         // Only copy if checksum exists and matches
                    //    {
                    File.Copy(Element, patchDestination + Path.GetFileName(Element), true);
                    //        smcLog.WriteLog("Patching: Copy >" + Element + " to " + patchDestination + Path.GetFileName(Element), LogLevel.Debug);
                    //    }
                }
            }
        }

        public static void RunPendingRestartChanges()
        {
            if (PendingRestartChanges.Count > 0)
            {
                foreach (var pendingChange in PendingRestartChanges)
                {
                    switch (pendingChange)
                    {
                        case PendingChanges.ClearCache:
                            try
                            {
                                //smcLog.WriteLog("Clearing skin cache");
                                string path = Path.Combine(SkinInfo.mpPaths.cacheBasePath, "Avalon");
                                if (Directory.Exists(path)) Directory.Delete(path, true);
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error deleting skin cache: {0}", ex.Message);
                            }
                            break;
                    }
                }
                PendingRestartChanges.Clear();
            }
        }

        public static void RestartMediaPortal()
        {
            string restartExe = Path.Combine(SkinInfo.mpPaths.sMPbaseDir, "AvalonMediaPortalRestart.exe");
            ProcessStartInfo processStart = new ProcessStartInfo(restartExe);
            processStart.Arguments = @"/restartmp ";
            processStart.Arguments += ampSettings.mpSetAsFullScreen ? "true" : "false";
            processStart.Arguments += " \"" + Path.Combine(Path.Combine(SkinInfo.mpPaths.AvalonPath, "Media"), "splashscreen.png") + "\"";

            //smcLog.WriteLog("SMPMediaPortalRestart Parameter: " + processStart.Arguments, LogLevel.Info);

            processStart.WorkingDirectory = Path.GetDirectoryName(restartExe);
            System.Diagnostics.Process.Start(processStart);
            if (ampSettings.mpSetAsFullScreen)
                Thread.Sleep(2000);

            RunPendingRestartChanges();

            if (MinimiseOnExit)
                Environment.Exit(0);
            else
            {
                Action exitAction = new Action(Action.ActionType.ACTION_EXIT, 0, 0);
                GUIGraphicsContext.OnAction(exitAction);
            }
        }

        private void DeInitAvalon()
        {
            Log.Info("De-Initializing Avalon");

            GUIWindowManager.OnActivateWindow -= new GUIWindowManager.WindowActivationHandler(GUIWindowManager_OnActivateWindow);
        }

        public static void ShowRestartMessage(int windowID, string windowName)
        {
            GUIDialogYesNo dlg = (GUIDialogYesNo)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_YES_NO);
            dlg.Reset();
            dlg.SetHeading(windowName);
            dlg.SetLine(1, Translation.ConfigRequiresRestartLine1);
            dlg.SetLine(2, Translation.ConfigRequiresRestartLine2);
            dlg.SetLine(3, Translation.ConfigRequiresRestartLine3);
            dlg.DoModal(windowID);

            if (dlg.IsConfirmed)
            {
                RestartMediaPortal();
            }
        }

    /// <summary>
    /// Shows Virtual Keyboard
    /// </summary>
    /// <param name="input">Inital Text in Keyboard</param>
    /// <param name="output">Response from User</param>
    /// <returns>true if value has changed</returns>
    public static bool ShowKeyboard(string input, out string output)
    {
      output = input;

      try
      {
        VirtualKeyboard keyboard = (VirtualKeyboard)GUIWindowManager.GetWindow((int)Window.WINDOW_VIRTUAL_KEYBOARD);
        if (null == keyboard)
          return false;

        keyboard.Reset();
        keyboard.Text = input;        
        keyboard.DoModal(GUIWindowManager.ActiveWindow);

        if (keyboard.IsConfirmed)
        {
          output = keyboard.Text;          
          return !string.Equals(input, output, StringComparison.InvariantCultureIgnoreCase);
        }
        else
          return false;
      }
      catch (Exception ex)
      {
        Log.Info("Error showing virtual keyboard " + ex.Message);
        
          return false;
      }
    }


        #endregion

        void InitLabelRotate()
        {
            if (GUIWindowManager.ActiveWindow == 35)
            {
                string cReturn1 = GUIPropertyManager.GetProperty("#uplabel1");
                string cReturn2 = GUIPropertyManager.GetProperty("#uplabel2");

                if (cReturn1 == "yes")
                {
                    updateLabelRotate1();
                }
                if (cReturn2 == "yes")
                {
                    updateLabelRotate();
                }
            }
        }

        void GUIWindowManager_OnNewAction(Action action)
        {
            //Trakt Menu
            GUIWindow currentWindow = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow);
            GUIControl currentButton = null;

            switch (action.wID)
            {
                case Action.ActionType.ACTION_MOUSE_CLICK:
                case Action.ActionType.ACTION_KEY_PRESSED:
                    switch (GUIWindowManager.ActiveWindow)
                    {
                        case (int)96742:
                            currentButton = currentWindow.GetControl((int)66661);
                            if (currentButton != null && currentButton.IsFocused)
                            {
                                GUIControl.FocusControl((int)96742, 50);
                                ShowTraktMenu();
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        #region Private Methods

        private void updateLabelRotate()
        {
            mrTimer.Enabled = true;
            mrTimer.Interval = 5000;
            mrTimer.Tick += new EventHandler(mrTimer_Tick1);
        }

        private void updateLabelRotate1()
        {
            mrTimer.Enabled = true;
            mrTimer.Interval = 5000;
            mrTimer.Tick += new EventHandler(mrTimer_Tick2);
        }

        private void checkUpdateLabelRotate()
        {
            SetProperty("#uplabel1", "yes");
            SetProperty("#uplabel2", "");
            mrTimer.Enabled = false;
            mrTimer.Tick -= new EventHandler(mrTimer_Tick1);
            InitLabelRotate();
        }

        void mrTimer_Tick1(object sender, EventArgs e)
        {
            checkUpdateLabelRotate();
        }

        private void checkUpdateLabelRotate1()
        {
            SetProperty("#uplabel2", "yes");
            SetProperty("#uplabel1", "");
            mrTimer.Enabled = false;
            mrTimer.Tick -= new EventHandler(mrTimer_Tick2);
            InitLabelRotate();
        }

        void mrTimer_Tick2(object sender, EventArgs e)
        {
            checkUpdateLabelRotate1();
        }

        private void GUIWindowManager_OnActivateWindow(int windowID)
        {
            if (GUIWindowManager.ActiveWindow == (int)MediaPortalWindows.Home)
                return;

            if (GUIWindowManager.ActiveWindow == (int)MediaPortalWindows.BasicHome)
            {
                string cReturn1 = GUIPropertyManager.GetProperty("#Avalon.UpdateAvailable");

                SetProperty("#uplabel1", "yes");
                SetProperty("#uplabel2", "");
                if (cReturn1 == "true")
                {
                    InitLabelRotate();
                }
            }
        }

        private void GUIWindowManager_OnDeActivateWindow(int windowID)
        {
            // Settings/General window
            if (windowID == (int)Window.WINDOW_SETTINGS_SKIN)
            {
                // check if skin changed
                if (settings.CurrentSkin != settings.PreviousSkin && settings.CurrentSkin.EndsWith("Avalon"))
                {
                    Log.Info("Skin Changed to Avalon from GUI, initializing plugin.");
                    InitAvalon();
                    settings.PreviousSkin = settings.CurrentSkin;
                }
                else if (settings.CurrentSkin != settings.PreviousSkin && settings.PreviousSkin.EndsWith("Avalon"))
                {
                    Log.Info("Avalon is no longer default skin, de-initializing plugin.");
                    DeInitAvalon();
                    settings.PreviousSkin = settings.CurrentSkin;
                }

                //check if language changed
                if (settings.CurrentLanguage != settings.PreviousLanguage)
                {
                    Log.Info(string.Format("Language Changed to '{0}' from GUI, initializing translations.", settings.CurrentLanguage));
                    InitTranslations();
                }

            }

            // BasicHome
            if (GUIWindowManager.ActiveWindow == (int)MediaPortalWindows.BasicHome)
            {
                mrTimer.Enabled = false;
                mrTimer.Tick -= new EventHandler(mrTimer_Tick1);
                mrTimer.Tick -= new EventHandler(mrTimer_Tick2);
            }
        }

        private void InitAvalon()
        {
            #region Init Translations
            InitTranslations();
            #endregion

            #region Load Settings
            // we need to set properties on startup, load corresponding sections
            settings.Load(settings.cXMLSectionUpdate);
            settings.Load(settings.cXMLSectionTVSeries);
            settings.Load(settings.cXMLSectionMovingPictures);
            settings.Load(settings.cXMLSectionTV);
            settings.Load(settings.cXMLSectionMyFilms);
            settings.Load(settings.cXMLSectionMisc);
            settings.Load(settings.cXMLSectionVideo);

            //settings.LoadEditorProperties();
            #endregion

            #region PostPatchInstall
            if (AvalonGUIConfig.patchAppliedLastRun)
            {
                Log.Info("Avalon: Patch Applied, updating settings...");
                //PostPatchUpdate.UpdateSettings();
                //smcLog.WriteLog("Settings Updated", LogLevel.Info);

                AvalonGUIConfig.patchAppliedLastRun = false;
                settings.Save(settings.cXMLSectionUpdate);

                // Refresh GUI
                //smcLog.WriteLog("Refreshing GUI", LogLevel.Info);
                //GUIWindowManager.OnResize();
            }
            #endregion

            #region Init Updates
            // Should we check for an update on startup, if so check and set skin properties 
            // to control the visibility of the icons indicating and update is available?
            if (AvalonGUIConfig.checkOnStart)
            {
                if (updateCheck.updateAvailable(false))
                {
                    AvalonGUIConfig.updateAvailable = true;

                    // This property controls the visibility of the large update icon on the home screens
                    SetProperty("#Avalon.UpdateAvailable", "true");

                    // This property controls the visibility or the indicator icon displyed next to the clock on the home screens
                    SetProperty("#Avalon.ShowUpdateInd", "true");

                    // This property controls the visibility or the indicator icon that is displayed next to the skin item in the settings screens
                    SetProperty("#Avalon.ShowSettingsUpdateInd", "true");
                }
            }

            // Should we check for an update at a specific time
            if (AvalonGUIConfig.checkForUpdateAt)
            {
                DateTime stored;
                // Setup the callback
                TimerCallback updateCallback = new TimerCallback(checkUpdateOnTimer);

                // Compare the date strored (if any) with the current time
                try
                {
                    stored = Convert.ToDateTime(AvalonGUIConfig.nextUpdateCheck);
                }
                catch
                {
                    stored = DateTime.Now;
                }

                if (stored.CompareTo(DateTime.Now) > 0)
                {
                    //We have a save date for an update check that is in the future, we will use that.....
                    TimeSpan setStored = stored - DateTime.Now;
                    //smcLog.WriteLog("Stored Timer Set for : " + setStored.ToString(), LogLevel.Debug);
                    updateChkTimer = new System.Threading.Timer(updateCallback, null, setStored, TimeSpan.FromHours(AvalonGUIConfig.hours));
                }
                else
                {
                    updateChkTimer = new System.Threading.Timer(updateCallback, null, nextCheckAt, TimeSpan.FromHours(AvalonGUIConfig.hours));

                    // The line below is for testing, comment the above line and uncomment this - the update check will fire 30sec after startup
                    // updateChkTimer = new Timer(updateCallback, null, 30000, 60000);
                }
            }
            #endregion

            #region Init Misc
            MiscConfigGUI.SetProperties();
            #endregion

            #region Init Events
            GUIWindowManager.OnActivateWindow += new GUIWindowManager.WindowActivationHandler(GUIWindowManager_OnActivateWindow);
            #endregion  
        }

        private void InitTranslations()
        {
            Translation.Init();

            // Set all fixed translation properties 
            // these have defaults defined in the code and are used internally
            try
            {
                foreach (string name in Translation.Strings.Keys)
                {
                    if (!string.IsNullOrEmpty(Translation.Strings[name]))
                    {
                        Log.Info("#Avalon." + name + ": " + Translation.Strings[name]);
                        SetProperty("#Avalon." + name, Translation.Strings[name]);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("AvalonConfig: Translation Exception: {0}", ex.Message));
            }

            /*
            // Set user defined property, this is defined in the langauge file as the full 
            // property name including the the # i.e #Avalon.MyDefinedProperty
            foreach (string propName in Translation.FixedTranslations.Keys)
            {
                if (!string.IsNullOrEmpty(propName))
                {
                    string propValue;
                    Translation.FixedTranslations.TryGetValue(propName, out propValue);
                    if (IsInteger(propValue))
                    {
                        Log.Info(string.Format("Converting MediaPortal translation '{0}' to Avalon translation -> {1}", propValue, propName));
                        SetProperty(propName, GUILocalizeStrings.Get(int.Parse(propValue)));
                    }
                    else
                    {
                        Log.Info(propName + ": " + propValue);
                        SetProperty(propName, propValue);
                    }
                }
            }
        }

        bool IsInteger(string theValue)
        {
            if (string.IsNullOrEmpty(theValue)) return false;
            Match m = _isNumber.Match(theValue);
            return m.Success;
        */
        }

        private void ShowTraktRate()
        {
            TraktRateMovie rateMovie = new TraktRateMovie
            {
                IMDBID = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.imdb_id").Trim(),
                Title = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.title").Trim(),
                Year = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.year").Trim(),
                Rating = "love",
                UserName = TraktSettings.Username,
                Password = TraktSettings.Password

            };

            TraktPlugin.GUI.GUIUtils.ShowRateDialog<TraktRateMovie>(rateMovie);
        }

        private void ShowTraktShouts()
        {
            TraktPlugin.GUI.MovieShout movieInfo = new TraktPlugin.GUI.MovieShout
            {
                IMDbId = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.imdb_id").Trim(),
                Title = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.title").Trim(),
                Year = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.year").Trim()
            };
            TraktPlugin.GUI.GUIShouts.ShoutType = TraktPlugin.GUI.GUIShouts.ShoutTypeEnum.movie;
            TraktPlugin.GUI.GUIShouts.MovieInfo = movieInfo;

            TraktPlugin.GUI.GUIShouts.Fanart = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.backdropfullpath").Trim();
            GUIWindowManager.ActivateWindow(87280);
        }

        private void TraktAddToWatchlist()
        {
                string IMDB = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.imdb_id").Trim();
                string Title= GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.title").Trim();
                string Year = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.year").Trim();

                TraktPlugin.TraktHelper.AddMovieToWatchList(Title, Year, IMDB, true);
        }

        private void TraktAddToList()
        {
                string IMDBID = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.imdb_id").Trim();
                string Title = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.title").Trim();
                string Year = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.year").Trim();

                TraktHelper.AddRemoveMovieInUserList(Title, Year, IMDBID, false);
        }

        private void TraktShowRelatedMovies()
        {
            string IMDBID = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.imdb_id").Trim();
            string Title = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.title").Trim();
            string Year = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.year").Trim();

            TraktHelper.ShowRelatedMovies(IMDBID, Title, Year);
        }

        private void ShowTraktMenu()
        {
            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg == null) return;

            dlg.Reset();
            dlg.SetHeading("Trakt");

            GUIListItem listItem = null;

            // Display Movie Relavent Windows

            listItem = new GUIListItem(Translation.Trakt);
            dlg.Add(listItem);
            listItem.ItemId = (int)TraktMenuItems.Main;

            listItem = new GUIListItem(Translation.Recommendations);
            dlg.Add(listItem);
            listItem.ItemId = (int)TraktMenuItems.Recommendations;

            listItem = new GUIListItem(Translation.Trending);
            dlg.Add(listItem);
            listItem.ItemId = (int)TraktMenuItems.Trending;

            listItem = new GUIListItem(Translation.Watchlist);
            dlg.Add(listItem);
            listItem.ItemId = (int)TraktMenuItems.WatchList;

            listItem = new GUIListItem(Translation.AddToWatchList);
            dlg.Add(listItem);
            listItem.ItemId = (int)TraktMenuItems.AddToWatchList;

            listItem = new GUIListItem(Translation.AddToList);
            dlg.Add(listItem);
            listItem.ItemId = (int)TraktMenuItems.AddToList;

            listItem = new GUIListItem(Translation.RelatedMovies);
            dlg.Add(listItem);
            listItem.ItemId = (int)TraktMenuItems.RelatedMovies;
            
            listItem = new GUIListItem(Translation.Rate);
            dlg.Add(listItem);
            listItem.ItemId = (int)TraktMenuItems.Rate;

            listItem = new GUIListItem(Translation.Shouts);
            dlg.Add(listItem);
            listItem.ItemId = (int)TraktMenuItems.Shouts;

            // Show Context Menu
            dlg.DoModal(GUIWindowManager.ActiveWindow);
            if (dlg.SelectedId < 0) return;

            switch (dlg.SelectedId)
            {

                case ((int)TraktMenuItems.Main):
                    // Jump to Trakt Main Window
                    GUIWindowManager.ActivateWindow(87258);
                    break;

                case ((int)TraktMenuItems.Recommendations):
                    // Jump to Trakt Movie Recommendations Window
                    GUIWindowManager.ActivateWindow(87263);
                    break;

                case ((int)TraktMenuItems.Trending):
                    // Jump to Trakt Movie Trending Window
                    GUIWindowManager.ActivateWindow(87266);
                    break;

                case ((int)TraktMenuItems.WatchList):
                    // Jump to Trakt Movie Watch List Window
                    GUIWindowManager.ActivateWindow(87270);
                    break;

                case ((int)TraktMenuItems.AddToWatchList):
                    // Add to Watchlist
                    TraktAddToWatchlist();
                    break;

               case ((int)TraktMenuItems.AddToList):
                    // Add to Watchlist
                    TraktAddToList();
                    break;

               case ((int)TraktMenuItems.RelatedMovies):
                    // Rate window
                    TraktShowRelatedMovies();
                    break;

                case ((int)TraktMenuItems.Rate):
                    // Rate window
                    ShowTraktRate();
                    break;

                case ((int)TraktMenuItems.Shouts):
                    // Jump to Trakt Movie Shouts
                    ShowTraktShouts();
                    break;

                default:
                    break;
            }
        }
            
        #endregion

        #region ISetupForm Members
        /// <summary>
        /// With GetID it will be an window-plugin / otherwise a process-plugin
        /// Enter the id number here again
        /// </summary>
        public override int GetID
        {
            get { return GetWindowId(); }
            set { }
        }

        /// <summary>
        /// Returns the name of the plugin which is shown in the plugin menu
        /// </summary>
        /// <returns>the name of the plugin which is shown in the plugin menu</returns>
        public string PluginName()
        {
            return "Avalon Configuration";
        }

        /// <summary>
        /// Returns the description of the plugin which is shown in the plugin menu
        /// </summary>
        /// <returns>the description of the plugin which is shown in the plugin menu</returns>
        public string Description()
        {
            return "Avalon Skin control";
        }

        /// <summary>
        /// Returns the author of the plugin which is shown in the plugin menu
        /// </summary>
        /// <returns>the author of the plugin which is shown in the plugin menu</returns>
        public string Author()
        {
            return "ncoH";
        }

        /// <summary>
        /// Indicates whether plugin can be enabled/disabled
        /// </summary>
        public void ShowPlugin()
        {
            ConfigurationForm configurationForm = new ConfigurationForm();
            configurationForm.ShowDialog();
        }

        /// <summary>
        /// Indicates whether plugin can be enabled/disabled
        /// </summary>
        /// <returns>true if the plugin can be enabled/disabled</returns>
        public bool CanEnable()
        {
            return false;
        }

        /// <summary>
        /// Get Windows-ID
        /// </summary>
        /// <returns>unique id for this plugin</returns>
        public int GetWindowId()
        {
            // WindowID of windowplugin belonging to this setup
            // enter your own unique code
            return (int)155100;
        }

        /// <summary>
        /// Indicates if plugin is enabled by default
        /// </summary>
        /// <returns>true if this plugin is enabled by default</returns>
        public bool DefaultEnabled()
        {
            return true;
        }

        /// <summary>
        /// indicates if a plugin has its own setup screen
        /// </summary>
        /// <returns>true if the plugin has its own setup screen</returns>
        public bool HasSetup()
        {
            return true;
        }

        /// <summary>
        /// no Home for this plugin, return false
        /// </summary>
        /// <param name="strButtonText"></param>
        /// <param name="strButtonImage"></param>
        /// <param name="strButtonImageFocus"></param>
        /// <param name="strPictureImage"></param>
        /// <returns></returns>
        public bool GetHome(out string strButtonText, out string strButtonImage,
                            out string strButtonImageFocus, out string strPictureImage)
        {
            strButtonText = String.Empty;
            strButtonImage = String.Empty;
            strButtonImageFocus = String.Empty;
            strPictureImage = String.Empty;
            return false;
        }
        #endregion

    }
}