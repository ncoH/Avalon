using System;
using System.Collections.Generic;
using System.Text;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;

namespace AvalonGUIConfig
{
    public class TVSeriesConfig : GUIWindow
    {
        #region Enums
        private enum GUIControls
        {
            Style = 2,
            widebannerMod = 3
        }

        public enum Views
        {
            Default,
            BannerNoFanart,
        }
        #endregion

        #region Skin Controls
       [SkinControl((int)GUIControls.Style)]
       protected GUIToggleButtonControl btnStyle = null;

       [SkinControl((int)GUIControls.widebannerMod)]
       protected GUIButtonControl btnwidebannerMod = null;

        #endregion

        #region Constructor
        public TVSeriesConfig() { }
        #endregion

        #region Public Properties
        public static bool IsDefaultStyle { get; set; }
        public static Views widebannerMod { get; set; }
        #endregion

        #region Private Methods
        private void SetControlStates()
        {
            // Set Toggle Button if Default or Fanart Style
            btnStyle.Selected = !IsDefaultStyle;
            //btnStyle.Label = "Enable Info";

            // Set Label for Current WideBanner Mod
            btnwidebannerMod.Label = GetWidebannerName(widebannerMod);
        }

        private void GetControlStates()
        {

            // Get Default/NoInfo Style
            IsDefaultStyle = !btnStyle.Selected;
        }

        private void ShowWidebannerContextMenu()
        {
            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg == null) return;

            dlg.Reset();
            dlg.SetHeading(Translation.Thumbnails);

            foreach (int value in Enum.GetValues(typeof(Views)))
            {
                Views banner = (Views)Enum.Parse(typeof(Views), value.ToString());
                string label = GetWidebannerName(banner);

                // Create new item
                GUIListItem listItem = new GUIListItem(label);
                listItem.ItemId = value;

                // Set selected if current
                if (banner == widebannerMod) listItem.Selected = true;

                // Add new item to context menu
                dlg.Add(listItem);
            }

            dlg.DoModal(GUIWindowManager.ActiveWindow);
            if (dlg.SelectedId <= 0) return;

            // Set new Selection
            widebannerMod = (Views)Enum.GetValues(typeof(Views)).GetValue(dlg.SelectedLabel);
            btnwidebannerMod.Label = dlg.SelectedLabelText;
        }

        /// <summary>
        /// Returns a Translated name for selected Thumbnail Mod
        /// </summary>
        /// <param name="widebanner">Thumbnail enum to translate</param>
        /// <returns>Translated Name</returns>
        private string GetWidebannerName(Views thumb)
        {
            switch (thumb)
            {
                case Views.Default:
                    return Translation.WidebannerDefault;

                case Views.BannerNoFanart:
                    return Translation.WidebannerNoFanart;

                default:
                    return Translation.Widebanner;
            }
        } 

        /// <summary>
        /// Apply changes to TVSeries.xml
        /// </summary>
        private void ApplyConfigurationChanges()
        {
            string skinFile = GUIGraphicsContext.Skin + @"\TVSeries.xml";

            string style = btnStyle.Selected ? "default" : "noinfo";

            // Set <import> paths

            AvalonHelper.SetSkinImport(skinFile, "Background", string.Format("TVSeries.background.{0}.xml", style));
            AvalonHelper.SetSkinImport(skinFile, "Views", string.Format("TVSeries.views.{0}.xml", style));
            AvalonHelper.SetSkinImport(skinFile, "Facade", string.Format("TVSeries.facade.{0}.xml", style));
            //Log.Info(skinFile);

            switch (widebannerMod)
            {
                case Views.Default:
                    AvalonHelper.SetSkinImport(skinFile, "Background", string.Format("TVSeries.background.{0}.xml", style));
                    AvalonHelper.SetSkinImport(skinFile, "Views", string.Format("TVSeries.views.{0}.xml", style));
                    AvalonHelper.SetSkinImport(skinFile, "Facade", string.Format("TVSeries.facade.{0}.xml", style));
                    break;

                case Views.BannerNoFanart:
                    AvalonHelper.SetSkinImport(skinFile, "Background", string.Format("TVSeries.background.{0}.widebanner.nofanart.xml", style));
                    AvalonHelper.SetSkinImport(skinFile, "Views", string.Format("TVSeries.views.{0}.widebanner.nofanart.xml", style));
                    AvalonHelper.SetSkinImport(skinFile, "Facade", string.Format("TVSeries.facade.{0}.widebanner.nofanart.xml", style));
                    break;

            }

        }
        #endregion

        #region Base Overrides
        public override int GetID
        {
            get
            {
                return (int)AvalonGUIConfig.AvalonScreenID.AvalonTVSeriesConfig;
            }
        }

        public override bool Init()
        {
            return Load(GUIGraphicsContext.Skin + @"\AvalonConfig_tvseries.xml");
        }

        protected override void OnPageLoad()
        {
            // Load Settings
            settings.Load(settings.cXMLSectionTVSeries);

            // Update Controls
            SetControlStates();
        }

        protected override void OnPageDestroy(int newWindowId)
        {
            // Get Current State of controls
            GetControlStates();

            // Apply Configuration changes
            ApplyConfigurationChanges();

            // Save Settings
            settings.Save(settings.cXMLSectionTVSeries);
        }

        protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
        {
            switch (controlId)
            {
                case (int)GUIControls.Style:
                    break;

                case (int)GUIControls.widebannerMod:
                    ShowWidebannerContextMenu();
                    break;

            }
            base.OnClicked(controlId, control, actionType);
        }

        #endregion
    }
}
