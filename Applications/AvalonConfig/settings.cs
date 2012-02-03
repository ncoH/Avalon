using System;
using System.Collections.Generic;
using System.Globalization;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Util;
using System.Xml;
using System.IO;

namespace AvalonGUIConfig
{
    class settings
    {

        #region XML Configuration Sections
        public const string cXMLSectionMovingPictures = "MovingPicturesConfig";
        public const string cXMLSectionTV = "TVConfig";
        public const string cXMLSectionTVSeries = "TVSeriesConfig";
        public const string cXMLSectionUpdate = "UpdateConfig";
        public const string cXMLSectionMyFilms = "MyFilmsConfig";
        public const string cXMLSectionMisc = "MiscConfig";
        public const string cXMLSectionVideo = "VideoConfig";
        #endregion

        #region XML Configuration Strings
        public const string cXMLSettingMovingPicturesThumbMod = "MovingPicturesThumbMod";
        public const string cXMLSettingMovingPicturesView = "MovingPicturesInfo";

        public const string cXMLSettingMyFilmsThumbMod = "MyFilmsThumbMod";
        public const string cXMLSettingMyFilmsView = "MyFilmsInfo";

        public const string cXMLSettingTVMiniGuideSize = "TVMiniGuideSize";
        public const string cXMLSettingTVGuideSize = "TVGuideSize";
        public const string cXMLSettingTVHomeLayout = "TVHomeLayout";

        public const string cXMLSettingTVSeriesWidebannerMod = "TVSeriesWidebannerMod";
        public const string cXMLSettingTVSeriesView = "TVSeriesInfo";

        public const string cXMLSettingUpdateCheckOnStart = "updateCheckOnStart";
        public const string cXMLSettingUpdateCheckForUpdateAt = "updateCheckForUpdateAt";
        public const string cXMLSettingUpdateCheckInterval = "updateCheckInterval";
        public const string cXMLSettingUpdateCheckTime = "updateCheckTime";
        public const string cXMLSettingUpdateRunPatchUtilityUnattended = "updateRunPatchUtilityUnattended";
        public const string cXMLSettingUpdateNextUpdateCheck = "updateNextUpdateCheck";
        public const string cXMLSettingUpdatePatchUtilityRestartMP = "updatePatchUtilityRestartMP";
        public const string cXMLSettingUpdatePatchAppliedLastRun = "updatePatchAppliedLastRun";

        public const string cXMLSettingMiscHidePoster = "MiscHidePoster";
        public const string cXMLSettingMiscShowRSS = "MiscShowRSS";
        public const string cXMLSettingMiscShowHiddenMenu = "MiscShowHiddenMenu";
        public const string cXMLSettingMiscShow5DayWeather = "MiscShow5DayWeather";
        public const string cXMLSettingMiscTrailerSite = "MiscTrailerSite";

        public const string cXMLSettingMiscUnfocusedAlphaListItems = "miscUnfocusedAlphaListItems";
        public const string cXMLSettingMiscUnfocusedAlphaThumbs = "miscUnfocusedAlphaThumbs";
        public const string cXMLSettingMiscTextColor = "miscTextColor";
        public const string cXMLSettingMiscTextColor2 = "miscTextColor2";
        public const string cXMLSettingMiscTextColor3 = "miscTextColor3";
        public const string cXMLSettingMiscWatchedColor = "miscWatchedColor";
        public const string cXMLSettingMiscRemoteColor = "miscRemoteColor";

        public const string cXMLSettingMiscUseLargeFonts = "miscUseLargeFonts";

        public const string cXMLSettingVideoThumbMod = "VideoThumbMod";
        public const string cXMLSettingVideoView = "VideoInfo";

        public const string cXMLSettingMiscUseWeatherInfoservice = "miscUseWeatherInfoservice";

        #endregion

        #region Public Properties

        public bool mpSetAsFullScreen
        {
            get
            {
                return _mpSetAsFullScreen();
            }
        }

        #endregion

        public static string CurrentSkin { get { return GUIGraphicsContext.Skin; } }
        public static string PreviousSkin { get; set; }

        public static string CurrentLanguage
        {
            get
            {
                string language = string.Empty;
                try
                {
                    language = GUILocalizeStrings.GetCultureName(GUILocalizeStrings.CurrentLanguage());
                }
                catch (Exception)
                {
                    language = CultureInfo.CurrentUICulture.Name;
                }
                return language;
            }
        }

        public static string PreviousLanguage { get; set; }

