using System;
using System.Collections.Generic;
using System.Text;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;

namespace AvalonGUIConfig
{
    public class MyFilmsConfig : GUIWindow
    {
        #region Enums
        private enum GUIControls
        {
            Style = 2,
            ThumbViewMod = 3
        }

        public enum Views
        {
            Default,
            ThumbsNoFanart,
            ThumbsXL,
        }
        #endregion

        #region Skin Controls
        [SkinControl((int)GUIControls.Style)]
        protected GUICheckButton btnStyle = null;

        [SkinControl((int)GUIControls.ThumbViewMod)]
        protected GUIButtonControl btnThumbViewMod = null;

        #endregion

        #region Constructor
        public MyFilmsConfig() { }
        #endregion

        #region Public Properties
        public static bool IsDefaultStyle { get; set; }
        public static Views ThumbViewMod { get; set; }
        #endregion

        #region Private Methods
        private void SetControlStates()
        {
            // Set Toggle Button if Default or Fanart Style
            btnStyle.Selected = !IsDefaultStyle;
            //btnStyle.Label = "Enable Info";

            // Set Label for Current WideBanner Mod
            btnThumbViewMod.Label = GetThumbnailName(ThumbViewMod);
        }

        private void GetControlStates()
        {

            // Get Default/NoInfo Style
            IsDefaultStyle = !btnStyle.Selected;
        }

        private void ShowThumbnailContextMenu()
        {
            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg == null) return;

            dlg.Reset();
            dlg.SetHeading(Translation.Thumbnails);

            foreach (int value in Enum.GetValues(typeof(Views)))
            {
                Views thumb = (Views)Enum.Parse(typeof(Views), value.ToString());
                string label = GetThumbnailName(thumb);

                // Create new item
                GUIListItem listItem = new GUIListItem(label);
                listItem.ItemId = value;

                // Set selected if current
                if (thumb == ThumbViewMod) listItem.Selected = true;

                // Add new item to context menu
                dlg.Add(listItem);
            }

            dlg.DoModal(GUIWindowManager.ActiveWindow);
            if (dlg.SelectedId <= 0) return;

            // Set new Selection
            ThumbViewMod = (Views)Enum.GetValues(typeof(Views)).GetValue(dlg.SelectedLabel);
            btnThumbViewMod.Label = dlg.SelectedLabelText;
        }

        /// <summary>
        /// Returns a Translated name for selected Thumbnail Mod
        /// </summary>
        /// <param name="widebanner">Thumbnail enum to translate</param>
        /// <returns>Translated Name</returns>
        private string GetThumbnailName(Views thumb)
        {
            switch (thumb)
            {
                case Views.Default:
                    return Translation.ThumbnailDefault;

                case Views.ThumbsNoFanart:
                    return Translation.ThumbnailsNoFanart;

                case Views.ThumbsXL:
                    return Translation.ThumbnailXL;

                default:
                    return Translation.Thumbnails;
            }
        }

        /// <summary>
        /// Apply changes to MovingPictures.xml
        /// </summary>
        private void ApplyConfigurationChanges()
        {
            string skinFile = GUIGraphicsContext.Skin + @"\MyFilms.xml";

            string style = btnStyle.Selected ? "default" : "noinfo";

            // Set <import> paths

            AvalonHelper.SetSkinImport(skinFile, "Background", string.Format("MyFilms.background.{0}.xml", style));
            AvalonHelper.SetSkinImport(skinFile, "Views", string.Format("MyFilms.views.{0}.xml", style));
            AvalonHelper.SetSkinImport(skinFile, "Facade", string.Format("MyFilms.facade.{0}.xml", style));
            AvalonHelper.SetSkinImport(skinFile, "Mediainfo", string.Format("MyFilms.mediainfo.{0}.xml", style));
            //Log.Info(skinFile);

            switch (ThumbViewMod)
            {
                case Views.Default:
                    AvalonHelper.SetSkinImport(skinFile, "Background", string.Format("MyFilms.background.{0}.xml", style));
                    AvalonHelper.SetSkinImport(skinFile, "Facade", string.Format("MyFilms.facade.{0}.xml", style));
                    break;

                case Views.ThumbsNoFanart:
                    AvalonHelper.SetSkinImport(skinFile, "Background", string.Format("MyFilms.background.{0}.thumbs.nofanart.xml", style));
                    AvalonHelper.SetSkinImport(skinFile, "Facade", string.Format("MyFilms.facade.{0}.thumbs.nofanart.xml", style));
                    break;

                case Views.ThumbsXL:
                    AvalonHelper.SetSkinImport(skinFile, "Background", string.Format("MyFilms.background.{0}.thumbs.XL.xml", style));
                    AvalonHelper.SetSkinImport(skinFile, "Facade", string.Format("MyFilms.facade.{0}.thumbs.XL.xml", style));
                    break;
            }


        }
        #endregion

        #region Base Overrides
        public override int GetID
        {
            get
            {
                return (int)AvalonGUIConfig.AvalonScreenID.AvalonMyFilmsConfig;
            }
        }

        public override bool Init()
        {
            return Load(GUIGraphicsContext.Skin + @"\AvalonConfig_MyFilms.xml");
        }

        protected override void OnPageLoad()
        {
            // Load Settings
            settings.Load(settings.cXMLSectionMyFilms);

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
            settings.Save(settings.cXMLSectionMyFilms);
        }

        protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
        {
            switch (controlId)
            {
                case (int)GUIControls.Style:
                    break;

                case (int)GUIControls.ThumbViewMod:
                    ShowThumbnailContextMenu();
                    break;

            }
            base.OnClicked(controlId, control, actionType);
        }

        #endregion
    }
}
