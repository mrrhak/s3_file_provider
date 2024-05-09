using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.FileProviders;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    /// <summary>
    /// Represents a file in an S3 bucket.
    /// </summary>
    public class S3FileInfo : IFileInfo
    {
        private readonly IAmazonS3 amazonS3;
        private readonly string bucketName;
        private readonly string key;
        private bool? exists;

        /// <summary>
        /// Initializes a new instance of the <see cref="S3FileInfo"/> class.
        /// </summary>
        /// <param name="amazonS3">The Amazon S3 client.</param>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="key">The key of the file in the S3 bucket.</param>
        public S3FileInfo(IAmazonS3 amazonS3, string bucketName, string key)
        {
            this.amazonS3 = amazonS3;
            this.bucketName = bucketName;
            this.key = key;
        }

        /// <summary>
        /// Gets a value indicating whether the file exists.
        /// </summary>
        public bool Exists
        {
            get
            {
                if (!exists.HasValue)
                {
                    var fileObject = GetFileObject();
                    exists = fileObject.Key == key;
                }
                return exists.Value;
            }
        }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        public long Length => GetFileObject().ContentLength;

        /// <summary>
        /// Gets the physical path of the file.
        /// </summary>
        public string? PhysicalPath => null;

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name => Path.GetFileName(GetFileObject().Key.TrimEnd('/'));

        /// <summary>
        /// Gets the last modified date and time of the file.
        /// </summary>
        public DateTimeOffset LastModified => GetFileObject().LastModified;

        /// <summary>
        /// Gets a value indicating whether the file is a directory.
        /// </summary>
#if NETFRAMEWORK || NETSTANDARD
        public bool IsDirectory => GetFileObject().Key.EndsWith("/");
#else
        public bool IsDirectory => GetFileObject().Key.EndsWith('/');
#endif

        /// <summary>
        /// Creates a read stream for the file.
        /// </summary>
        /// <returns>A seekable stream representing the file content.</returns>
        public Stream CreateReadStream() => AmazonS3Util.MakeStreamSeekable(GetFileObject().ResponseStream);

        private GetObjectResponse GetFileObject() => amazonS3.GetObjectAsync(bucketName, key).Result;
    }
}