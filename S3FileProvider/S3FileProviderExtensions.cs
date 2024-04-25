using Amazon.S3;
using Microsoft.AspNetCore.Builder;

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
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseS3StaticFiles(this IApplicationBuilder app, string bucketName)
        {
            var s3FileProvider = app.ApplicationServices.GetS3FileProvider(bucketName);
            var staticFilesOption = new StaticFileOptions() { FileProvider = s3FileProvider };
            return app.UseStaticFiles(staticFilesOption);
        }
    }
}