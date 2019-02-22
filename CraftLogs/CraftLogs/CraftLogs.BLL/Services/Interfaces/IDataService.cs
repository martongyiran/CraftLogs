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

namespace CraftLogs.BLL.Services.Interfaces
{
    public interface IDataService
    {
        /// <summary> 
        /// Verifies that the specified file already exists. 
        /// </summary>
        /// <param name="fileName">
        /// File name.
        /// </param>
        bool IsFileExist(string fileName);

        /// <summary> Create a new file. </summary>
        /// <param name="fileName">File name.</param>
        bool CreateFile(string fileName);

        /// <summary> Writes all text to the given file. </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="content">The text that we want to write to the file.</param>
        void WriteAllText(string fileName, string content = "");

        /// <summary> Reads all text from the given file. </summary>
        /// <param name="fileName">File name.</param>
        string ReadAllText(string fileName);

        /// <summary> Delete a file. </summary>
        /// <param name="fileName">File name.</param>
        void DeleteFile(string fileName);

        /// <summary> Reads embedded resource and seriealize it. </summary>
        /// <param name="fileName">File name.</param>
        T ReadFromMockData<T>(string fileName);
    }
}
