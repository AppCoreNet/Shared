﻿// <auto-generated />

#if !NETCOREAPP3_0_OR_GREATER && APPCORENET_ATTRIBUTES_ENABLED

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

/// <summary>
/// Indicates that a parameter captures the expression passed for another parameter as a string.
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(AttributeTargets.Parameter)]
[Description("https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callerargumentexpressionattribute")]
internal sealed class CallerArgumentExpressionAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CallerArgumentExpressionAttribute"/> class.
    /// </summary>
    /// <param name="parameterName">
    /// The name of the parameter whose expression should be captured as a string.
    /// </param>
    public CallerArgumentExpressionAttribute(string parameterName) =>
        ParameterName = parameterName;

    /// <summary>
    /// Gets the name of the parameter whose expression should be captured as a string.
    /// </summary>
    public string ParameterName { get; }
}

#endif