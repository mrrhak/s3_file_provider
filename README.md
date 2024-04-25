![S3 File Provider](https://raw.githubusercontent.com/mrrhak/s3_file_provider/master/icon-small.png) S3 File Provider
======================

[![NuGet](http://img.shields.io/nuget/vpre/MrrHak.Extensions.FileProviders.S3FileProvider.svg?label=NuGet&logo=nuget)](https://www.nuget.org/packages/MrrHak.Extensions.FileProviders.S3FileProvider)
[![NuGet Downloads](https://img.shields.io/nuget/dt/MrrHak.Extensions.FileProviders.S3FileProvider?style=flat&logo=docusign&label=Downloads&link=https%3A%2F%2Fwww.nuget.org%2Fstats%2Fpackages%2FMrrHak.Extensions.FileProviders.S3FileProvider)](https://www.nuget.org/stats/packages/MrrHak.Extensions.FileProviders.S3FileProvider?groupby=Version)
[![File count](https://img.shields.io/github/directory-file-count/mrrhak/s3_file_provider?type=file&style=flat&logo=onlyoffice&label=Files&link=https%3A%2F%2Fgithub.com%2Fmrrhak%2Fs3_file_provider)](https://github.com/mrrhak/s3_file_provider)
[![Repo size](https://img.shields.io/github/repo-size/mrrhak/s3_file_provider?style=flat&logo=github&label=Repo%20size&link=https%3A%2F%2Fgithub.com%2Fmrrhak%2Fs3_file_provider)](https://github.com/mrrhak/s3_file_provider)
[![Code size](https://img.shields.io/github/languages/code-size/mrrhak/s3_file_provider?logo=csharp&color=blue&label=Code%20size)](https://github.com/mrrhak/s3_file_provider)
[![Star on Github](https://img.shields.io/github/stars/mrrhak/s3_file_provider.svg?style=flat&logo=github&colorB=deeppink&label=Stars)](https://github.com/mrrhak/s3_file_provider)
[![Forks on Github](https://img.shields.io/github/forks/mrrhak/s3_file_provider?style=flat&label=Forks&logo=github)](https://github.com/mrrhak/s3_file_provider)
[![Test Status](https://github.com/mrrhak/s3_file_provider/actions/workflows/dotnet.yml/badge.svg) ](https://github.com/mrrhak/s3_file_provider/actions?query=workflow%3A)
[![Publish Status](https://github.com/mrrhak/s3_file_provider/actions/workflows/publish.yml/badge.svg) ](https://github.com/mrrhak/s3_file_provider/actions?query=workflow%3A)
[![SonarCloud Status](https://github.com/mrrhak/s3_file_provider/actions/workflows/sonarcloud.yml/badge.svg) ](https://github.com/mrrhak/s3_file_provider/actions?query=workflow%3A)
[![Sonar Quality Gate](https://img.shields.io/sonar/quality_gate/mrrhak_s3_file_provider?server=https%3A%2F%2Fsonarcloud.io&style=flat&logo=sonarcloud&label=Quality%20Gate)](https://sonarcloud.io/summary/overall?id=mrrhak_s3_file_provider)
[![Sonar Coverage](https://img.shields.io/sonar/coverage/mrrhak_s3_file_provider?server=https%3A%2F%2Fsonarcloud.io&style=flat&logo=sonarcloud&label=Coverage)](https://sonarcloud.io/summary/overall?id=mrrhak_s3_file_provider)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=mrrhak_s3_file_provider&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=mrrhak_s3_file_provider)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=mrrhak_s3_file_provider&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=mrrhak_s3_file_provider)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=mrrhak_s3_file_provider&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=mrrhak_s3_file_provider)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=mrrhak_s3_file_provider&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=mrrhak_s3_file_provider)
[![License: MIT](https://img.shields.io/github/license/mrrhak/s3_file_provider?label=License&color=red&logo=Leanpub)](https://opensource.org/licenses/MIT)
[![Developer](https://img.shields.io/badge/Developed_by-Mrr_Hak-blue.svg?logo=devdotto)](https://mrrhak.com)
[![Framework](https://img.shields.io/badge/Frameworks-.Net_8.0_|_.Net_7.0_|_.Net_6.0_|_.Net_Standard_2.0_|_.Net_Framework_4.6.2-blue.svg?logo=dotnet)](https://www.nuget.org/packages/MrrHak.Extensions.FileProviders.S3FileProvider)

This package represents a file provider with an Amazon S3 bucket. It constructs virtual file systems that implement IFileProvider and integrate with AWS S3 SDK to provide the functionality for serving static files in ASP.NET.

![S3 File Provider](https://raw.githubusercontent.com/mrrhak/s3_file_provider/master/s3-file-provider-banner.png)

---

## Where can I get it?

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [MrrHak.Extensions.FileProviders.S3FileProvider](https://www.nuget.org/packages/MrrHak.Extensions.FileProviders.S3FileProvider) from the package manager console:

```
PM> NuGet\Install-Package MrrHak.Extensions.FileProviders.S3FileProvider
```
Or from the .NET CLI as:
```
dotnet add package MrrHak.Extensions.FileProviders.S3FileProvider
```

## How do I get started?

Creating a `S3FileProvider` instance is very simple:

```csharp
var s3FileProvider = new S3FileProvider(amazonS3, bucketName);
```

### Using S3 for serve as static files

First, configure `S3FileProvider` with `UseStaticFiles`, in the program/startup of your application:

```csharp
var s3FileProvider = new S3FileProvider(amazonS3, bucketName);
var staticFilesOption = new StaticFileOptions(){ FileProvider = s3FileProvider};
app.UseStaticFiles(staticFilesOption);
```

Or using `UseS3StaticFiles` extension method (recommended)
```csharp
app.UseS3StaticFiles(bucketName);
```


> **Note**: [AWSSDK.S3](https://www.nuget.org/packages/AWSSDK.S3) is required to create an Amazon S3 client.
> >For Amazon S3 Service instance
> 
> ```csharp
> var amazonS3 = new AmazonS3Client("AccessKeyId", "SecretAccessKey", Amazon.RegionEndpoint.APSoutheast1);
> ```
>
> > Or if you have already registered Amazon S3 services
>
> ```csharp
> var amazonS3 = app.Services.GetService<IAmazonS3>();
> ```
> 


  ## Example

  ### Program.cs
  1. Register `IAmazonS3` client to `services` collection
       - `AWSOptions` is using from `AWSSDK.Extensions.NETCore.Setup`

        ```csharp
            // This value should be get from appsettings.json
            const string S3_BUCKET_NAME = "bucket-name";
            const string S3_ACCESS_KEY = "access-key";
            const string S3_SECRET_KEY = "secret-key";
            const string DEFAULT_REGION = "region";

            // Get AWS profile info directly from configuration (Profile authentication)
            AWSOptions awsOptions = builder.Configuration.GetAWSOptions();
            awsOptions.Region = RegionEndpoint.GetBySystemName(DEFAULT_REGION);
            builder.Services.AddDefaultAWSOptions(awsOptions);

            // IAM user authentication
            AWSOptions s3Options = awsOptions;
            if (!string.IsNullOrEmpty(S3_ACCESS_KEY) && !string.IsNullOrEmpty(S3_SECRET_KEY))
            {
                s3Options = new AWSOptions() { Credentials = new BasicAWSCredentials(S3_ACCESS_KEY, S3_SECRET_KEY) };
                s3Options.Region = RegionEndpoint.GetBySystemName(DEFAULT_REGION);
            }
            builder.Services.AddAWSService<IAmazonS3>(s3Options);
        ```

  2. Register `S3FileProvider` with `UseStaticFiles`
        ```csharp
        var amazonS3 = app.Services.GetService<IAmazonS3>();
        var s3FileProvider = new S3FileProvider(amazonS3, S3_BUCKET_NAME);
        var staticFilesOption = new StaticFileOptions(){ FileProvider = s3FileProvider};
        app.UseStaticFiles(staticFilesOption);
        ```

        Or using `UseS3StaticFiles` extension method (recommended)
        ```csharp
        app.UseS3StaticFiles(S3_BUCKET_NAME);
        ```

## Build and Test Source Code

```bash
dotnet build
dotnet test
```

## Publish NuGet Package

### Upgrade version

To upgrade the version, use the following command to update the version in `S3FileProvider/S3FileProvider.csproj`.

```bash
sed -i '' 's/1.0.0/1.0.1/g' S3FileProvider/S3FileProvider.csproj
```

Start Package file for publishing with version `1.0.0`.

```bash
dotnet pack -c Release -o publish /p:Version=1.0.0
dotnet nuget push publish/MrrHak.Extensions.FileProviders.S3FileProvider.*.nupkg -s https://api.nuget.org/v3/index.json -k $NUGET_API_KEY
```

## Buy me a coffee

 [![Buy Me A Coffee](https://user-images.githubusercontent.com/26390946/161375563-69c634fd-89d2-45ac-addd-931b03996b34.png)](https://www.buymeacoffee.com/mrrhak) [![Ko-fi](https://user-images.githubusercontent.com/26390946/161375565-e7d64410-bbcf-4a28-896b-7514e106478e.png)](https://ko-fi.com/mrrhak)
