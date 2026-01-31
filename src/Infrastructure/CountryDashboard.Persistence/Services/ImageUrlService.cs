namespace CountryDashboard.Persistence.Services
{
    public class ImageUrlService : IImageUrlService
    {
        private readonly string _baseImagePath;

        public ImageUrlService(IOptions<AppSettings> options)
        {
            _baseImagePath = options.Value.ImageRepository;
        }

        public string ToFullImageUrl(string? imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
                return string.Empty;

            return $"{_baseImagePath.TrimEnd('/')}/{imageName.TrimStart('/')}";
        }
    }
}
