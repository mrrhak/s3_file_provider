using System.Collections;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.FileProviders;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    /// <summary>
    /// Represents the contents of an S3 directory.
    /// </summary>
    public class S3DirectoryContents : IDirectoryContents
    {
        private readonly string subpath;
        private readonly IAmazonS3 amazonS3;
        private readonly string bucketName;

        private bool IsRoot => subpath == "/";
        private IEnumerable<IFileInfo> contents = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="S3DirectoryContents"/> class.
        /// </summary>
        /// <param name="amazonS3">The Amazon S3 client.</param>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="subpath">The subpath for which to retrieve the directory contents.</param>
        public S3DirectoryContents(IAmazonS3 amazonS3, string bucketName, string subpath)
        {
            this.amazonS3 = amazonS3;
            this.bucketName = bucketName;
            this.subpath = subpath.TrimEnd('/') + "/";
        }

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        public bool Exists
        {
            get
            {
                // Root directory always exists
                if (IsRoot) return true;

                // List objects with the prefix of the subpath
                var listResponse = amazonS3.ListObjectsV2Async(new ListObjectsV2Request { BucketName = bucketName, Prefix = subpath }).Result;

                // If there are any objects returned, the directory exists
                return listResponse.S3Objects.Exists(x => x.Key == subpath);
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
            try
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
            catch (Exception e)
            {
                string message = $"Could not list objects from the S3 bucket {bucketName} with the prefix {subpath}.";
                Console.WriteLine(e.Message);
                Console.WriteLine(message);
            }
        }
    }
}