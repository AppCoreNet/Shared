// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet;

public static class TestContracts
{
    public interface IGenericInterface<T1, T2>
    {
    }

    public class GenericType<T1, T2> : IGenericInterface<string, char>
    {
        public class NestedType
        {
        }
    }

    public class ClosedGenericType : GenericType<string, char>
    {
    }

    public class TypeWithNestedType
    {
        public class NestedType
        {
        }
    }
}