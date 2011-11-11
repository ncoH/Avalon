using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Text.RegularExpressions;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;

namespace AvalonGUIConfig
{
    class Translation
    {
        #region Private variables

        private static Dictionary<string, string> _translations;
        private static Dictionary<string, string> DynamicTranslations = new Dictionary<string, string>();
        private static readonly string _path = string.Empty;
        private static readonly DateTimeFormatInfo _info;

        #endregion

        #region Constructor

        static Translation()
        {
            _info = DateTimeFormatInfo.GetInstance(CultureInfo.CurrentUICulture);
            _path = Config.GetSubFolder(Config.Dir.Language, "Avalon");
        }

        #endregion

        #region Public Properties

        public static Dictionary<string, string> FixedTranslations = new Dictionary<string, string>();

        /// <summary>
        /// Gets the translated strings collection in the active language
        /// </summary>
        public static Dictionary<string, string> Strings
        {
            get
            {
                if (_translations == null)
                {
                    _translations = new Dictionary<string, string>();
                    Type transType = typeof(Translation);
                    var fields = transType.GetFields(BindingFlags.Public | BindingFlags.Static)
                                .Where(p => p.FieldType == typeof(string));

                    foreach (var field in fields)
                    {
                        if (DynamicTranslations.ContainsKey(field.Name))
                        {
                            if (field.GetValue(transType).ToString() != string.Empty)
                                _translations.Add(field.Name + ":" + DynamicTranslations[field.Name], field.GetValue(transType).ToString());
                        }
                        else
                        {
                            if (field.GetValue(transType).ToString() != string.Empty)
                                _translations.Add(field.Name, field.GetValue(transType).ToString());
                        }
                    }
                }
                return _translations;
            }
        }

        #endregion

        #region Public Methods

        public static void Init()
        {
            // reset active translations
            _translations = null;
            FixedTranslations.Clear();

            string lang = settings.PreviousLanguage = settings.CurrentLanguage;


            if (!System.IO.Directory.Exists(_path))
                System.IO.Directory.CreateDirectory(_path);

            LoadTranslations(lang);
        }

        public static int LoadTranslations(string lang)
        {
            XmlDocument doc = new XmlDocument();
            Dictionary<string, string> TranslatedStrings = new Dictionary<string, string>();
            string langPath = "";
            try
            {
                langPath = Path.Combine(_path, lang + ".xml");
                doc.Load(langPath);
            }
            catch (Exception e)
            {
                if (lang == "en")
                    return 0; // otherwise we are in an endless loop!

                if (e.GetType() == typeof(FileNotFoundException))
                    Log.Info(string.Format("Avalon Translation: Cannot find translation file {0}.  Failing back to English", langPath));
                else
                {
                    Log.Info(string.Format("Avalon Translation: Error in translation xml file: {0}. Failing back to English", lang));
                    Log.Info("Avalon Translation:" + e.ToString());
                }

                return LoadTranslations("en");
            }
            foreach (XmlNode stringEntry in doc.DocumentElement.ChildNodes)
            {
                if (stringEntry.NodeType == XmlNodeType.Element)
                    try
                    {
                        if (stringEntry.Attributes.GetNamedItem("Field").Value.StartsWith("#"))
                        {
                            FixedTranslations.Add(stringEntry.Attributes.GetNamedItem("Field").Value, stringEntry.InnerText);
                        }
                        else
                            TranslatedStrings.Add(stringEntry.Attributes.GetNamedItem("Field").Value, stringEntry.InnerText);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Avalon Translation: Error in Translation Engine");
                        Log.Error("Avalon Translation:" + ex.ToString());
                    }
            }

            Type TransType = typeof(Translation);
            var fieldInfos = TransType.GetFields(BindingFlags.Public | BindingFlags.Static)
                             .Where(p => p.FieldType == typeof(string));

            foreach (var fi in fieldInfos)
            {
                if (TranslatedStrings != null && TranslatedStrings.ContainsKey(fi.Name))
                    TransType.InvokeMember(fi.Name, BindingFlags.SetField, null, TransType, new object[] { TranslatedStrings[fi.Name] });
                else
                {
                    // There is no hard-coded translation so create one
                    Log.Info(string.Format("Avalon Translation: Translation not found for field: {0}.  Using hard-coded English default.", fi.Name));
                }
            }
            return TranslatedStrings.Count;
        }

