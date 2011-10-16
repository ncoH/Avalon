using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace AvalonMediaPortalRestart
{
    public partial class AvalonMediaPortalRestart : Form
    {
        int xpos;
        int ypos;
        string splashScreenImage;

        public AvalonMediaPortalRestart()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void AvalonMediaPortalRestart_Load(object sender, EventArgs e)
        {
            // The Parameter we need is in postion 1, assume MediaPortal not starting in fullscreen
            bool weIsFullscreen = false;
            bool isRestartingMp = true; // By default MP will restart
            bool isUpgrading = false; // By default SMP is not upgrading
            bool isUpgradingUnattented = false; // By default silent upgrade is off
            string upgradePath = string.Empty;

            string[] args = Environment.GetCommandLineArgs();

            // Argument structure (can be used in conjuction but in specific order):
            //    /upgrade {FileNameToExecute} [/unattended]
            //        Performs upgrade of MP. Executes the [FileNameToExecute] with unattended option if specified
            //    /restartmp [{true|false}] [{splashScreenImage}]
            //        Restarts MP in fullscreen or not with a specified splashscreen

            if (args.Length > 0)
            {
                int currentArgumentIndex = 1;
                isRestartingMp = false; // There are command line arguments, by default MP will not restart

                if (args[currentArgumentIndex++] == "/upgrade")
                {
                    isUpgrading = true;
                    upgradePath = args[currentArgumentIndex++];

                    if (args.Length > currentArgumentIndex)
                    {
                        if (args[currentArgumentIndex++] == "/unattended")
                        {
                            isUpgradingUnattented = true;
                        }
                    }
                }
                else
                {
                    currentArgumentIndex--;
                }

                if (args.Length > currentArgumentIndex)
                {
                    if (args[currentArgumentIndex++] == "/restartmp")
                    {
                        isRestartingMp = true;

                        if (args.Length > currentArgumentIndex)
                        {
                            bool.TryParse(args[currentArgumentIndex++], out weIsFullscreen);

                            if (args.Length > currentArgumentIndex)
                            {
                                splashScreenImage = args[currentArgumentIndex++];
                            }
                        }
                    }
                }
            }

            if (isUpgrading && !string.IsNullOrEmpty(upgradePath))
            {
                ProcessStartInfo upgradeProcess = new ProcessStartInfo(upgradePath);
                upgradeProcess.WorkingDirectory = Path.GetDirectoryName(upgradePath);
                if (isUpgradingUnattented) upgradeProcess.Arguments = " /unattended";
                Process p = System.Diagnostics.Process.Start(upgradeProcess);   // Kick off an elevated process. Our process remains un-elevated to restart MP !!
                int dontWaitForeverCounter = 300; // 10 minutes are enough

                while (!p.HasExited)
                {
                    p.WaitForExit(2000);
                    dontWaitForeverCounter--;

                    if (dontWaitForeverCounter < 0)
                    {
                        break;
                    }
                }

                if (dontWaitForeverCounter <= 0)
                {
                    Application.Exit(); // users will probably restart mp themshelves after 10 minutes
                }
            }

            if (!isRestartingMp) Application.Exit();

            pbSplashScreen.Image = Image.FromFile(splashScreenImage);

            lbStatus.Parent = pbSplashScreen;
            lbStatus.BackColor = Color.Transparent;
            if (weIsFullscreen)
            {
                lbStatus.Text = "Restarting MediaPortal in 3 Seconds";
                xpos = (Screen.PrimaryScreen.Bounds.Width / 2) - (lbStatus.Width / 2);
                ypos = (Screen.PrimaryScreen.Bounds.Height / 2) - (lbStatus.Height / 2);
            }
            else
            {
                lbStatus.Font = ChangeFontSize(lbStatus.Font, (float)9.00);
                lbStatus.Text = "Restarting MediaPortal in 3 Seconds";
                xpos = (600 / 2) - (lbStatus.Width / 2);
                ypos = (338 / 2) - (lbStatus.Height / 2);
            }

            if (weIsFullscreen)
            {
                // MP set to start in fullscreen - fullscreen splash screen
                this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                this.Location = new Point(0, 0);
                pbSplashScreen.Size = new Size(Screen.PrimaryScreen.Bounds.Width + 1, Screen.PrimaryScreen.Bounds.Height + 1);
                lbStatus.Location = new Point(xpos, ypos);
            }
            else
            {
                // MP Not set to start fullscreen - small splash screen
                this.Size = new Size(600, 338);
                this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - 300, Screen.PrimaryScreen.Bounds.Height / 2 - 169);
                pbSplashScreen.Size = new Size(600, 338);
                lbStatus.Location = new Point(xpos, ypos);
            }

            this.Show();

            for (int i = 3; i > 0; i--)
            {
                lbStatus.Text = string.Format("Restarting MediaPortal in {0} Seconds", i);
                this.Refresh();
                Thread.Sleep(1000);
            }
            ProcessStartInfo process = new ProcessStartInfo("MediaPortal.exe");
            process.UseShellExecute = true;
            Process.Start(process);
            Thread.Sleep(1000);
            Application.Exit();
        }


        static public Font ChangeFontSize(Font font, float fontSize)
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

    }
}
