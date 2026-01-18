using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using MrrHak.Extensions.FileProviders.S3FileProvider;

namespace Test;

public class S3OwinDirectoryContentsTest
{
    private readonly static string bucketName = "s3-file-providers-test";

    [Fact]
    public void T001_GetEnumerable()
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
                CommonPrefixes = [],
                S3Objects = new List<S3Object>(){
                    new() {
                        BucketName = bucketName,
                        Key = subFolder,
                    }
                }
            });


        // Act
        var subContents = new S3OwinDirectoryContents(mockS3Client.Object, bucketName, "/");
        var enumerable = subContents.GetEnumerable();

        // Assert
        Assert.NotNull(enumerable);
        Assert.NotEmpty(enumerable);
        Assert.NotNull(enumerable.First());
        Assert.Equal(subFolder.TrimEnd('/'), enumerable.First().Name);
        Assert.True(enumerable.First().IsDirectory);
    }

    [Fact]
    public void T002_GetEnumerable_Exception()
    {
        // Arrange
        // Mock IAmazonS3 client
        var mockS3Client = new Mock<IAmazonS3>();
        mockS3Client
            .Setup(client => client.ListObjectsV2Async(It.IsAny<ListObjectsV2Request>(), default))
            .ThrowsAsync(new AmazonS3Exception("Test Exception"));

        // Act
        var subContents = new S3OwinDirectoryContents(mockS3Client.Object, bucketName, "/");
        var enumerable = subContents.GetEnumerable();

        // Assert
        Assert.Empty(enumerable);
    }
}