namespace CountryDashboard.Application.Common.Utilities
{
    public static class IconUrlMapping
    {
        public static class IconUrlMappings
        {
            public static void Register(IImageUrlService imageUrlService)
            {
                // Any DTO with FullIconUrl
                TypeAdapterConfig.GlobalSettings.ForType<IHasIcon, object>()
                    .AfterMapping((src, dest) =>
                    {
                        var prop = dest.GetType().GetProperty("IconUrl");
                        if (prop != null)
                        {
                            prop.SetValue(dest, imageUrlService.ToFullImageUrl(src.IconURL));
                        }
                    });
            }
        }

    }
}
