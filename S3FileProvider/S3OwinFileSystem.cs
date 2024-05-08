using Amazon.S3;
using Microsoft.Owin.FileSystems;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    /// <summary>
    /// Represents a file system for Amazon S3 buckets.
    /// </summary>
    public class S3OwinFileSystem(IAmazonS3 amazonS3, string bucketName) : IFileSystem, IDisposable
    {
        private static readonly char[] pathSeparators = ['/'];
        private static readonly char[] invalidFileNameChars = ['\\', '{', '}', '^', '%', '`', '[', ']', '\'', '"', '>', '<', '~', '#', '|', .. Enumerable.Range(128, 255).Select(x => (char)x)];

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
            subpath = subpath.TrimStart(pathSeparators);
            try
            {
                var s3DirectoryContents = new S3OwinDirectoryContents(amazonS3, bucketName, subpath);
                contents = s3DirectoryContents.GetEnumerable();
                return true;
            }
            catch (Exception)
            {
                contents = [];
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
            if (HasInvalidFileNameChars(subpath)) throw new ArgumentException(nameof(subpath));
            subpath = subpath.TrimStart(pathSeparators);
            if (string.IsNullOrEmpty(subpath)) throw new ArgumentException(nameof(subpath));
            fileInfo = new S3OwinFileInfo(amazonS3, bucketName, subpath);
            return true;
        }

        private static bool HasInvalidFileNameChars(string path) => path.IndexOfAny(invalidFileNameChars) != -1;
    }
}