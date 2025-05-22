// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;
using static AppCoreNet.TestContracts;

namespace AppCoreNet;

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
    public void GetDisplayNameNestedSimpleTypeFormatsName()
    {
        typeof(System.Environment.SpecialFolder).GetDisplayName()
                                                .Should()
                                                .Be("System.Environment.SpecialFolder");
    }

    [Fact]
    public void GetDisplayNameNestedSimpleGenericTypeFormatsName()
    {
        typeof(System.Collections.Generic.List<string>).GetDisplayName()
                                                       .Should()
                                                       .Be("System.Collections.Generic.List<System.String>");
    }

    [Fact]
    public void GetDisplayNameNestedTypeFormatsName()
    {
        typeof(TypeWithNestedType.NestedType).GetDisplayName()
                                             .Should()
                                             .Be("AppCoreNet.TestContracts.TypeWithNestedType.NestedType");
    }

    [Fact]
    public void GetDisplayNameGenericNestedTypeFormatsName()
    {
        typeof(GenericType<,>.NestedType).GetDisplayName()
                                         .Should()
                                         .Be("AppCoreNet.TestContracts.GenericType<T1,T2>.NestedType");
    }

    [Fact]
    public void GetDisplayNameFormatsGenericTypeArguments()
    {
        typeof(GenericType<string, char>).GetDisplayName()
                                         .Should()
                                         .Be("AppCoreNet.TestContracts.GenericType<System.String,System.Char>");
    }

    [Fact]
    public void GetDisplayNameFormatsOpenGenericType()
    {
        typeof(GenericType<,>).GetDisplayName()
                              .Should()
                              .Be("AppCoreNet.TestContracts.GenericType<T1,T2>");
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