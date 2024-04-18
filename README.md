🍃 S3 File Provider 🍃
======================

[![NuGet](http://img.shields.io/nuget/vpre/MrrHak.Extensions.FileProviders.S3FileProvider.svg?label=NuGet&logo=nuget)](https://www.nuget.org/packages/AutoMapper/)
![NuGet Downloads](https://img.shields.io/nuget/dt/MrrHak.Extensions.FileProviders.S3FileProvider?style=flat&logo=docusign&label=Downloads&link=https%3A%2F%2Fwww.nuget.org%2Fstats%2Fpackages%2FMrrHak.Extensions.FileProviders.S3FileProvider)
[![Code size](https://img.shields.io/github/languages/code-size/mrrhak/s3_file_provider?logo=csharp&color=blue&label=Size)](https://github.com/mrrhak/s3_file_provider)
[![Lines of code](https://tokei.rs/b1/github/mrrhak/s3_file_provider?category=code&label=Total%20Lines&style=flat)](https://github.com/mrrhak/s3_file_provider)
[![Star on Github](https://img.shields.io/github/stars/mrrhak/s3_file_provider.svg?style=flat&logo=github&colorB=deeppink&label=Stars)](https://github.com/mrrhak/s3_file_provider)
[![Forks on Github](https://img.shields.io/github/forks/mrrhak/s3_file_provider?style=flat&label=Forks&logo=github)](https://github.com/mrrhak/s3_file_provider)
[![Build Status](https://github.com/mrrhak/s3_file_provider/actions/workflows/dotnet.yml/badge.svg) ](https://github.com/mrrhak/s3_file_provider/actions?query=workflow%3A)
[![License: MIT](https://img.shields.io/github/license/mrrhak/s3_file_provider?label=License&color=red&logo=Leanpub)](https://opensource.org/licenses/MIT)

This package constructs virtual file systems that implement IFileProvider and AWS S3 SDK to provide the functionality for serving static files from Amazon S3.

![S3 File Provider](https://raw.githubusercontent.com/mrrhak/s3_file_provider/master/s3_file_provider.png)

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

> **Note**
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

## Publish NuGet Package

### Upgrade version

To upgrade the version, use the following command to update the version in `S3FileProvider/S3FileProvider.csproj`.

```bash
sed -i -e 's/{{VERSION}}/1.0.0/g' S3FileProvider/S3FileProvider.csproj
```

Start Package file for publishing with version `1.0.0`.

```bash
dotnet pack -c Release -o publish /p:Version=1.0.0
dotnet nuget push publish/MrrHak.Extensions.FileProviders.S3FileProvider.*.nupkg -s https://api.nuget.org/v3/index.json -k $NUGET_API_KEY
```

---

[![](https://img.shields.io/badge/Develop_and_Maintain_by-Mrr_Hak-blue.svg?logo=)](https://mrrhak.com)
