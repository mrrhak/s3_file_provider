using System.Collections;
using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.FileProviders;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    /// <summary>
    /// Represents the contents of an S3 directory.
    /// </summary>
    public class S3DirectoryContents(IAmazonS3 amazonS3, string bucketName, string subpath) : IDirectoryContents
    {
        private readonly string subpath = subpath.TrimEnd('/') + "/";
        private bool IsRoot => subpath == "/";
        private IEnumerable<IFileInfo> contents = [];

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        public bool Exists
        {
            get
            {
                try
                {
                    // Root folder always exists
                    if (IsRoot) return true;
                    amazonS3.GetObjectMetadataAsync(bucketName, subpath).Wait();
                    return true;
                }
                catch (AggregateException e)
                {
                    e.Handle(ie => ie is AmazonS3Exception _ie && _ie.StatusCode == HttpStatusCode.NotFound);
                    return false;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the directory contents.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the directory contents.</returns>
        /// <exception cref="Exception">Thrown if there is an error during enumeration.</exception>
        public IEnumerator<IFileInfo> GetEnumerator()
        {
            EnumerateContents();
            return contents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            EnumerateContents();
            return contents.GetEnumerator();
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
            var files = response.S3Objects.Where(x => x.Key != subpath).Select(x => new S3FileInfo(amazonS3, bucketName, x.Key));
            var directories = response.CommonPrefixes.Select(x => new S3FileInfo(amazonS3, bucketName, x));
            contents = directories.Concat(files);
        }
    }
}