using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public class PluginInfo
    {
        internal PluginInfo(string title, string filename, string typeName, string version, string description)
        {
            Title = title;
            Filename = filename;
            TypeName = typeName;
            Version = version;
            Description = description;
        }

        public readonly string Title;
        public readonly string Filename;
        public readonly string TypeName;
        public readonly string Version;
        public readonly string Description;

    }
}
