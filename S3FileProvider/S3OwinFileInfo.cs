using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Owin.FileSystems;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    /// <summary>
    /// Represents a file in an S3 bucket.
    /// </summary>
    public class S3OwinFileInfo(IAmazonS3 amazonS3, string bucketName, string key) : IFileInfo
    {
        private bool? exists;

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
        public DateTime LastModified => GetFileObject().LastModified;

        /// <summary>
        /// Gets a value indicating whether the file is a directory.
        /// </summary>
        public bool IsDirectory => GetFileObject().Key.EndsWith("/");

        /// <summary>
        /// Creates a read stream for the file.
        /// </summary>
        /// <returns>A seekable stream representing the file content.</returns>
        public Stream CreateReadStream() => AmazonS3Util.MakeStreamSeekable(GetFileObject().ResponseStream);

        private GetObjectResponse GetFileObject() => amazonS3.GetObjectAsync(bucketName, key).Result;

    }
}