using Amazon.S3;
using Microsoft.Owin;
using MrrHak.Extensions.FileProviders.S3FileProvider;
using Owin;

[assembly: OwinStartup(typeof(Asp.NetFramework.Startup))]

namespace Asp.NetFramework
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var s3Client = new AmazonS3Client("your-access-key", "your-secret-key", Amazon.RegionEndpoint.APSoutheast1);
            app.UseS3StaticFiles(s3Client, "your-bucket-name");

            //Setup default
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value == null || context.Request.Path.Value == "/")
                {
                    await context.Response.WriteAsync("Hello World!");
                }
            });
        }
    }
}
