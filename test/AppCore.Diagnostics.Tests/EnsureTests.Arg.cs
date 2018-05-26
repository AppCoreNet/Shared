// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace AppCore.Diagnostics
{
    public partial class EnsureTests
    {
        public class Arg
        {
            class ExpectedArgBaseType : IEnumerable<string>
            {
                public IEnumerator<string> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }

            class ExpectedArgType : ExpectedArgBaseType
            {
            }

            [Theory]
            [InlineData(typeof(IEnumerable))]
            [InlineData(typeof(IEnumerable<string>))]
            [InlineData(typeof(IEnumerable<>))]
            [InlineData(typeof(ExpectedArgBaseType))]
            [InlineData(typeof(ExpectedArgType))]
            public void OfTypeDoesNotThrowForRelatedTypes(Type expectedType)
            {
                Type type = typeof(ExpectedArgType);
                Ensure.Arg.OfType(type, expectedType, "param");
            }

            [Fact]
            public void OfTypeThrowsForUnrelatedType()
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => Ensure.Arg.OfType(typeof(string), typeof(ExpectedArgType), "param"));

                exception.ParamName.Should()
                         .Be("param");
            }
        }
    }
}
