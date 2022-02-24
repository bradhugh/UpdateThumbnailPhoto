namespace UpdateThumbnailPhoto.Services
{
    using System.Threading.Tasks;

    /// <summary>
    /// A service interface for the authentication service.
    /// </summary>
    internal interface IAuthService
    {
        /// <summary>
        /// Gets the auth token.
        /// </summary>
        /// <returns>The auth token.</returns>
        Task<string> GetAuthTokenAsync();
    }
}