using System;
using System.Collections.Generic;
using System.Text;

namespace Nanook.TheGhost
{
    public interface IPlugin
    {
        string Title();
        string Description();
        float Version();

    }
}
