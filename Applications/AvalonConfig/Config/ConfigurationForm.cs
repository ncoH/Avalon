using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;


namespace AvalonGUIConfig
{
    partial class ConfigurationForm : Form
    {
        #region Variables
        // Private Variables
        private DateTimePicker timePicker;
        SkinInfo skInfo = new SkinInfo();

        // Protected Variables
        // Public Variables
        #endregion

        #region Public methods

        public ConfigurationForm()
        {
            InitializeComponent();

            settings.Load(settings.cXMLSectionUpdate);
            //settings.Load(settings.cXMLSectionMisc);

            timePicker = new DateTimePicker();
            timePicker.Format = DateTimePickerFormat.Time;
            timePicker.ShowUpDown = true;
            timePicker.Location = new Point(300, 53);
            timePicker.Width = 100;
            CheckUpdate.Controls.Add(timePicker);

        }

        #endregion

        #region Private methods

        // Save settings to file
        void btSave_Click(object sender, EventArgs e)
        {
            // get current settings and save to file
            AvalonGUIConfig.checkOnStart = cbCheckOnStart.Checked;
            AvalonGUIConfig.checkForUpdateAt = cbCheckForUpdateAt.Checked;
            AvalonGUIConfig.checkInterval = comboCheckInterval.SelectedIndex;
            AvalonGUIConfig.checkTime = timePicker.Value;
            AvalonGUIConfig.nextUpdateCheck = AvalonGUIConfig.nextCheckAt.ToString();
            AvalonGUIConfig.patchUtilityRunUnattended = cbRunUnattended.Checked;
            AvalonGUIConfig.patchUtilityRestartMP = cbRestartMP.Checked;

            settings.Save(settings.cXMLSectionUpdate);

            this.Close();
        }

        void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Load settings from xml
        void ConfigurationForm_Load(object sender, EventArgs e)
        {

            cbRunUnattended.Checked = AvalonGUIConfig.patchUtilityRunUnattended;
            cbRestartMP.Checked = AvalonGUIConfig.patchUtilityRestartMP;

            cbCheckOnStart.Checked = AvalonGUIConfig.checkOnStart;
            cbCheckForUpdateAt.Checked = AvalonGUIConfig.checkForUpdateAt;
            if (AvalonGUIConfig.checkForUpdateAt)
            {
                cbCheckForUpdateAt.Checked = AvalonGUIConfig.checkForUpdateAt;
                comboCheckInterval.SelectedIndex = AvalonGUIConfig.checkInterval;
                timePicker.Value = AvalonGUIConfig.checkTime;
            }
        }

        private static DateTime getLinkerTimeStamp(string filePath)
        {
            const int PeHeaderOffset = 60;
            const int LinkerTimestampOffset = 8;

            byte[] b = new byte[2047];
            using (System.IO.Stream s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                s.Read(b, 0, 2047);
            }
            int secondsSince1970 = BitConverter.ToInt32(b, BitConverter.ToInt32(b, PeHeaderOffset) + LinkerTimestampOffset);
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(secondsSince1970);
        }

        void btCheckForUpdate_Click(object sender, EventArgs e)
        {
            updateCheck.patchList.Clear();
            if (updateCheck.updateAvailable(true))
                updateFound.displayDetail(true);
            else
                MessageBox.Show("No patches are currently available");
        }

        #endregion


    }
}