namespace CountryDashboard.Application.Common.Models.Settings
{
    public class FileAttachmentSettings
    {
        // Extensions
        public string AllowedImageExtensions { get; set; } = string.Empty;
        public string AllowedVideoExtensions { get; set; } = string.Empty;
        public string AllowedDocumentExtensions { get; set; } = string.Empty;

        // Size (MB per type)
        public int MaxImageSizeInMB { get; set; }
        public int MaxVideoSizeInMB { get; set; }
        public int MaxDocumentSizeInMB { get; set; }

        // Lists
        public List<string> ImageExtensionsList => AllowedImageExtensions
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(e => e.Trim().ToLower())
            .ToList();

        public List<string> VideoExtensionsList => AllowedVideoExtensions
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(e => e.Trim().ToLower())
            .ToList();

        public List<string> DocumentExtensionsList => AllowedDocumentExtensions
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(e => e.Trim().ToLower())
            .ToList();

        // Byte conversion for each type
        public int MaxImageSizeInBytes => MaxImageSizeInMB * 1024 * 1024;
        public int MaxVideoSizeInBytes => MaxVideoSizeInMB * 1024 * 1024;
        public int MaxDocumentSizeInBytes => MaxDocumentSizeInMB * 1024 * 1024;
    }


}
