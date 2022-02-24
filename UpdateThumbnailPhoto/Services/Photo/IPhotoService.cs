namespace UpdateThumbnailPhoto.Services.Photo
{
    using System.Threading.Tasks;

    /// <summary>
    /// A service contract for the photo service.
    /// </summary>
    internal interface IPhotoService
    {
        /// <summary>
        /// Deletes the current user's photo.
        /// </summary>
        /// <returns>A task.</returns
        Task DeletePhotoAsync();

        /// <summary>
        /// Gets the current user's photo.
        /// </summary>
        /// <returns>The photo bytes.</returns>
        Task<byte[]> GetPhotoAsync();

        /// <summary>
        /// Uploads the current user's photo.
        /// </summary>
        /// <param name="content">The photo content.</param>
        /// <returns>A task.</returns>
        Task UploadPhotoAsync(byte[] content);
    }
}