using SS.Toolkit.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SS.UnWxapkg
{
    public class WxapkgHelper
    {
        public static void UnWxapkg(string filePath)
        {

            var bytes = File.ReadAllBytes(filePath);

            ByteBuffer byteBuffer = ByteBuffer.Allocate(bytes);

            var firstMark = byteBuffer.ReadByte();
            Console.WriteLine($"first header mark = {firstMark}");

            var info1 = byteBuffer.ReadInt();
            Console.WriteLine($"info1 = {info1}");

            var indexInfoLength = byteBuffer.ReadInt();
            Console.WriteLine($"indexInfoLength = {indexInfoLength}");

            var bodyInfoLength = byteBuffer.ReadInt();
            Console.WriteLine($"bodyInfoLength = {bodyInfoLength}");

            var lastMark = byteBuffer.ReadByte();
            Console.WriteLine($"last header mark = {lastMark}");

            if (firstMark != 0xBE || lastMark != 0xED)
            {
                Console.WriteLine("its not a wxapkg file!");
            }

            var fileCount = byteBuffer.ReadInt();
            Console.WriteLine($"fileCount = {fileCount}");

            var fileList = new List<WxapkgFile>();

            for (int i = 0; i < fileCount; i++)
            {
                var nameLen = byteBuffer.ReadInt();
                byte[] nameByte = new byte[nameLen];
                byteBuffer.ReadBytes(nameByte, 0, nameLen);
                var name = Encoding.UTF8.GetString(nameByte);
                var offset = byteBuffer.ReadInt();
                var size = byteBuffer.ReadInt();
                var wxapkgFile = new WxapkgFile() { Name = name, NameLen = nameLen, Offset = offset, Size = size };
                fileList.Add(wxapkgFile);
                Console.WriteLine($"readFile = {name} at Offset = {offset}");
            }
            var rootPath = System.Environment.CurrentDirectory;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            Console.WriteLine($"rootPath = {rootPath}");
            Console.WriteLine($"fileNameWithoutExtension = {fileNameWithoutExtension}");

            var unzipPath = Path.Combine(rootPath, fileNameWithoutExtension);
            Console.WriteLine($"unzipPath = {unzipPath}");
            if (!Directory.Exists(unzipPath))
            {
                Directory.CreateDirectory(unzipPath);
            }

            foreach (var wxapkgFile in fileList)
            {
                var wxapkgFileName = unzipPath + wxapkgFile.Name.Replace('/', Path.DirectorySeparatorChar);
                Console.WriteLine($"wxapkgFileName = {wxapkgFileName}");
                var directoryName = Path.GetDirectoryName(wxapkgFileName);
                Console.WriteLine($"directoryName = {directoryName}");
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                byteBuffer.SetReaderIndex(wxapkgFile.Offset);
                var fileBytes = new byte[wxapkgFile.Size];
                byteBuffer.ReadBytes(fileBytes, 0, fileBytes.Length);
                File.WriteAllBytes(wxapkgFileName, fileBytes);
            }

        }


    }
}
