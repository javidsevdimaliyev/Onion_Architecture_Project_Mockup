using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SolutionName.Application.Abstractions.Services.Storage.Local;
using SolutionName.Application.Utilities.Converters;
using SolutionName.Application.Utilities.Utility;

namespace SolutionName.Infrastructure.Services.Storage.Local
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LocalStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
      
        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public async Task DeleteAsync(string path, string fileName)
          => File.Delete($"{path}\\{fileName}");

        public bool HasFile(string path, string fileName)
            => File.Exists($"{path}\\{fileName}");
       
        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await (new FileNameGenerator().FileRenameAsync(path, file.Name, HasFile));

                await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
                datas.Add((fileNewName, $"{path}\\{fileNewName}"));
            }

            return datas;
        }


        async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }
        }


    }
}
