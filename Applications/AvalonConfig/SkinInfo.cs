using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Microsoft.Win32;

namespace AvalonGUIConfig
{
    class SkinInfo
    {
        #region Enums

        public enum errorCode
        {
            info,
            infoQuestion,
            loadError,
            readError,
            major,
        };

        #endregion

        #region Structs

        public struct editorPaths
        {
            public string sMPbaseDir;
            public string skinBasePath;
            public string cacheBasePath;
            public string configBasePath;
            public string langBasePath;
            public string AvalonPath;
            public string pluginPath;
            public string databasePath;
            public string thumbsPath;
        }

        #endregion

        #region Variables
        // Private Variables
        // Protected Variables
        // Public Variables
        public static editorPaths mpPaths = new editorPaths();

        #endregion

        #region Constructor

        public SkinInfo()
        {
            GetMediaPortalPath(ref mpPaths);
            readMediaPortalDirs();
        }

        #endregion

        #region Public methods

        public string configuredSkin
        {
            get
            {
                return readMPConfiguration("skin", "name");
            }
        }

        public string minimiseMPOnExit
        {
            get
            {
                return readMPConfiguration("general", "minimizeonexit");
            }
        }

        #endregion

        #region Private methods

        void GetMediaPortalPath(ref editorPaths mpPaths)
        {
            string sRegRoot = "SOFTWARE";
            if (IntPtr.Size > 4)
                sRegRoot += "\\Wow6432Node";
            try
            {
                RegistryKey MediaPortalKey = Registry.LocalMachine.OpenSubKey(sRegRoot + "\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\MediaPortal\\", false);
                if (MediaPortalKey != null)
                {
                    mpPaths.sMPbaseDir = MediaPortalKey.GetValue("InstallPath").ToString();
                }
                else
                {
                    MediaPortalKey = MediaPortalKey.OpenSubKey(sRegRoot + "\\Team MediaPortal\\MediaPortal\\", false);
                    if (MediaPortalKey != null)
                    {
                        mpPaths.sMPbaseDir = MediaPortalKey.GetValue("ApplicationDir").ToString();
                    }
                    else
                        mpPaths.sMPbaseDir = null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception while attempting to read MediaPortal location from registry\n\nMediaPortal must be installed, is MediaPortal Installed?\n\n" + e.Message.ToString());
                mpPaths.sMPbaseDir = null;
            }
        }

        void readMediaPortalDirs()
        {
            // Check if user MediaPortalDirs.xml exists in Personal Directory
            string PersonalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string fMPdirs = Path.Combine(PersonalFolder, @"Team MediaPortal\MediaPortalDirs.xml");

            if (!File.Exists(fMPdirs))
                fMPdirs = mpPaths.sMPbaseDir + "\\MediaPortalDirs.xml";

            XmlDocument doc = new XmlDocument();
            if (!File.Exists(fMPdirs))
            {
                MessageBox.Show("Can't find MediaPortalDirs.xml \r\r" + fMPdirs);
                return;
            }
            doc.Load(fMPdirs);
            XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/Config/Dir");
            foreach (XmlNode node in nodeList)
            {
                XmlNode innerNode = node.Attributes.GetNamedItem("id");
                // get the Skin base path
                if (innerNode.InnerText == "Skin")
                {
                    XmlNode path = node.SelectSingleNode("Path");
                    if (path != null)
                    {
                        mpPaths.skinBasePath = GetMediaPortalDir(path.InnerText);
                    }
                }
                // get the Cache base path
                if (innerNode.InnerText == "Cache")
                {
                    XmlNode path = node.SelectSingleNode("Path");
                    if (path != null)
                    {
                        mpPaths.cacheBasePath = GetMediaPortalDir(path.InnerText);
                    }
                }
                // get the Config base path
                if (innerNode.InnerText == "Config")
                {
                    XmlNode path = node.SelectSingleNode("Path");
                    if (path != null)
                    {
                        mpPaths.configBasePath = GetMediaPortalDir(path.InnerText);
                    }
                }
                // get the Plugin base path
                if (innerNode.InnerText == "Plugins")
                {
                    XmlNode path = node.SelectSingleNode("Path");
                    if (path != null)
                    {
                        mpPaths.pluginPath = GetMediaPortalDir(path.InnerText);
                    }
                }

                // get the Thumbs base path
                if (innerNode.InnerText == "Thumbs")
                {
                    XmlNode path = node.SelectSingleNode("Path");
                    if (path != null)
                    {
                        mpPaths.thumbsPath = GetMediaPortalDir(path.InnerText);
                    }
                }

                // get the Languages base path
                if (innerNode.InnerText == "Language")
                {
                    XmlNode path = node.SelectSingleNode("Path");
                    if (path != null)
                    {
                        mpPaths.langBasePath = GetMediaPortalDir(path.InnerText);
                    }
                }
                // get the Database base path
                if (innerNode.InnerText == "Database")
                {
                    XmlNode path = node.SelectSingleNode("Path");
                    if (path != null)
                    {
                        mpPaths.databasePath = GetMediaPortalDir(path.InnerText);
                    }
                }
            }
            mpPaths.AvalonPath = mpPaths.skinBasePath + configuredSkin + "\\";
        }

        string GetMediaPortalDir(string path)
        {
            string CommonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // Replace special folder variables if they exist
            // MediaPortal currently only uses two types
            path = path.ToLower();
            path = path.Replace("%appdata%", AppData);
            path = path.Replace("%programdata%", CommonAppData);
            // Check if the path is not rooted ie. a custom directory (including UNC)
            // If directory is relative e.g. 'skin\', prefix with Base Dir
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(SkinInfo.mpPaths.sMPbaseDir, path);
            }
            // Check if there is a trailing backslash
            if (!path.EndsWith(@"\"))
            {
                path += @"\";
            }
            return path;
        }

        string readMPConfiguration(string sectionName, string entryName)
        {
            string fMPdirs = mpPaths.configBasePath + "MediaPortal.xml";
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(fMPdirs))
            {
                MessageBox.Show("Can't find MediaPortal.xml \r\r" + fMPdirs);
                return null;
            }
            doc.Load(fMPdirs);
            XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/profile/section");
            foreach (XmlNode node in nodeList)
            {
                XmlNode innerNode = node.Attributes.GetNamedItem("name");
                if (innerNode.InnerText == sectionName)
                {
                    XmlNode path = node.SelectSingleNode("entry[@name=\"" + entryName + "\"]");
                    if (path != null)
                    {
                        entryName = path.InnerText;
                        return entryName;
                    }
                }
            }
            return string.Empty;
        }

        #endregion

    }
}
