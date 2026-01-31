using CountryDashboard.Application.Common.Response;

namespace CountryDashboard.Persistence.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly FileAttachmentSettings _settings;

        public FileStorageService(IOptions<FileAttachmentSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<ApiResponse> UploadFileAsync(IFormFile file, string targetPath, FileType fileType)
        {
            if (file == null || file.Length == 0)
                return ApiResponse.Failure("File is empty");

            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowedExtensions = GetExtensionsByFileType(fileType);
            var maxSize = GetMaxSizeByFileType(fileType);

            if (!allowedExtensions.Contains(extension))
                return ApiResponse.Failure($"Extension {extension} is not allowed for {fileType} files.");

            if (file.Length > maxSize)
                return ApiResponse.Failure($"File size exceeds limit for {fileType}. Max size is {maxSize / (1024 * 1024)} MB.");

            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(targetPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return ApiResponse.Success(new { FileName = fileName, Path = fullPath });
        }


        public async Task<ApiResponse> UploadFilesAsync(IEnumerable<IFormFile> files, string targetPath, FileType fileType)
        {
            var responses = new List<object>();
            foreach (var file in files)
            {
                var result = await UploadFileAsync(file, targetPath, fileType);
                if (!result.IsSuccess)
                    return result;

                responses.Add(result.Value);
            }
            return ApiResponse.Success(responses);
        }

        public Task<ApiResponse> DeleteFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
                return Task.FromResult(ApiResponse.Failure("File does not exist."));

            File.Delete(filePath);
            return Task.FromResult(ApiResponse.Success("File deleted successfully."));
        }
        private List<string> GetExtensionsByFileType(FileType fileType)
        {
            return fileType switch
            {
                FileType.Image => _settings.ImageExtensionsList,
                FileType.Video => _settings.VideoExtensionsList,
                FileType.Document => _settings.DocumentExtensionsList,
                FileType.Any => _settings.ImageExtensionsList
                                     .Concat(_settings.VideoExtensionsList)
                                     .Concat(_settings.DocumentExtensionsList)
                                     .ToList(),
                _ => new List<string>()
            };
        }
        private int GetMaxSizeByFileType(FileType fileType)
        {
            return fileType switch
            {
                FileType.Image => _settings.MaxImageSizeInBytes,
                FileType.Video => _settings.MaxVideoSizeInBytes,
                FileType.Document => _settings.MaxDocumentSizeInBytes,
                FileType.Any => Math.Max(
                                    Math.Max(_settings.MaxImageSizeInBytes,
                                             _settings.MaxVideoSizeInBytes),
                                    _settings.MaxDocumentSizeInBytes),
                _ => _settings.MaxImageSizeInBytes
            };
        }

    }
}
