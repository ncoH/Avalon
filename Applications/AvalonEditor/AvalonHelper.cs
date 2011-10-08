using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using MediaPortal.Configuration;

namespace ProcessPlugins.AvalonEditor
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

        #endregion

    }
}
