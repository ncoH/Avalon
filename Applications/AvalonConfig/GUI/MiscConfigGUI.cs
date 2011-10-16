using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;

namespace AvalonGUIConfig
{
    public class MiscConfigGUI : GUIWindow
    {
        #region Enums
        private enum GUIControls
        {
            hidePoster = 2,
            showRSS = 3,
            showHiddenMenu = 4,
            LargeFontSize = 5,
            UnfocusedAlpha = 6,
            showFiveDayWeather = 7,
            trailerSite = 8
        }

        private enum TrailerSite
        {
            IMDb,
            iTunes,
            YouTube
        }

        #endregion

        #region Local Variables

        private string UnfocusedAlphaListTemp;
        private string UnfocusedAlphaThumbsTemp;

        #endregion

        #region Skin Controls

        [SkinControl((int)GUIControls.hidePoster)]
        protected GUIToggleButtonControl btnHidePoster = null;

        [SkinControl((int)GUIControls.showRSS)]
        protected GUIToggleButtonControl btnshowRSS = null;

        [SkinControl((int)GUIControls.showHiddenMenu)]
        protected GUIToggleButtonControl btnshowHiddenMenu = null;

        [SkinControl((int)GUIControls.UnfocusedAlpha)]
        protected GUIButtonControl btnUnfocusedAlpha = null;

        [SkinControl((int)GUIControls.LargeFontSize)]
        protected GUIToggleButtonControl btnLargeFontSize = null;

        [SkinControl((int)GUIControls.showFiveDayWeather)]
        protected GUIToggleButtonControl btnshowFiveDayWeather = null;

        [SkinControl((int)GUIControls.trailerSite)]
        protected GUIButtonControl btntrailerSite = null;
        #endregion

        #region Constructor
        public MiscConfigGUI() { }
        #endregion

        #region Public Properties
        public static bool HidePoster { get; set; }
        public static bool showRSS { get; set; }
        public static bool showHiddenMenu { get; set; }
        public static int UnfocusedAlphaListItems { get; set; }
        public static int UnfocusedAlphaThumbs { get; set; }
        public static bool UseLargeFonts { get; set; }
        public static bool showFiveDayWeather { get; set; }
        public static int siteUtil { get; set; } 
        #endregion

        #region Public Methods

        public static void SetProperties()
        {
            string param = string.Empty;

            if (siteUtil == 0)
            {
                param = "IMDb Movie Trailers";
            }

            if (siteUtil == 1)
            {
                param = "iTunes Movie Trailers";
            }

            if (siteUtil == 2)
            {
                param = "YouTube";
            }

            AvalonGUIConfig.SetProperty("#Avalon.EnablePosterHide", MiscConfigGUI.HidePoster ? "true" : "false");
            AvalonGUIConfig.SetProperty("#Avalon.RSS", MiscConfigGUI.showRSS ? "true" : "false");
            AvalonGUIConfig.SetProperty("#Avalon.HiddenMenu", MiscConfigGUI.showHiddenMenu ? "true" : "false");
            AvalonGUIConfig.SetProperty("#Avalon.5DayWeather", MiscConfigGUI.showFiveDayWeather ? "true" : "false");
            AvalonGUIConfig.SetProperty("#Avalon.TrailerSite", param);
        }

        public static void SetAlphaTransparency()
        {
            string file = GUIGraphicsContext.Skin + @"\references.xml";

            AvalonHelper.SetNodeText(file, "/controls/control[type='listcontrol']/unfocusedAlpha", MiscConfigGUI.UnfocusedAlphaListItems.ToString());
            AvalonHelper.SetNodeText(file, "/controls/control[type='playlistcontrol']/unfocusedAlpha", MiscConfigGUI.UnfocusedAlphaListItems.ToString());
            AvalonHelper.SetNodeText(file, "/controls/control[type='thumbnailpanel']/unfocusedAlpha", MiscConfigGUI.UnfocusedAlphaThumbs.ToString());
        }
        #endregion

