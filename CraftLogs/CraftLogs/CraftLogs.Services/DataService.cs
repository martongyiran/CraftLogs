using System.IO;

namespace CraftLogs.Services
{
    public static class DataService
    {
        #region Private functions
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
        #endregion

        #region Public functions
        /// <summary> Verifies that the specified file already exists. </summary>
        /// <param name="fileName">File name.</param>
        public static bool IsFileExist(this string fileName)
        {
            return File.Exists(GetFilePath(fileName));
        }

        /// <summary> Create a new file. </summary>
        /// <param name="fileName">File name.</param>
        public static bool CreateFile(this string fileName)
        {
            if (!fileName.IsFileExist())
            {
                File.WriteAllLines(GetFilePath(fileName), new string[] { "" });
                return true;
            }
            return false;
        }

        /// <summary> Writes all text to the given file. </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="content">The text that we want to write to the file.</param>
        public static void WriteAllText(this string fileName, string content = "")
        {
            File.WriteAllText(GetFilePath(fileName), content);
        }

        /// <summary> Reads all text from the given file. </summary>
        /// <param name="fileName">File name.</param>
        public static string ReadAllText(this string fileName)
        {
            string content = "";
            if (fileName.IsFileExist())
            {
                content = File.ReadAllText(GetFilePath(fileName));
            }
            return content;
        }

        /// <summary> Delete a file. </summary>
        /// <param name="fileName">File name.</param>
        public static void DeleteFile(string fileName)
        {
            File.Delete(GetFilePath(fileName));
        }
        #endregion
    }
}
