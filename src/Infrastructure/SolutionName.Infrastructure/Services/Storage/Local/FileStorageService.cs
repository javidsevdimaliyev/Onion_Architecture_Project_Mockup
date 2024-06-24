using Microsoft.AspNetCore.Http;
using SolutionName.Application.Abstractions.Services.Storage.Local;
using SolutionName.Application.DTOs.Configurations;
using SolutionName.Application.DTOs.FileManagement;
using SolutionName.Application.Enums;
using SolutionName.Application.Exceptions;
using SolutionName.Application.Models.FileManagement;
using SolutionName.Application.Utilities.Extensions;
using SolutionName.Application.Utilities.Utility;

namespace SolutionName.Infrastructure.Services.Storage.Local
{
    internal class FileStorageService : IFileStorageService
    {
        #region ..::fields::..
        private readonly string _saveDirectory;

        #endregion

        #region ..::ctors::..
        public FileStorageService(LocalStorageConfig settings)
        {
            _saveDirectory = settings.SaveDirectory;
#if DEBUG
            _saveDirectory = settings.SaveDirectoryDebug;
#endif
        }

        #endregion

        #region ..::methods::..

        #region Check
        public bool IsValidDirectory()
        {
            var exist = Directory.Exists(_saveDirectory);
#if DEBUG
            if (!exist)
            {
                Directory.CreateDirectory(_saveDirectory);
                return true;
            }
#endif
            return exist;
        }



        public bool ExistFile(string path)
        {
            var file = Path.Combine(_saveDirectory, path);
            return File.Exists(file);
        }

        #endregion

        #region Upload
        public void CreateDirectory(string folder)
        {
            var fullFolderPath = Path.Combine(_saveDirectory, folder);

            if (!Directory.Exists(fullFolderPath))
                Directory.CreateDirectory(fullFolderPath);
        }

        public async Task<FileSaveResult> UploadFileAsync(FileSaveRequest request, CancellationToken cancellationToken = default)
        {
            ThrowExceptionExtension.ThrowIfFalse(IsValidDirectory(), "No folder was found for File ! - (Directory)");
            var folder = Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
            CreateDirectory(folder);

            //var fileExtension = Path.GetExtension(request.File.FileName);
            var uniqueFileName = Guid.NewGuid() + "_" + request.FileName;
            var path = Path.Combine(folder, uniqueFileName);
            var fullFolderPath = Path.Combine(_saveDirectory, path);

            if (File.Exists(fullFolderPath))
                return null;

            if (!string.IsNullOrEmpty(request.Base64Content))
            {
                var contentBytes = Convert.FromBase64String(request.Base64Content.Remove(0, request.Base64Content.IndexOf(',') + 1));
                if (contentBytes.Length <= 0)
                    throw new ApiException(HttpResponseStatus.FileNotSave, HttpResponseStatus.FileNotSave.GetDescription());
                await File.WriteAllBytesAsync(fullFolderPath, contentBytes, cancellationToken);
            }
            //else
            //{
            //    await using var stream = request.File.Content?.OpenReadStream();
            //    await File.WriteAllBytesAsync(fullFolderPath, ReadFully(stream));
            //}


            return new FileSaveResult()
            {
                Path = path
            };

        }

        public async Task<FileSaveResult> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            try
            {
                ThrowExceptionExtension.ThrowIfFalse(IsValidDirectory(), "Folder for file not found! - (Directory)");

                var folder = Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
                CreateDirectory(folder);

                var uniqueFileName = Guid.NewGuid() + "_" + file.FileName;
                var path = Path.Combine(folder, uniqueFileName);
                var fullFolderPath = Path.Combine(_saveDirectory, path);

                if (File.Exists(fullFolderPath))
                    return null;


                await using var stream = file?.OpenReadStream();
                await File.WriteAllBytesAsync(fullFolderPath, FileReader.ReadFully(stream), cancellationToken);



                return new FileSaveResult()
                {
                    Path = path
                };
            }
            catch (Exception e)
            {
                throw new ApiException(HttpResponseStatus.FileNotSave, HttpResponseStatus.FileNotSave.GetDescription());
            }
        }

