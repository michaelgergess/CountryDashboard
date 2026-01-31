namespace CountryDashboard.Application.Common.Constants
{
    public static class CacheKeys
    {
        // Key for a single user
        public static string User(int userId) => $"user:{userId}";

        // Key for list of users (if you store the whole list)
        public static string UsersList => "users:list";

        // If you want indexing by city later    for example
        //  public static string UsersByCity(int cityId) => $"users:city:{cityId}";

        // Any other keys if needed
    }
}

