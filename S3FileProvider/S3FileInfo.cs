using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.FileProviders;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    public class S3FileInfo(IAmazonS3 amazonS3, string bucketName, string key) : IFileInfo
    {
        private GetObjectResponse? fileObject;
        private bool? exists;

        public bool Exists
        {
            get
            {
                if (!exists.HasValue)
                {
                    try
                    {
                        GetFileObject();
                        exists = true;
                    }
                    catch (AmazonS3Exception e)
                    {
                        if (e.StatusCode == HttpStatusCode.NotFound) exists = false;
                        throw;
                    }
                }
                return exists.Value;
            }
        }

        public long Length => GetFileObject().ContentLength;

        public string? PhysicalPath => null;

        public string Name => Path.GetFileName(GetFileObject().Key.TrimEnd('/'));

        public DateTimeOffset LastModified => GetFileObject().LastModified;

        public bool IsDirectory => GetFileObject().Key.EndsWith('/');

        public Stream CreateReadStream() => GetFileObject().ResponseStream;

        private GetObjectResponse GetFileObject() => fileObject ??= amazonS3.GetObjectAsync(bucketName, key).Result;
    }
}