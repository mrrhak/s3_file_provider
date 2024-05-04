using Microsoft.AspNetCore.StaticFiles;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    /// <summary>
    /// Provides options for configuring S3 static file provider.
    /// </summary>
    public class S3StaticFileOptions
    {
        /// <summary>
        /// The name of the Amazon S3 bucket.
        /// </summary>
        public string BucketName { get; set; } = string.Empty;

        /// <summary>
        /// The relative request path that maps to static resources.
        /// </summary>
        public string? RequestPath { get; set; }

        /// <summary>
        /// If the file is not a recognized content-type should it be served? Default: false.
        /// </summary>
        public bool? ServeUnknownFileTypes { get; set; }

        /// <summary>
        /// The default content type for a request if the ContentTypeProvider cannot determine one. None is provided by default, so the client must determine the format themselves.
        /// </summary>
        public string? DefaultContentType { get; set; }

        /// <summary>
        /// Used to map files to content-types.
        /// </summary>
        public IContentTypeProvider? ContentTypeProvider { get; set; }

        /// <summary>
        /// Called after the status code and headers have been set, but before the body has been written. This can be used to add or change the response headers.
        /// </summary>
        public Action<StaticFileResponseContext>? OnPrepareResponse { get; set; }
    }
}