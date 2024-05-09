using Amazon.S3;
using Microsoft.Owin.FileSystems;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    /// <summary>
    /// Represents a file system for Amazon S3 buckets.
    /// </summary>
    public class S3OwinFileSystem : IFileSystem, IDisposable
    {
        private readonly IAmazonS3 amazonS3;
        private readonly string bucketName;

        /// <summary>
        /// Initializes a new instance of the <see cref="S3OwinFileSystem"/> class.
        /// </summary>
        /// <param name="amazonS3">The Amazon S3 client.</param>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        public S3OwinFileSystem(IAmazonS3 amazonS3, string bucketName)
        {
            this.amazonS3 = amazonS3;
            this.bucketName = bucketName;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has been disposed.
        /// </summary>
        public bool IsDeposed { get; private set; } = false;

        /// <summary>
        /// Disposes the Amazon S3 client and suppresses the finalize for this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the Amazon S3 client.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                amazonS3.Dispose();
                IsDeposed = true;
            }
        }

        /// <summary>
        /// Retrieves the directory contents for the specified subpath.
        /// </summary>
        /// <param name="subpath">The subpath for which to retrieve the directory contents.</param>
        /// <param name="contents">The directory contents.</param>
        public bool TryGetDirectoryContents(string subpath, out IEnumerable<IFileInfo> contents)
        {
            subpath = subpath.TrimStart(S3Constant.PATH_SEPARATORS);
            try
            {
                var s3DirectoryContents = new S3OwinDirectoryContents(amazonS3, bucketName, subpath);
                contents = s3DirectoryContents.GetEnumerable();
                return true;
            }
            catch (Exception)
            {
                contents = Enumerable.Empty<S3OwinFileInfo>();
                return false;
            }
        }

        /// <summary>
        /// Retrieves information about a file.
        /// </summary>
        /// <param name="subpath">The path of the file.</param>
        /// <param name="fileInfo">An instance of the IFileInfo interface representing the file.</param>
        public bool TryGetFileInfo(string subpath, out IFileInfo fileInfo)
        {
            if (HasInvalidFileNameChars(subpath)) throw new ArgumentException("Invalid file name.", nameof(subpath));
            subpath = subpath.TrimStart();
            if (string.IsNullOrEmpty(subpath)) throw new ArgumentException("Empty file name.", nameof(subpath));
            fileInfo = new S3OwinFileInfo(amazonS3, bucketName, subpath);
            return true;
        }

        private static bool HasInvalidFileNameChars(string path) => path.IndexOfAny(S3Constant.INVALID_FILE_NAME_CHARS.ToArray()) != -1;
    }
}