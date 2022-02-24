namespace UpdateThumbnailPhoto.Services
{
    using System;

    using Microsoft.Win32;

    /// <summary>
    /// The file picker service.
    /// </summary>
    internal class FilePickerService : IFilePickerService
    {
        /// <inheritdoc/>
        public bool PickFile(out string filePath)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg)|*.jpg|All files (*.*)|*.*";
            var result = dialog.ShowDialog();
            filePath = dialog.FileName;
            return result == true && !string.IsNullOrEmpty(filePath);
        }
    }
}
