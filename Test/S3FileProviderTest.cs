using System.Net;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using MrrHak.Extensions.FileProviders.S3FileProvider;

namespace Test;

public class S3FileProviderTest
{
    private readonly static string bucketName = "s3-file-providers-test";

    [Fact]
    public void T001_GetRootDirectoryContents()
    {
        // Arrange
        const string root = "/";
        const string subFolder = "folder-1/";
        const string dummyFile = "dummy.txt";
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), default))
            .ReturnsAsync(new ListObjectsV2Response
            {
                HttpStatusCode = HttpStatusCode.OK,
                S3Objects = new List<S3Object>{
                    new() {
                        BucketName = bucketName,
                        Key = subFolder,
                    },
                    new() {
                        BucketName = bucketName,
                        Key = dummyFile,
                    }
                },
            });

        // Act
        var s3FileProvider = new S3FileProvider(mockS3Client.Object, bucketName, root);
        var rootContents = s3FileProvider.GetDirectoryContents(root);

        // Assert
        Assert.True(rootContents.Exists);
        Assert.NotEmpty(rootContents);
        Assert.Equal(2, rootContents.Count());
    }

    [Fact]
    public void T002_GetSubDirectoryContents()
    {
        // Arrange
        const string subFolderA = "folder-A/";
        const string subFolderB = "folder-B/";
        const string subFolderB1 = "folder-B/folder-B1/";
        const string subFolderB2 = "folder-B/folder-B2/";
        // Mock IAmazonS3 client
        var mockS3Client1 = new Mock<IAmazonS3>();
        mockS3Client1
            .Setup(client => client.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), default))
            .ReturnsAsync(new ListObjectsV2Response
            {
                HttpStatusCode = HttpStatusCode.OK,
                S3Objects = new List<S3Object>{
                    new() {
                        BucketName = bucketName,
                        Key = subFolderA,
                    }
                },
            });

        var mockS3Client2 = new Mock<IAmazonS3>();
        mockS3Client2
            .Setup(client => client.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), default))
            .ReturnsAsync(new ListObjectsV2Response
            {
                HttpStatusCode = HttpStatusCode.OK,
                S3Objects = new List<S3Object>{
                    new() {
                        BucketName = bucketName,
                        Key = subFolderB,
                    },
                    new() {
                        BucketName = bucketName,
                        Key = subFolderB1,
                    },
                    new() {
                        BucketName = bucketName,
                        Key = subFolderB2,
                    }
                },
            });

        // Act
        var s3FileProvider1 = new S3FileProvider(mockS3Client1.Object, bucketName);
        var s3FileProvider2 = new S3FileProvider(mockS3Client2.Object, bucketName);
        var subContents1 = s3FileProvider1.GetDirectoryContents(subFolderA);
        var subContents2 = s3FileProvider2.GetDirectoryContents(subFolderB);

        // Assert
        Assert.Empty(subContents1);
        Assert.True(subContents1.Exists);

        Assert.NotEmpty(subContents2);
        Assert.True(subContents2.Exists);
        Assert.Equal(2, subContents2.Count());
    }

    [Fact]
    public void T003_GetFileInfo()
    {
        // Arrange
        const string key = "hello-world.txt";
        const string expectedContent = "Hello, World!";
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(new GetObjectResponse
            {
                BucketName = bucketName,
                HttpStatusCode = HttpStatusCode.OK,
                Key = key,
                ResponseStream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContent))
            });

        // Act
        var s3FileProvider = new S3FileProvider(mockS3Client.Object, bucketName);
        var fileInfo = s3FileProvider.GetFileInfo(key);
        using var textReader = new StreamReader(fileInfo.CreateReadStream());

        // Assert
        Assert.True(fileInfo.Exists);
        Assert.Equal(key, fileInfo.Name);
        Assert.Equal(expectedContent, textReader.ReadToEnd());
    }

    [Fact]
    public void T004_GetFileInfo_RootPath()
    {
        // Arrange
        const string rootPath = "/";
        const string key = "hello-world.txt";
        const string expectedContent = "Hello, World!";
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(new GetObjectResponse
            {
                BucketName = bucketName,
                HttpStatusCode = HttpStatusCode.OK,
                Key = key,
                ResponseStream = new MemoryStream(Encoding.UTF8.GetBytes(expectedContent))
            });

        // Act
        var s3FileProvider = new S3FileProvider(mockS3Client.Object, bucketName, rootPath);
        var fileInfo = s3FileProvider.GetFileInfo(key);
        using var textReader = new StreamReader(fileInfo.CreateReadStream());

        // Assert
        Assert.True(fileInfo.Exists);
        Assert.Equal(key, fileInfo.Name);
        Assert.Equal(expectedContent, textReader.ReadToEnd());
    }

    [Fact]
    public void T005_GetFileInfo_NotFound()
    {
        // Arrange
        const string key = "hello-world.txt";
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(new GetObjectResponse
            {
                HttpStatusCode = HttpStatusCode.NotFound,
            });

        // Act
        var s3FileProvider = new S3FileProvider(mockS3Client.Object, bucketName);
        var notFoundFileInfo = s3FileProvider.GetFileInfo(key);

        // Assert
        Assert.False(notFoundFileInfo.Exists);
        Assert.Equal(0, notFoundFileInfo.Length);
    }

    [Fact]
    public void T006_GetFileInfo_EmptyKey()
    {
        // Arrange
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();

        // Act
        var s3FileProvider = new S3FileProvider(mockS3Client.Object, bucketName);

        // Assert
        Assert.Contains("Empty file name", Assert.Throws<ArgumentException>(() => s3FileProvider.GetFileInfo("")).Message);
    }

    [Fact]
    public void T007_GetFileInfo_InvalidKey()
    {
        // Arrange
        const string invalidKey = "invalid-#.txt";
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();

        // Act 
        var s3FileProvider = new S3FileProvider(mockS3Client.Object, bucketName);

        // Assert
        Assert.Contains("Invalid file name", Assert.Throws<ArgumentException>(() => s3FileProvider.GetFileInfo(invalidKey)).Message);
    }

    [Fact]
    public void T008_Watch()
    {
        // Arrange
        const string key = "hello-world.txt";
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
        var s3FileProvider = new S3FileProvider(mockS3Client.Object, bucketName);
        var changeToken = s3FileProvider.Watch("*.txt");

        // Assert
        Assert.False(changeToken.ActiveChangeCallbacks);
        Assert.False(changeToken.HasChanged);
    }

    [Fact]
    public void T009_Dispose()
    {
        // Arrange
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();

        // Act
        var s3FileProvider = new S3FileProvider(mockS3Client.Object, bucketName);
        s3FileProvider.Dispose();

        // Assert
        Assert.True(s3FileProvider.IsDeposed);
    }
}