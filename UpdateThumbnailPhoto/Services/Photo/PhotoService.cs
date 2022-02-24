namespace UpdateThumbnailPhoto.Services.Photo
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// A photo service for interacting with the user photo.
    /// </summary>
    internal class PhotoService
    {
        /// <summary>
        /// The URI to get information about the authenticated user.
        /// </summary>
        private const string MyUserUri = "https://graph.windows.net/me?api-version=1.6";

        /// <summary>
        /// The template to GET, PUT, or DELETE the thumbnail photo.
        /// </summary>
        private const string ThumbnailPhotoTemplate = "https://graph.windows.net/{0}/users/{1}/thumbnailPhoto?api-version=1.6";

        /// <summary>
        /// The authentication service.
        /// </summary>
        private readonly IAuthService authService;

        /// <summary>
        /// The configuration service.
        /// </summary>
        private readonly IConfigService config;

        /// <summary>
        /// The HTTP client.
        /// </summary>
        private HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoService"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        /// <param name="config">The configuration service.</param>
        public PhotoService(IAuthService authService, IConfigService config)
        {
            this.authService = authService;
            this.config = config;
        }

        /// <summary>
        /// Gets the current user's photo.
        /// </summary>
        /// <returns>The photo bytes.</returns>
        public async Task<byte[]> GetMyPhotoAsync()
        {
            var authToken = await this.authService.GetAuthTokenAsync();

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var user = await this.LookupAuthenticatedUserAsync();
            if (string.IsNullOrEmpty(user?.UserPrincipalName))
            {
                throw new Exception("User does not have a valid UserPrincipalName");
            }

            var response = await this.httpClient.GetAsync(
                string.Format(ThumbnailPhotoTemplate, Uri.EscapeDataString(this.config.TenantId), Uri.EscapeDataString(user.UserPrincipalName)));

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var photo = await response.Content.ReadAsByteArrayAsync();

            return photo;
        }

        /// <summary>
        /// Uploads the current user's photo.
        /// </summary>
        /// <param name="content">The photo content.</param>
        /// <returns>A task.</returns>
        public async Task UploadPhotoAsync(byte[] content)
        {
            var authToken = await this.authService.GetAuthTokenAsync();

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var user = await this.LookupAuthenticatedUserAsync();
            if (string.IsNullOrEmpty(user?.UserPrincipalName))
            {
                throw new Exception("User does not have a valid UserPrincipalName");
            }

            var httpContent = new ByteArrayContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("images/*");

            var response = await this.httpClient.PutAsync(
                string.Format(ThumbnailPhotoTemplate, this.config.TenantId, Uri.EscapeDataString(user.UserPrincipalName)),
                httpContent);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Looks up the authenticated user.
        /// </summary>
        /// <returns>The user object.</returns>
        private async Task<User> LookupAuthenticatedUserAsync()
        {
            var authToken = await this.authService.GetAuthTokenAsync();

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var user = await this.httpClient.GetFromJsonAsync<User>(MyUserUri);
            if (user == null)
            {
                throw new Exception("User lookup failed");
            }

            return user;
        }
    }
}
