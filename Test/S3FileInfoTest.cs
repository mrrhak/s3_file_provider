using System.Net;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using MrrHak.Extensions.FileProviders.S3FileProvider;

namespace Test;

public class S3FileInfoTest
{
    private readonly static string bucketName = "s3-file-providers-test";
    private readonly string key = "hello-world.txt";

    [Fact]
    public void T001_Exists()
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
        var s3FileInfo = new S3FileInfo(mockS3Client.Object, bucketName, key);
        var s3FileInfoNotExist = new S3FileInfo(mockS3Client.Object, bucketName, "not-exist.txt");

        // Assert
        Assert.True(s3FileInfo.Exists);
        Assert.False(s3FileInfoNotExist.Exists);
    }

    [Fact]
    public void T002_Length()
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
        var s3FileInfo = new S3FileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.Equal(expectedLength, s3FileInfo.Length);
    }

    [Fact]
    public void T003_PhysicalPath()
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
        var s3FileInfo = new S3FileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.Null(s3FileInfo.PhysicalPath);
    }

    [Fact]
    public void T004_Name()
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
        var s3FileInfo = new S3FileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.Equal(key, s3FileInfo.Name);
    }

    [Fact]
    public void T005_LastModified()
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
        var s3FileInfo = new S3FileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.Equal(expectedLastModified, s3FileInfo.LastModified);
    }

    [Fact]
    public void T006_IsDirectory()
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
        var s3FileInfo = new S3FileInfo(mockS3Client.Object, bucketName, key);
        var s3FileInfo2 = new S3FileInfo(mockS3Client2.Object, bucketName, folder);

        // Assert
        Assert.False(s3FileInfo.IsDirectory);
        Assert.True(s3FileInfo2.IsDirectory);
    }

    [Fact]
    public void T007_CreateReadStream()
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
        var s3FileInfo = new S3FileInfo(mockS3Client.Object, bucketName, key);
        var stream = s3FileInfo.CreateReadStream();

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