using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Moq;
using MrrHak.Extensions.FileProviders.S3FileProvider;

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

        }

        [Fact]
        public void T001_UseS3StaticFiles()
        {
            // Arrange
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(sp => sp.GetService(typeof(IAmazonS3))).Returns(new Mock<IAmazonS3>().Object);

            var builder = new Mock<IApplicationBuilder>();
            builder.Setup(b => b.ApplicationServices).Returns(serviceProvider.Object);

            // Act And Assert
            try
            {
                builder.Object.UseS3StaticFiles(bucketName);
            }
            catch (Exception ex)
            {
                Assert.Null(ex);
            }
        }
    }
}