        public static string GetByName(string name)
        {
            if (!Strings.ContainsKey(name))
                return name;

            return Strings[name];
        }

        public static string GetByName(string name, params object[] args)
        {
            return String.Format(GetByName(name), args);
        }

        /// <summary>
        /// Takes an input string and replaces all ${named} variables with the proper translation if available
        /// </summary>
        /// <param name="input">a string containing ${named} variables that represent the translation keys</param>
        /// <returns>translated input string</returns>
        public static string ParseString(string input)
        {
            Regex replacements = new Regex(@"\$\{([^\}]+)\}");
            MatchCollection matches = replacements.Matches(input);
            foreach (Match match in matches)
            {
                input = input.Replace(match.Value, GetByName(match.Groups[1].Value));
            }
            return input;
        }

        #endregion

        #region Translations / Strings

        /// <summary>
        /// These will be loaded with the language files content
        /// if the selected lang file is not found, it will first try to load en(us).xml as a backup
        /// if that also fails it will use the hardcoded strings as a last resort.
        /// </summary>

        //Settings
        public static string SkinSettings = "SkinSettings";
        public static string SkinUpdateNF = "Update available";
        public static string SkinUpdateNF2 = "Press left and push the button to install";
        public static string SkinSettingsMovpicsLayout = "Movie Information Layout";
        public static string SkinSettingsSeriesLayout = "Series Information Layout";
        public static string SkinSettingsThumbview = "Thumbnails";
        public static string SkinSettingsInfo = "Enable Infos at bottom of the screen";
        public static string SkinSettingsPosterHide = "Hide Poster on listview when not scrolling";
        public static string SkinSettingsShowRSS = "Enable RSS Feeds on BasicHome/Home";
        public static string SkinSettingsShowHiddenMenu = "Show Hidden Menu Image";
        public static string SkinSettingsShow5DayWeather = "Show 5 Day Weather forecast on BasicHome/Home";
        public static string SkinSettingsTV = "Television";
        public static string SkinSettingsTVSeries = "TV Series";
        public static string SkinSettingsUpdate = "Update";
        public static string SkinSettingsMovingPictures = "MovingPictures";
        public static string SkinSettingsMyFilms = "MyFilms";
        public static string SkinSettingsMisc = "Misc";
        public static string SkinSettingsVideo = "Video";
        public static string Thumbnails = "Thumbnails";
        public static string ThumbnailDefault = "Thumbnails: Default";
        public static string ThumbnailsNoFanart = "Thumbnails: Without Fanart";
        public static string ThumbnailXL = "Thumbnails: XL";
        public static string TVMiniGuideSize = "TV Mini Guide Size";
        public static string TVMiniGuideThreeRows = "TV Mini Guide: 3 Rows";
        public static string TVMiniGuideEightRows = "TV Mini Guide: 8 Rows";
        public static string TVMiniGuideRows = "TV Mini Guide Rows";
        public static string TVGuideSize = "TV Guide Rows";
        public static string TVGuideEightRows = "TV Guide: 8 Rows";
        public static string TVGuideTenRows = "TV Guide: 10 Rows";
        public static string Widebanner = "WideBanners";
        public static string WidebannerDefault = "WideBanners: Default";
        public static string WidebannerNoFanart = "WideBanners: Without Fanart";
        public static string UnfocusedAlpha = "Transparency/Opacity for thumbs and list";
        public static string UnfocusedAlphaMenuLists = "Unfocused Alpha on List Layouts: {0}";
        public static string UnfocusedAlphaMenuThumbs = "Unfocused Alpha on Thumb Layouts: {0}";
        public static string UnfocusedAlphaInvalidLine1 = "Invalid value entered, please enter a value";
        public static string UnfocusedAlphaInvalidLine2 = "between 0 - 255.";
        public static string UnfocusedAlphaInvalidLine3 = "Opaque = 255, Transparent = 0";
        public static string RestoreDefaults = "Restore Defaults";
        public static string ConfigRequiresRestartLine1 = "The following configuration changes will take";
        public static string ConfigRequiresRestartLine2 = "affect on next MediaPortal restart.";
        public static string ConfigRequiresRestartLine3 = "Would you like to restart now?";
        public static string UseLargeFonts = "Use Large Fonts";
        public static string TrailerSite = "Choose Trailer Search Site";
        public static string ListColors = "Set List Colors";
        public static string ListDefaultColor = "Default Color: {0}";
        public static string ListText2Color = "Text 2 Color: {0}";
        public static string ListText3Color = "Text 3 Color: {0}";
        public static string ListWatchedColor = "Watched Color: {0}";
        public static string ListRemoteColor = "Remote Color: {0}";
        public static string ListColorInvalidLine1 = "Invalid color code entered, please enter a";
        public static string ListColorInvalidLine2 = "valid HEX code e.g. the color red is FF0000.";
        public static string ListColorInvalidLine3 = "which is '255' red, '0' green, and '0' blue.";
        public static string CustomColor = "Custom Color";


