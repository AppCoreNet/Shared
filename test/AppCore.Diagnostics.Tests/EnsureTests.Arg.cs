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

            [Fact]
            public void NotNullThrowsWhenNull()
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => Ensure.Arg.NotNull((string)null!, "param"));

                exception.ParamName.Should()
                         .Be("param");
            }

            [Fact]
            public void NotNullDoesNotThrowWhenNotNull()
            {
               Ensure.Arg.NotNull("test string", "param");
            }

            [Fact]
            public void NotEmptyThrowsWhenNull()
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => Ensure.Arg.NotEmpty(null, "param"));

                exception.ParamName.Should()
                         .Be("param");
            }

            [Fact]
            public void NotEmptyThrowsWhenDefault()
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => Ensure.Arg.NotEmpty(Guid.Empty, "param"));

                exception.ParamName.Should()
                         .Be("param");
            }

            [Fact]
            public void NotEmptyDoesNotThrowWhenNotDefault()
            {
                Ensure.Arg.NotEmpty(Guid.NewGuid(), "param");
            }

            [InlineData("")]
            [InlineData("   ")]
            [Theory]
            public void NotEmptyThrowsWhenEmptyOrWhitespace(string value)
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => Ensure.Arg.NotEmptyButNull(value, "param"));

                exception.ParamName.Should()
                         .Be("param");
            }

            [InlineData("")]
            [InlineData("   ")]
            [Theory]
            public void NotEmptyButNullThrowsWhenEmptyOrWhitespace(string value)
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => Ensure.Arg.NotEmpty(value, "param"));

                exception.ParamName.Should()
                         .Be("param");
            }


            [Fact]
            public void NotEmptyThrowsForNullCollection()
            {
                var exception = Assert.Throws<ArgumentNullException>(
                    () => Ensure.Arg.NotEmpty((IReadOnlyCollection<string>)null!, "param"));

                exception.ParamName.Should()
                         .Be("param");
            }

            [Fact]
            public void NotEmptyThrowsForEmptyCollection()
            {
                var exception = Assert.Throws<ArgumentException>(
                    () => Ensure.Arg.NotEmpty(new List<string>(), "param"));

                exception.ParamName.Should()
                         .Be("param");
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(11)]
            public void InRangeThrowsWhenValueExceedsRange(int value)
            {
                var exception = Assert.Throws<ArgumentOutOfRangeException>(
                    () => Ensure.Arg.InRange(value, 0, 10, "param"));

                exception.ParamName.Should()
                         .Be("param");

                exception.ActualValue.Should()
                         .Be(value);
            }

            [Fact]
            public void MaxLengthThrowsForLongerString()
            {
                var exception = Assert.Throws<ArgumentOutOfRangeException>(
                    () => Ensure.Arg.MaxLength(" ", 0, "param"));

                exception.ParamName.Should()
                         .Be("param");

                exception.ActualValue.Should()
                         .Be(1);
            }

            [Fact]
            public void MinLengthThrowsForShorterString()
            {
                var exception = Assert.Throws<ArgumentOutOfRangeException>(
                    () => Ensure.Arg.MinLength("", 1, "param"));

                exception.ParamName.Should()
                         .Be("param");

                exception.ActualValue.Should()
                         .Be(0);
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
