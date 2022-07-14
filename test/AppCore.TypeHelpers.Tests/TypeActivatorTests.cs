using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace AppCore
{
    [ExcludeFromCodeCoverage]
    public class TypeActivatorTests
    {
        class TestType
        {
            public int Arg1;
            public string? Arg2;
            public char Arg3;
            public object? Arg4;

            public TestType()
            {
            }

            public TestType(int arg1)
            {
                Arg1 = arg1;
            }

            public TestType(int arg1, string arg2)
            {
                Arg1 = arg1;
                Arg2 = arg2;
            }

            public TestType(int arg1, string arg2, char arg3)
            {
                Arg1 = arg1;
                Arg2 = arg2;
                Arg3 = arg3;
            }

            public TestType(int arg1, string arg2, char arg3, TestType arg4)
            {
                Arg1 = arg1;
                Arg2 = arg2;
                Arg3 = arg3;
                Arg4 = arg4;
            }
        }

        [Fact]
        public void GenericWithNoArgsSucceeds()
        {
            var instance = TypeActivator.CreateInstance<TestType>();
            instance.Should()
                    .NotBeNull();
        }

        [Fact]
        public void ReflectedWithNoArgsSucceeds()
        {
            object instance = typeof(TestType).CreateInstance();
            instance.Should()
                    .NotBeNull();
        }

        [Fact]
        public void GenericWithOneArgSucceeds()
        {
            int arg1 = 1;
            TestType instance = TypeActivator.CreateInstance<TestType, int>(arg1);
            instance.Arg1.Should().Be(arg1);
        }

        [Fact]
        public void ReflectedWithOneArgSucceeds()
        {
            int arg1 = 1;
            var instance = (TestType) typeof(TestType).CreateInstance(arg1);
            instance.Arg1.Should().Be(arg1);
        }

        [Fact]
        public void GenericWithTwoArgsSucceeds()
        {
            int arg1 = 1;
            string arg2 = "abc";
            TestType instance = TypeActivator.CreateInstance<TestType, int, string>(arg1, arg2);
            instance.Arg1.Should()
                    .Be(arg1);
            instance.Arg2.Should()
                    .Be(arg2);
        }

        [Fact]
        public void ReflectedWithTwoArgsSucceeds()
        {
            int arg1 = 1;
            string arg2 = "abc";
            var instance = (TestType) typeof(TestType).CreateInstance(arg1, arg2);
            instance.Arg1.Should()
                    .Be(arg1);
            instance.Arg2.Should()
                    .Be(arg2);
        }

        [Fact]
        public void GenericWithThreeArgsSucceeds()
        {
            int arg1 = 1;
            string arg2 = "abc";
            char arg3 = 'x';
            TestType instance = TypeActivator.CreateInstance<TestType, int, string, char>(arg1, arg2, arg3);
            instance.Arg1.Should()
                    .Be(arg1);
            instance.Arg2.Should()
                    .Be(arg2);
            instance.Arg3.Should()
                    .Be(arg3);
        }

        [Fact]
        public void ReflectedWithThreeArgsSucceeds()
        {
            int arg1 = 1;
            string arg2 = "abc";
            char arg3 = 'x';
            var instance = (TestType) typeof(TestType).CreateInstance(arg1, arg2, arg3);
            instance.Arg1.Should()
                    .Be(arg1);
            instance.Arg2.Should()
                    .Be(arg2);
            instance.Arg3.Should()
                    .Be(arg3);
        }

        [Fact]
        public void GenericWithFourArgsSucceeds()
        {
            int arg1 = 1;
            string arg2 = "abc";
            char arg3 = 'x';
            var arg4 = new TestType();
            TestType instance = TypeActivator.CreateInstance<TestType, int, string, char, TestType>(arg1, arg2, arg3, arg4);
            instance.Arg1.Should()
                    .Be(arg1);
            instance.Arg2.Should()
                    .Be(arg2);
            instance.Arg3.Should()
                    .Be(arg3);
            instance.Arg4.Should()
                    .BeSameAs(arg4);
        }

        [Fact]
        public void ReflectedWithFourArgsSucceeds()
        {
            int arg1 = 1;
            string arg2 = "abc";
            char arg3 = 'x';
            var arg4 = new TestType();
            var instance = (TestType) typeof(TestType).CreateInstance(arg1, arg2, arg3, arg4);
            instance.Arg1.Should()
                    .Be(arg1);
            instance.Arg2.Should()
                    .Be(arg2);
            instance.Arg3.Should()
                    .Be(arg3);
            instance.Arg4.Should()
                    .BeSameAs(arg4);
        }
    }
}
