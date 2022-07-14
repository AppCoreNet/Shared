// Licensed under the MIT License.
// Copyright (c) 2022 the AppCore .NET project.

using System.Diagnostics.CodeAnalysis;

#if !NETCOREAPP3_0_OR_GREATER

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