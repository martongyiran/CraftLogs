using System.IO;

namespace CraftLogs.Services
{
    public class DataService : IDataService
    {
        #region Private functions
        private string GetPath()
        {
            var tempPath = Path.GetTempPath();
            var path = tempPath.Replace("/cache/", "");
            path += string.Format("/{0}", "files");
            return path;
        }
        
        private string GetFilePath(string fileName)
        {
            return Path.Combine(GetPath(), fileName);
        }
        #endregion

        #region Public functions
        /// <summary> Verifies that the specified file already exists. </summary>
        /// <param name="fileName">File name.</param>
        public bool IsFileExist(string fileName)
        {
            return File.Exists(GetFilePath(fileName));
        }

        /// <summary> Create a new file. </summary>
        /// <param name="fileName">File name.</param>
        public bool CreateFile(string fileName)
        {
            if (!IsFileExist(fileName))
            {
                File.WriteAllLines(GetFilePath(fileName), new string[] { "" });
                return true;
            }
            return false;
        }

        /// <summary> Writes all text to the given file. </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="content">The text that we want to write to the file.</param>
        public void WriteAllText(string fileName, string content = "")
        {
            File.WriteAllText(GetFilePath(fileName), content);
        }

        /// <summary> Reads all text from the given file. </summary>
        /// <param name="fileName">File name.</param>
        public string ReadAllText(string fileName)
        {
            string content = "";
            if (IsFileExist(fileName))
            {
                content = File.ReadAllText(GetFilePath(fileName));
            }
            return content;
        }

        /// <summary> Delete a file. </summary>
        /// <param name="fileName">File name.</param>
        public void DeleteFile(string fileName)
        {
            File.Delete(GetFilePath(fileName));
        }
        #endregion
    }
}