        #region Private Methods

        private void ShowInvalidUnfocusedAlphaMessage()
        {
            GUIDialogOK dlg = (GUIDialogOK)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_OK);
            dlg.Reset();
            dlg.SetHeading(Translation.UnfocusedAlpha);
            dlg.SetLine(1, Translation.UnfocusedAlphaInvalidLine1);
            dlg.SetLine(2, Translation.UnfocusedAlphaInvalidLine2);
            dlg.SetLine(3, Translation.UnfocusedAlphaInvalidLine3);
            dlg.DoModal(GetID);
        }

        private void ShowUnfocusedAlphaMenu()
        {
            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg == null) return;

            dlg.Reset();
            dlg.SetHeading(Translation.UnfocusedAlpha);

            // Create menu items
            GUIListItem listItem = new GUIListItem(string.Format(Translation.UnfocusedAlphaMenuLists, UnfocusedAlphaListTemp));
            dlg.Add(listItem);

            listItem = new GUIListItem(string.Format(Translation.UnfocusedAlphaMenuThumbs, UnfocusedAlphaThumbsTemp));
            dlg.Add(listItem);

            listItem = new GUIListItem(Translation.RestoreDefaults);
            dlg.Add(listItem);

            dlg.DoModal(GUIWindowManager.ActiveWindow);
            if (dlg.SelectedId <= 0) return;

            string retValue = string.Empty;
            bool invalid = false;
            switch (dlg.SelectedId)
            {
                case 1:
                    string input = UnfocusedAlphaListTemp;
                    if (AvalonGUIConfig.ShowKeyboard(input, out retValue))
                    {
                        int output = 0;
                        if (int.TryParse(retValue, out output))
                        {
                            if (output < 0 || output > 255)
                                invalid = true;
                            else
                                UnfocusedAlphaListTemp = retValue;
                        }
                        else
                            invalid = true;
                    }
                    break;

                case 2:
                    input = UnfocusedAlphaThumbsTemp;
                    if (AvalonGUIConfig.ShowKeyboard(input, out retValue))
                    {
                        int output = 0;
                        if (int.TryParse(retValue, out output))
                        {
                            if (output < 0 || output > 255)
                                invalid = true;
                            else
                                UnfocusedAlphaThumbsTemp = retValue;
                        }
                        else
                            invalid = true;
                    }
                    break;

                case 3:
                    UnfocusedAlphaThumbsTemp = "150";
                    UnfocusedAlphaListTemp = "150";
                    break;
            }

