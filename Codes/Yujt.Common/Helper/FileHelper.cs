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
            var stream = File.Create(path);
            stream.Close();
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
    }
}
