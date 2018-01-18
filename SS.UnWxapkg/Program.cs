using SS.Toolkit.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SS.UnWxapkg
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("usage: SS.UnWxapkg.dll filepath");
                return;
            }
            var filePath = args[0];
            WxapkgHelper.UnWxapkg(filePath);
        }
    }
}