            if (invalid)
            {
                ShowInvalidUnfocusedAlphaMessage();
                return;
            }

        }

        private void ShowTrailersMenu()
        {
            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            dlg.Reset();
            dlg.SetHeading("Trailer Site");

            foreach (TrailerSite site in Enum.GetValues(typeof(TrailerSite)))
            {
                string menuItem = Enum.GetName(typeof(TrailerSite), site);
                GUIListItem pItem = new GUIListItem(menuItem);
                dlg.Add(pItem);
            }

            dlg.DoModal(GUIWindowManager.ActiveWindow);

            if (dlg.SelectedLabel >= 0)
            {
                int trailerSite = 0;

                switch (dlg.SelectedLabelText)
                {
                    case ("IMDb"):
                        trailerSite = 0;
                        break;

                    case ("iTunes"):
                        trailerSite = 1;
                        break;

                    case ("YouTube"):
                        trailerSite = 2;
                        break;
                }

                siteUtil = trailerSite;
                
            }
        }

        private void SetControlStates()
        {
            btnHidePoster.Selected = HidePoster;
            btnshowRSS.Selected = showRSS;
            btnshowFiveDayWeather.Selected = showFiveDayWeather;
            btnshowFiveDayWeather.Label = Translation.SkinSettingsShow5DayWeather;
            
            btnshowHiddenMenu.Selected = showHiddenMenu;
            btnUnfocusedAlpha.Label = string.Format("{0}", Translation.UnfocusedAlpha);
            
            btnLargeFontSize.Selected = UseLargeFonts;
            btnLargeFontSize.Label = Translation.UseLargeFonts;

            btntrailerSite.Label = Translation.TrailerSite;
        }

        private void GetControlStates()
        {
            HidePoster = btnHidePoster.Selected;
            showRSS = btnshowRSS.Selected;
            showHiddenMenu = btnshowHiddenMenu.Selected;
            showFiveDayWeather = btnshowFiveDayWeather.Selected;
        }

        private void ApplyConfigurationChanges()
        {
            bool requiresRestart = false;

            SetProperties();

            #region Unfocused Alpha
            if (MiscConfigGUI.UnfocusedAlphaListItems != int.Parse(UnfocusedAlphaListTemp)) requiresRestart = true;
            if (MiscConfigGUI.UnfocusedAlphaThumbs != int.Parse(UnfocusedAlphaThumbsTemp)) requiresRestart = true;
            MiscConfigGUI.UnfocusedAlphaListItems = int.Parse(UnfocusedAlphaListTemp);
            MiscConfigGUI.UnfocusedAlphaThumbs = int.Parse(UnfocusedAlphaThumbsTemp);
            SetAlphaTransparency();
            #endregion

            #region Fonts
            if (UseLargeFonts != btnLargeFontSize.Selected)
            {
                requiresRestart = true;
                UseLargeFonts = btnLargeFontSize.Selected;
                string sourceFile = Path.Combine(SkinInfo.mpPaths.AvalonPath, UseLargeFonts ? "fonts.large.xml" : "fonts.normal.xml");
                string destinationFile = Path.Combine(SkinInfo.mpPaths.AvalonPath, "fonts.xml");
                try
                {
                    Log.Info("Setting Font Size to: {0}", UseLargeFonts ? "Large" : "Normal");
                    File.Copy(sourceFile, destinationFile, true);
                    AvalonGUIConfig.PendingRestartChanges.Add(AvalonGUIConfig.PendingChanges.ClearCache);
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to copy fonts file: {0}", ex.Message);
                }
            }
            #endregion

            if (requiresRestart)
            {
                AvalonGUIConfig.ShowRestartMessage(GetID, Translation.SkinSettingsMisc);
            }

        }

        #endregion

        #region Base Overrides
        public override int GetID
        {
            get
            {
                return (int)AvalonGUIConfig.AvalonScreenID.AvalonMiscConfig;
            }
        }

        public override bool Init()
        {
            return Load(GUIGraphicsContext.Skin + @"\AvalonConfig_misc.xml");
        }

        protected override void OnPageLoad()
        {
            // Load Settings
            settings.Load(settings.cXMLSectionMisc);

            // Update Controls
            SetControlStates();

            UnfocusedAlphaListTemp = MiscConfigGUI.UnfocusedAlphaListItems.ToString();
            UnfocusedAlphaThumbsTemp = MiscConfigGUI.UnfocusedAlphaThumbs.ToString();
        }

        protected override void OnPageDestroy(int newWindowId)
        {
            // Get Current State of controls
            GetControlStates();

            // Apply Configuration changes
            ApplyConfigurationChanges();

            // Save Settings
            settings.Save(settings.cXMLSectionMisc);
        }

        protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
        {
            switch (controlId)
            {
                case (int)GUIControls.hidePoster:
                    break;
                case (int)GUIControls.showRSS:
                    break;
                case (int)GUIControls.showHiddenMenu:
                    break;
                case (int)GUIControls.showFiveDayWeather:
                    break;
                case (int) GUIControls.LargeFontSize:
                    break;
                case (int)GUIControls.UnfocusedAlpha:
                    ShowUnfocusedAlphaMenu();
                    break;
                case (int)GUIControls.trailerSite:
                    ShowTrailersMenu();
                    break;
            }
            base.OnClicked(controlId, control, actionType);
        }

        #endregion
    }
}
