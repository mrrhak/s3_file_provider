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
    public void T002_Exists_FileObject_Null()
    {
        // Arrange
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ThrowsAsync(new FileNotFoundException("File not found.", key));

        // Act
        var s3FileInfo = new S3FileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.False(s3FileInfo.Exists);
    }

    [Fact]
    public void T003_Length()
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
    public void T004_Length_FileObject_Null()
    {
        // Arrange
        const long expectedLength = -1;
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ThrowsAsync(new FileNotFoundException("File not found.", key));

        // Act
        var s3FileInfo = new S3FileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.Equal(expectedLength, s3FileInfo.Length);
    }

    [Fact]
    public void T005_PhysicalPath()
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
    public void T006_Name()
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
    public void T007_Name_FileObject_Null()
    {
        // Arrange
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ThrowsAsync(new FileNotFoundException("File not found.", key));

        // Act
        var s3FileInfo = new S3FileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.Equal(key, s3FileInfo.Name);
    }

    [Fact]
    public void T008_LastModified()
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
    public void T009_LastModified_FileObject_Null()
    {
        // Arrange
        var expectedLastModified = DateTimeOffset.MinValue;
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ThrowsAsync(new FileNotFoundException("File not found.", key));

        // Act
        var s3FileInfo = new S3FileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.Equal(expectedLastModified, s3FileInfo.LastModified);
    }

    [Fact]
    public void T010_IsDirectory()
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
        var s3FileInfo1 = new S3FileInfo(mockS3Client.Object, bucketName, key);
        var s3FileInfo2 = new S3FileInfo(mockS3Client2.Object, bucketName, folder);

        // Assert
        Assert.False(s3FileInfo1.IsDirectory);
        Assert.True(s3FileInfo2.IsDirectory);
    }

    [Fact]
    public void T011_IsDirectory_FileObject_Null()
    {
        // Arrange
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ThrowsAsync(new FileNotFoundException("File not found.", key));

        // Act
        var s3FileInfo1 = new S3FileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.False(s3FileInfo1.IsDirectory);
    }

    [Fact]
    public void T012_CreateReadStream()
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

    [Fact]
    public void T013_CreateReadStream_FileObject_Null()
    {
        // Arrange
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ThrowsAsync(new FileNotFoundException("File not found.", key));

        // Act
        var s3FileInfo = new S3FileInfo(mockS3Client.Object, bucketName, key);

        // Assert
        Assert.Throws<FileNotFoundException>(() => s3FileInfo.CreateReadStream());
        Assert.Equal("File not found.", Assert.Throws<FileNotFoundException>(() => s3FileInfo.CreateReadStream()).Message);
    }
}