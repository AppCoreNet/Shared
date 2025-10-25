# AppCoreNet.Attributes.Sources

This package includes sources for attributes which are defined in newer .NET versions (>= .NET Core 3.0) such as
attributes required for nullable reference types, attributes required for the `required` keyword and others.

## Getting started

Just install the NuGet package and the sources will be included in your project.
If you are using the `Polyfill` or `Nullable` package these attributes are automatically disabled.

## Usage

You can control inclusion of the attributes with the following MSBuild properties:

- `AppCoreNetAttributesEnabled`
  Enables/disables all attributes.
- `AppCoreNetNullableAttributesEnabled`
  Enables/disables the nullable attributes.
- `AppCoreNetTrimmingAttributesEnabled`
  Enables/disables the trimming attributes.

Example:

```msbuild
<Project>
  <PropertyGroup>
    <AppCoreNetAttributesEnabled>false</AppCoreNetAttributesEnabled>
  </PropertyGroup>
</Project>
```

This will disable all attributes.
