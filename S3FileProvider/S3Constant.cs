using System.Collections.Immutable;

namespace MrrHak.Extensions.FileProviders.S3FileProvider
{
    /// <summary>
    /// Provides constant values related to the S3 file provider.
    /// </summary>
    public static class S3Constant
    {
        /// <summary>
        /// Array of characters used as path separators.
        /// </summary>
        public static readonly char[] PATH_SEPARATORS = new char[] { '/' };

        /// <summary>
        /// Array of characters that are not allowed in file names.
        /// </summary>
        public static readonly ImmutableArray<char> INVALID_FILE_NAME_CHARS = ImmutableArray.CreateRange(new char[] { '\\', '{', '}', '^', '%', '`', '[', ']', '\'', '"', '>', '<', '~', '#', '|' }.Concat(Enumerable.Range(128, 255).Select(x => (char)x)));
    }
}