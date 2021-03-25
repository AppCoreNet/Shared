// Licensed under the MIT License.
// Copyright (c) 2020 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCore.Reflection;
using FluentAssertions;
using Xunit;

namespace AppCore
{
    public class AssemblyExtensionsTests
    {
        [Fact]
        public void GetTypesAssignableFrom()
        {
            Assembly assembly = typeof(AssemblyExtensionsTests).Assembly;

            IEnumerable<Type> types = assembly.GetTypesAssignableFrom(typeof(IGenericInterface<,>));
            types.Should()
                 .BeEquivalentTo(
                     typeof(IGenericInterface<,>),
                     typeof(ClosedGenericType));
        }

        [Fact]
        public void GetTypesAssignableFrom2()
        {
            Assembly assembly = typeof(AssemblyExtensionsTests).Assembly;

            IEnumerable<Type> types = assembly.GetTypesAssignableFrom(typeof(IGenericInterface<string,char>));
            types.Should()
                 .BeEquivalentTo(
                     typeof(ClosedGenericType));
        }
    }
}