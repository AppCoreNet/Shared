AppCore.NET Shared
------------------

![Nuget](https://img.shields.io/nuget/v/AppCoreNet.Attributes.Sources?label=NuGet) ![MyGet](https://img.shields.io/myget/appcorenet/vpre/AppCoreNet.Attributes.Sources?label=MyGet)

This repository includes projects, targeting .NET Framework and .NET Standard, containing utility types commonly
used across other AppCore .NET projects.

All artifacts are licensed under the [MIT license](LICENSE). You are free to use them in open-source or commercial
projects as long as you keep the copyright notice intact when redistributing or otherwise reusing our artifacts.

## Packages

Latest development packages can be found on [MyGet](https://www.myget.org/gallery/appcorenet).

The following packages are published as source NuGet packages, your application or library will not
have a runtime dependency on them.

| Package                          | Description                                                                   |
|----------------------------------|-------------------------------------------------------------------------------|
| `AppCoreNet.Attributes.Sources`  | Provides poly fills for .NET attributes.                                      |
| `AppCoreNet.Diagnostics.Sources` | Provides static classes to ensure program contracts.                          |
| `AppCoreNet.TypeHelpers.Sources` | Includes extensions for `Type` and `Assembly` and a fast activator for types. |

## Contributing

Contributions, whether you file an issue, fix some bug or implement a new feature, are highly appreciated. The whole user community
will benefit from them.

Please refer to the [Contribution guide](CONTRIBUTING.md).

# Usage

## AppCoreNet.Attributes.Sources

This packages includes sources for attributes which are defined in newer .NET versions (>= .NET Core 3.0) such as
attributes required for nullable reference types, attributes required for the `required` keyword and others.

If you are using the `Polyfill` or `Nullable` package these attributes are automatically disabled. You can control the
inclusion of these attributes with the following MSBuild properties:

- `AppCoreNetAttributesEnabled`
  Enables/disables all attributes.
- `AppCoreNetNullableAttributesEnabled`
  Enables/disables the nullable attributes.
- `AppCoreNetTrimmingAttributesEnabled`
  Enables/disables the nullable attributes.


For example:

```msbuild
<Project>
  <PropertyGroup>
    <AppCoreNetAttributesEnabled>false</AppCoreNetAttributesEnabled>
  </PropertyGroup>
</Project>
```

Will disable all attributes.

## AppCoreNet.Diagnostics.Sources

This package includes sources of static classes to ensure program contracts such as pre-conditions, post-conditions and
invariants.

### Pre-conditions

Ensuring that some argument is not null:
```csharp
public void SomeMethod(object obj)
{
    // this will throw 'ArgumentNullException' if 'obj' is null
    Ensure.Arg.NotNull(obj);
}
```

Ensuring that some string argument is not null or empty/only whitespace:
```csharp
public void SomeMethod(string str)
{
    // this will throw 'ArgumentNullException' or 'ArgumentException' if 'str' is null or empty
    Ensure.Arg.NotEmpty(str);
}
```

Ensuring that some value argument is in range:
```csharp
public void SomeMethod(int val)
{
    // this will throw or 'ArgumentOutOfRangeException' if 'val' is < 0 || > 10
    Ensure.Arg.InRange(val, 0, 10);
}
```

Other pre-condition checks are available: `Ensure.Arg.MinLength()`, `Ensure.Arg.MaxLength()`

Note that the package also supports annotations for ReSharper. Simply add the package `ReSharper.Annotations` to your
project.

## AppCoreNet.TypeHelpers.Sources

Includes various extensions for the `Type` and `Assembly` classes to make your live easier when working
with open generic types. Also provides a fast activator (factory) for dynamically instantiating types.

Getting friendly type name for display, supports generics:
```csharp
class MyType<T> { }
typeof(MyType<>).GetDisplayName() // returns 'MyNamespace.MyType<T>'
```

Creating an instance of a type:

```csharp
// creates an instance of 'MyType' with default constructor
TypeActivator.CreateInstance<MyType>()

// alternatively (if the Type is not known at compile time):
TypeActivator.CreateInstance(typeof(MyType))
```

Checking if some type implements generic type:

```csharp
class StringList : List<string> { }
typeof(StringList).IsClosedTypeOf(typeof(List<>))
```
