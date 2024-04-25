using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using MrrHak.Extensions.FileProviders.S3FileProvider;

var builder = WebApplication.CreateBuilder(args);

string? DEFAULT_REGION = builder.Configuration["DEFAULT_REGION"];
string? S3_ACCESS_KEY = builder.Configuration["S3_ACCESS_KEY"];
string? S3_SECRET_KEY = builder.Configuration["S3_SECRET_KEY"];
string? S3_BUCKET_NAME = builder.Configuration["S3_BUCKET_NAME"];

// Get AWS profile info directly from configuration (Profile authentication)
AWSOptions awsOptions = builder.Configuration.GetAWSOptions();
if (!string.IsNullOrEmpty(DEFAULT_REGION)) awsOptions.Region = RegionEndpoint.GetBySystemName(DEFAULT_REGION);
builder.Services.AddDefaultAWSOptions(awsOptions);

// IAM user authentication
AWSOptions s3Options = awsOptions;
if (!string.IsNullOrEmpty(S3_ACCESS_KEY) && !string.IsNullOrEmpty(S3_SECRET_KEY))
{
    s3Options = new AWSOptions() { Credentials = new BasicAWSCredentials(S3_ACCESS_KEY, S3_SECRET_KEY) };
    if (!string.IsNullOrEmpty(DEFAULT_REGION)) s3Options.Region = RegionEndpoint.GetBySystemName(DEFAULT_REGION);
}

builder.Services.AddAWSService<IAmazonS3>(s3Options);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Use Extension from S3 File Provider
app.UseS3StaticFiles(S3_BUCKET_NAME!);

app.Run();
