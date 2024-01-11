// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace AppCoreNet;

public class GenericType<T1, T2>
{
    public class NestedType
    {
    }
}

public interface IGenericInterface<T1, T2>
{
}

public class ClosedGenericType : GenericType<string, char>, IGenericInterface<string, char>
{
}

public class TypeWithNestedType
{
    public class NestedType
    {
    }
}

[ExcludeFromCodeCoverage]
public class TypeExtensionsTests
{
    [Fact]
    public void GetDisplayNameFormatsName()
    {
        typeof(string).GetDisplayName()
                      .Should()
                      .Be("System.String");
    }

    [Fact]
    public void GetDisplayNameNestedTypeFormatsName()
    {
        typeof(TypeWithNestedType.NestedType).GetDisplayName()
                                             .Should()
                                             .Be("AppCore.TypeWithNestedType.NestedType");
    }

    [Fact]
    public void GetDisplayNameGenericNestedTypeFormatsName()
    {
        typeof(GenericType<,>.NestedType).GetDisplayName()
                                         .Should()
                                         .Be("AppCore.GenericType<T1,T2>.NestedType");
    }

    [Fact]
    public void GetDisplayNameFormatsGenericTypeArguments()
    {
        typeof(GenericType<string, char>).GetDisplayName()
                                         .Should()
                                         .Be("AppCore.GenericType<System.String,System.Char>");
    }

    [Fact]
    public void GetDisplayNameFormatsOpenGenericType()
    {
        typeof(GenericType<,>).GetDisplayName()
                              .Should()
                              .Be("AppCore.GenericType<T1,T2>");
    }

    [Fact]
    public void FindClosedTypeOfFindsClosedClass()
    {
        typeof(ClosedGenericType).FindClosedTypeOf(typeof(GenericType<,>))
                                 .Should()
                                 .Be<GenericType<string, char>>();
    }

    [Fact]
    public void FindClosedTypeOfFindsClosedInterface()
    {
        typeof(ClosedGenericType).FindClosedTypeOf(typeof(IGenericInterface<,>))
                                 .Should()
                                 .Be<IGenericInterface<string, char>>();
    }

    [Fact]
    public void GetTypesAssignableFromReturnsInheritedTypesAndImplementedInterfaces()
    {
        typeof(ClosedGenericType).GetTypesAssignableFrom()
                                 .Should()
                                 .BeEquivalentTo(
                                     new[]
                                     {
                                         typeof(ClosedGenericType),
                                         typeof(GenericType<string, char>),
                                         typeof(IGenericInterface<string, char>),
                                         typeof(object),
                                     });
    }

    [Fact]
    public void GetTypesAssignableFromReturnsInheritedTypesAndImplementedInterfacesAndOpenGenerics()
    {
        typeof(ClosedGenericType).GetTypesAssignableFrom(true)
                                 .Should()
                                 .BeEquivalentTo(
                                     new[]
                                     {
                                         typeof(ClosedGenericType),
                                         typeof(GenericType<string, char>),
                                         typeof(GenericType<,>),
                                         typeof(IGenericInterface<string, char>),
                                         typeof(IGenericInterface<,>),
                                         typeof(object),
                                     });
    }
}