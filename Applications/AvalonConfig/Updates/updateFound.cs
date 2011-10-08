using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using System.Reflection;

namespace AvalonGUIConfig
{
    class updateFound
    {
        #region Variables

        // Private Variables
        private static Form changlogForm = new Form();
        private static RichTextBox changeLog = new RichTextBox();
        private static Button okButton = new Button();
        private static Button cancelButton = new Button();
        // Protected Variables
        // Public Variables
        [DllImport("user32.dll", EntryPoint = "HideCaret")]
        public static extern long HideCaret(IntPtr hwnd);

        //private static readonly logger smcLog = logger.GetInstance();


        #endregion

        #region Public methods

        public static void displayDetail(bool calledFromConfig)
        {
            changlogForm.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            changlogForm.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            changlogForm.Name = "frmDownloadDetail";
            changlogForm.ControlBox = true;
            changlogForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            changlogForm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            changlogForm.StartPosition = FormStartPosition.CenterScreen;
            changlogForm.Text = "Avalon Skin Update - Change log";
            changlogForm.Width = 600;
            changlogForm.Height = 300;
            changlogForm.MaximizeBox = false;
            changlogForm.MinimizeBox = false;
            changlogForm.TopMost = true;

            changeLog.Name = "tboxChangeLog";
            changeLog.Location = new System.Drawing.Point(10, 10);
            changeLog.Width = 580;
            changeLog.Height = 200;
            changeLog.Multiline = true;
            changeLog.WordWrap = true;
            changeLog.ReadOnly = true;

            downloadChangeLog(calledFromConfig);
            if (File.Exists(Path.Combine(Path.GetTempPath(), "ChangeLog.rtf")))
                changeLog.LoadFile(Path.Combine(Path.GetTempPath(), "ChangeLog.rtf"), RichTextBoxStreamType.RichText);
            else
                changeLog.Text = "\n          Error During ChangeLog Download\n\n          You can chose to try and install the patch from here or alternatively download the patch from the StreamedMP\n          Site and try a manual update.";

            changeLog.ScrollBars = RichTextBoxScrollBars.Vertical;
            changeLog.SelectionStart = 0;

            changlogForm.Controls.Add(changeLog);

            okButton.Text = "Install Update";
            okButton.Width = 120;
            okButton.Location = new System.Drawing.Point(20, 240);
            okButton.Click += new System.EventHandler(okButton_Click);
            okButton.Focus();
            changlogForm.Controls.Add(okButton);

            cancelButton.Text = "Cancel";
            cancelButton.Width = 120;
            cancelButton.Location = new System.Drawing.Point(460, 240);
            cancelButton.Click += new System.EventHandler(cancelButton_Click);
            changlogForm.Controls.Add(cancelButton);

            changlogForm.Show();
            HideCaret(changlogForm.Handle);
        }

        public static void downloadChangeLog(bool calledFromConfig)
        {
            // sort the list of patches with the newest first, this correctly display the change log
            updateCheck.patchList.Sort(delegate(updateCheck.patches p1, updateCheck.patches p2) { return p2.patchVersion.CompareTo(p1.patchVersion); });

            RichTextBox richTextBoxInput = new RichTextBox();
            RichTextBox richTextBoxOutput = new RichTextBox();
            //
            // Download the change logs
            //
            foreach (updateCheck.patches thePatch in updateCheck.patchList)
            {
                if (thePatch.patchChangeLog.StartsWith("C:\\"))
                {
                    System.IO.File.Copy(thePatch.patchChangeLog, Path.Combine(Path.GetTempPath(), "ChangeLog.rtf"), true);
                    return;
                }

                WebClient client = new WebClient();
                try
                {
                    client.DownloadFile(thePatch.patchChangeLog, Path.Combine(Path.GetTempPath(), "ChangeLog-" + thePatch.patchVersion.MinorRevision.ToString() + ".rtf"));
                }
                catch (Exception e)
                {
                    if (calledFromConfig)
                        MessageBox.Show("Unable to access ChangeLog-" + thePatch.patchVersion.MinorRevision.ToString() + ".rtf\n\n" + e.Message, "ChangeLog access issue");

                    //smcLog.WriteLog("Unable to access ChangeLog-" + thePatch.patchVersion.MinorRevision.ToString() + ".rtf > " + e.Message, LogLevel.Error);
                    return;
                }
               // smcLog.WriteLog("Downloaded File : " + Path.Combine(Path.GetTempPath(), "ChangeLog-" + thePatch.patchVersion.MinorRevision.ToString() + ".rtf"), LogLevel.Info);
            }
            //
            // And combine them into a single change log
            //
            if (File.Exists(Path.Combine(Path.GetTempPath(), "ChangeLog.rtf")))
                File.Delete(Path.Combine(Path.GetTempPath(), "ChangeLog.rtf"));

            //smcLog.WriteLog("Processing Change Log...", LogLevel.Info);

            try
            {
                // Add each file and save as ChangeLog.rtf
                int patchCnt = 1;
                AvalonGUIConfig.theRevisions = "Patch: ";
                foreach (updateCheck.patches thePatch in updateCheck.patchList)
                {
                    richTextBoxInput.LoadFile(Path.Combine(Path.GetTempPath(), "ChangeLog-" + thePatch.patchVersion.MinorRevision.ToString() + ".rtf"));
                    richTextBoxInput.SelectAll();
                    richTextBoxInput.Copy();
                    richTextBoxOutput.Paste();
                    System.IO.File.Delete(Path.Combine(Path.GetTempPath(), "ChangeLog-" + thePatch.patchVersion.MinorRevision.ToString() + ".rtf"));
                    if (patchCnt < updateCheck.patchList.Count)
                        AvalonGUIConfig.theRevisions = AvalonGUIConfig.theRevisions + thePatch.patchVersion.ToString() + " / ";
                    else
                        AvalonGUIConfig.theRevisions = AvalonGUIConfig.theRevisions + thePatch.patchVersion.ToString();
                    patchCnt++;
                }
                richTextBoxOutput.SaveFile(Path.Combine(Path.GetTempPath(), "ChangeLog.rtf"));
                richTextBoxInput.Dispose();
                richTextBoxOutput.Dispose();
            }
            catch (Exception ex)
            {
               Log.Error("Exception Reading Change Logs: " + ex.Message + "\\n" + ex.StackTrace);
            }
        }


        #endregion

        #region Private methods

        private static void okButton_Click(object sender, EventArgs e)
        {
            updateCheck.installUpdate();
            changlogForm.Hide();
            AvalonGUIConfig.updateAvailable = false;
        }

        private static void cancelButton_Click(object sender, EventArgs e)
        {
            changlogForm.Hide();
        }

        #endregion
    }
}
