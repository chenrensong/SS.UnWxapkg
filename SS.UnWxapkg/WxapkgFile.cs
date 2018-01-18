using System;
using System.Collections.Generic;
using System.Text;

namespace SS.UnWxapkg
{
    public class WxapkgFile
    {
        public int NameLen { get; set; } = 0;

        public string Name { get; set; } = string.Empty;

        public int Offset { get; set; } = 0;


        public int Size { get; set; } = 0;

    }
}
