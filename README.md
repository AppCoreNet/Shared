AppCore .NET Shared
-------------------

[![Build Status](https://dev.azure.com/AppCoreNet/Shared/_apis/build/status/AppCoreNet.Shared?branchName=dev)](https://dev.azure.com/AppCoreNet/Shared/_build/latest?definitionId=1&branchName=dev)

This repository includes projects, targeting .NET Framework and .NET Core, containing utility types commonly used
across other AppCore .NET projects.

All artifacts are licensed under the [MIT license](LICENSE). You are free to use them in open-source or commercial
projects as long as you keep the copyright notice intact when redistributing or otherwise reusing our artifacts.

## Packages

Latest development packages can be found on [MyGet](https://www.myget.org/gallery/appcorenet).

The following packages are published as source NuGet packages, your application or library will not
have a runtime dependency on them.

Package                           | Description
----------------------------------|---------------------------------------------------------------
`AppCore.Diagnostics.Sources` | Provides static classes to ensure program contracts.
`AppCore.TypeHelpers.Sources` | Includes extensions for `Type` and `Assembly` and a fast activator for types.

### Diagnostics

This package includes static classes to ensure program contracts such as pre-conditions, post-conditions and
invariants.

Note that the package also includes annotations for ReSharper. If you want to bring your own you can disable
them by defining the compilation symbol `APPCORE_DISABLE_RESHARPER_ANNOTATIONS`.

### TypeHelpers

Includes various extensions for the `Type` and `Assembly` classes to make your live easier when working
with open generic types. Also provides a fast activator (factory) for dynamically instantiating types.

## Contributing

Contributions, whether you file an issue, fix some bug or implement a new feature, are highly appreciated. The whole user community
will benefit from them.

Please refer to the [Contribution guide](CONTRIBUTING.md).
