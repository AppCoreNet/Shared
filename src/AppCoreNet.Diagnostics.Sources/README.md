# AppCoreNet.Diagnostics.Sources

This package includes sources of static classes to ensure program contracts such as pre-conditions, post-conditions and
invariants.

## Getting started

Just install the NuGet package and the sources will be included in your project. Note that the package also supports
annotations for ReSharper. Simply add the package `ReSharper.Annotations` or `ReSharper.Annotations.Sources` to your
project. Note that the package is compatible with AOT and trimming.

## Usage

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
    // this will throw 'ArgumentOutOfRangeException' if 'val' is < 0 || > 10
    Ensure.Arg.InRange(val, 0, 10);
}
```

Other pre-condition checks are available: `Ensure.Arg.NotNullButEmpty()`, `Ensure.Arg.MinLength()`,
`Ensure.Arg.MaxLength()`, `Ensure.Arg.OfType()`.
