using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;
using System.Drawing;

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
            trailerSite = 8,
            ListColors = 9,
            WeatherInfoservice = 10
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

        private string TextColorTemp;
        private string TextColor2Temp;
        private string TextColor3Temp;
        private string WatchedColorTemp;
        private string RemoteColorTemp;

        private Dictionary<int, string> KnownColors = new Dictionary<int, string>();

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

        [SkinControl((int)GUIControls.ListColors)]
        protected GUIButtonControl btnListColors = null;

        [SkinControl((int)GUIControls.WeatherInfoservice)]
        protected GUIToggleButtonControl btnWeatherInfoservice = null;


        #endregion

        #region Constructor
        public MiscConfigGUI() { }
        #endregion

        #region Public Properties
        public static bool HidePoster { get; set; }
        public static bool showRSS { get; set; }
        public static bool showHiddenMenu { get; set; }
        public static string TextColor { get; set; }
        public static string TextColor2 { get; set; }
        public static string TextColor3 { get; set; }
        public static string WatchedColor { get; set; }
        public static string RemoteColor { get; set; }
        public static int UnfocusedAlphaListItems { get; set; }
        public static int UnfocusedAlphaThumbs { get; set; }
        public static bool UseLargeFonts { get; set; }
        public static bool showFiveDayWeather { get; set; }
        public static int siteUtil { get; set; }
        public static bool UseWeatherInfoservice { get; set; }
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

        public static void SetColors()
        {
            string file = GUIGraphicsContext.Skin + @"\references.xml";

            AvalonHelper.SetNodeText(file, "/controls/control[type='listcontrol']/textcolor", string.Concat("FF", MiscConfigGUI.TextColor));
            AvalonHelper.SetNodeText(file, "/controls/control[type='listcontrol']/textcolor2", string.Concat("FF", MiscConfigGUI.TextColor2));
            AvalonHelper.SetNodeText(file, "/controls/control[type='listcontrol']/textcolor3", string.Concat("FF", MiscConfigGUI.TextColor3));
            AvalonHelper.SetNodeText(file, "/controls/control[type='listcontrol']/playedColor", string.Concat("FF", MiscConfigGUI.WatchedColor));
            AvalonHelper.SetNodeText(file, "/controls/control[type='listcontrol']/remoteColor", string.Concat("FF", MiscConfigGUI.RemoteColor));

            AvalonHelper.SetNodeText(file, "/controls/control[type='playlistcontrol']/textcolor", string.Concat("FF", MiscConfigGUI.TextColor));
            AvalonHelper.SetNodeText(file, "/controls/control[type='playlistcontrol']/textcolor2", string.Concat("FF", MiscConfigGUI.TextColor2));
            AvalonHelper.SetNodeText(file, "/controls/control[type='playlistcontrol']/textcolor3", string.Concat("FF", MiscConfigGUI.TextColor3));
            AvalonHelper.SetNodeText(file, "/controls/control[type='playlistcontrol']/playedColor", string.Concat("FF", MiscConfigGUI.WatchedColor));
            AvalonHelper.SetNodeText(file, "/controls/control[type='playlistcontrol']/remoteColor", string.Concat("FF", MiscConfigGUI.RemoteColor));
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

        private void ShowInvalidColorMessage()
        {
            GUIDialogOK dlg = (GUIDialogOK)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_OK);
            dlg.Reset();
            dlg.SetHeading(Translation.ListColors);
            dlg.SetLine(1, Translation.ListColorInvalidLine1);
            dlg.SetLine(2, Translation.ListColorInvalidLine2);
            dlg.SetLine(3, Translation.ListColorInvalidLine3);
            dlg.DoModal(GetID);
        }

        private Dictionary<string, string> GetColors()
        {
            Dictionary<string, string> colors = new Dictionary<string, string>();

            // get the color names from the Known color enum
            string[] colorNames = Enum.GetNames(typeof(KnownColor));

            foreach (string colorName in colorNames)
            {
                // cast the colorName into a KnownColor
                KnownColor knownColor = (KnownColor)Enum.Parse(typeof(KnownColor), colorName);
                Color color = Color.FromKnownColor(knownColor);

                // check if the knownColor variable is a System color      
                if (knownColor > KnownColor.Transparent)
                {
                    colors.Add(colorName, GetHexCode(color));
                }
            }
            return colors;
        }

        private string GetHexCode(Color color)
        {
            byte[] data = { color.R, color.G, color.B };
            string hex = BitConverter.ToString(data);
            return hex.Replace("-", string.Empty);
        }

        private Color GetColor(string hexCode)
        {
            try
            {
                int r = Convert.ToInt32(hexCode.Substring(0, 2), 16);
                int g = Convert.ToInt32(hexCode.Substring(2, 2), 16);
                int b = Convert.ToInt32(hexCode.Substring(4, 2), 16);
                return Color.FromArgb(r, g, b);
            }
            catch
            {
                return Color.Empty;
            }
        }

        private string GetColorName(string hexCode)
        {
            Color color = GetColor(hexCode);

            if (KnownColors.Count == 0)
            {
                Color someColor;
                Array knownColors = Enum.GetValues(typeof(KnownColor));
                foreach (KnownColor knownColor in knownColors)
                {
                    someColor = Color.FromKnownColor(knownColor);
                    if (!someColor.IsSystemColor && !KnownColors.ContainsKey(someColor.ToArgb()))
                    {
                        KnownColors.Add(someColor.ToArgb(), someColor.Name);
                    }
                }
            }

            string colorName = string.Empty;

            if (!KnownColors.TryGetValue(color.ToArgb(), out colorName))
            {
                colorName = color.Name.Substring(2);
            }
            return colorName;
        }

        private bool IsValidColor(string hexCode)
        {
            if (hexCode.Length != 6) return false;
            return GetColor(hexCode) != Color.Empty;
        }

        private bool ShowColorSelectorMenu(string currentColor, out string chosenColor)
        {
            chosenColor = currentColor;

            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg == null) return false;

            dlg.Reset();
            dlg.SetHeading(Translation.ListColors);

            // Get List of Known Colors
            Dictionary<string, string> colors = GetColors();

            // Create Custom Color menu item
            GUIListItem listItem = new GUIListItem(string.Format("{0} ...", Translation.CustomColor));
            listItem.IconImage = GetColorTextureFromMemory(new Size(34, 34), currentColor);
            dlg.Add(listItem);

            // Create color menu items
            foreach (var color in colors)
            {
                listItem = new GUIListItem(color.Key);
                listItem.IconImage = GetColorTextureFromMemory(new Size(34, 34), color.Value);
                dlg.Add(listItem);
            }

            dlg.DoModal(GUIWindowManager.ActiveWindow);
            if (dlg.SelectedId <= 0) return false;

            string retValue = string.Empty;
            bool validColor = false;

            switch (dlg.SelectedId)
            {
                case 1:
                    // Custom Color, invoke keyboard
                    while (!validColor)
                    {
                        if (AvalonGUIConfig.ShowKeyboard(currentColor, out retValue))
                        {
                            if (IsValidColor(retValue))
                            {
                                chosenColor = retValue;
                                validColor = true;
                            }
                            else
                                ShowInvalidColorMessage();
                        }
                        else
                            return false;
                    }
                    break;

                default:
                    // Convert Known Color to HEX string
                    Color color = Color.FromName(dlg.SelectedLabelText);
                    chosenColor = GetHexCode(color);
                    break;
            }

            return true;
        }

        private string GetColorTextureFromMemory(Size size, string identifer)
        {
            try
            {
                // create a solid image
                Bitmap bmp = new Bitmap(size.Width, size.Height);
                Graphics gfx = Graphics.FromImage(bmp);
                Brush brsh = new SolidBrush(GetColor(identifer));
                gfx.FillRectangle(brsh, new Rectangle(0, 0, size.Width, size.Height));

                string textureIdentifer = string.Concat("[#Avalon.", identifer, "]");

                // will load from memory if it exists
                GUITextureManager.LoadFromMemory(bmp, textureIdentifer, 0, size.Width, size.Height);
                return textureIdentifer;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        private void ShowListColorsMenu()
        {
            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg == null) return;

            dlg.Reset();
            dlg.SetHeading(Translation.ListColors);

            // Create menu items
            GUIListItem listItem = new GUIListItem(string.Format(Translation.ListDefaultColor, GetColorName(TextColorTemp)));
            listItem.IconImage = GetColorTextureFromMemory(new Size(34, 34), TextColorTemp);
            dlg.Add(listItem);

            listItem = new GUIListItem(string.Format(Translation.ListWatchedColor, GetColorName(WatchedColorTemp)));
            listItem.IconImage = GetColorTextureFromMemory(new Size(34, 34), WatchedColorTemp);
            dlg.Add(listItem);

            listItem = new GUIListItem(string.Format(Translation.ListRemoteColor, GetColorName(RemoteColorTemp)));
            listItem.IconImage = GetColorTextureFromMemory(new Size(34, 34), RemoteColorTemp);
            dlg.Add(listItem);

            listItem = new GUIListItem(string.Format(Translation.ListText2Color, GetColorName(TextColor2Temp)));
            listItem.IconImage = GetColorTextureFromMemory(new Size(34, 34), TextColor2Temp);
            dlg.Add(listItem);

            listItem = new GUIListItem(string.Format(Translation.ListText3Color, GetColorName(TextColor3Temp)));
            listItem.IconImage = GetColorTextureFromMemory(new Size(34, 34), TextColor3Temp);
            dlg.Add(listItem);

            listItem = new GUIListItem(Translation.RestoreDefaults);
            dlg.Add(listItem);

            dlg.DoModal(GUIWindowManager.ActiveWindow);
            if (dlg.SelectedId <= 0) return;

            string chosenColor = string.Empty;

            switch (dlg.SelectedId)
            {
                case 1:
                    if (ShowColorSelectorMenu(TextColorTemp, out chosenColor))
                    {
                        TextColorTemp = chosenColor;
                    }
                    break;

                case 2:
                    if (ShowColorSelectorMenu(WatchedColorTemp, out chosenColor))
                    {
                        WatchedColorTemp = chosenColor;
                    }
                    break;

                case 3:
                    if (ShowColorSelectorMenu(RemoteColorTemp, out chosenColor))
                    {
                        RemoteColorTemp = chosenColor;
                    }
                    break;

                case 4:
                    if (ShowColorSelectorMenu(TextColor2Temp, out chosenColor))
                    {
                        TextColor2Temp = chosenColor;
                    }
                    break;

                case 5:
                    if (ShowColorSelectorMenu(TextColor3Temp, out chosenColor))
                    {
                        TextColor3Temp = chosenColor;
                    }
                    break;

                case 6:
                    TextColorTemp = "FFFFFF";
                    TextColor2Temp = "FFFFFF";
                    TextColor3Temp = "FFFFFF";
                    WatchedColorTemp = "85CFFF";
                    RemoteColorTemp = "FC7B19";
                    break;
            }

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
            btnListColors.Label = string.Format("{0} ...", Translation.ListColors);

            
            btnLargeFontSize.Selected = UseLargeFonts;
            btnLargeFontSize.Label = Translation.UseLargeFonts;

            btntrailerSite.Label = Translation.TrailerSite;

            btnWeatherInfoservice.Selected = UseWeatherInfoservice;
            btnWeatherInfoservice.Label = Translation.UseWeatherInfoservice;
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

            #region Colors
            if (MiscConfigGUI.TextColor != TextColorTemp) requiresRestart = true;
            if (MiscConfigGUI.TextColor2 != TextColor2Temp) requiresRestart = true;
            if (MiscConfigGUI.TextColor3 != TextColor3Temp) requiresRestart = true;
            if (MiscConfigGUI.WatchedColor != WatchedColorTemp) requiresRestart = true;
            if (MiscConfigGUI.RemoteColor != RemoteColorTemp) requiresRestart = true;
            MiscConfigGUI.TextColor = TextColorTemp;
            MiscConfigGUI.TextColor2 = TextColor2Temp;
            MiscConfigGUI.TextColor3 = TextColor3Temp;
            MiscConfigGUI.WatchedColor = WatchedColorTemp;
            MiscConfigGUI.RemoteColor = RemoteColorTemp;
            SetColors();
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

            #region Weather Data
            if (UseWeatherInfoservice != btnWeatherInfoservice.Selected)
            {
                UseWeatherInfoservice = btnWeatherInfoservice.Selected;
                string sourceFile = Path.Combine(SkinInfo.mpPaths.AvalonPath, UseWeatherInfoservice ? "BasicHome.weather.infoservice.xml" : "BasicHome.weather.worldweather.xml");
                string destinationFile = Path.Combine(SkinInfo.mpPaths.AvalonPath, "BasicHome.weather.xml");

                string sourceFile2 = Path.Combine(SkinInfo.mpPaths.AvalonPath, UseWeatherInfoservice ? "common.time.infoservice.xml" : "common.time.worldweather.xml");
                string destinationFile2 = Path.Combine(SkinInfo.mpPaths.AvalonPath, "common.time.xml");
                
                try
                {
                    Log.Info("Setting Weather Data to: {0}", UseWeatherInfoservice ? "Infoservice" : "WorldWeather");
                    File.Copy(sourceFile, destinationFile, true);
                    File.Copy(sourceFile2, destinationFile2, true);
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to copy weather data file: {0}", ex.Message);
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

            TextColorTemp = MiscConfigGUI.TextColor;
            TextColor2Temp = MiscConfigGUI.TextColor2;
            TextColor3Temp = MiscConfigGUI.TextColor3;
            WatchedColorTemp = MiscConfigGUI.WatchedColor;
            RemoteColorTemp = MiscConfigGUI.RemoteColor;

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
                case (int)GUIControls.ListColors:
                    ShowListColorsMenu();
                    break;
                case (int)GUIControls.trailerSite:
                    ShowTrailersMenu();
                    break;
                case (int)GUIControls.WeatherInfoservice:
                    break;
            }
            base.OnClicked(controlId, control, actionType);
        }

        #endregion
    }
}
