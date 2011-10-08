namespace AvalonGUIConfig
{
    partial class ConfigurationForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.CheckUpdate = new System.Windows.Forms.TabPage();
            this.cbRestartMP = new System.Windows.Forms.CheckBox();
            this.cbRunUnattended = new System.Windows.Forms.CheckBox();
            this.btCheckForUpdate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboCheckInterval = new System.Windows.Forms.ComboBox();
            this.cbCheckForUpdateAt = new System.Windows.Forms.CheckBox();
            this.cbCheckOnStart = new System.Windows.Forms.CheckBox();
            this.btSave = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.CheckUpdate.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.CheckUpdate);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(415, 185);
            this.tabControl1.TabIndex = 0;
            // 
            // CheckUpdate
            // 
            this.CheckUpdate.Controls.Add(this.cbRestartMP);
            this.CheckUpdate.Controls.Add(this.cbRunUnattended);
            this.CheckUpdate.Controls.Add(this.btCheckForUpdate);
            this.CheckUpdate.Controls.Add(this.label2);
            this.CheckUpdate.Controls.Add(this.comboCheckInterval);
            this.CheckUpdate.Controls.Add(this.cbCheckForUpdateAt);
            this.CheckUpdate.Controls.Add(this.cbCheckOnStart);
            this.CheckUpdate.Location = new System.Drawing.Point(4, 22);
            this.CheckUpdate.Name = "CheckUpdate";
            this.CheckUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.CheckUpdate.Size = new System.Drawing.Size(407, 159);
            this.CheckUpdate.TabIndex = 0;
            this.CheckUpdate.Text = "Skin Update";
            this.CheckUpdate.UseVisualStyleBackColor = true;
            // 
            // cbRestartMP
            // 
            this.cbRestartMP.AutoSize = true;
            this.cbRestartMP.Location = new System.Drawing.Point(8, 99);
            this.cbRestartMP.Name = "cbRestartMP";
            this.cbRestartMP.Size = new System.Drawing.Size(186, 17);
            this.cbRestartMP.TabIndex = 1;
            this.cbRestartMP.Text = "Restart MediaPortal/Configuration";
            this.cbRestartMP.UseVisualStyleBackColor = true;
            // 
            // cbRunUnattended
            // 
            this.cbRunUnattended.AutoSize = true;
            this.cbRunUnattended.Location = new System.Drawing.Point(8, 76);
            this.cbRunUnattended.Name = "cbRunUnattended";
            this.cbRunUnattended.Size = new System.Drawing.Size(172, 17);
            this.cbRunUnattended.TabIndex = 5;
            this.cbRunUnattended.Text = "Run Patch Update unattended";
            this.cbRunUnattended.UseVisualStyleBackColor = true;
            // 
            // btCheckForUpdate
            // 
            this.btCheckForUpdate.Location = new System.Drawing.Point(6, 122);
            this.btCheckForUpdate.Name = "btCheckForUpdate";
            this.btCheckForUpdate.Size = new System.Drawing.Size(158, 23);
            this.btCheckForUpdate.TabIndex = 4;
            this.btCheckForUpdate.Text = "Check for Skin Update Now";
            this.btCheckForUpdate.UseVisualStyleBackColor = true;
            this.btCheckForUpdate.Click += new System.EventHandler(this.btCheckForUpdate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(260, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Time";
            // 
            // comboCheckInterval
            // 
            this.comboCheckInterval.FormattingEnabled = true;
            this.comboCheckInterval.Items.AddRange(new object[] {
            "Every Day",
            "Every Week",
            "Every 2 Weeks",
            "Every 4 Weeks"});
            this.comboCheckInterval.Location = new System.Drawing.Point(124, 53);
            this.comboCheckInterval.Name = "comboCheckInterval";
            this.comboCheckInterval.Size = new System.Drawing.Size(121, 21);
            this.comboCheckInterval.TabIndex = 2;
            // 
            // cbCheckForUpdateAt
            // 
            this.cbCheckForUpdateAt.AutoSize = true;
            this.cbCheckForUpdateAt.Location = new System.Drawing.Point(8, 53);
            this.cbCheckForUpdateAt.Name = "cbCheckForUpdateAt";
            this.cbCheckForUpdateAt.Size = new System.Drawing.Size(110, 17);
            this.cbCheckForUpdateAt.TabIndex = 1;
            this.cbCheckForUpdateAt.Text = "Check for Update";
            this.cbCheckForUpdateAt.UseVisualStyleBackColor = true;
            // 
            // cbCheckOnStart
            // 
            this.cbCheckOnStart.AutoSize = true;
            this.cbCheckOnStart.Location = new System.Drawing.Point(8, 30);
            this.cbCheckOnStart.Name = "cbCheckOnStart";
            this.cbCheckOnStart.Size = new System.Drawing.Size(224, 17);
            this.cbCheckOnStart.TabIndex = 0;
            this.cbCheckOnStart.Text = "Check for update when MediaPortal starts";
            this.cbCheckOnStart.UseVisualStyleBackColor = true;
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(267, 203);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 1;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(348, 203);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 243);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.tabControl1);
            this.Name = "ConfigurationForm";
            this.Text = "Avalon Skin Configuration";
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.CheckUpdate.ResumeLayout(false);
            this.CheckUpdate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage CheckUpdate;
        private System.Windows.Forms.CheckBox cbCheckOnStart;
        private System.Windows.Forms.CheckBox cbCheckForUpdateAt;
        private System.Windows.Forms.CheckBox cbRestartMP;
        private System.Windows.Forms.CheckBox cbRunUnattended;
        private System.Windows.Forms.Button btCheckForUpdate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboCheckInterval;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btCancel;
    }
}