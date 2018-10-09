using CraftLogs.BLL.Services.Interfaces;
using System.IO;

namespace CraftLogs.BLL.Services
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
        public bool IsFileExist(string fileName)
        {
            return File.Exists(GetFilePath(fileName));
        }

        public bool CreateFile(string fileName)
        {
            if (!IsFileExist(fileName))
            {
                File.WriteAllLines(GetFilePath(fileName), new string[] { "" });
                return true;
            }
            return false;
        }

        public void WriteAllText(string fileName, string content = "")
        {
            File.WriteAllText(GetFilePath(fileName), content);
        }

        public string ReadAllText(string fileName)
        {
            string content = "";
            if (IsFileExist(fileName))
            {
                content = File.ReadAllText(GetFilePath(fileName));
            }
            return content;
        }

        public void DeleteFile(string fileName)
        {
            File.Delete(GetFilePath(fileName));
        }
        #endregion
    }
}
