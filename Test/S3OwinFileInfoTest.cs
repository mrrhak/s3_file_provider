using System.Net;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using MrrHak.Extensions.FileProviders.S3FileProvider;

namespace Test;

public class S3OwinFileInfoTest
{
    private readonly static string bucketName = "s3-file-providers-test";
    private readonly string key = "hello-world.txt";

    [Fact]
    public void T001_Length()
    {
        // Arrange
        const long expectedLength = 12;
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(new GetObjectResponse
            {
                BucketName = bucketName,
                HttpStatusCode = HttpStatusCode.OK,
                Key = key,
                ContentLength = expectedLength,
            });

        // Act
        var s3OwinFileInfo = new S3OwinFileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.Equal(expectedLength, s3OwinFileInfo.Length);
    }

    [Fact]
    public void T002_PhysicalPath()
    {
        // Arrange
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(new GetObjectResponse
            {
                BucketName = bucketName,
                HttpStatusCode = HttpStatusCode.OK,
                Key = key,
            });

        // Act
        var s3OwinFileInfo = new S3OwinFileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.Null(s3OwinFileInfo.PhysicalPath);
    }

    [Fact]
    public void T003_Name()
    {
        // Arrange
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(new GetObjectResponse
            {
                BucketName = bucketName,
                HttpStatusCode = HttpStatusCode.OK,
                Key = key,
            });

        // Act
        var s3OwinFileInfo = new S3OwinFileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.Equal(key, s3OwinFileInfo.Name);
    }

    [Fact]
    public void T004_LastModified()
    {
        // Arrange
        var expectedLastModified = DateTime.UtcNow;
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(new GetObjectResponse
            {
                BucketName = bucketName,
                HttpStatusCode = HttpStatusCode.OK,
                Key = key,
                LastModified = expectedLastModified,
            });

        // Act
        var s3OwinFileInfo = new S3OwinFileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.Equal(expectedLastModified, s3OwinFileInfo.LastModified);
    }

    [Fact]
    public void T005_IsDirectory()
    {
        // Arrange
        const string folder = "/folder/";
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(new GetObjectResponse
            {
                BucketName = bucketName,
                HttpStatusCode = HttpStatusCode.OK,
                Key = key,
            });

        var mockS3Client2 = new Mock<IAmazonS3>();
        mockS3Client2
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(new GetObjectResponse
            {
                BucketName = bucketName,
                HttpStatusCode = HttpStatusCode.OK,
                Key = folder,
            });

        // Act
        var s3OwinFileInfo1 = new S3OwinFileInfo(mockS3Client.Object, bucketName, key);
        var s3OwinFileInfo2 = new S3OwinFileInfo(mockS3Client2.Object, bucketName, folder);

        // Assert
        Assert.False(s3OwinFileInfo1.IsDirectory);
        Assert.True(s3OwinFileInfo2.IsDirectory);
    }

    [Fact]
    public void T006_CreateReadStream()
    {
        // Arrange
        const string streamContent = "Hello, World!";
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(new GetObjectResponse
            {
                BucketName = bucketName,
                HttpStatusCode = HttpStatusCode.OK,
                Key = key,
                ResponseStream = new MemoryStream(Encoding.UTF8.GetBytes(streamContent)),
            });

        // Act
        var s3OwinFileInfo = new S3OwinFileInfo(mockS3Client.Object, bucketName, key);
        var stream = s3OwinFileInfo.CreateReadStream();

        // Assert
        Assert.NotNull(stream);
        Assert.True(stream.CanRead);
        Assert.True(stream.CanWrite);
        Assert.True(stream.CanSeek);
        Assert.Equal(0, stream.Position);
        Assert.Equal(streamContent.Length, stream.Length);
        Assert.Equal(streamContent, new StreamReader(stream).ReadToEnd());
    }
}