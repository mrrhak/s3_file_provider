using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Owin.FileSystems;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    /// <summary>
    /// Represents the contents of an S3 directory.
    /// </summary>
    public class S3OwinDirectoryContents(IAmazonS3 amazonS3, string bucketName, string subpath)
    {
        private readonly string subpath = subpath.TrimEnd('/') + "/";
        private bool IsRoot => subpath == "/";
        private IEnumerable<IFileInfo> contents = [];

        /// <summary>
        /// Returns an enumerator that iterates through the directory contents.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the directory contents.</returns>
        /// <exception cref="Exception">Thrown if there is an error during enumeration.</exception>
        public IEnumerable<IFileInfo> GetEnumerable()
        {
            EnumerateContents();
            return contents;
        }

        private void EnumerateContents()
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                Delimiter = "/",
                Prefix = IsRoot ? "" : subpath
            };
            var response = amazonS3.ListObjectsV2Async(request).Result;
            var files = response.S3Objects.Where(x => x.Key != subpath).Select(x => new S3OwinFileInfo(amazonS3, bucketName, x.Key));
            var directories = response.CommonPrefixes.Select(x => new S3OwinFileInfo(amazonS3, bucketName, x));
            contents = directories.Concat(files);
        }
    }
}