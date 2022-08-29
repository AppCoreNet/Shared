// Licensed under the MIT License.
// Copyright (c) 2022 the AppCore .NET project.

#if !NETCOREAPP3_0_OR_GREATER && ENABLE_NULLABLE

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [ExcludeFromCodeCoverage]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        public string ParameterName { get; }

        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }
    }
}

#endif