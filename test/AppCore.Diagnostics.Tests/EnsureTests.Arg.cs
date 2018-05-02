// Copyright 2018 the AppCore project.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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