        #region Private Methods

        bool _mpSetAsFullScreen()
        {
            using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml")))
            {
                return xmlreader.GetValueAsBool("general", "startfullscreen", false);
            }
        }

        #endregion

        public static void Load(string section)
        {
            Log.Info(string.Format("AvalonConfig: Settings.Load({0})", section));
            using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "Avalon.xml")))
            {
                switch (section)
                {
                    #region MovingPictures
                    case settings.cXMLSectionMovingPictures:
                        MovingPicturesConfig.IsDefaultStyle = xmlreader.GetValueAsInt(section, settings.cXMLSettingMovingPicturesView, 1) == 1;
                        MovingPicturesConfig.ThumbViewMod = (MovingPicturesConfig.Views)xmlreader.GetValueAsInt(section, settings.cXMLSettingMovingPicturesThumbMod, 0);
                        break;
                    #endregion

                    #region TV
                    case settings.cXMLSectionTV:
                        TVConfig.TVMiniGuideRowSize = (TVConfig.TVMiniGuideRows)xmlreader.GetValueAsInt(section, settings.cXMLSettingTVMiniGuideSize, 3);
                        TVConfig.TVGuideRowSize = (TVConfig.TVGuideRows)xmlreader.GetValueAsInt(section, settings.cXMLSettingTVGuideSize, 8);
                        TVConfig.TVHomeLayoutType = (TVConfig.TVHomeLayout)xmlreader.GetValueAsInt(section, settings.cXMLSettingTVHomeLayout, 0);
                        break;
                    #endregion

                    #region TVSeries
                    case settings.cXMLSectionTVSeries:
                        TVSeriesConfig.IsDefaultStyle = xmlreader.GetValueAsInt(section, "TVSeriesInfo", 1) == 1;
                        TVSeriesConfig.widebannerMod = (TVSeriesConfig.Views)xmlreader.GetValueAsInt(section, settings.cXMLSettingTVSeriesWidebannerMod, 0);
                        break;
                    #endregion

                    #region Update
                    case settings.cXMLSectionUpdate:
                        if (xmlreader.GetValueAsInt(section, settings.cXMLSettingUpdateCheckOnStart, 1) != 0)
                            AvalonGUIConfig.checkOnStart = true;
                        if (xmlreader.GetValueAsInt(section, settings.cXMLSettingUpdateCheckForUpdateAt, 1) != 0)
                            AvalonGUIConfig.checkForUpdateAt = true;
                        if (xmlreader.GetValueAsInt(section, settings.cXMLSettingUpdateRunPatchUtilityUnattended, 1) != 0)
                            AvalonGUIConfig.patchUtilityRunUnattended = true;
                        if (xmlreader.GetValueAsInt(section, settings.cXMLSettingUpdatePatchUtilityRestartMP, 1) != 0)
                            AvalonGUIConfig.patchUtilityRestartMP = true;
                        if (xmlreader.GetValueAsInt(section, settings.cXMLSettingUpdatePatchAppliedLastRun, 0) != 0)
                            AvalonGUIConfig.patchAppliedLastRun = true;

                        if (AvalonGUIConfig.checkForUpdateAt)
                        {
                            string checkTime = xmlreader.GetValueAsString(section, settings.cXMLSettingUpdateCheckTime, "");

                            if (string.IsNullOrEmpty(checkTime))
                            {
                                AvalonGUIConfig.checkTime = DateTime.Parse("03:00");
                                AvalonGUIConfig.checkInterval = 1;
                            }
                            else
                            {
                                DateTime dtCheckTime = new DateTime();
                                if (!DateTime.TryParse(checkTime, CultureInfo.CurrentCulture, DateTimeStyles.None, out dtCheckTime))
                                    dtCheckTime = DateTime.Now;
                                AvalonGUIConfig.checkTime = dtCheckTime;

                                AvalonGUIConfig.nextUpdateCheck = xmlreader.GetValueAsString(section, settings.cXMLSettingUpdateNextUpdateCheck, "");
                                AvalonGUIConfig.checkInterval = xmlreader.GetValueAsInt(section, settings.cXMLSettingUpdateCheckInterval, 1);
                            }
                        }
                        break;
                    #endregion

                    #region MyFilms
                    case settings.cXMLSectionMyFilms:
                        MyFilmsConfig.IsDefaultStyle = xmlreader.GetValueAsInt(section, settings.cXMLSettingMyFilmsView, 1) == 1;
                        MyFilmsConfig.ThumbViewMod = (MyFilmsConfig.Views)xmlreader.GetValueAsInt(section, settings.cXMLSettingMyFilmsThumbMod, 0);
                        break;
                    #endregion

                    #region Misc
                    case settings.cXMLSectionMisc:
                        MiscConfigGUI.HidePoster = xmlreader.GetValueAsInt(section, settings.cXMLSettingMiscHidePoster, 0) == 1;
                        MiscConfigGUI.showRSS = xmlreader.GetValueAsInt(section, settings.cXMLSettingMiscShowRSS, 1) == 1;
                        MiscConfigGUI.showHiddenMenu = xmlreader.GetValueAsInt(section, settings.cXMLSettingMiscShowHiddenMenu, 1) == 1;
                        MiscConfigGUI.UnfocusedAlphaListItems = xmlreader.GetValueAsInt(section, settings.cXMLSettingMiscUnfocusedAlphaListItems, 150);
                        MiscConfigGUI.UnfocusedAlphaThumbs = xmlreader.GetValueAsInt(section, settings.cXMLSettingMiscUnfocusedAlphaThumbs, 150);
                        MiscConfigGUI.UseLargeFonts = xmlreader.GetValueAsInt(section, settings.cXMLSettingMiscUseLargeFonts, 0) == 1;
                        MiscConfigGUI.showFiveDayWeather = xmlreader.GetValueAsInt(section, settings.cXMLSettingMiscShow5DayWeather, 1) == 1;
                        MiscConfigGUI.siteUtil = xmlreader.GetValueAsInt(section, settings.cXMLSettingMiscTrailerSite, 0);
                        MiscConfigGUI.TextColor = xmlreader.GetValueAsString(section, settings.cXMLSettingMiscTextColor, "FFFFFF");
                        MiscConfigGUI.TextColor2 = xmlreader.GetValueAsString(section, settings.cXMLSettingMiscTextColor2, "FFFFFF");
                        MiscConfigGUI.TextColor3 = xmlreader.GetValueAsString(section, settings.cXMLSettingMiscTextColor3, "FFFFFF");
                        MiscConfigGUI.WatchedColor = xmlreader.GetValueAsString(section, settings.cXMLSettingMiscWatchedColor, "85CFFF");
                        MiscConfigGUI.RemoteColor = xmlreader.GetValueAsString(section, settings.cXMLSettingMiscRemoteColor, "FC7B19");
                        MiscConfigGUI.UseWeatherInfoservice = xmlreader.GetValueAsInt(section, settings.cXMLSettingMiscUseWeatherInfoservice, 0) == 1;
                        break;
                    #endregion

                    #region Video
                    case settings.cXMLSectionVideo:
                        VideoConfig.IsDefaultStyle = xmlreader.GetValueAsInt(section, settings.cXMLSettingVideoView, 1) == 1;
                        VideoConfig.ThumbViewMod = (VideoConfig.Views)xmlreader.GetValueAsInt(section, settings.cXMLSettingVideoThumbMod, 0);
                        break;
                    #endregion
                }
            }
        }


        public static void Save(string section)
        {
            using (MediaPortal.Profile.Settings xmlwriter = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "Avalon.xml")))
            {
                switch (section)
                {
                    #region MovingPictures
                    case settings.cXMLSectionMovingPictures:
                        xmlwriter.SetValue(section, cXMLSettingMovingPicturesView, MovingPicturesConfig.IsDefaultStyle ? 1 : 0);
                        xmlwriter.SetValue(section, cXMLSettingMovingPicturesThumbMod, (int)MovingPicturesConfig.ThumbViewMod);
                        break;
                    #endregion

                    #region TV
                    case settings.cXMLSectionTV:
                        xmlwriter.SetValue(section, cXMLSettingTVMiniGuideSize, (int)TVConfig.TVMiniGuideRowSize);
                        xmlwriter.SetValue(section, cXMLSettingTVGuideSize, (int)TVConfig.TVGuideRowSize);
                        xmlwriter.SetValue(section, cXMLSettingTVHomeLayout, (int)TVConfig.TVHomeLayoutType);
                        break;
                    #endregion

                    #region TVSeries
                    case settings.cXMLSectionTVSeries:
                        xmlwriter.SetValue(section, cXMLSettingTVSeriesView, TVSeriesConfig.IsDefaultStyle ? 1 : 0);
                        xmlwriter.SetValue(section, cXMLSettingTVSeriesWidebannerMod, (int)TVSeriesConfig.widebannerMod);
                        break;
                    #endregion

                    #region Update
                    case settings.cXMLSectionUpdate:
                        xmlwriter.SetValue(section, settings.cXMLSettingUpdateCheckOnStart, AvalonGUIConfig.checkOnStart ? 1 : 0);
                        xmlwriter.SetValue(section, settings.cXMLSettingUpdateCheckForUpdateAt, AvalonGUIConfig.checkForUpdateAt ? 1 : 0);
                        xmlwriter.SetValue(section, settings.cXMLSettingUpdateCheckInterval, AvalonGUIConfig.checkInterval);
                        xmlwriter.SetValue(section, settings.cXMLSettingUpdateCheckTime, AvalonGUIConfig.checkTime.ToShortTimeString());
                        xmlwriter.SetValue(section, settings.cXMLSettingUpdateNextUpdateCheck, AvalonGUIConfig.nextUpdateCheck);
                        xmlwriter.SetValue(section, settings.cXMLSettingUpdateRunPatchUtilityUnattended, AvalonGUIConfig.patchUtilityRunUnattended ? 1 : 0);
                        xmlwriter.SetValue(section, settings.cXMLSettingUpdatePatchUtilityRestartMP, AvalonGUIConfig.patchUtilityRestartMP ? 1 : 0);
                        xmlwriter.SetValue(section, settings.cXMLSettingUpdatePatchAppliedLastRun, AvalonGUIConfig.patchAppliedLastRun ? 1 : 0);
                        break;
                    #endregion

                    #region MyFilms
                    case settings.cXMLSectionMyFilms:
                        xmlwriter.SetValue(section, cXMLSettingMyFilmsView, MyFilmsConfig.IsDefaultStyle ? 1 : 0);
                        xmlwriter.SetValue(section, cXMLSettingMyFilmsThumbMod, (int)MyFilmsConfig.ThumbViewMod);
                        break;
                    #endregion

                    #region Misc
                    case settings.cXMLSectionMisc:
                        xmlwriter.SetValue(section, cXMLSettingMiscHidePoster, MiscConfigGUI.HidePoster ? 1 : 0);
                        xmlwriter.SetValue(section, cXMLSettingMiscShowRSS, MiscConfigGUI.showRSS ? 1 : 0);
                        xmlwriter.SetValue(section, cXMLSettingMiscShowHiddenMenu, MiscConfigGUI.showHiddenMenu ? 1 : 0);
                        xmlwriter.SetValue(section, settings.cXMLSettingMiscUnfocusedAlphaListItems, MiscConfigGUI.UnfocusedAlphaListItems);
                        xmlwriter.SetValue(section, settings.cXMLSettingMiscUnfocusedAlphaThumbs, MiscConfigGUI.UnfocusedAlphaThumbs);
                        xmlwriter.SetValue(section, settings.cXMLSettingMiscUseLargeFonts, MiscConfigGUI.UseLargeFonts ? 1 : 0);
                        xmlwriter.SetValue(section, settings.cXMLSettingMiscShow5DayWeather, MiscConfigGUI.showFiveDayWeather ? 1 : 0);
                        xmlwriter.SetValue(section, settings.cXMLSettingMiscTrailerSite, MiscConfigGUI.siteUtil);
                        xmlwriter.SetValue(section, settings.cXMLSettingMiscTextColor, MiscConfigGUI.TextColor);
                        xmlwriter.SetValue(section, settings.cXMLSettingMiscTextColor2, MiscConfigGUI.TextColor2);
                        xmlwriter.SetValue(section, settings.cXMLSettingMiscTextColor3, MiscConfigGUI.TextColor3);
                        xmlwriter.SetValue(section, settings.cXMLSettingMiscRemoteColor, MiscConfigGUI.RemoteColor);
                        xmlwriter.SetValue(section, settings.cXMLSettingMiscWatchedColor, MiscConfigGUI.WatchedColor);
                        xmlwriter.SetValue(section, settings.cXMLSettingMiscUseWeatherInfoservice, MiscConfigGUI.UseWeatherInfoservice ? 1 : 0);
                        break;
                    #endregion

                    #region Video
                    case settings.cXMLSectionVideo:
                        xmlwriter.SetValue(section, cXMLSettingVideoView, VideoConfig.IsDefaultStyle ? 1 : 0);
                        xmlwriter.SetValue(section, cXMLSettingVideoThumbMod, (int)VideoConfig.ThumbViewMod);
                        break;
                    #endregion
                }
            }
        }
    }
}
