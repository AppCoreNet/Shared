// <auto-generated />

#if !NET5_0_OR_GREATER && APPCORENET_ATTRIBUTES_ENABLED && APPCORENET_NULLABLE_ATTRIBUTES_ENABLED

// ReSharper disable once CheckNamespace
namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// Specifies that the method or property will ensure that the listed field and property members have
/// not-<see langword="null"/> values.
/// </summary>
[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, Inherited = false, AllowMultiple = true)]
internal sealed class MemberNotNullAttribute : Attribute
{
    /// <summary>
    /// Gets field or property member names.
    /// </summary>
    public string[] Members { get; }

    /// <summary>
    /// Initializes the attribute with a field or property member.
    /// </summary>
    /// <param name="member">
    /// The field or property member that is promised to be not-null.
    /// </param>
    public MemberNotNullAttribute(string member) =>
        Members = new [] { member };

    /// <summary>
    /// Initializes the attribute with the list of field and property members.
    /// </summary>
    /// <param name="members">
    /// The list of field and property members that are promised to be not-null.
    /// </param>
    public MemberNotNullAttribute(params string[] members) =>
        Members = members;
}

#endif