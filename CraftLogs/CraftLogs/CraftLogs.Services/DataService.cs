using System.IO;

namespace CraftLogs.Services
{
    public static class DataService
    {
        public static bool IsFileExist(this string fileName)
        {
            return File.Exists(GetFilePath(fileName));
        }

        public static bool CreateFile(this string fileName)
        {
            if (!fileName.IsFileExist())
            {
                File.WriteAllLines(GetFilePath(fileName), new string[] { "" });
                return true;
            }
            return false;
        }

        public static void WriteAllText(this string fileName, string content = "")
        {
            File.WriteAllText(GetFilePath(fileName), content);
        }

        public static string ReadAllText(this string fileName)
        {
            string content = "";
            if (fileName.IsFileExist())
            {
                content = File.ReadAllText(GetFilePath(fileName));
            }
            return content;
        }

        public static void DeleteFile(string fileName)
        {
            File.Delete(GetFilePath(fileName));
        }

        private static string GetPath()
        {
            var tempPath = Path.GetTempPath();
            var path = tempPath.Replace("/cache/", "");
            path += string.Format("/{0}", "files");
            return path;
        }

        private static string GetFilePath(string fileName)
        {
            return Path.Combine(GetPath(), fileName);
        }
    }
}
