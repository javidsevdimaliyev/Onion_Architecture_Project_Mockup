using Microsoft.AspNetCore.Http;
using SolutionName.Application.DTOs.FileManagement;
using SolutionName.Application.Models.FileManagement;

namespace SolutionName.Application.Abstractions.Services.Storage.Local
{
    public interface IFileStorageService
    {
        void CreateDirectory(string folder);
        Task UploadFileAsync(byte[] byteArray, string path, string filename, CancellationToken cancellationToken = default);
        Task UploadFileAsync(Stream stream, string path, string filename, CancellationToken cancellationToken = default);
        Task<FileSaveResult> UploadFileAsync(FileSaveRequest request, CancellationToken cancellationToken = default);
        Task<FileSaveResult> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default);
        void DeleteDirectory(string folder);
        void DeleteFile(string filePath);
        bool ExistFile(string path);
        string GetFileInDirectory(string fileName);
        bool IsValidDirectory();
        void MoveFile(string? sourceFile, string targetFile);
        Task<byte[]> ReadFileAsync(string filePath, CancellationToken cancellationToken = default);
        Task<string> ReadFileToBase64Async(string filePath, CancellationToken cancellationToken = default);
        string GetFileMimeType(string extension);
        int GetUnixTimeStamp();
    }
}