        public async Task UploadFileAsync(byte[] byteArray, string path, string filename, CancellationToken cancellationToken = default)
        {
            ThrowExceptionExtension.ThrowIfFalse(IsValidDirectory(), "Folder for file not found! - (Directory)");

            var fullFolderPath = Path.Combine(_saveDirectory, path, filename);

            await File.WriteAllBytesAsync(fullFolderPath, byteArray, cancellationToken);
        }

        public async Task UploadFileAsync(Stream stream, string path, string filename, CancellationToken cancellationToken = default)
        {
            ThrowExceptionExtension.ThrowIfFalse(IsValidDirectory(), "Folder for file not found! - (Directory)");

            var fullFolderPath = Path.Combine(_saveDirectory, path, filename);

            await File.WriteAllBytesAsync(fullFolderPath, FileReader.ReadFully(stream), cancellationToken);
        }

        #endregion

        #region Read
        public async Task<byte[]> ReadFileAsync(string filePath, CancellationToken cancellationToken = default)
        {
            ThrowExceptionExtension.ThrowIfFalse(IsValidDirectory(), "Folder for file not found! - (Directory)");
            ThrowExceptionExtension.ThrowIfFalse(ExistFile(filePath), "File not found!)");

            var fullFilePath = Path.Combine(_saveDirectory, filePath);

            return await File.ReadAllBytesAsync(fullFilePath, cancellationToken);
        }

        public async Task<string> ReadFileToBase64Async(string filePath, CancellationToken cancellationToken = default)
        {
            ThrowExceptionExtension.ThrowIfFalse(IsValidDirectory(), "Folder for file not found! - (Directory)");
            ThrowExceptionExtension.ThrowIfFalse(ExistFile(filePath), "File not found!)");

            var fullFilePath = Path.Combine(_saveDirectory, filePath);

            return Convert.ToBase64String(await File.ReadAllBytesAsync(fullFilePath, cancellationToken));
        }

        public string GetFileInDirectory(string fileName)
        {
            DirectoryInfo hdDirectoryInWhichToSearch = new(_saveDirectory);
            FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles(fileName + "*.*");

            Console.WriteLine($"filesInDir.Length:{filesInDir.Length}");
            Console.WriteLine($"founded file name {filesInDir[0].FullName}");
            return filesInDir[0].FullName;
        }
        #endregion

        #region Delete
        public void DeleteFile(string filePath)
        {
            ThrowExceptionExtension.ThrowIfFalse(ExistFile(filePath), "File not found to delete!");

            var fullFilePath = Path.Combine(_saveDirectory, filePath);

            File.Delete(fullFilePath);
        }

        public void DeleteDirectory(string folder)
        {
            var fullFolderPath = Path.Combine(_saveDirectory, folder);

            Directory.Delete(fullFolderPath);
        }

        #endregion

        #region Move
        public void MoveFile(string? sourceFile, string targetFile)
        {
            var targetFilePath = Path.Combine(_saveDirectory, targetFile);

            try
            {
                // Ensure that the target does not exist.  
                if (File.Exists(targetFilePath))
                    File.Delete(targetFilePath);
                // Move the file.  
                File.Move(sourceFile, targetFilePath);
                Console.WriteLine("{0} was moved to {1}.", sourceFile, targetFilePath);

                // See if the original exists now.  
                Console.WriteLine(File.Exists(sourceFile)
                    ? "The original file still exists, which is unexpected."
                    : "The original file no longer exists, which is expected.");
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
        #endregion

        #region Helper
        public string GetFileMimeType(string extension)
        {
            string mimetype = "";
            switch (extension)
            {
                case ".pdf":
                    mimetype = "application/pdf";
                    break;

                case ".docx":
                    mimetype += "application/docx";
                    break;

                case ".jpg":
                    mimetype += "image/jpeg";
                    break;

                case ".adoc":
                    mimetype += "application/vnd.bdoc-1.0";
                    break;

                case ".edoc":
                    mimetype += "application/edoc";
                    break;

                default:
                    mimetype += "application/octet-stream";
                    break;
            }
            return mimetype;
        }

        public int GetUnixTimeStamp()
        {
            DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
            DateTime point = new(1970, 1, 1);
            TimeSpan time = now.Subtract(point);

            return (int)time.TotalSeconds;
        }
        #endregion

        #endregion
    }
}
