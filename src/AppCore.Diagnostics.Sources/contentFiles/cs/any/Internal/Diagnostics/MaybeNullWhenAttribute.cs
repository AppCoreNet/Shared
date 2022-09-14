// Licensed under the MIT License.
// Copyright (c) 2022 the AppCore .NET project.

#if !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1 && ENABLE_NULLABLE

namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class MaybeNullWhenAttribute : Attribute
    {
        public bool ReturnValue { get; }

        public MaybeNullWhenAttribute(bool returnValue)
        {
            ReturnValue = returnValue;
        }
    }
}

#endif