namespace UpdateThumbnailPhoto.Services
{
    /// <summary>
    /// A service interface for the file picker.
    /// </summary>
    internal interface IFilePickerService
    {
        /// <summary>
        /// Allows the user to select a file from the filesystem.
        /// </summary>
        /// <param name="filePath">The selected file path</param>
        /// <returns>True if user selected a file and clicked ok.</returns>
        bool PickFile(out string filePath);
    }
}