namespace UpdateThumbnailPhoto.Services
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Identity.Client;

    /// <summary>
    /// Service used for obtaining auth tokens.
    /// </summary>
    internal class AuthService : IAuthService
    {
        /// <summary>
        /// The Azure AD Application ID.
        /// </summary>
        private const string AzureADAppId = "1b730954-1685-4b74-9bfd-dac224a7b894";

        /// <summary>
        /// The Authority for the token. This should be good for Business and Consumer accounts.
        /// </summary>
        private const string Authority = "https://login.microsoftonline.com/common/";

        /// <summary>
        /// The redirect URI.
        /// </summary>
        private const string RedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient";

        /// <summary>
        /// The OAuth scopes for the token.
        /// </summary>
        private static readonly string[] Scopes = new[]
        {
            "https://graph.windows.net/user_impersonation",
        };

        /// <summary>
        /// The authentication client.
        /// </summary>
        private IPublicClientApplication client;

        /// <summary>
        /// Contains the last account for which the auth token was obtained.
        /// </summary>
        private IAccount account;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="config">The configuration service.</param>
        public AuthService(IConfigService config)
        {
            var builder = PublicClientApplicationBuilder.Create(AzureADAppId)
                .WithAuthority(Authority)
                .WithTenantId(config.TenantId)
                .WithRedirectUri(RedirectUri)
                .WithCacheOptions(new CacheOptions(useSharedCache: true));

            this.client = builder.Build();
        }

        /// <inheritdoc/>
        public async Task<string> GetAuthTokenAsync()
        {
            AuthenticationResult result;

            if (this.account != null)
            {
                result = await this.client.AcquireTokenSilent(Scopes, this.account).ExecuteAsync();
                return result.AccessToken;
            }

            result = await this.client.AcquireTokenInteractive(Scopes)
                .WithUseEmbeddedWebView(true)
                .ExecuteAsync()
                .ConfigureAwait(false);

            this.account = result.Account;

            return result.AccessToken;
        }
    }
}
