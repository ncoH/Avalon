using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading;


namespace AvalonPatch
{
    public partial class AvalonPatch : Form
    {

        #region Varibles

        List<patchFile> patchFiles = new List<patchFile>();

        public string tempExtractPath = Path.Combine(Path.GetTempPath(), "AvalonPatch" + DateTime.Now.Ticks.ToString());
        public XmlTextReader reader;

        bool unattendedInatall = false;
        bool restartMediaPortal = false;
        bool restartConfiguration = false;
        bool patchesToInstall = false;

        int i = 0;

        Version minSMPVersion = new Version();
        SplashScreen splash = new SplashScreen();
        SkinInfo skInfo = new SkinInfo();
        //CheckSum checkSum = new CheckSum();

        #endregion

        #region Public Methods

        public AvalonPatch()
        {
            InitializeComponent();

            if (SkinInfo.mpPaths.sMPbaseDir == null)
            {
                DialogResult result = MessageBox.Show("MediaPortal is not Installed on this system or Avalon is NOT configured as your default skin. - Please install MediaPortal/Avalon or set Avalon as your default skin before running this patch.",
                      "Patch Error............",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error);

                if (result == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                // Run unattended - This will run the program minimised and exit
                if (arg.ToLower().Contains("unattended") || arg.ToLower().StartsWith("/s"))
                {
                    unattendedInatall = true;
                    btInstallPatch.Enabled = false;
                    this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
                }
                else
                    this.WindowState = System.Windows.Forms.FormWindowState.Normal;

                // Do we restart MediaPortal after the update 
                if (arg.ToLower().Contains("restartmp"))
                {
                    restartMediaPortal = true;
                }
                // Do we restart Configuration after the update 
                if (arg.ToLower().Contains("restartconfiguration"))
                {
                    restartConfiguration = true;
                }
            }
        }

        #endregion

        #region Private Methods

        private void AvalonPatch_Load(object sender, EventArgs e)
        {
            //Ensure this is always on top
            //this.TopMost = true;

            //Display full screen Splash if in unattended mode.
            if (unattendedInatall)
            {
                Version patchVersion = new Version(Application.ProductVersion);
                splash.statusTextLine1 = "Applying Avalon Patch Revision (" + patchVersion.ToString() + ")";
                splash.statusTextLine2 = " ";
                splash.Show();
                splash.Refresh();

            }
            if (skInfo.configuredSkin != "Avalon")
            {
                MessageBox.Show("Sorry, the Avalon is configured as your default skin.\n\nThis patch updates Avalon only - Please install Avalon\n or set Avalon as your defult skin before running this patch.", "Patch Installation Error");
                Environment.Exit(0);
            }


            string processess = null;

            CheckProcesses mediaportal = new CheckProcesses("mediaportal");
            CheckProcesses configuration = new CheckProcesses("configuration");
            CheckProcesses avaloneditor = new CheckProcesses("avaloneditor");
            CheckProcesses pluginconfigloader = new CheckProcesses("pluginconfigloader");

            if (skInfo.minimiseMPOnExit.ToLower() == "yes")
            {
                mediaportal.kill();
                Thread.Sleep(3000);
            }

            introBox.SelectionStart = 0;

            int waitCount = 0;
            while (!checkRunningProcess())
            {
                // Check running process and wait 10sec for processes to exit
                if (!checkRunningProcess())
                    if (mediaportal.running)
                        processess = "MediaPortal\n";
                if (configuration.running)
                    processess += "Configuration.exe\n";
                if (avaloneditor.running)
                    processess += "Avalon BasicHome Editor\n";
                if (pluginconfigloader.running)
                    processess += "Plugin Config Loader\n";

                if (waitCount > 10)
                {
                    DialogResult result = MessageBox.Show("The Follow Process are Still Running\n\n" + processess + "\nPlease close application and Retry\n\nPressing Cancel will Abort the Upgrade Process",
                          "Retry",
                          MessageBoxButtons.RetryCancel,
                          MessageBoxIcon.Question,
                          MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Cancel)
                    {
                        Application.Exit();
                    }
                }
                else
                    Thread.Sleep(1000);

                processess = null;
                waitCount++;
            }

            // Create the temp directory to store the extracted patches
            Directory.CreateDirectory(tempExtractPath);
            readPatchControl();
            if (skInfo.skinVersion.CompareTo(minSMPVersion) < 0)
            {
                MessageBox.Show("The Installed Version of Avalon V" + skInfo.skinVersion + " does not support this patch.\n\n  Please install Avalon V" + minSMPVersion.ToString() + " or greater before applying this patch.\n\n                                  This program will now terminate", "Incompatible Skin Version");
                Application.Exit();
            }
            // First check if we have any patches to install - this could be being run on a system that is already patched
            foreach (patchFile patch in patchFiles)
            {
                if (thePatches.Items[i].ImageIndex != 0)
                    patchesToInstall = true;
                i++;
            }
            if (!patchesToInstall)
                btInstallPatch.Enabled = false;

            if (unattendedInatall)
            {
                if (patchesToInstall)
                {
                    installThePatches();
                    splash.statusTextLine1 = "Avalon Sucessfully Updated to Version : " + skInfo.skinVersion.ToString();
                    splash.Refresh();
                }
                else
                {
                    splash.statusTextLine1 = "Avalon is fully patched - no updates are required.";
                    splash.Refresh();
                }
                for (int i = 5; i > 0; i--)
                {
                    if (restartMediaPortal)
                        splash.statusTextLine2 = String.Format("Restarting MediaPortal ({0})", i);
                    else if (restartConfiguration)
                        splash.statusTextLine2 = String.Format("Restarting Configuration ({0})", i);
                    else
                    {
                        splash.Refresh();
                        Thread.Sleep(2000);
                        break;
                    }
                    splash.Refresh();
                    Thread.Sleep(1000);
                }
                exitAndCleanup();
            }
        }

        bool checkRunningProcess()
        {

            CheckProcesses mediaportal = new CheckProcesses("mediaportal");
            CheckProcesses configuration = new CheckProcesses("configuration");
            CheckProcesses avaloneditor = new CheckProcesses("avaloneditor");
            CheckProcesses pluginconfigloader = new CheckProcesses("pluginconfigloader");

            if (!mediaportal.running)
            {
                mediaportalStatus.ForeColor = Color.Green;
                mediaportalStatus.Text = "Shutdown";
            }
            else
                return false;

            if (!configuration.running)
            {
                configurationStatus.ForeColor = Color.Green;
                configurationStatus.Text = "Shutdown";
            }
            else
                return false;

            if (!avaloneditor.running)
            {
                avaloneditorStatus.ForeColor = Color.Green;
                avaloneditorStatus.Text = "Shutdown";
            }
            else
                return false;

            if (!pluginconfigloader.running)
            {
                configLoaderStatus.ForeColor = Color.Green;
                configLoaderStatus.Text = "Shutdown";
            }
            else
                return false;


            return true;
        }

        void readPatchControl()
        {
            // Extract the control file
            FileInfo outFile = new FileInfo(Path.Combine(tempExtractPath, "PatchControl.xml"));
            FileStream streamOutFile = outFile.OpenWrite();
            extractFile(GetEmbeddedFile("AvalonPatch", "EmbeddedPatches.PatchControl.xml"), streamOutFile);

            // Read the file
            string elementName = "";
            reader = new XmlTextReader(Path.Combine(tempExtractPath, "PatchControl.xml"));
            reader.MoveToContent();
            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "patches"))
            {
                while (reader.Read())
                {
                    // Get the minium skin version required
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "minskinversion")
                    {
                        reader.Read();
                        minSMPVersion = new Version(reader.Value);

                    }
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "patch")
                    {
                        patchFile thisPatch = new patchFile();
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                                elementName = reader.Name;
                            else
                            {
                                if ((reader.NodeType == XmlNodeType.Text) && (reader.HasValue))
                                {
                                    switch (elementName)
                                    {
                                        case "name":
                                            thisPatch.patchFileName = reader.Value;
                                            break;
                                        case "action":
                                            thisPatch.patchAction = reader.Value;
                                            break;
                                        case "location":
                                            thisPatch.patchLocation = reader.Value;
                                            break;
                                        case "version":
                                            thisPatch.patchVersion = reader.Value;
                                            break;
                                    }
                                }
                            }
                            if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                if (reader.Name == "patch")
                                {
                                    handlePatch(thisPatch.patchFileName, thisPatch);
                                    break;
                                }
                            }
                        }
                    }
                }
                reader.Close();
            }
        }

        void handlePatch(string patchName, patchFile pf)
        {
            FileInfo outFile = new FileInfo(Path.Combine(tempExtractPath, patchName));
            FileStream streamOutFile = outFile.OpenWrite();
            extractFile(GetEmbeddedFile("AvalonPatch", "EmbeddedPatches." + patchName), streamOutFile);
            // Now get the version and fill in the info
            fillInfo(patchName, pf);
        }

        void fillInfo(string fileName, patchFile pf)
        {
            if (pf.patchAction != "unzip")
            {
                pf.patchVersion = fileVersion(Path.Combine(tempExtractPath, fileName));
                pf.patchSize = fileSize(Path.Combine(tempExtractPath, fileName));
            }

            // if patch file is a plugin
            if (pf.patchLocation.ToLower().StartsWith("process") || pf.patchLocation.ToLower().StartsWith("windows"))
            {
                pf.destinationPath = Path.Combine(SkinInfo.mpPaths.pluginPath, pf.patchLocation);
                pf.installedVersion = fileVersion(Path.Combine(pf.destinationPath, fileName));
                pf.installedSize = fileSize(Path.Combine(pf.destinationPath, fileName));
            }
            if (pf.patchLocation.ToLower().StartsWith("mediaportal"))
            {
                pf.destinationPath = SkinInfo.mpPaths.sMPbaseDir;
                pf.installedVersion = fileVersion(Path.Combine(pf.destinationPath, fileName));
                pf.installedSize = fileSize(Path.Combine(pf.destinationPath, fileName));
            }
            if (pf.patchAction == "unzip")
                pf.installedVersion = skInfo.skinVersion.ToString();

            if (pf.installedVersion == "0.0.0.0" && pf.patchAction != "mandatoryinstall")
                return;

            patchFiles.Add(pf);
            ListViewItem item = new ListViewItem(new[] { pf.patchFileName, pf.patchVersion, pf.installedVersion });
            item.ImageIndex = 1;

            Version patchVersion = new Version(pf.patchVersion);
            Version installedVersion = new Version(pf.installedVersion);

            if (patchVersion.CompareTo(installedVersion) < 0)
            {
                item.ImageIndex = 0;
            }
            else if (patchVersion.CompareTo(installedVersion) == 0)
            {
                // check filesize...we cant do modified date as embedded resource
                // streams and writes a new file
                if (pf.patchSize.CompareTo(pf.installedSize) <= 0)
                    item.ImageIndex = 0;
                else
                    item.ImageIndex = 1;
            }
            else
                item.ImageIndex = 1;

            thePatches.Items.Add(item);
        }

        string fileVersion(string fileToCheck)
        {
            string version = "0.0.0.0";
            if (File.Exists(fileToCheck))
            {
                FileVersionInfo fv = FileVersionInfo.GetVersionInfo(fileToCheck);
                if (string.IsNullOrEmpty(fv.FileVersion))
                    version = "0.0.0.0";
                else
                    version = fv.FileVersion;
            }
            return version;
        }

        long fileSize(string fileToCheck)
        {
            long returnValue = 0;

            if (File.Exists(fileToCheck))
            {
                FileInfo f = new FileInfo(fileToCheck);
                returnValue = f.Length;
            }
            return returnValue;
        }

        private void AvalonPatch_FormClosing(object sender, FormClosingEventArgs e)
        {
            exitAndCleanup();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            exitAndCleanup();
        }

        void exitAndCleanup()
        {
            // Cleanup
            try
            {
                if (Directory.Exists(tempExtractPath))
                    Directory.Delete(tempExtractPath, true);
            }
            catch
            {
            }
            // Check and start Mediaportal if required
            if (restartMediaPortal)
            {
                ProcessStartInfo process = new ProcessStartInfo("mediaportal.exe");
                process.WorkingDirectory = SkinInfo.mpPaths.sMPbaseDir;
                process.UseShellExecute = true;
                Process.Start(process);
            }
            // Check and start Mediaportal if required
            if (restartConfiguration)
            {
                ProcessStartInfo process = new ProcessStartInfo("configuration.exe");
                process.WorkingDirectory = SkinInfo.mpPaths.sMPbaseDir;
                process.UseShellExecute = true;
                Process.Start(process);
            }
            if (skInfo.startFullScreen)
                Thread.Sleep(2000);
            Application.Exit();
        }

        private void btInstallPatch_Click(object sender, EventArgs e)
        {
            installThePatches();
        }

        void installThePatches()
        {
            if (patchesToInstall)
            {
                i = 0;
                patchProgressBar.Step = (100 / patchFiles.Count);
                patchProgressBar.Maximum = 100;
                foreach (patchFile patch in patchFiles)
                {
                    // Dont copy if the installed version is newer
                    if (thePatches.Items[i].ImageIndex != 0)
                        installPatch(patch);

                    if ((patchProgressBar.Value + patchProgressBar.Step) >= patchProgressBar.Maximum)
                        patchProgressBar.Value = 100;
                    else
                        patchProgressBar.Value += patchProgressBar.Step;

                    thePatches.Items[i].ImageIndex = 0;
                    i++;
                }
                btInstallPatch.Enabled = false;
                clearCacheDir();
            }
        }

        void installPatch(patchFile thePatch)
        {
            try
            {
                if (thePatch.patchAction.Contains("install"))
                {
                    if (thePatch.patchLocation.ToLower().StartsWith("process") || thePatch.patchLocation.ToLower().StartsWith("windows"))
                    {
                        File.Copy(Path.Combine(tempExtractPath, thePatch.patchFileName), Path.Combine(thePatch.destinationPath, thePatch.patchFileName), true);
                    }
                    if (thePatch.patchLocation.ToLower().StartsWith("mediaportal"))
                    {
                        File.Copy(Path.Combine(tempExtractPath, thePatch.patchFileName), Path.Combine(thePatch.destinationPath, thePatch.patchFileName), true);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // catch access issues...maybe from UAC
                // really need to elevate
                string message = "User Account Control maybe preventing Avalon from updating the required file: ";
                string message2 = "Its recommended you address the issue and run the patch again.";
                DialogResult result = MessageBox.Show(string.Format("{0}{1}.\n\n{2}\n\n{3}", message, thePatch.patchFileName, ex.Message, message2), "AvalonPatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // exit now, so we dont update version and prevent user from applying patch again
                Application.Exit();
            }
            catch (Exception e)
            {
                // This is to catch the issue where we are running the patch from SMPMediaPortalRestart
                // and cannot then update SMPMediaPortalRestart.
                // Check it is SMPMediaPortalRestart we are trying to update, continue if we are 
                // otherwise show the error and abort update process.
                if (!thePatch.patchFileName.ToLower().Contains("avalonmediaportalrestart"))
                {
                    string message = "Exception due update while updating:";
                    string message2 = "Its recommended you address the issue and run the patch again.";
                    DialogResult result = MessageBox.Show(string.Format("{0}{1}.\n\n{2}\n\n{3}", message, thePatch.patchFileName, e.Message, message2), "AvalonPatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // exit now, so we dont update version and prevent user from applying patch again
                    Application.Exit();
                }
            }

            if (thePatch.patchAction.ToLower() == "unzip")
            {
                installZip(thePatch);
            }

            if (thePatch.patchAction.ToLower() == "run")
            {
            }
        }


        void installZip(patchFile thePatch)
        {
            FastZip fz = new FastZip();
            fz.ExtractZip(Path.Combine(tempExtractPath, thePatch.patchFileName), Path.Combine(tempExtractPath, Path.GetFileNameWithoutExtension(thePatch.patchFileName)), "");
            checkAndCopy(Path.Combine(tempExtractPath, Path.GetFileNameWithoutExtension(thePatch.patchFileName)));
        }

        void checkAndCopy(string destinationPath)
        {
            String[] Files;
            Files = Directory.GetFileSystemEntries(destinationPath);
            foreach (string patchdir in Files)
            {
                if (patchdir.ToLower().EndsWith("language"))
                    copyDirectory(Path.Combine(destinationPath, "language"), SkinInfo.mpPaths.langBasePath);
                if (patchdir.ToLower().EndsWith("skin"))
                    copyDirectory(Path.Combine(destinationPath, "skin"), SkinInfo.mpPaths.skinBasePath);
                if (patchdir.ToLower().EndsWith("thumbs"))
                    copyDirectory(Path.Combine(destinationPath, "thumbs"), SkinInfo.mpPaths.thumbsPath);
                if (patchdir.ToLower().EndsWith("database"))
                    copyDirectory(Path.Combine(destinationPath, "database"), SkinInfo.mpPaths.databasePath);
            }
            if (patchProgressBar.Value <= 90)
                patchProgressBar.Value += 10;
            else
                patchProgressBar.Value = 100;
        }

        void copyDirectory(string patchSource, string patchDestination)
        {
            String[] patchFiles;

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
                    // If we are replacing an xml, check if the exiting files checksum matches (or it does not have one) and only then copy the file.
                    // we only want to do checksum compares on Avalon specific skin files
                    if (Path.GetExtension(Element).ToLower() == ".xml" && patchDestination.StartsWith(SkinInfo.mpPaths.AvalonPath))
                    {
                        string destFilename = Path.Combine(patchDestination, Path.GetFileName(Element));

                        // check if file exists and is a skin file
                        try
                        {
                            //if (checkSum.Compare(destFilename))
                                File.Copy(Element, destFilename, true);
                        }
                        catch (FileNotFoundException)
                        {
                            // most likely a new file, copy anyway
                            File.Copy(Element, destFilename, true);
                        }

                        // replace checksum so we can patch next time
                        //checkSum.Replace(destFilename);
                    }
                    else
                        File.Copy(Element, patchDestination + Path.GetFileName(Element), true);
                }
            }
        }

        private void clearCacheDir()
        {
            try
            {
                System.IO.Directory.Delete(Path.Combine(SkinInfo.mpPaths.cacheBasePath, "Avalon"), true);
                //showError("Skin cache has been cleared\n\nOk To Continue", errorCode.info);
            }
            catch
            {
                //showError("Exception while deleteing Cache\n\n" + ex.Message, errorCode.info);
            }
        }

        #endregion

        #region Extract Patches

        /// <summary>
        /// Extracts an embedded file out of a given assembly.
        /// </summary>
        /// <param name="assemblyName">The namespace of you assembly.</param>
        /// <param name="fileName">The name of the file to extract.</param>
        /// <returns>A stream containing the file data.</returns>
        public static Stream GetEmbeddedFile(string assemblyName, string fileName)
        {
            try
            {
                //System.Reflection.Assembly a = System.Reflection.Assembly.Load(assemblyName);
                System.Reflection.Assembly a = System.Reflection.Assembly.GetCallingAssembly();
                Stream str = a.GetManifestResourceStream(assemblyName + "." + fileName);

                if (str == null)
                    throw new Exception("Could not locate embedded resource '" + fileName + "' in assembly '" + assemblyName + "'");
                return str;
            }
            catch (Exception e)
            {
                throw new Exception(assemblyName + ": " + e.Message);
            }
        }

        #region Overloads

        public static Stream GetEmbeddedFile(System.Reflection.Assembly assembly, string fileName)
        {
            string assemblyName = assembly.GetName().Name;
            return GetEmbeddedFile(assemblyName, fileName);
        }

        public static Stream GetEmbeddedFile(Type type, string fileName)
        {
            string assemblyName = type.Assembly.GetName().Name;
            return GetEmbeddedFile(assemblyName, fileName);
        }

        #endregion

        void extractFile(Stream inFile, Stream outFile)
        {
            const int size = 4096;
            byte[] bytes = new byte[4096];
            int numBytes;
            while ((numBytes = inFile.Read(bytes, 0, size)) > 0)
            {
                outFile.Write(bytes, 0, numBytes);
            }
            outFile.Close();
            inFile.Close();
        }

        #endregion

        #region Class Defines

        public class patchFile
        {
            public string patchFileName { get; set; }
            public string patchVersion { get; set; }
            public long patchSize { get; set; }
            public string patchAction { get; set; }
            public string patchLocation { get; set; }
            public string installedVersion { get; set; }
            public long installedSize { get; set; }
            public string destinationPath { get; set; }
        }

        #endregion

    }
}
