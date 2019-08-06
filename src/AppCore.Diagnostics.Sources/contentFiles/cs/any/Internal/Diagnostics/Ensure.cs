// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Diagnostics.CodeAnalysis;

namespace AppCore.Diagnostics
{
    /// <summary>
    /// Provides helper methods to ensure program contracts such as preconditions, postconditions, and invariants.
    /// </summary>
    #if !APPCORE_SHARED_TEST_SOURCES
    [ExcludeFromCodeCoverage]
    #endif
    internal static partial class Ensure
    {
    }
}