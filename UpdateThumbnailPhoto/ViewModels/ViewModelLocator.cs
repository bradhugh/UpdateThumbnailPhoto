namespace UpdateThumbnailPhoto.ViewModels
{
    using System;

    using CommunityToolkit.Mvvm.DependencyInjection;

    using Microsoft.Extensions.DependencyInjection;

    using UpdateThumbnailPhoto.Services;
    using UpdateThumbnailPhoto.Services.Photo;

    /// <summary>
    /// A class to use for initial setup and binding to the view model.
    /// </summary>
    internal class ViewModelLocator
    {
        /// <summary>
        /// Static constructor for the <see cref="ViewModelLocator"/> class.
        /// </summary>
        static ViewModelLocator()
        {
            var services = new ServiceCollection();
            services.AddTransient<MainViewModel>();
            services.AddTransient<IConfigService, ConfigService>();
            services.AddTransient<IFilePickerService, FilePickerService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<PhotoService>();

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }

        /// <summary>
        /// Gets the main view model.
        /// </summary>
        public MainViewModel Main => Ioc.Default.GetRequiredService<MainViewModel>();
    }
}