        //Update
        public static string SkinUpdate = "Skin Update";
        public static string DownloadingPatch = "Downloading Patch: {0}";
        public static string DownloadingProgress = "Downloaded {0}KB out of {1}KB ({2}%)";
        public static string UpdateInstall = "Update and Install";
        public static string NoUpdatesAvailable = "No patches are currently available";
        public static string mupdateheader = "Patch Installer Downloaded to Desktop";
        public static string mupdateline1 = "Manual Update Required - To apply close";
        public static string mupdateline2 = "MediaPortal and/or Configuration";
        public static string mupdateline3 = "and run:  {0}";
        public static string mupdateline4 = "which can be found on your Desktop";
        public static string NumPatchesInstalled = "Number of patch files installed: {0}";
        public static string PatchUpdateComplete = "Update to Skin Version: {0} Complete";

        //TV
        public static string Recording = "Recording";

        //TVSeries
        public static string View = "View";

        //MovingPictures/MyFilms
        public static string Runtime = "Runtime";
        public static string ReleaseDate = "Release date";
        public static string Minutes = "minutes";
        public static string Overview = "Start/Overview";
        public static string Description = "Description";
        public static string Comments = "Comments";
        public static string Actors = "Actors";
        public static string Techinfo = "Technical Info";
        public static string VideoFormat = "Video Format";
        public static string Resolution = "Resolution";
        public static string Framerate = "Framrate";
        public static string AudioFormat = "Audio Format";
        public static string AudioChannels = "Audio Channels";
        public static string DateAdded = "Date Added";
        public static string MediaType = "Media Type";
        public static string InfoUrl = "Info URL";
        public static string Source = "Source";
        public static string Aspectratio = "Aspectratio";
        public static string WatchedCount = "Watched Count";
        public static string AddToWatchList = "Add movie to watchlist";
        public static string AddToList = "Add movie to a list";
        public static string Rate = "Rate movie";
        public static string Trakt = "Trakt";
        public static string Watchlist = "Watchlist";
        public static string Recommendations = "Recommendations";
        public static string Trending = "Trending movies";
        public static string Shouts = "Shouts";
        public static string List = "List";
        public static string RelatedMovies = "Related Movies";
        
        //Latest Media
        public static string LatestMovies = "Latest movies";
        public static string LatestEpisodes = "Latest episodes";
        public static string LatestMusic = "Latest music";
        public static string LatestRecordings = "Latest recordings";
        
        //Misc
        public static string FanartScraping = "Fanart scraping";
        public static string ReturnListMovies = "Press left to return to movie list";
        public static string ReturnListSeries = "Press left to return to series list";
        public static string Watched = "Watched";
        public static string UnWatched = "Unwatched";
        public static string SearchMusic = "Search Music";
        public static string SearchPhrase = "Search Phrase";
        public static string SearchHistory = "Search History";
        public static string SearchFields = "Search Fields";
        public static string SearchType = "Search Type";
        public static string CaseSensitive = "Case Sensitive";
        public static string Lastplayed = "Lastplayed";
        public static string Filename = "Filename";
        public static string Playcount = "Playcount";


        #endregion
    }
}
