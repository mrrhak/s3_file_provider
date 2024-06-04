using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using MrrHak.Extensions.FileProviders.S3FileProvider;

namespace Test;

public class S3DirectoryContentsTest
{
    private readonly static string bucketName = "s3-file-providers-test";

    [Fact]
    public void T001_Exists()
    {
        // Arrange
        const string subFolder = "sub-folder/";
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();

        mockS3Client
            .Setup(client => client.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), default))
            .ReturnsAsync(new ListObjectsV2Response
            {
                HttpStatusCode = HttpStatusCode.OK,
                S3Objects = new List<S3Object>(){
                    new() {
                        BucketName = bucketName,
                        Key = subFolder,
                    }
                }
            });


        // Act
        var subContents = new S3DirectoryContents(mockS3Client.Object, bucketName, subFolder);
        var notFountContents = new S3DirectoryContents(mockS3Client.Object, bucketName, "not-exist/");

        // Assert
        Assert.True(subContents.Exists);
        Assert.False(notFountContents.Exists);
    }

    [Fact]
    public void T002_GetEnumerator()
    {
        // Arrange
        const string subFolder = "sub-folder/";
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();

        mockS3Client
            .Setup(client => client.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .ReturnsAsync(new GetObjectResponse
            {
                BucketName = bucketName,
                HttpStatusCode = HttpStatusCode.OK,
                Key = subFolder,
            });

        mockS3Client
            .Setup(client => client.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), default))
            .ReturnsAsync(new ListObjectsV2Response
            {
                HttpStatusCode = HttpStatusCode.OK,
                S3Objects = new List<S3Object>(){
                    new() {
                        BucketName = bucketName,
                        Key = subFolder,
                    }
                }
            });


        // Act
        var subContents = new S3DirectoryContents(mockS3Client.Object, bucketName, "/");
        var enumeration = subContents.GetEnumerator();

        // Assert
        Assert.Null(enumeration.Current);
        Assert.True(enumeration.MoveNext());
        Assert.NotNull(enumeration.Current);
        Assert.True(enumeration.Current.Exists);
        Assert.Equal(subFolder.TrimEnd('/'), enumeration.Current.Name);
        Assert.True(enumeration.Current.IsDirectory);
        Assert.False(enumeration.MoveNext());
        Assert.Null(enumeration.Current);
    }

    [Fact]
    public void T003_GetEnumerator_Exception()
    {
        // Arrange
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), default))
            .ThrowsAsync(new AmazonS3Exception("Test Exception"));

        // Act
        var subContents = new S3DirectoryContents(mockS3Client.Object, bucketName, "/");
        var enumeration = subContents.GetEnumerator();

        // Assert
        Assert.False(enumeration.MoveNext());
    }
}