

using Microsoft.AspNetCore.Http;
using CountryDashboard.Domain.Enums;

namespace CountryDashboard.Application.Common.Interfaces.Services.FileServices
{
    public interface IFileStorageService
    {
        Task<ApiResponse> UploadFileAsync(IFormFile file, string targetPath, FileType fileType);
        Task<ApiResponse> UploadFilesAsync(IEnumerable<IFormFile> files, string targetPath, FileType fileType);
        Task<ApiResponse> DeleteFileAsync(string filePath);
    }
}
