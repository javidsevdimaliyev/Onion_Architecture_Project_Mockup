namespace SolutionName.Application.DTOs.FileManagement
{
    public class FileSaveResult
    {
        public string Name { get; set; }
        public long? Size { get; set; }
        public string Content { get; set; }
        public int PageCount { get; set; }
        /// <summary>
        /// File full path
        /// </summary>
        public string Path { get; set; }
    }
}
