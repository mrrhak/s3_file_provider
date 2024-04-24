using Amazon.S3;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    /// <summary>
    /// Represents a file provider for Amazon S3 buckets.
    /// </summary>
    public class S3FileProvider(IAmazonS3 amazonS3, string bucketName) : IFileProvider, IDisposable
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
        /// <returns>An instance of the IDirectoryContents interface representing the directory contents.</returns>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            subpath = subpath.TrimStart(pathSeparators);
            return new S3DirectoryContents(amazonS3, bucketName, subpath);
        }

        /// <summary>
        /// Retrieves information about a file.
        /// </summary>
        /// <param name="subpath">The path of the file.</param>
        /// <returns>An instance of the IFileInfo interface representing the file.</returns>
        public IFileInfo GetFileInfo(string subpath)
        {
            if (HasInvalidFileNameChars(subpath)) return new NotFoundFileInfo(subpath);
            subpath = subpath.TrimStart(pathSeparators);
            if (string.IsNullOrEmpty(subpath)) return new NotFoundFileInfo(subpath);
            return new S3FileInfo(amazonS3, bucketName, subpath);
        }

        /// <summary>
        /// Watches for changes to the specified filter and returns a change token.
        /// </summary>
        /// <param name="filter">The filter to watch for changes.</param>
        /// <returns>A change token that represents the changes made to the filter.</returns>
        public IChangeToken Watch(string filter) => NullChangeToken.Singleton;

        private static bool HasInvalidFileNameChars(string path) => path.IndexOfAny(invalidFileNameChars) != -1;
    }
}