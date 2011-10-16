namespace AvalonPatch
{
    partial class AvalonPatch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btInstallPatch = new System.Windows.Forms.Button();
            this.introBox = new System.Windows.Forms.TextBox();
            this.thePatches = new System.Windows.Forms.ListView();
            this.PatchFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.patchFileVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.installedVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.processCheckGroup = new System.Windows.Forms.GroupBox();
            this.configLoaderStatus = new System.Windows.Forms.Label();
            this.avaloneditorStatus = new System.Windows.Forms.Label();
            this.configurationStatus = new System.Windows.Forms.Label();
            this.mediaportalStatus = new System.Windows.Forms.Label();
            this.lbPluginConfigLoader = new System.Windows.Forms.Label();
            this.SMPEditorActivelb = new System.Windows.Forms.Label();
            this.ConfigurationStatusLb = new System.Windows.Forms.Label();
            this.mediaPortalStatusLb = new System.Windows.Forms.Label();
            this.patchProgressBar = new System.Windows.Forms.ProgressBar();
            this.btExit = new System.Windows.Forms.Button();
            this.processCheckGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // btInstallPatch
            // 
            this.btInstallPatch.Location = new System.Drawing.Point(12, 351);
            this.btInstallPatch.Name = "btInstallPatch";
            this.btInstallPatch.Size = new System.Drawing.Size(104, 24);
            this.btInstallPatch.TabIndex = 0;
            this.btInstallPatch.Text = "Install";
            this.btInstallPatch.UseVisualStyleBackColor = true;
            this.btInstallPatch.Click += new System.EventHandler(this.btInstallPatch_Click);
            // 
            // introBox
            // 
            this.introBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.introBox.Location = new System.Drawing.Point(12, 29);
            this.introBox.Multiline = true;
            this.introBox.Name = "introBox";
            this.introBox.ReadOnly = true;
            this.introBox.Size = new System.Drawing.Size(353, 41);
            this.introBox.TabIndex = 9;
            this.introBox.Text = "This utility will update the parts of the skin that can not be updated while any " +
                "of the Skin or MediaPortal processes are active.";
            // 
            // thePatches
            // 
            this.thePatches.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PatchFileName,
            this.patchFileVersion,
            this.installedVersion});
            this.thePatches.Location = new System.Drawing.Point(12, 59);
            this.thePatches.Name = "thePatches";
            this.thePatches.Size = new System.Drawing.Size(466, 156);
            this.thePatches.SmallImageList = this.imageList1;
            this.thePatches.TabIndex = 15;
            this.thePatches.UseCompatibleStateImageBehavior = false;
            this.thePatches.View = System.Windows.Forms.View.Details;
            // 
            // PatchFileName
            // 
            this.PatchFileName.Text = "File";
            this.PatchFileName.Width = 220;
            // 
            // patchFileVersion
            // 
            this.patchFileVersion.Text = "Version";
            this.patchFileVersion.Width = 130;
            // 
            // installedVersion
            // 
            this.installedVersion.Text = "Installed Version";
            this.installedVersion.Width = 110;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // processCheckGroup
            // 
            this.processCheckGroup.Controls.Add(this.configLoaderStatus);
            this.processCheckGroup.Controls.Add(this.avaloneditorStatus);
            this.processCheckGroup.Controls.Add(this.configurationStatus);
            this.processCheckGroup.Controls.Add(this.mediaportalStatus);
            this.processCheckGroup.Controls.Add(this.lbPluginConfigLoader);
            this.processCheckGroup.Controls.Add(this.SMPEditorActivelb);
            this.processCheckGroup.Controls.Add(this.ConfigurationStatusLb);
            this.processCheckGroup.Controls.Add(this.mediaPortalStatusLb);
            this.processCheckGroup.Location = new System.Drawing.Point(12, 237);
            this.processCheckGroup.Name = "processCheckGroup";
            this.processCheckGroup.Size = new System.Drawing.Size(466, 108);
            this.processCheckGroup.TabIndex = 11;
            this.processCheckGroup.TabStop = false;
            this.processCheckGroup.Text = "Active Process Check";
            // 
            // configLoaderStatus
            // 
            this.configLoaderStatus.AutoSize = true;
            this.configLoaderStatus.ForeColor = System.Drawing.Color.Red;
            this.configLoaderStatus.Location = new System.Drawing.Point(384, 86);
            this.configLoaderStatus.Name = "configLoaderStatus";
            this.configLoaderStatus.Size = new System.Drawing.Size(47, 13);
            this.configLoaderStatus.TabIndex = 7;
            this.configLoaderStatus.Text = "Running";
            // 
            // avaloneditorStatus
            // 
            this.avaloneditorStatus.AutoSize = true;
            this.avaloneditorStatus.ForeColor = System.Drawing.Color.Red;
            this.avaloneditorStatus.Location = new System.Drawing.Point(384, 61);
            this.avaloneditorStatus.Name = "avaloneditorStatus";
            this.avaloneditorStatus.Size = new System.Drawing.Size(47, 13);
            this.avaloneditorStatus.TabIndex = 6;
            this.avaloneditorStatus.Text = "Running";
            // 
            // configurationStatus
            // 
            this.configurationStatus.AutoSize = true;
            this.configurationStatus.ForeColor = System.Drawing.Color.Red;
            this.configurationStatus.Location = new System.Drawing.Point(384, 39);
            this.configurationStatus.Name = "configurationStatus";
            this.configurationStatus.Size = new System.Drawing.Size(47, 13);
            this.configurationStatus.TabIndex = 5;
            this.configurationStatus.Text = "Running";
            // 
            // mediaportalStatus
            // 
            this.mediaportalStatus.AutoSize = true;
            this.mediaportalStatus.ForeColor = System.Drawing.Color.Red;
            this.mediaportalStatus.Location = new System.Drawing.Point(384, 16);
            this.mediaportalStatus.Name = "mediaportalStatus";
            this.mediaportalStatus.Size = new System.Drawing.Size(47, 13);
            this.mediaportalStatus.TabIndex = 4;
            this.mediaportalStatus.Text = "Running";
            // 
            // lbPluginConfigLoader
            // 
            this.lbPluginConfigLoader.AutoSize = true;
            this.lbPluginConfigLoader.Location = new System.Drawing.Point(9, 86);
            this.lbPluginConfigLoader.Name = "lbPluginConfigLoader";
            this.lbPluginConfigLoader.Size = new System.Drawing.Size(73, 13);
            this.lbPluginConfigLoader.TabIndex = 3;
            this.lbPluginConfigLoader.Text = "Config Loader";
            // 
            // SMPEditorActivelb
            // 
            this.SMPEditorActivelb.AutoSize = true;
            this.SMPEditorActivelb.Location = new System.Drawing.Point(9, 61);
            this.SMPEditorActivelb.Name = "SMPEditorActivelb";
            this.SMPEditorActivelb.Size = new System.Drawing.Size(70, 13);
            this.SMPEditorActivelb.TabIndex = 2;
            this.SMPEditorActivelb.Text = "Avalon Editor";
            // 
            // ConfigurationStatusLb
            // 
            this.ConfigurationStatusLb.AutoSize = true;
            this.ConfigurationStatusLb.Location = new System.Drawing.Point(9, 39);
            this.ConfigurationStatusLb.Name = "ConfigurationStatusLb";
            this.ConfigurationStatusLb.Size = new System.Drawing.Size(69, 13);
            this.ConfigurationStatusLb.TabIndex = 1;
            this.ConfigurationStatusLb.Text = "Configuration";
            // 
            // mediaPortalStatusLb
            // 
            this.mediaPortalStatusLb.AutoSize = true;
            this.mediaPortalStatusLb.Location = new System.Drawing.Point(9, 16);
            this.mediaPortalStatusLb.Name = "mediaPortalStatusLb";
            this.mediaPortalStatusLb.Size = new System.Drawing.Size(62, 13);
            this.mediaPortalStatusLb.TabIndex = 0;
            this.mediaPortalStatusLb.Text = "Mediaportal";
            // 
            // patchProgressBar
            // 
            this.patchProgressBar.Location = new System.Drawing.Point(12, 221);
            this.patchProgressBar.Name = "patchProgressBar";
            this.patchProgressBar.Size = new System.Drawing.Size(466, 10);
            this.patchProgressBar.TabIndex = 12;
            // 
            // btExit
            // 
            this.btExit.Location = new System.Drawing.Point(374, 351);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(104, 24);
            this.btExit.TabIndex = 14;
            this.btExit.Text = "Exit";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // AvalonPatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 381);
            this.Controls.Add(this.patchProgressBar);
            this.Controls.Add(this.thePatches);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.btInstallPatch);
            this.Controls.Add(this.introBox);
            this.Controls.Add(this.processCheckGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AvalonPatch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Avalon Patch Utility";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AvalonPatch_FormClosing);
            this.Load += new System.EventHandler(this.AvalonPatch_Load);
            this.processCheckGroup.ResumeLayout(false);
            this.processCheckGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btInstallPatch;
        private System.Windows.Forms.TextBox introBox;
        private System.Windows.Forms.ListView thePatches;
        private System.Windows.Forms.GroupBox processCheckGroup;
        private System.Windows.Forms.Label lbPluginConfigLoader;
        private System.Windows.Forms.Label SMPEditorActivelb;
        private System.Windows.Forms.Label ConfigurationStatusLb;
        private System.Windows.Forms.Label mediaPortalStatusLb;
        private System.Windows.Forms.Label configLoaderStatus;
        private System.Windows.Forms.Label avaloneditorStatus;
        private System.Windows.Forms.Label configurationStatus;
        private System.Windows.Forms.Label mediaportalStatus;
        private System.Windows.Forms.ProgressBar patchProgressBar;
        private System.Windows.Forms.ColumnHeader PatchFileName;
        private System.Windows.Forms.ColumnHeader patchFileVersion;
        private System.Windows.Forms.ColumnHeader installedVersion;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btExit;
    }
}

