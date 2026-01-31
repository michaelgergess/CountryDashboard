
namespace CountryDashboard.API.Filters
{
    // <summary>
    /// Attribute used to indicate that specific headers should be ignored during validation 
    /// by the <see cref="RequiredHeadersAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class IgnoreRequiredHeadersAttribute : Attribute
    {
        /// <summary>
        /// Gets the list of headers to be ignored during header validation.
        /// </summary>
        public HeaderProperties[] IgnoredHeaders { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreRequiredHeadersAttribute"/> class
        /// with the specified headers to be excluded from validation.
        /// </summary>
        /// <param name="ignoredHeaders">The headers to exclude from validation.</param>
        public IgnoreRequiredHeadersAttribute(params HeaderProperties[] ignoredHeaders)
        {
            IgnoredHeaders = ignoredHeaders;
        }
    }

}
