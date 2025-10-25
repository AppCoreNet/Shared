# AppCoreNet.TypeHelpers.Sources

This package includes various extensions for the `Type` and `Assembly` classes to make your live easier when working
with open generic types. Also provides a fast activator (factory) for dynamically instantiating types.
Note that the package is compatible with AOT and trimming.

## Getting started

Just install the NuGet package and the sources will be included in your project.

## Usage

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
