using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;


namespace Nanook.TheGhost
{
    public class PluginManager
    {
        internal PluginManager()
        {

            _audioExportPlugins = new Dictionary<string, PluginInfo>();
            _audioImportPlugins = new Dictionary<string, PluginInfo>();
            _fileCopyPlugins = new Dictionary<string, PluginInfo>();
        }

        internal void LoadPlugins()
        {
            Assembly ass;
            string assName;

            DirectoryInfo di = ((FileInfo)new FileInfo(Assembly.GetEntryAssembly().Location)).Directory;

            foreach (FileInfo fi in di.GetFiles("*.dll"))
            {
                try
                {
                    assName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
                    ass = Assembly.Load(assName);
                }
                catch
                {
                    continue;
                }

                try
                {
                    if (ass != null)
                    {
                        foreach (Type t in ass.GetTypes())
                        {
                            if (!t.IsInterface && t.GetInterface(typeof(IPlugin).Name) != null)
                            {
                                try
                                {
                                    IPlugin ip = (IPlugin)Activator.CreateInstance(t);
                                    PluginInfo ppi = new PluginInfo(ip.Title(), assName, string.Format("{0},{1}", t.FullName, assName), ip.Version().ToString(), ip.Description());

                                    if (t.GetInterface(typeof(IPluginAudioExport).Name) != null)
                                        _audioExportPlugins.Add(ppi.Title, ppi);
                                    else if (t.GetInterface(typeof(IPluginAudioImport).Name) != null)
                                        _audioImportPlugins.Add(ppi.Title, ppi);
                                    else if (t.GetInterface(typeof(IPluginFileCopy).Name) != null)
                                        _fileCopyPlugins.Add(ppi.Title, ppi);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public Dictionary<string, PluginInfo> AudioExportPlugins
        {
            get { return _audioExportPlugins; }
        }

        public Dictionary<string, PluginInfo> AudioImportPlugins
        {
            get { return _audioImportPlugins; }
        }

        public Dictionary<string, PluginInfo> FileCopyPlugins
        {
            get { return _fileCopyPlugins; }
        }

        public IPlugin LoadPlugin(PluginInfo plugin)
        {

            Assembly ass;
            // You must supply a valid fully qualified assembly name here.            
            string[] a = plugin.TypeName.Split(new char[] { ',' }, 2);
            ass = Assembly.Load(a[1]);


            Type t = ass.GetType(a[0]);
            return (IPlugin)Activator.CreateInstance(t);

        }

        /// <summary>
        /// Convert from a type name to a display name
        /// </summary>
        /// <param name="plugins"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        internal string GetPluginNameFromTypeName(Dictionary<string, PluginInfo> plugins, string typeName)
        {
            foreach (PluginInfo ppi in plugins.Values)
            {
                if (ppi.TypeName == typeName)
                    return ppi.Title;
            }
            return string.Empty;
        }



        private Dictionary<string, PluginInfo> _audioExportPlugins;
        private Dictionary<string, PluginInfo> _audioImportPlugins;
        private Dictionary<string, PluginInfo> _fileCopyPlugins;
    }
}
