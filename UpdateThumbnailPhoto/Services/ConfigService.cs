namespace UpdateThumbnailPhoto.Services
{
    using System;
    using System.Configuration;

    /// <summary>
    /// The configuration service.
    /// </summary>
    internal class ConfigService : IConfigService
    {
        /// <inheritdoc/>
        public string TenantId => ConfigurationManager.AppSettings["TenantId"] ?? throw new ConfigurationErrorsException($"{nameof(TenantId)} cannot be null");
    }
}
