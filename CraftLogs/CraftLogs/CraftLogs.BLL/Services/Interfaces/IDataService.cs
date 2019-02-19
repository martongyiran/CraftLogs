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
