# UtilitiesNet

[![NuGet version][version-badge]][nuget]
[![NuGet downloads][downloads-badge]][nuget]
[![Build and tests][build-badge]][workflow]
[![License: MIT][license-badge]][license]
[![GitHub issues][issues-badge]][issues]

This is a C# base library project with reusable utilities for text, Unicode, email addresses, files, byte arrays, XML, and encryption. 

Available as the
[`DigitalZenWorks.Common.Utilities` NuGet package][nuget].

## Installation

Add the package to your application:

```shell
dotnet add package DigitalZenWorks.Common.Utilities
```

Or use Visual Studio's Package Manager Console:

```powershell
Install-Package DigitalZenWorks.Common.Utilities
```

## Quick start

String extensions use the `DigitalZenWorks.Common.Utilities.Extensions`
namespace:

```csharp
using DigitalZenWorks.Common.Utilities.Extensions;

string? propertyName = "email_address".ToCamelCase();
// emailAddress

string digits = "+1 (555) 010-0200".ToDigitsOnly();
// 15550100200
```

Other utilities use the `DigitalZenWorks.Common.Utilities` namespace:

```csharp
using DigitalZenWorks.Common.Utilities;

bool valid = EmailUtilities.IsValidEmailAddress("user@example.com");
// true
```

## Available utilities

| Area | Main classes |
| --- | --- |
| Text casing and string operations | `TextCase`, `StringExtender` |
| Unicode and language checks | `UnicodeNormalizer`, `LanguagesSupport` |
| Email validation and normalization | `EmailUtilities` |
| Files, streams, and directories | `FileUtils` |
| Bits, byte arrays, and conversions | `BitBytes`, `GeneralUtilities` |
| XML loading and schema validation | `XmlUtilities` |
| Encryption helpers | `AESGCM`, `PasswordEncryption` |

See the XML documentation in the [library source][source] for individual
method parameters, return values, and limitations.

## Target frameworks

The current Release configuration targets .NET Standard 2.0 and .NET 5, 6,
7, 8, 9, and 10. Debug builds target .NET 10 only. See the [package page][nuget]
for the frameworks included in a published version and its dependencies.

## Build and test

Install the .NET 10 SDK to build this checkout. Clone the repository:

```shell
git clone https://github.com/jamesjohnmcguire/UtilitiesNet.git
cd UtilitiesNet
```

From the repository root, build and run the NUnit tests:

```shell
dotnet build UtilitiesNet.sln --configuration Debug
dotnet test UtilitiesNet.sln --configuration Debug
```

Release builds compile all configured library targets and generate a NuGet
package under `UtilitiesNetLibrary/bin/Release/`:

```shell
dotnet build UtilitiesNet.sln --configuration Release
```

## Contributing

Report bugs and propose improvements through [GitHub issues][issues].

 For a pull request:

1. Fork the Project
2. Create your focused branch
3. Add your code changes
3. Add or update tests to validate your changes
3. Commit and Push your Changes
4. Open a Pull Request. Include the description of the change, its validation, and any related issue in the PR.

### Coding style
Please match the current coding style.  Most notably:
1. One operation per line
2. Use complete, self-documenting English words in variable and method names
3. Attempt to declare variable and method names in a self-documenting manner
4. Use tabs and Allman braces

Keep public API compatibility in mind: this library is consumed as NuGet package.

## License

Distributed under the [MIT License][license].

## Contact

James John McGuire — jamesjohnmcguire@gmail.com

[Project repository][repository]. If the library is useful to you, consider
giving it a star.

[version-badge]:
  https://img.shields.io/nuget/v/DigitalZenWorks.Common.Utilities.svg
[downloads-badge]:
  https://img.shields.io/nuget/dt/DigitalZenWorks.Common.Utilities.svg
[license-badge]:
  https://img.shields.io/badge/license-MIT-blue.svg
[build-badge]:
  https://img.shields.io/github/actions/workflow/status/jamesjohnmcguire/UtilitiesNet/dotnet.yml?branch=main
[workflow]:
  https://github.com/jamesjohnmcguire/UtilitiesNet/actions/workflows/dotnet.yml
[issues-badge]:
  https://img.shields.io/github/issues/jamesjohnmcguire/UtilitiesNet
[nuget]: https://www.nuget.org/packages/DigitalZenWorks.Common.Utilities
[license]: https://github.com/jamesjohnmcguire/UtilitiesNet/blob/main/LICENSE
[issues]: https://github.com/jamesjohnmcguire/UtilitiesNet/issues
[source]:
  https://github.com/jamesjohnmcguire/UtilitiesNet/tree/main/UtilitiesNetLibrary
[repository]: https://github.com/jamesjohnmcguire/UtilitiesNet
