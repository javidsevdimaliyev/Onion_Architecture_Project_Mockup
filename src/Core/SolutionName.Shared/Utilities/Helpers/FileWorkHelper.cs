using Microsoft.AspNetCore.Http;
using System.IO.Compression;

namespace SolutionName.Application.Shared.Utilities.Helpers
{
    public static class FileWorkHelper
    {
        public static bool SaveFileToLocal(this string filePath, IFormFile file)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
                stream.Close();
            }
            return file.Length > default(int) ? true : false;
        }

        public static bool DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return true;
        }

        public static List<FileSaveResult> ExtractToZipFile(string path, string folderPath, long stamp, string folder)
        {
            List<FileSaveResult> results = new();
            using (ZipArchive archive = ZipFile.OpenRead(path))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string extractGenerateFileName = $"{stamp}_{folder}_{entry.FullName}";
                    string destinationPath = Path.GetFullPath(Path.Combine(folderPath, extractGenerateFileName));
                    entry.ExtractToFile(destinationPath);
                    results.Add(new FileSaveResult { Name = extractGenerateFileName, Size = entry.Length});

                }
                archive.Dispose();
                DeleteFile(path);
            }

            return results;
        }

      

        public static byte[] GetFileExportZip(this FileSaveRequest[] files)
        {
            byte[] archiveFile;
            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);

                        using var zipStream = zipArchiveEntry.Open();
                        zipStream.Write(file.Content, 0, file.Content.Length);
                    }
                }

                archiveFile = archiveStream.ToArray();
            }

            return archiveFile;
        }


        public static byte[] CompressFile(List<string> paths)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    paths.ForEach(x => ziparchive.CreateEntryFromFile(x, Path.GetFileName(x)));
                }

                return memoryStream.ToArray();
            }
        }

        public static byte[] GetByteToArray(this Stream stream)
        {
            int length = stream.Length > int.MaxValue ? int.MaxValue : Convert.ToInt32(stream.Length);
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            return buffer;
        }


        public static (string[] fileNameSplit, string path) GetFileNameSplitAndPath(this string fileName, string directory)
        {

            string[] fileNameSplit = fileName.Split('_');

            if (fileNameSplit.Length <= 0)
                throw new FileNotFoundException("File not found");

            string path = $"{directory}\\{fileNameSplit[1]}\\{fileName}";

            return (fileNameSplit, path);
        }


        public static string GetFileMimeTypeName(this string fileName)
        {
            //var fileMimeTypeIndexOf = fileName.IndexOf('.');//test.docx
            //return fileName.Substring(fileMimeTypeIndexOf + 1).ToLower();//docx
            return Path.GetExtension(fileName).Substring(1).ToLower();
        }

    }
}
