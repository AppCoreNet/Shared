// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

#pragma warning disable 8600
#pragma warning disable 8625

namespace AppCoreNet.Diagnostics;

public partial class EnsureTests
{
    public class Arg
    {
        private class ExpectedArgBaseType : IEnumerable<string>
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

        private class ExpectedArgType : ExpectedArgBaseType
        {
        }

        [Fact]
        public void NotNullThrowsWhenNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => Ensure.Arg.NotNull((string)null, "param"));

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
                () => Ensure.Arg.NotEmpty((IReadOnlyCollection<string>)null, "param"));

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
                () => Ensure.Arg.MinLength(string.Empty, 1, "param"));

            exception.ParamName.Should()
                     .Be("param");

            exception.ActualValue.Should()
                     .Be(0);
        }

        [Theory]
        [InlineData(typeof(IEnumerable))]
        [InlineData(typeof(IEnumerable<string>))]
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

        [Theory]
        [InlineData(typeof(IEnumerable))]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(ExpectedArgBaseType))]
        [InlineData(typeof(ExpectedArgType))]
        public void OfTypeDoesNotThrowForRelatedValue(Type expectedType)
        {
            object value = new ExpectedArgType();
            Ensure.Arg.OfType(value, expectedType, "param");
        }

        [Fact]
        public void OfTypeThrowsForUnrelatedValue()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => Ensure.Arg.OfType(string.Empty, typeof(ExpectedArgType), "param"));

            exception.ParamName.Should()
                     .Be("param");
        }

        [Fact]
        public void OfTypeDoesNotThrowForNullType()
        {
            Ensure.Arg.OfType(null, typeof(string));
        }

        [Fact]
        public void OfTypeDoesNotThrowForNullValue()
        {
            Ensure.Arg.OfType((object?)null, typeof(string));
        }

        [Theory]
        [InlineData(typeof(IEnumerable))]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(IEnumerable<>))]
        [InlineData(typeof(ExpectedArgBaseType))]
        [InlineData(typeof(ExpectedArgType))]
        public void OfGenericTypeDoesNotThrowForRelatedTypes(Type expectedType)
        {
            Type type = typeof(ExpectedArgType);
            Ensure.Arg.OfGenericType(type, expectedType, "param");
        }

        [Theory]
        [InlineData(typeof(IEnumerable))]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(IEnumerable<>))]
        [InlineData(typeof(ExpectedArgBaseType))]
        [InlineData(typeof(ExpectedArgType))]
        public void OfGenericTypeDoesNotThrowForRelatedValue(Type expectedType)
        {
            object value = new ExpectedArgType();
            Ensure.Arg.OfGenericType(value, expectedType, "param");
        }

        [Fact]
        public void OfGenericTypeDoesNotThrowForNullType()
        {
            Ensure.Arg.OfGenericType(null, typeof(string));
        }

        [Fact]
        public void OfGenericTypeDoesNotThrowForNullValue()
        {
            Ensure.Arg.OfGenericType((object?)null, typeof(string));
        }

        [Fact]
        public void OfGenericTypeThrowsForUnrelatedType()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => Ensure.Arg.OfGenericType(typeof(string), typeof(ExpectedArgType), "param"));

            exception.ParamName.Should()
                     .Be("param");
        }

        [Fact]
        public void OfGenericTypeThrowsForUnrelatedValue()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => Ensure.Arg.OfGenericType(string.Empty, typeof(ExpectedArgType), "param"));

            exception.ParamName.Should()
                     .Be("param");
        }
    }
}