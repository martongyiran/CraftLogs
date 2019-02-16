﻿using CraftLogs.BLL.Services.Interfaces;
using CraftLogs.Values;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

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

        private void LogToDebug(string text)
        {
            System.Diagnostics.Debug.WriteLine(this.GetType().Name+" | Saved json: "+ text);
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
            LogToDebug(content);
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

        public T ReadFromMockData<T>(string fileName)
        {
            var assembly = typeof(DataService).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(string.Format(FileNames.FileAssembly, fileName));

            T data;
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                data = JsonConvert.DeserializeObject<T>(json);
            }
            return data;
        }

        #endregion
    }
}
