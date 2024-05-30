using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Moq;
using MrrHak.Extensions.FileProviders.S3FileProvider;
using Owin;

namespace Test
{
    public class S3FileProviderExtensionsTest
    {
        private readonly static string bucketName = "s3-file-providers-test";

        [Fact]
        public void T001_GetS3FileProvider()
        {
            // Arrange
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(sp => sp.GetService(typeof(IAmazonS3))).Returns(new Mock<IAmazonS3>().Object);
            var serviceProviderException = new Mock<IServiceProvider>();

            // Act
            try
            {
                var s3FileProvider = serviceProvider.Object.GetS3FileProvider(bucketName);
                Assert.NotNull(s3FileProvider);
                Assert.False(s3FileProvider.IsDeposed);
            }
            catch (Exception ex)
            {
                Assert.Null(ex);
            }

            try
            {
                serviceProviderException.Object.GetS3FileProvider(bucketName);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
                Assert.Equal("Could not get an IAmazonS3 instance from the service provider.", ex.Message);
            }
        }

        [Fact]
        public void T002_UseS3StaticFiles()
        {
            // Arrange
            string requestPath = "/static";
            string rootPath = "/";
            bool serveUnknownFileTypes = true;
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(sp => sp.GetService(typeof(IAmazonS3))).Returns(new Mock<IAmazonS3>().Object);

            var builder = new Mock<IApplicationBuilder>();
            builder.Setup(b => b.ApplicationServices).Returns(serviceProvider.Object);

            // Act And Assert
            try
            {
                // Case 1
                builder.Object.UseS3StaticFiles(bucketName);

                // Case 2
                builder.Object.UseS3StaticFiles(bucketName, requestPath: requestPath);

                // Case 3
                builder.Object.UseS3StaticFiles(bucketName, serveUnknownFileTypes: serveUnknownFileTypes);

                // Case 4
                builder.Object.UseS3StaticFiles(bucketName, requestPath: requestPath, serveUnknownFileTypes: serveUnknownFileTypes);

                // Case 5
                builder.Object.UseS3StaticFiles(bucketName, requestPath: requestPath, rootPath: rootPath, serveUnknownFileTypes: serveUnknownFileTypes);
            }
            catch (Exception ex)
            {
                Assert.Null(ex);
            }
        }

        [Fact]
        public void T003_UseS3StaticFiles_With_Options()
        {
            // Arrange
            string requestPath = "/static";
            string rootPath = "/";
            bool serveUnknownFileTypes = true;
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(sp => sp.GetService(typeof(IAmazonS3))).Returns(new Mock<IAmazonS3>().Object);

            var builder = new Mock<IApplicationBuilder>();
            builder.Setup(b => b.ApplicationServices).Returns(serviceProvider.Object);

            // Act And Assert
            try
            {
                // Case 1
                var s3StaticFileOptions = new S3StaticFileOptions() { BucketName = bucketName };
                builder.Object.UseS3StaticFiles(s3StaticFileOptions);

                // Case 2
                s3StaticFileOptions.RequestPath = requestPath;
                builder.Object.UseS3StaticFiles(s3StaticFileOptions);

                // Case 3
                s3StaticFileOptions.RootPath = rootPath;
                builder.Object.UseS3StaticFiles(s3StaticFileOptions);

                // Case 4
                s3StaticFileOptions.ServeUnknownFileTypes = serveUnknownFileTypes;
                builder.Object.UseS3StaticFiles(s3StaticFileOptions);

                // Case 5
                s3StaticFileOptions.DefaultContentType = "text/html";
                builder.Object.UseS3StaticFiles(s3StaticFileOptions);

                // Case 6
                s3StaticFileOptions.ContentTypeProvider = new Mock<IContentTypeProvider>().Object;
                builder.Object.UseS3StaticFiles(s3StaticFileOptions);

                // Case 7
                s3StaticFileOptions.OnPrepareResponse = (context) =>
                {
                    context.Context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                    context.Context.Response.Headers["Expires"] = "-1";
                };
                builder.Object.UseS3StaticFiles(s3StaticFileOptions);
            }
            catch (Exception ex)
            {
                Assert.Null(ex);
            }
        }

        [Fact]
        public void T004_UseS3StaticFiles_Framework()
        {
            // Arrange
            string requestPath = "/static";
            string rootPath = "/";
            bool serveUnknownFileTypes = true;
            var mockS3Client = new Mock<IAmazonS3>();
            var builder = new Mock<IAppBuilder>();

            // Act And Assert
            try
            {
                // Case 1
                builder.Object.UseS3StaticFiles(mockS3Client.Object, bucketName);

                // Case 2
                builder.Object.UseS3StaticFiles(mockS3Client.Object, bucketName, requestPath: requestPath);

                // Case 3
                builder.Object.UseS3StaticFiles(mockS3Client.Object, bucketName, serveUnknownFileTypes: serveUnknownFileTypes);

                // Case 4
                builder.Object.UseS3StaticFiles(mockS3Client.Object, bucketName, requestPath: requestPath, serveUnknownFileTypes: serveUnknownFileTypes);

                // Case 5
                builder.Object.UseS3StaticFiles(mockS3Client.Object, bucketName, requestPath: requestPath, rootPath: rootPath, serveUnknownFileTypes: serveUnknownFileTypes);
            }
            catch (Exception ex)
            {
                Assert.Null(ex);
            }
        }
    }
}