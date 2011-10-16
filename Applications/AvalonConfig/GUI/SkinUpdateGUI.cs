using System;
using System.Collections;
using System.IO;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;
using MediaPortal.Dialogs;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;
using System.Diagnostics;

namespace AvalonGUIConfig
{
    public class SkinUpdateGUI : GUIWindow
    {
        #region Skin Connection

        private enum GUIControls
        {
            UpdateChangeLog = 2,
            DoUpdate = 3
        }

        [SkinControl((int)GUIControls.UpdateChangeLog)]
        protected GUITextControl cmc_ChangeLog = null;
        [SkinControl((int)GUIControls.DoUpdate)]
        protected GUIButtonControl btDoUpdate = null;

        #endregion

        #region Local Variables

        private static Stream strResponse;
        private static Stream strLocal;
        private static HttpWebRequest webRequest;
        private static HttpWebResponse webResponse;
        private static int PercentProgress;

        private static string optionDownloadURL = null;
        private static string optionDownloadPath = null;
        private static string destinationPath = null;

        private static bool updateCancelled = false;

        //private static readonly logger smcLog = logger.GetInstance();
        SkinInfo skInfo = new SkinInfo();


        #endregion

        #region Private methods

        void installUpdateGUI()
        {
            SkinUpdateGUI skinUpdate = new SkinUpdateGUI();

            destinationPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(optionDownloadURL) + DateTime.Now.Ticks.ToString());

            GUIDialogProgress progressDialog = (GUIDialogProgress)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_PROGRESS);
            progressDialog.Reset();
            progressDialog.DisplayProgressBar = true;
            progressDialog.ShowWaitCursor = false;
            progressDialog.SetHeading(Translation.SkinSettingsUpdate);
            progressDialog.Percentage = 0;
            progressDialog.SetLine(1, string.Empty);
            progressDialog.SetLine(2, string.Empty);
            progressDialog.StartModal(skinUpdate.GetID);
            GUIWindowManager.Process();

            // sort the list of patches with the oldest first - this is the order they will be insalled in
            updateCheck.patchList.Sort(delegate(updateCheck.patches p1, updateCheck.patches p2) { return p1.patchVersion.CompareTo(p2.patchVersion); });

            foreach (updateCheck.patches thePatch in updateCheck.patchList)
            {
                optionDownloadURL = thePatch.patchURL;
                optionDownloadPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(thePatch.patchURL));
                progressDialog.SetLine(1, string.Format(Translation.DownloadingPatch, thePatch.patchVersion.ToString()));
                Log.Debug("Avalon: Starting Patch Installation of Patch : " + optionDownloadURL + "....Download");

