// Licensed under the MIT License.
// Copyright (c) 2022 the AppCore .NET project.

#if !NETCOREAPP3_0_OR_GREATER

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal class CallerArgumentExpressionAttribute : Attribute
    {
        public string ParameterName { get; }

        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }
    }
}

#endif