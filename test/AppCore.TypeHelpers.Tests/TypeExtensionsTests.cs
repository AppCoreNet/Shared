// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace AppCore
{
    public class GenericType<T1, T2>
    {
    }

    public interface IGenericInterface<T1, T2>
    {
    }

    public class ClosedGenericType : GenericType<string, char>, IGenericInterface<string, char>
    {
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
                                         typeof(ClosedGenericType),
                                         typeof(GenericType<string, char>),
                                         typeof(IGenericInterface<string, char>),
                                         typeof(object));
        }
    }
}