                using (WebClient wcDownload = new WebClient())
                {
                    try
                    {
                        int upd = 1;
                        int bytesSize = 0;
                        byte[] downBuffer = new byte[2048];

                        webRequest = (HttpWebRequest)WebRequest.Create(optionDownloadURL);
                        webRequest.Credentials = CredentialCache.DefaultCredentials;
                        webResponse = (HttpWebResponse)webRequest.GetResponse();
                        Int64 fileSize = webResponse.ContentLength;
                        strResponse = wcDownload.OpenRead(optionDownloadURL);
                        strLocal = new FileStream(optionDownloadPath, FileMode.Create, FileAccess.Write, FileShare.None);
                        while ((bytesSize = strResponse.Read(downBuffer, 0, downBuffer.Length)) > 0)
                        {
                            strLocal.Write(downBuffer, 0, bytesSize);
                            PercentProgress = Convert.ToInt32((strLocal.Length * 100) / fileSize);
                            progressDialog.Percentage = PercentProgress;
                            progressDialog.SetLine(2, string.Format(Translation.DownloadingProgress, (strLocal.Length / 1024).ToString(), (fileSize / 1024).ToString(), PercentProgress.ToString()));
                            // Only Update the progress bar every 1MB of downloaded data,
                            // any more offen slows the download to much.
                            if (upd > 50)
                            {
                                GUIWindowManager.Process();
                                upd = 0;
                                if (progressDialog.IsCanceled)
                                {
                                    updateCancelled = true;
                                    break;
                                }
                            }
                            ++upd;
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error("Error in Download" + e.Message);
                    }
                    finally
                    {
                        Log.Info("Avalon: Patch Download Complete");
                        webResponse.Close();
                        strResponse.Close();
                        strLocal.Close();
                        if (System.IO.File.Exists(optionDownloadPath) && !progressDialog.IsCanceled)
                        {
                            if (Path.GetExtension(optionDownloadPath).ToLower() != ".zip")
                            {
                                if (Path.GetFileNameWithoutExtension(optionDownloadPath).ToLower().StartsWith("avalonpatch"))
                                {
                                    //Lets run it
                                    if (File.Exists(optionDownloadPath))
                                    {
                                        string restartExe = Path.Combine(SkinInfo.mpPaths.sMPbaseDir, "AvalonMediaPortalRestart.exe");
                                        ProcessStartInfo upgradeProcess = new ProcessStartInfo(restartExe);
                                        upgradeProcess.UseShellExecute = false;
                                        upgradeProcess.WorkingDirectory = Path.GetDirectoryName(restartExe);
                                        upgradeProcess.Arguments = string.Concat("/upgrade \"", optionDownloadPath, "\"");

                                        if (AvalonGUIConfig.patchUtilityRunUnattended)
                                        {
                                            upgradeProcess.Arguments += " /unattended";
                                        }

                                        if (AvalonGUIConfig.patchUtilityRestartMP)
                                        {
                                            upgradeProcess.Arguments += " /restartmp ";
                                            upgradeProcess.Arguments += AvalonGUIConfig.ampSettings.mpSetAsFullScreen ? "true" : "false";
                                            upgradeProcess.Arguments += " \"" + Path.Combine(Path.Combine(SkinInfo.mpPaths.AvalonPath, "Media"), "splashscreen.png") + "\"";
                                        }

                                        // signal to next run that we have applied update and settings need updating
                                        AvalonGUIConfig.patchAppliedLastRun = true;
                                        settings.Save(settings.cXMLSectionUpdate);

                                        Log.Debug("Avalon: Starting Upgrade process, Arguments : " + upgradeProcess.Arguments);
                                        System.Diagnostics.Process.Start(upgradeProcess);

                                        Log.Debug("Avalon: Upgrade Process Started....");
                                        Action exitAction = new Action(Action.ActionType.ACTION_EXIT, 0, 0);

                                        Log.Debug("Calling exit Action....");
                                        if (skInfo.minimiseMPOnExit == "yes")
                                        {
                                            // This is not a nice way of closing down MP
                                            GUIGraphicsContext.CurrentState = GUIGraphicsContext.State.STOPPING;
                                            Environment.Exit(0);
                                        }
                                        else
                                            GUIGraphicsContext.OnAction(exitAction);
                                    }
                                }
                                else
                                {
                                    // Not a zip so can't process internally - download to desktop and set flag so we can inform the user to exit MP and manually install the update
                                    System.IO.File.Copy(optionDownloadPath, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.GetFileName(optionDownloadPath)), true);
                                    System.IO.File.Delete(optionDownloadPath);
                                    AvalonGUIConfig.manualInstallNeeded = true;
                                }
                            }
                            else
                            {
                                FastZip fz = new FastZip();
                                fz.ExtractZip(optionDownloadPath, destinationPath, "");
                                System.IO.File.Delete(optionDownloadPath);
                                AvalonGUIConfig.manualInstallNeeded = false;
                                AvalonGUIConfig.updateAvailable = false;
                                // Now check what we have and copy to the right places.....
                                AvalonGUIConfig.checkAndCopy(destinationPath);
                                Directory.Delete(destinationPath, true);
                            }
                        }
                    }
                }
            }
            progressDialog.Close();
        }


        #endregion

        #region Public methods

        public SkinUpdateGUI()
        {
        }

        #endregion

        #region Base overrides

        public override int GetID
        {
            get
            {
                return (int)AvalonGUIConfig.AvalonScreenID.AvalonSkinUpdate;
            }
            set
            {
            }
        }

        public override bool Init()
        {
            return Load(GUIGraphicsContext.Skin + @"\AvalonConfig_update.xml");
        }

        protected override void OnPageLoad()
        {
            cmc_ChangeLog.Visible = false;
            btDoUpdate.Visible = false;

            if (updateCheck.updateAvailable(false))
            {
                btDoUpdate.Label = Translation.UpdateInstall;
                updateFound.downloadChangeLog(false);
                try
                {
                    System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();
                    string s = File.ReadAllText(Path.Combine(Path.GetTempPath(), "ChangeLog.rtf"));
                    rtBox.Rtf = s;

                    Log.Debug("Change Log: \\n" + rtBox.Text);

                    cmc_ChangeLog.Label = rtBox.Text;
                    cmc_ChangeLog.Visible = true;
                    btDoUpdate.Visible = true;
                    GUIPropertyManager.SetProperty("#Avalon.Revisions", AvalonGUIConfig.theRevisions);
                }
                catch (Exception ex)
                {
                    Log.Error("Exception Generating Change Log: " + ex.Message + "\\n" + ex.StackTrace);
                }
            }
            else
            {
                GUIDialogOK dlg = (GUIDialogOK)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_OK);
                dlg.Reset();
                dlg.SetHeading(Translation.SkinUpdate);
                dlg.SetLine(1, Translation.NoUpdatesAvailable);
                dlg.DoModal(GUIWindowManager.ActiveWindow);
                GUIWindowManager.ShowPreviousWindow();
            }
        }

        protected override void OnClicked(int controlId, GUIControl control, MediaPortal.GUI.Library.Action.ActionType actionType)
        {
            if (control == btDoUpdate)
            {
                //install the patch(s)
                installUpdateGUI();
                // tell the user what has been done
                cmc_ChangeLog.Visible = false;
                btDoUpdate.Visible = false;
                if (AvalonGUIConfig.manualInstallNeeded)
                {
                    GUIDialogOK dlgDone = (GUIDialogOK)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_OK);
                    dlgDone.SetHeading(Translation.mupdateheader);
                    dlgDone.SetLine(1, Translation.mupdateline1);
                    dlgDone.SetLine(2, Translation.mupdateline2);
                    dlgDone.SetLine(3, string.Format(Translation.mupdateline3, Path.GetFileName(optionDownloadPath)));
                    dlgDone.SetLine(4, Translation.mupdateline4);
                    dlgDone.DoModal(GUIWindowManager.ActiveWindow);
                }
                else
                {
                    if (!updateCancelled)
                    {
                        GUIDialogOK dlgDone = (GUIDialogOK)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_OK);
                        dlgDone.SetHeading(Translation.SkinUpdate);
                        dlgDone.SetLine(1, string.Format(Translation.NumPatchesInstalled, updateCheck.patchList.Count.ToString()));
                        dlgDone.SetLine(2, String.Empty);
                        dlgDone.SetLine(3, string.Format(Translation.PatchUpdateComplete, updateCheck.SkinVersion()));
                        dlgDone.DoModal(GUIWindowManager.ActiveWindow);
                    }
                }
                GUIWindowManager.ShowPreviousWindow();
                AvalonGUIConfig.updateAvailable = false;
            }
        }

        #endregion
    }
}
