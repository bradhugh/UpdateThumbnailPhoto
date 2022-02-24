namespace UpdateThumbnailPhoto.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    using UpdateThumbnailPhoto.Services;
    using UpdateThumbnailPhoto.Services.Photo;

    /// <summary>
    /// The main view model.
    /// </summary>
    internal class MainViewModel : ObservableObject
    {
        /// <summary>
        /// The file picker service.
        /// </summary>
        private readonly IFilePickerService filePicker;

        /// <summary>
        /// The photo service.
        /// </summary>
        private readonly PhotoService photoService;

        /// <summary>
        /// The user photo file data.
        /// </summary>
        private byte[] fileData;
        
        /// <summary>
        /// The photo image source.
        /// </summary>
        private ImageSource photo;

        /// <summary>
        /// A value indicating whether the file data has been changed (is dirty).
        /// </summary>
        private bool fileDataDirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="filePicker">The file picker.</param>
        /// <param name="photoService">The photo service.</param>
        public MainViewModel(IFilePickerService filePicker, PhotoService photoService)
        {
            this.BrowseCommand = new RelayCommand(this.PickPhoto);
            this.UploadCommand = new AsyncRelayCommand(this.UploadPhotoAsync, this.CanExecuteUploadPhoto);
            this.LoadPhotoCommand = new AsyncRelayCommand(this.LoadPhotoAsync);
            this.filePicker = filePicker;
            this.photoService = photoService;
            this.PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// Gets or sets the photo file data.
        /// </summary>
        public byte[] FileData
        {
            get => this.fileData;
            set => this.SetProperty(ref this.fileData, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the file data has been changed (is dirty).
        /// </summary>
        public bool FileDataDirty
        {
            get => this.fileDataDirty;
            set => this.SetProperty(ref this.fileDataDirty, value);
        }

        /// <summary>
        /// Gets or sets the photo image source.
        /// </summary>
        public ImageSource Photo
        {
            get => this.photo;
            set => this.SetProperty(ref this.photo, value);
        }

        /// <summary>
        /// Gets the Browse command to allow the user to select a file.
        /// </summary>
        public RelayCommand BrowseCommand { get; private set; }

        /// <summary>
        /// Gets the Upload command to allow the user to upload a file.
        /// </summary>
        public AsyncRelayCommand UploadCommand { get; private set; }

        /// <summary>
        /// Gets the Load Photo command that loads the photo from Azure AD.
        /// </summary>
        public AsyncRelayCommand LoadPhotoCommand { get; private set; }

        /// <summary>
        /// Fires when a property on the view model changes.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(this.FileData):

                    if (this.FileData != null)
                    {
                        var image = new BitmapImage();
                        using (var ms = new MemoryStream(this.FileData))
                        {
                            image.BeginInit();
                            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = ms;
                            image.EndInit();
                            image.Freeze();
                        }

                        this.Photo = image;
                    }

                    this.UploadCommand.NotifyCanExecuteChanged();
                    break;

                case nameof(this.FileDataDirty):
                    this.UploadCommand.NotifyCanExecuteChanged();
                    break;
            }
        }

        /// <summary>
        /// Fired when the user is browsing for a photo.
        /// </summary>
        private void PickPhoto()
        {
            if (this.filePicker.PickFile(out var path))
            {
                if (File.Exists(path))
                {
                    this.FileData = File.ReadAllBytes(path);
                    this.FileDataDirty = true;
                }
            }
        }

        /// <summary>
        /// Checks whether the state is valid for the user to click the Upload button.
        /// </summary>
        /// <returns>True if upload is allowed.</returns>
        private bool CanExecuteUploadPhoto()
        {
            return this.FileData?.Length > 0 && this.FileDataDirty;
        }

        /// <summary>
        /// Uploads the current photo.
        /// </summary>
        /// <returns>A task.</returns>
        private async Task UploadPhotoAsync()
        {
            if (this.FileData?.Length > 0 && this.FileDataDirty)
            {
                await this.photoService.UploadPhotoAsync(this.FileData);
                this.FileDataDirty = false;
                MessageBox.Show(App.Current.MainWindow, "Your photo was uploaded.");
            }
        }

        /// <summary>
        /// Loads the photo from Azure AD.
        /// </summary>
        /// <returns>A task.</returns>
        private async Task LoadPhotoAsync()
        {
            this.FileData = await this.photoService.GetMyPhotoAsync();
            this.FileDataDirty = false;
        }
    }
}
