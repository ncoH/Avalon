using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using MediaPortal.Configuration;
using MediaPortal.Profile;

namespace AvalonGUIConfig
{
    class AvalonHelper
    {

        public string fileVersion(string fileToCheck)
        {
            if (File.Exists(fileToCheck))
            {
                FileVersionInfo fv = FileVersionInfo.GetVersionInfo(fileToCheck);
                return fv.FileVersion;

            }
            else
                return "0.0.0.0";
        }

        #region Assembly Helpers
        public static bool IsAssemblyAvailable(string name, Version ver)
        {
            return IsAssemblyAvailable(name, ver, null);
        }
        public static bool IsAssemblyAvailable(string name, Version ver, string filename)
        {
            bool result = false;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly a in assemblies)
            {
                try
                {
                    if (a.GetName().Name == name)
                    {
                        if (a.GetName().Version >= ver)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch
                {
                    result = false;
                }
            }

            if (!result)
            {
                try
                {
                    Assembly assembly = null;
                    if (string.IsNullOrEmpty(filename))
                    {
                        assembly = Assembly.ReflectionOnlyLoad(name);
                        if (assembly.GetName().Version >= ver) result = true;
                    }
                    else
                    {
                        assembly = Assembly.ReflectionOnlyLoadFrom(filename);
                        if (assembly.GetName().Version >= ver) result = true;
                    }
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        #endregion

        #region Plugin Helpers
        public static bool IsPluginEnabled(string name)
        {
            using (Settings xmlreader = new MPSettings())
            {
                return xmlreader.GetValueAsBool("plugins", name, false);
            }
        }

        public static bool IsMovingPicturesAvailableAndEnabled
        {
            get
            {
                return File.Exists(Path.Combine(Config.GetSubFolder(Config.Dir.Plugins, "Windows"), "MovingPictures.dll")) && IsPluginEnabled("Moving Pictures");
            }
        }

        #endregion

        #region XML Helpers
        /// <summary>
        /// Loads and returns an XML Document
        /// </summary>
        /// <param name="file"></param>    
        public static XmlDocument LoadXMLDocument(string file)
        {
            // Check if File Exist
            if (!File.Exists(file))
                return null;

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(file);
            }
            catch (XmlException e)
            {
                return null;
            }
            return doc;
        }
        /// <summary>
        /// Sets a new path for a skin files <import></import>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="importname"></param>
        /// <param name="value"></param>
        public static void SetSkinImport(string file, string importtag, string value)
        {
            XmlDocument doc = LoadXMLDocument(file);
            if (doc == null) return;

            // build xpath string
            string xpath = string.Format("/window/controls/import[@tag='{0}']", importtag);

            XmlNode node = doc.DocumentElement.SelectSingleNode(xpath);
            if (node == null)
                return;

            node.InnerText = value;
            doc.Save(file);
        }

        /// <summary>
        /// Set XML Property
        /// </summary>
        public static void SetNodeText(string file, string path, string value)
        {
            XmlDocument doc = LoadXMLDocument(file);
            if (doc == null) return;

            XmlNode node = doc.SelectSingleNode(path);
            if (node == null)
                return;

            try
            {
                node.InnerText = value;

                doc.Save(file);
            }
            catch (Exception ex)
            {
                //smcLog.WriteLog("Exception setting skin text property: " + ex.Message, LogLevel.Error);
            }
        }

        #endregion

    }
}
