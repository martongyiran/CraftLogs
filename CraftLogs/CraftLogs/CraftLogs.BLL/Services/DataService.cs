/*
Copyright 2018 Gyirán Márton Áron

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. 
*/

using CraftLogs.BLL.Services.Interfaces;
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
