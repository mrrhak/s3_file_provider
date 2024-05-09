using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.Owin;
using Owin;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    /// <summary>
    /// Provides extension methods for configuring S3 static file provider.
    /// </summary>
    public static class S3FileProviderExtensions
    {

        /// <summary>
        /// Retrieves the S3 file provider from the service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <returns>The S3 file provider.</returns>
        public static S3FileProvider GetS3FileProvider(this IServiceProvider serviceProvider, string bucketName)
        {
            var amazonS3 = serviceProvider.GetService(typeof(IAmazonS3)) as IAmazonS3 ?? throw new InvalidOperationException("Could not get an IAmazonS3 instance from the service provider.");
            return new S3FileProvider(amazonS3, bucketName);
        }

        /// <summary>
        /// Extension method for configuring the application to use S3 static files.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="requestPath">The relative request path that maps to static resources.</param>
        /// <param name="serveUnknownFileTypes">If the file is not a recognized content-type should it be served? Default: false</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseS3StaticFiles(this IApplicationBuilder app, string bucketName, string? requestPath = null, bool? serveUnknownFileTypes = false)
        {
            var s3FileProvider = app.ApplicationServices.GetS3FileProvider(bucketName);
            var staticFilesOption = new StaticFileOptions() { FileProvider = s3FileProvider };
            if (!string.IsNullOrEmpty(requestPath)) staticFilesOption.RequestPath = requestPath;
            if (serveUnknownFileTypes.HasValue) staticFilesOption.ServeUnknownFileTypes = serveUnknownFileTypes.Value;
            return app.UseStaticFiles(staticFilesOption);
        }

        /// <summary>
        /// Extension method for configuring the application to use S3 static files with custom options.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="options">The options for configuring the S3 static files.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseS3StaticFiles(this IApplicationBuilder app, S3StaticFileOptions options)
        {
            var s3FileProvider = app.ApplicationServices.GetS3FileProvider(options.BucketName);
            var staticFilesOption = new StaticFileOptions() { FileProvider = s3FileProvider };
            if (!string.IsNullOrEmpty(options.RequestPath)) staticFilesOption.RequestPath = options.RequestPath;
            if (options.ServeUnknownFileTypes.HasValue) staticFilesOption.ServeUnknownFileTypes = options.ServeUnknownFileTypes.Value;
            if (!string.IsNullOrEmpty(options.DefaultContentType)) staticFilesOption.DefaultContentType = options.DefaultContentType;
            if (options.ContentTypeProvider != null) staticFilesOption.ContentTypeProvider = options.ContentTypeProvider;
            if (options.OnPrepareResponse != null) staticFilesOption.OnPrepareResponse = options.OnPrepareResponse;
            return app.UseStaticFiles(staticFilesOption);
        }

        /// <summary>
        /// Extension method for configuring the application to use S3 static files with Owin.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="amazonS3">The Amazon S3 client.</param>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="requestPath">The relative request path that maps to static resources.</param>
        /// <param name="serveUnknownFileTypes">If the file is not a recognized content-type should it be served? Default: false</param>
        /// <returns>The updated application builder.</returns>
        public static IAppBuilder UseS3StaticFiles(this IAppBuilder app, IAmazonS3 amazonS3, string bucketName, string? requestPath = null, bool? serveUnknownFileTypes = false)
        {
            Microsoft.Owin.StaticFiles.StaticFileOptions staticFileOptions = new() { FileSystem = new S3OwinFileSystem(amazonS3, bucketName) };
            if (!string.IsNullOrEmpty(requestPath)) staticFileOptions.RequestPath = PathString.FromUriComponent(requestPath);
            if (serveUnknownFileTypes.HasValue) staticFileOptions.ServeUnknownFileTypes = serveUnknownFileTypes.Value;
            return app.UseStaticFiles(staticFileOptions);
        }
    }
}