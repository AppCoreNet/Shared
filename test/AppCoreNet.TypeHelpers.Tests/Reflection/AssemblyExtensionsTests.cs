// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using Xunit;
using static AppCoreNet.TestContracts;

namespace AppCoreNet.Reflection;

public class AssemblyExtensionsTests
{
    [Fact]
    [RequiresUnreferencedCode("Types might be removed")]
    public void GetTypesAssignableFrom()
    {
        Assembly assembly = typeof(AssemblyExtensionsTests).Assembly;

        IEnumerable<Type> types = assembly.GetTypesAssignableFrom(typeof(IGenericInterface<,>));
        types.Should()
             .BeEquivalentTo(
                 new[]
                 {
                     typeof(IGenericInterface<,>),
                     typeof(GenericType<,>),
                     typeof(ClosedGenericType),
                 });
    }

    [Fact]
    [RequiresUnreferencedCode("Types might be removed")]
    public void GetTypesAssignableFrom2()
    {
        Assembly assembly = typeof(AssemblyExtensionsTests).Assembly;

        IEnumerable<Type> types = assembly.GetTypesAssignableFrom(typeof(IGenericInterface<string, char>));
        types.Should()
             .BeEquivalentTo(
                 new[]
                 {
                     typeof(GenericType<,>),
                     typeof(ClosedGenericType),
                 });
    }
}