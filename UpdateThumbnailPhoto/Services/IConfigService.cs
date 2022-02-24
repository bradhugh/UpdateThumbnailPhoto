namespace UpdateThumbnailPhoto.Services
{
    internal interface IConfigService
    {
        /// <summary>
        /// Gets the tenant ID from the configuration.
        /// </summary>
        string TenantId { get; }
    }
}