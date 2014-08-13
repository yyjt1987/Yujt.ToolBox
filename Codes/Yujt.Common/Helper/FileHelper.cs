using System;
using System.IO;

namespace Yujt.Common.Helper
{
    public class FileHelper
    {
        public static void CreateParentDirectory(string path)
        {
            var parent = Directory.GetParent(path).FullName;
            if (!Directory.Exists(parent))
            {
                Directory.CreateDirectory(parent);
            }
        }

        public static void CreateFileAndParentDirectory(string path)
        {
            CreateParentDirectory(path);
            var s = File.Create(path);
            s.Close();
        }

        public static void TryDeleteFilesIn(string path)
        {
            var parent = Directory.GetParent(path).FullName;
            var fileList = Directory.GetFiles(parent);
            foreach (var file in fileList)
            {
                try
                {
                    File.Delete(file);
                }catch(Exception)
                { }
            }
        }

        public static long GetFileSize(string path)
        {
            var stream = new FileStream(path, FileMode.Open);
            var length = stream.Length;
            stream.Close();
            return length;
        }
    }
}
