using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Nanook.TheGhost
{
    public class TheGhostCore
    {
        public TheGhostCore()
        {
            _project = new Project(this);
            _pluginManager = null;
        }


        /// <summary>
        /// Lists found plugins and instantiates them
        /// </summary>
        public PluginManager PluginManager
        {
            get
            {
                if (_pluginManager == null)
                {
                    _pluginManager = new PluginManager();
                    _pluginManager.LoadPlugins();
                }

                return _pluginManager;
            }
        }

        /// <summary>
        /// The project settings for TheGHOST's project
        /// </summary>
        public Project Project
        {
            get { return _project; }
        }

        public static string CoreVersion
        {
            get
            {
                Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return string.Format("{0}.{1}", v.Major.ToString(), v.Minor.ToString());
            }
        }

        public static string AppName
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().GetModules()[0].Name);
            }
        }

        public static string AppVersion
        {
            get
            {
                Version v = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
                return string.Format("{0}.{1}", v.Major.ToString(), v.Minor.ToString());
            }
        }


        private static Project _project;
        private static PluginManager _pluginManager;
    }
}
