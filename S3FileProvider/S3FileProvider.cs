using Amazon.S3;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    public class S3FileProvider(IAmazonS3 amazonS3, string bucketName) : IFileProvider, IDisposable
    {
        private static readonly char[] pathSeparators = ['/'];
        private static readonly char[] invalidFileNameChars = ['\\', '{', '}', '^', '%', '`', '[', ']', '\'', '"', '>', '<', '~', '#', '|', .. Enumerable.Range(128, 255).Select(x => (char)x)];

        public void Dispose()
        {
            amazonS3.Dispose();
            GC.SuppressFinalize(this);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            ArgumentNullException.ThrowIfNull(subpath);
            subpath = subpath.TrimStart(pathSeparators);
            return new S3DirectoryContents(amazonS3, bucketName, subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (HasInvalidFileNameChars(subpath)) return new NotFoundFileInfo(subpath);
            subpath = subpath.TrimStart(pathSeparators);
            if (string.IsNullOrEmpty(subpath)) return new NotFoundFileInfo(subpath);
            return new S3FileInfo(amazonS3, bucketName, subpath);
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        private static bool HasInvalidFileNameChars(string path) => path.IndexOfAny(invalidFileNameChars) != -1;
    }
}