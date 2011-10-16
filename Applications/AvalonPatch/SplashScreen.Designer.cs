namespace AvalonPatch
{
    partial class SplashScreen
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
            this.pbSplashScreen = new System.Windows.Forms.PictureBox();
            this.lbStatus = new System.Windows.Forms.Label();
            this.lbStatus2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbSplashScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // pbSplashScreen
            // 
            this.pbSplashScreen.BackColor = System.Drawing.Color.Transparent;
            this.pbSplashScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbSplashScreen.Location = new System.Drawing.Point(-1, 1);
            this.pbSplashScreen.Name = "pbSplashScreen";
            this.pbSplashScreen.Size = new System.Drawing.Size(1280, 720);
            this.pbSplashScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSplashScreen.TabIndex = 0;
            this.pbSplashScreen.TabStop = false;
            this.pbSplashScreen.Click += new System.EventHandler(this.pbSplashScreen_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.ForeColor = System.Drawing.Color.White;
            this.lbStatus.Location = new System.Drawing.Point(263, 335);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(639, 37);
            this.lbStatus.TabIndex = 1;
            this.lbStatus.Text = "Avalon Update In Progress - Please Wait ";
            // 
            // lbStatus2
            // 
            this.lbStatus2.AutoSize = true;
            this.lbStatus2.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus2.ForeColor = System.Drawing.Color.White;
            this.lbStatus2.Location = new System.Drawing.Point(263, 399);
            this.lbStatus2.Name = "lbStatus2";
            this.lbStatus2.Size = new System.Drawing.Size(712, 37);
            this.lbStatus2.TabIndex = 2;
            this.lbStatus2.Text = "Please Wait - MediaPortal will Restart Shortly";
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.lbStatus2);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.pbSplashScreen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashScreen";
            this.Text = "SplashScreen";
            this.Load += new System.EventHandler(this.SplashScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbSplashScreen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbSplashScreen;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label lbStatus2;
    }
}