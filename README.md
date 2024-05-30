![S3 File Provider](https://raw.githubusercontent.com/mrrhak/s3_file_provider/master/icon-small.png) S3 File Provider
======================

[![NuGet](http://img.shields.io/nuget/vpre/MrrHak.Extensions.FileProviders.S3FileProvider.svg?label=NuGet&logo=nuget)](https://www.nuget.org/packages/MrrHak.Extensions.FileProviders.S3FileProvider)
[![NuGet Downloads](https://img.shields.io/nuget/dt/MrrHak.Extensions.FileProviders.S3FileProvider?style=flat&logo=nuget&label=NuGet%20Downloads&link=https%3A%2F%2Fwww.nuget.org%2Fstats%2Fpackages%2FMrrHak.Extensions.FileProviders.S3FileProvider)](https://www.nuget.org/stats/packages/MrrHak.Extensions.FileProviders.S3FileProvider?groupby=Version)
![MyGet Version](https://img.shields.io/myget/mrrhak/v/MrrHak.Extensions.FileProviders.S3FileProvider?style=flat&logo=myget&label=MyGet&link=https%3A%2F%2Fwww.myget.org%2Ffeed%2Fmrrhak%2Fpackage%2Fnuget%2FMrrHak.Extensions.FileProviders.S3FileProvider)
![MyGet Downloads](https://img.shields.io/myget/mrrhak/dt/MrrHak.Extensions.FileProviders.S3FileProvider?style=flat&logo=myget&label=MyGet%20Downloads&color=bule&link=https%3A%2F%2Fwww.myget.org%2Ffeed%2Fmrrhak%2Fpackage%2Fnuget%2FMrrHak.Extensions.FileProviders.S3FileProvider)
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

### .Net Core Configuration

Creating a `S3FileProvider` instance is very simple:

```csharp
var s3FileProvider = new S3FileProvider(amazonS3, bucketName);
```

Or using `GetS3FileProvider` extension method
```csharp
var s3FileProvider = app.Services.GetS3FileProvider(bucketName);
```


### Using S3 for serve as static files

First, configure `S3FileProvider` with `UseStaticFiles`, in the program/startup of your application:

```csharp
var s3FileProvider = new S3FileProvider(amazonS3, bucketName);
var staticFilesOption = new StaticFileOptions(){ FileProvider = s3FileProvider};
app.UseStaticFiles(staticFilesOption);
```
Or using `UseS3StaticFiles` extension method (recommended)

| Parameter               |   Type   | Required | Default Value | Description |
| :---------------------- | :------: | :------: | :-----------: | :---------- |
| `bucketName`            | `string` |    Yes   |               | The name of the S3 bucket |
| `requestPath`           | `string` |    No    |     `null`    | The request path for the static files |
| `rootPath`           | `string` |    No    |     `null`    | The root path for the S3 bucket |
| `serveUnknownFileTypes` |  `bool`  |    No    |     `false`   | Whether to serve unknown file types |

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

### S3StaticFileOptions
| Option                  |  Type    | Required | Default Value | Description |
| :---------------------- | :------- | :------: | :-----------: | :---------- |
| `BucketName`            | `string` |    Yes   |    `empty`    | The name of the S3 bucket |
| `RequestPath`           | `string` |    No    |    `null`     | The relative request path that maps to static resources |
| `RootPath`              | `string` |    No    |    `null`     | The root path for the S3 bucket |
| `ServeUnknownFileTypes` |  `bool`  |    No    |    `false`    | Whether to serve unknown file types |
| `DefaultContentType`    | `string` |    No    |    `null`     | The default content type for a request if the ContentTypeProvider cannot determine one. None is provided by default, so the client must determine the format themselves |
| `ContentTypeProvider`   | `IContentTypeProvider` | No | `null` | Used to map files to content-types |
| `OnPrepareResponse`     | `Action<PrepareResponseContext>` | No | `null` | Called after the status code and headers have been set, but before the body has been written. This can be used to add or change the response headers |


### Example

#### Program.cs
1. Register `IAmazonS3` client to `services` collection
    - `AWSOptions` is using from [`AWSSDK.Extensions.NETCore.Setup`](https://www.nuget.org/packages/AWSSDK.Extensions.NETCore.Setup)

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
---

### .Net Framework Configuration
#### 1. Install Required Packages for .Net Framework
```bash
dotnet add package Microsoft.Owin.Host.SystemWeb
```

#### 2. Create Owin Startup class
```csharp
public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        var s3Client = new AmazonS3Client("your-access-key", "your-secret-key", Amazon.RegionEndpoint.APSoutheast1);
        app.UseS3StaticFiles(s3Client, "your-bucket-name");

        // Another implementation
    }
}
```

#### 3. Update Web.config
```xml
<system.webServer>
    <handlers>
        <add name="Owin" verb="" path="*" type="Microsoft.Owin.Host.SystemWeb.OwinHttpHandler, Microsoft.Owin.Host.SystemWeb"/>
    </handlers>
</system.webServer>
```

---

## Build and Test Source Code

```bash
dotnet build
dotnet test
```

## Buy me a coffee

 [![Buy Me A Coffee](https://user-images.githubusercontent.com/26390946/161375563-69c634fd-89d2-45ac-addd-931b03996b34.png)](https://www.buymeacoffee.com/mrrhak) [![Ko-fi](https://user-images.githubusercontent.com/26390946/161375565-e7d64410-bbcf-4a28-896b-7514e106478e.png)](https://ko-fi.com/mrrhak)
