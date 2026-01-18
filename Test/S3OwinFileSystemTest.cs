using System.Net;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using MrrHak.Extensions.FileProviders.S3FileProvider;

namespace Test
{
    public class S3OwinFileSystemTest
    {
        private readonly static string bucketName = "s3-file-providers-test";

        [Fact]
        public void T001_TryGetRootDirectoryContents()
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
                    CommonPrefixes = [],
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
            var s3OwinFileSystem = new S3OwinFileSystem(mockS3Client.Object, bucketName, root);
            bool res = s3OwinFileSystem.TryGetDirectoryContents(root, out var rootContents);

            // Assert
            Assert.True(res);
            Assert.NotEmpty(rootContents);
            Assert.Equal(2, rootContents.Count());
        }

        [Fact]
        public void T002_TryGetSubDirectoryContents()
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
                    CommonPrefixes = [],
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
                    CommonPrefixes = [],
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
            var s3OwinFileSystem1 = new S3OwinFileSystem(mockS3Client1.Object, bucketName);
            var s3OwinFileSystem2 = new S3OwinFileSystem(mockS3Client2.Object, bucketName);
            bool resContents1 = s3OwinFileSystem1.TryGetDirectoryContents(subFolderA, out var subContents1);
            bool resContents2 = s3OwinFileSystem2.TryGetDirectoryContents(subFolderB, out var subContents2);

            // Assert
            Assert.True(resContents1);
            Assert.Empty(subContents1);

            Assert.True(resContents2);
            Assert.NotEmpty(subContents2);
            Assert.Equal(2, subContents2.Count());
        }

        [Fact]
        public void T003_TryGetFileInfo()
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

            // Act and Assert
            var s3OwinFileSystem = new S3OwinFileSystem(mockS3Client.Object, bucketName);

            try
            {
                var resFileInfo = s3OwinFileSystem.TryGetFileInfo(key, out var fileInfo);
                using var textReader = new StreamReader(fileInfo.CreateReadStream());
                Assert.True(resFileInfo);
                Assert.Equal(key, fileInfo.Name);
                Assert.Equal(expectedContent, textReader.ReadToEnd());
            }
            catch (ArgumentException ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void T004_TryGetFileInfo_RootPath()
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

            // Act and Assert
            var s3OwinFileSystem = new S3OwinFileSystem(mockS3Client.Object, bucketName, rootPath);

            try
            {
                var resFileInfo = s3OwinFileSystem.TryGetFileInfo(key, out var fileInfo);
                using var textReader = new StreamReader(fileInfo.CreateReadStream());
                Assert.True(resFileInfo);
                Assert.Equal(key, fileInfo.Name);
                Assert.Equal(expectedContent, textReader.ReadToEnd());
            }
            catch (ArgumentException ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void T005_TryGetFileInfo_NotFound()
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

            // Act and Assert
            var s3OwinFileSystem = new S3OwinFileSystem(mockS3Client.Object, bucketName);

            try
            {
                var resNotFoundFileInfo = s3OwinFileSystem.TryGetFileInfo(key, out var notFoundFileInfo);
                Assert.True(resNotFoundFileInfo);
                Assert.Equal(0, notFoundFileInfo.Length);
            }
            catch (ArgumentException ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Fact]
        public void T006_TryGetFileInfo_EmptyKey()
        {
            // Arrange
            // Mock IAmazonS3 client
            var mockS3Client = new Mock<IAmazonS3>();

            // Act
            var s3OwinFileSystem = new S3OwinFileSystem(mockS3Client.Object, bucketName);

            // Assert
            Assert.Contains("Empty file name", Assert.Throws<ArgumentException>(() => s3OwinFileSystem.TryGetFileInfo("", out _)).Message);
        }

        [Fact]
        public void T007_TryGetFileInfo_InvalidKey()
        {
            // Arrange
            const string invalidKey = "invalid-#.txt";
            // Mock IAmazonS3 client
            var mockS3Client = new Mock<IAmazonS3>();

            // Act
            var s3OwinFileSystem = new S3OwinFileSystem(mockS3Client.Object, bucketName);

            // Assert
            Assert.Contains("Invalid file name", Assert.Throws<ArgumentException>(() => s3OwinFileSystem.TryGetFileInfo(invalidKey, out _)).Message);
        }

        [Fact]
        public void T008_Dispose()
        {
            // Arrange
            // Mock IAmazonS3 client
            var mockS3Client = new Mock<IAmazonS3>();

            // Act
            var s3OwinFileSystem = new S3OwinFileSystem(mockS3Client.Object, bucketName);
            s3OwinFileSystem.Dispose();

            // Assert
            Assert.True(s3OwinFileSystem.IsDeposed);
        }
    }
}