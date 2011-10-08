using System;
using System.Collections.Generic;
using System.Text;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;

namespace AvalonGUIConfig
{
    public class TVConfig : GUIWindow
    {
        #region Enums
        private enum GUIControls
        {
            TVMiniGuideSize = 2,
            TVGuideSize = 3
        }

    public enum TVMiniGuideRows
    {
      Three = 3,
      Eight = 8,
    }

    public enum TVGuideRows
    {
        Eight = 8,
        Ten = 10
    }

        #endregion

        #region Skin Controls
    [SkinControl((int)GUIControls.TVMiniGuideSize)]
    protected GUIButtonControl btnTVMiniGuideSize = null;

    [SkinControl((int)GUIControls.TVGuideSize)]
    protected GUIButtonControl btnTVGuideSize = null;

        #endregion

        #region Constructor
        public TVConfig() { }
        #endregion

        #region Public Properties
        public static TVMiniGuideRows TVMiniGuideRowSize { get; set; }
        public static TVGuideRows TVGuideRowSize { get; set; }
        #endregion

        #region Private Methods
        private void SetControlStates()
        {
            btnTVMiniGuideSize.Label = GetTVMiniGuideSizeName(TVMiniGuideRowSize);
            btnTVGuideSize.Label = GetTVGuideSizeName(TVGuideRowSize);
        }

        private void GetControlStates() { }

        private void ShowTVGuideContextMenu()
        {
            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg == null) return;

            dlg.Reset();
            dlg.SetHeading(Translation.TVGuideSize);

            foreach (int value in Enum.GetValues(typeof(TVGuideRows)))
            {
                TVGuideRows rows = (TVGuideRows)Enum.Parse(typeof(TVGuideRows), value.ToString());
                string label = GetTVGuideSizeName(rows);

                // Create new item
                GUIListItem listItem = new GUIListItem(label);
                listItem.ItemId = value;

                // Set selected if current
                if (rows == TVGuideRowSize) listItem.Selected = true;

                // Add new item to context menu
                dlg.Add(listItem);
            }

            dlg.DoModal(GUIWindowManager.ActiveWindow);
            if (dlg.SelectedId <= 0) return;

            // Set new Selection
            TVGuideRowSize = (TVGuideRows)Enum.GetValues(typeof(TVGuideRows)).GetValue(dlg.SelectedLabel);
            btnTVGuideSize.Label = dlg.SelectedLabelText;
        }

        private void ShowTVMiniGuideContextMenu()
        {
            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg == null) return;

            dlg.Reset();
            dlg.SetHeading(Translation.TVMiniGuideSize);

            foreach (int value in Enum.GetValues(typeof(TVMiniGuideRows)))
            {
                TVMiniGuideRows rows = (TVMiniGuideRows)Enum.Parse(typeof(TVMiniGuideRows), value.ToString());
                string label = GetTVMiniGuideSizeName(rows);

                // Create new item
                GUIListItem listItem = new GUIListItem(label);
                listItem.ItemId = value;

                // Set selected if current
                if (rows == TVMiniGuideRowSize) listItem.Selected = true;

                // Add new item to context menu
                dlg.Add(listItem);
            }

            dlg.DoModal(GUIWindowManager.ActiveWindow);
            if (dlg.SelectedId <= 0) return;

            // Set new Selection
            TVMiniGuideRowSize = (TVMiniGuideRows)Enum.GetValues(typeof(TVMiniGuideRows)).GetValue(dlg.SelectedLabel);
            btnTVMiniGuideSize.Label = dlg.SelectedLabelText;
        }

        /// <summary>
        /// Returns a Translated name for selected TVGuide Size
        /// </summary>
        /// <param name="widebanner">TVGuideSize enum to translate</param>
        /// <returns>Translated Name</returns>
        private string GetTVMiniGuideSizeName(TVMiniGuideRows rows)
        {
            switch (rows)
            {
                case TVMiniGuideRows.Three:
                    return Translation.TVMiniGuideThreeRows;

                case TVMiniGuideRows.Eight:
                    return Translation.TVMiniGuideEightRows;

                default:
                    return Translation.TVMiniGuideSize;
            }
        }

        private string GetTVGuideSizeName(TVGuideRows rows)
        {
            switch (rows)
            {
                case TVGuideRows.Eight:
                    return Translation.TVGuideEightRows;

                case TVGuideRows.Ten:
                    return Translation.TVGuideTenRows;

                default:
                    return Translation.TVGuideEightRows;
            }
        }

        /// <summary>
        /// Apply changes to MovingPictures.xml
        /// </summary>
        private void ApplyConfigurationChanges()
        {
            // TVMiniGuide Imports exist in TVMiniGuide.xml
            string skinFile = skinFile = GUIGraphicsContext.Skin + @"\TVMiniGuide.xml";
            AvalonHelper.SetSkinImport(skinFile, "TVMiniGuide", string.Format("TVMiniGuide.{0}rows.xml", (int)TVMiniGuideRowSize));

        }

        public static void SetTVGuideSize()
        {
            // TVGuide Imports exist in mytvguide.xml and dialogTvGuide.xml
            string skinFile = GUIGraphicsContext.Skin + @"\mytvguide.xml";
            AvalonHelper.SetSkinImport(skinFile, "TVGuideChannels", string.Format("mytvguide.{0}rows.xml", (int)TVGuideRowSize));

            // 4TR TVGuide
            skinFile = GUIGraphicsContext.Skin + @"\4TR_TvGuide.xml";
            AvalonHelper.SetSkinImport(skinFile, "TVGuideChannels", string.Format("4TR_TvGuide.{0}rows.xml", (int)TVGuideRowSize));
        }
        #endregion

        #region Base Overrides
        public override int GetID
        {
            get
            {
                return (int)AvalonGUIConfig.AvalonScreenID.AvalonTVConfig;
            }
        }

        public override bool Init()
        {
            return Load(GUIGraphicsContext.Skin + @"\AvalonConfig_tv.xml");
        }

        protected override void OnPageLoad()
        {
            // Load Settings
            settings.Load(settings.cXMLSectionTV);

            // Update Controls
            SetControlStates();
        }

        protected override void OnPageDestroy(int newWindowId)
        {
            // Get Current State of controls
            GetControlStates();

            // Apply Configuration changes
            ApplyConfigurationChanges();
            SetTVGuideSize();

            // Save Settings
            settings.Save(settings.cXMLSectionTV);
        }

        protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
        {
            switch (controlId)
            {
                case (int)GUIControls.TVGuideSize:
                    ShowTVGuideContextMenu();
                    break;
                case (int)GUIControls.TVMiniGuideSize:
                    ShowTVMiniGuideContextMenu();
                   break;
            }
            base.OnClicked(controlId, control, actionType);
        }

        #endregion
    }
}
