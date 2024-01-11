// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace AppCoreNet.Reflection;

public class AssemblyExtensionsTests
{
    [Fact]
    public void GetTypesAssignableFrom()
    {
        Assembly assembly = typeof(AssemblyExtensionsTests).Assembly;

        IEnumerable<Type> types = assembly.GetTypesAssignableFrom(typeof(IGenericInterface<,>));
        types.Should()
             .BeEquivalentTo(
                 new[]
                 {
                     typeof(IGenericInterface<,>),
                     typeof(ClosedGenericType),
                 });
    }

    [Fact]
    public void GetTypesAssignableFrom2()
    {
        Assembly assembly = typeof(AssemblyExtensionsTests).Assembly;

        IEnumerable<Type> types = assembly.GetTypesAssignableFrom(typeof(IGenericInterface<string, char>));
        types.Should()
             .BeEquivalentTo(
                 new[]
                 {
                     typeof(ClosedGenericType),
                 });
    }
}