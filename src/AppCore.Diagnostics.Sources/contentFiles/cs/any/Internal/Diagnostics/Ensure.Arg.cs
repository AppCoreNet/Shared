// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace AppCore.Diagnostics
{
    internal static partial class Ensure
    {
        /// <summary>
        /// Provides helper methods to ensure method argument contracts.
        /// </summary>
        [ExcludeFromCodeCoverage]
        internal static class Arg
        {
            /// <summary>
            /// Ensures that the argument <paramref name="value"/> is not <c>null</c>.
            /// </summary>
            /// <typeparam name="T">The type of the argument.</typeparam>
            /// <param name="value">The argument value.</param>
            /// <param name="paramName">The parameter name.</param>
            /// <exception cref="ArgumentNullException">The <paramref name="value"/> is <c>null</c>.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [ContractAnnotation("value:null=>halt")]
            public static void NotNull<T>([NoEnumeration] T value, [InvokerParameterName] [NotNull] string paramName)
            {
                if (value == null)
                    throw new ArgumentNullException(paramName);
            }

            /// <summary>
            /// Ensures that the string argument <paramref name="value"/> is not <c>null</c> an empty or only whitespace string.
            /// </summary>
            /// <param name="value">The string value.</param>
            /// <param name="paramName">The parameter name.</param>
            /// <exception cref="ArgumentNullException">The <paramref name="value"/> is <c>null</c>.</exception>
            /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [ContractAnnotation("value:null=>halt")]
            public static void NotEmpty(string value, [InvokerParameterName] [NotNull] string paramName)
            {
                NotNull(value, paramName);

                if (value.Trim().Length == 0)
                    throw new ArgumentException($"Argument '{paramName}' must not be an empty string.", paramName);
            }

            /// <summary>
            /// Ensures that the string argument <paramref name="value"/> is not an empty or only whitespace string
            /// if it is not null.
            /// </summary>
            /// <param name="value">The string value.</param>
            /// <param name="paramName">The parameter name.</param>
            /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty string.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void NotEmptyButNull(string value, [InvokerParameterName] [NotNull] string paramName)
            {
                if (value != null && value.Trim().Length == 0)
                    throw new ArgumentException($"Argument '{paramName}' must not be an empty string.", paramName);
            }

            /// <summary>
            /// Ensures that the collection argument <paramref name="value"/> is not empty.
            /// </summary>
            /// <param name="value">The collection value.</param>
            /// <param name="paramName">The parameter name.</param>
            /// <exception cref="ArgumentNullException">The <paramref name="value"/> is <c>null</c>.</exception>
            /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty collection.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [ContractAnnotation("value:null=>halt")]
            public static void NotEmpty<T>(
                IReadOnlyCollection<T> value,
                [InvokerParameterName] [NotNull] string paramName)
            {
                NotNull(value, paramName);

                if (value.Count == 0)
                    throw new ArgumentException($"Argument '{paramName}' contains no elements.", paramName);
            }

            /// <summary>
            /// Ensures that the argument <paramref name="value"/> is in the specified range.
            /// </summary>
            /// <typeparam name="T">The value type.</typeparam>
            /// <param name="value">The value.</param>
            /// <param name="minValue">The allowed minimum value.</param>
            /// <param name="maxValue">The allowed maximum value.</param>
            /// <param name="paramName">The parameter name.</param>
            /// <exception cref="ArgumentOutOfRangeException">The value <paramref name="value"/> is out of range.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void InRange<T>(T value, T minValue, T maxValue, [InvokerParameterName] [NotNull] string paramName)
                where T : struct, IComparable<T>
            {
                if (value.CompareTo(minValue) < 0
                    || value.CompareTo(maxValue) > 0)
                {
                    throw new ArgumentOutOfRangeException(paramName, value, $"Argument '{paramName}' must be in range from {minValue} to {maxValue}.");
                }
            }

            /// <summary>
            /// Ensures that the string argument does not exceed the specified maximum length.
            /// </summary>
            /// <param name="value">The <see cref="string"/> argument.</param>
            /// <param name="maxLength">The allowed maximum length.</param>
            /// <param name="paramName">The parameter name.</param>
            /// <exception cref="ArgumentOutOfRangeException">The string <paramref name="value"/> exceeds the maximum length.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void MaxLength(string value, int maxLength, [InvokerParameterName] [NotNull] string paramName)
            {
                if (value != null && value.Length > maxLength)
                    throw new ArgumentOutOfRangeException(
                        paramName,
                        value.Length,
                        $"Argument '{paramName}' exceeds maximum length of {maxLength} characters.");
            }

            /// <summary>
            /// Ensures that the string argument does not deceed the specified minimum length.
            /// </summary>
            /// <param name="value">The <see cref="string"/> argument.</param>
            /// <param name="minLength">The required minimum length.</param>
            /// <param name="paramName">The parameter name.</param>
            /// <exception cref="ArgumentOutOfRangeException">The string <paramref name="value"/> deceeds the minimum length.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void MinLength(string value, int minLength, [InvokerParameterName] [NotNull] string paramName)
            {
                if (value != null && value.Length < minLength)
                    throw new ArgumentOutOfRangeException(
                        paramName,
                        value.Length,
                        $"Argument '{paramName}' deceeds minimum length of {minLength} characters.");
            }

            /// <summary>
            /// Ensures that the type argument is of the expected type.
            /// </summary>
            /// <param name="type">The <see cref="Type"/> argument.</param>
            /// <param name="expectedType">The expected <see cref="Type"/>.</param>
            /// <param name="paramName">The parameter name.</param>
            /// <exception cref="ArgumentException">The type argument is not of the expected type.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void OfType(Type type, Type expectedType, [InvokerParameterName] [NotNull] string paramName)
            {
                if (!IsAssignableTo(type, expectedType))
                    throw new ArgumentException($"Argument '{paramName}' is of type '{type}' but expected to be of type '{expectedType}'.", paramName);
            }

            /// <summary>
            /// Ensures that the type argument is of the expected type.
            /// </summary>
            /// <typeparam name="TExpected">The expected <see cref="Type"/>.</typeparam>
            /// <param name="type">The <see cref="Type"/> argument.</param>
            /// <param name="paramName">The parameter name.</param>
            /// <exception cref="ArgumentException">The type argument is not of the expected type.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void OfType<TExpected>(Type type, [InvokerParameterName] [NotNull] string paramName)
            {
                OfType(type, typeof(TExpected), paramName);
            }

            private static bool IsAssignableTo(Type givenType, Type genericType)
            {
                if (givenType == null || genericType == null)
                {
                    return false;
                }

                if (genericType.GetTypeInfo()
                               .IsAssignableFrom(givenType.GetTypeInfo()))
                    return true;

                return givenType == genericType
                       || MapsToGenericTypeDefinition(givenType, genericType)
                       || HasInterfaceThatMapsToGenericTypeDefinition(givenType, genericType)
                       || IsAssignableTo(givenType.GetTypeInfo().BaseType, genericType);
            }

            private static bool HasInterfaceThatMapsToGenericTypeDefinition(Type givenType, Type genericType)
            {
                return givenType
                       .GetTypeInfo()
                       .ImplementedInterfaces
                       .Where(it => it.GetTypeInfo().IsGenericType)
                       .Any(it => it.GetGenericTypeDefinition() == genericType);
            }

            private static bool MapsToGenericTypeDefinition(Type givenType, Type genericType)
            {
                return genericType.GetTypeInfo().IsGenericTypeDefinition
                       && givenType.GetTypeInfo().IsGenericType
                       && givenType.GetGenericTypeDefinition() == genericType;
            }
        }
    }
}
