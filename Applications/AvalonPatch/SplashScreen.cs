using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace AvalonPatch
{
    public partial class SplashScreen : Form
    {

        #region Variables

        int xpos;
        int ypos;
        SkinInfo skInfo = new SkinInfo();

        #endregion

        #region Constructor

        public SplashScreen()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        #endregion

        #region Private Methods

        void SplashScreen_Load(object sender, EventArgs e)
        {
            pbSplashScreen.Image = Image.FromFile(Path.Combine(Path.Combine(SkinInfo.mpPaths.AvalonPath, "Media"), "splashscreen.png"));

            lbStatus.Parent = pbSplashScreen;
            lbStatus.BackColor = Color.Transparent;
            lbStatus2.Parent = pbSplashScreen;
            lbStatus2.BackColor = Color.Transparent;

            if (skInfo.startFullScreen)
            {
                setXYPos(skInfo.startFullScreen);
                // MP Set to start fullscreen - fullscreen splash
                this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                this.Location = new Point(0, -1);
                pbSplashScreen.Size = new Size(Screen.PrimaryScreen.Bounds.Width + 2, Screen.PrimaryScreen.Bounds.Height + 2);
            }
            else
            {
                // MP Not set to start fullscreen - small splash screen
                setXYPos(skInfo.startFullScreen);
                this.Size = new Size(600, 338);
                this.Location = new Point((Screen.PrimaryScreen.Bounds.Width / 2) - 300, (Screen.PrimaryScreen.Bounds.Height / 2) - 169);
                pbSplashScreen.Size = new Size(600, 338);
                pbSplashScreen.Location = new Point(0, 0);
            }
        }

        void setXYPos(bool fullScreen)
        {
            if (fullScreen)
            {
                // Calculate and set the x,y position for Label 1
                xpos = (Screen.PrimaryScreen.Bounds.Width / 2) - (lbStatus.Width / 2);
                ypos = ((Screen.PrimaryScreen.Bounds.Height / 2) - (lbStatus.Height / 2)) - 40;
                lbStatus.Location = new Point(xpos, ypos);
                // Calculate and set the x,y position for Label 2
                xpos = (Screen.PrimaryScreen.Bounds.Width / 2) - (lbStatus2.Width / 2);
                ypos = ((Screen.PrimaryScreen.Bounds.Height / 2) - (lbStatus2.Height / 2)) + 40;
                lbStatus2.Location = new Point(xpos, ypos);
            }
            else
            {
                // Calculate and set the x,y position for Label 1
                xpos = (600 / 2) - (lbStatus.Width / 2);
                ypos = ((338 / 2) - (lbStatus.Height / 2)) - 20;
                lbStatus.Location = new Point(xpos, ypos);
                // Calculate and set the x,y position for Label 2
                xpos = (600 / 2) - (lbStatus2.Width / 2);
                ypos = ((338 / 2) - (lbStatus2.Height / 2)) + 20;
                lbStatus2.Location = new Point(xpos, ypos);
            }
        }

        private Font ChangeFontSize(Font font, float fontSize)
        {
            if (font != null)
            {
                float currentSize = font.Size;
                if (currentSize != fontSize)
                {
                    font = new Font(font.Name, fontSize,
                        font.Style, font.Unit,
                        font.GdiCharSet, font.GdiVerticalFont);
                }
            }
            return font;
        }

        #endregion

        #region Public Methods

        public string statusTextLine1
        {
            set
            {
                // Calculate and set the x,y position for Label 1
                if (!skInfo.startFullScreen)
                    lbStatus.Font = ChangeFontSize(lbStatus.Font, (float)9.00);

                lbStatus.Text = value;
                setXYPos(skInfo.startFullScreen);
            }
        }

        public string statusTextLine2
        {
            set
            {
                // Calculate and set the x,y position for Label 2
                if (!skInfo.startFullScreen)
                    lbStatus2.Font = ChangeFontSize(lbStatus2.Font, (float)9.00);

                lbStatus2.Text = value;
                setXYPos(skInfo.startFullScreen);
            }
        }

        #endregion

        private void pbSplashScreen_Click(object sender, EventArgs e)
        {

        }

    }
}
