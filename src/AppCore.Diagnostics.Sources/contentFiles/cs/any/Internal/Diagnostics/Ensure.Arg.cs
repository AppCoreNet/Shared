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
        #if !APPCORE_SHARED_TEST_SOURCES
        [ExcludeFromCodeCoverage]
        #endif
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
            public static void NotNull<T>([NoEnumeration] T value,
                                          [CallerArgumentExpression("value")]
                                          [InvokerParameterName]
                                          string paramName = null)
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
            public static void NotEmpty(string value,
                                        [CallerArgumentExpression("value")]
                                        [InvokerParameterName]
                                        string paramName = null)
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
            public static void NotEmptyButNull(string value,
                                               [CallerArgumentExpression("value")]
                                               [InvokerParameterName]
                                               string paramName = null)
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
            public static void NotEmpty<T>(IReadOnlyCollection<T> value,
                                           [CallerArgumentExpression("value")]
                                           [InvokerParameterName]
                                           string paramName = null)
            {
                NotNull(value, paramName);

                if (value.Count == 0)
                    throw new ArgumentException($"Argument '{paramName}' contains no elements.", paramName);
            }

            /// <summary>
            /// Ensures that the value argument <paramref name="value"/> does not have the default value.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="paramName">The parameter name.</param>
            /// <exception cref="ArgumentException">The <paramref name="value"/> is an empty collection.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void NotEmpty<T>(T value,
                                           [CallerArgumentExpression("value")]
                                           [InvokerParameterName]
                                           string paramName = null)
                where T : struct
            {
                bool hasDefaultValue = value is IEquatable<T> equatable
                    ? equatable.Equals(default)
                    : value.Equals(default(T));

                if (hasDefaultValue)
                    throw new ArgumentException($"Argument '{paramName}' must not be empty.", paramName);
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
            public static void InRange<T>(T value, T minValue, T maxValue,
                                          [CallerArgumentExpression("value")]
                                          [InvokerParameterName]
                                          string paramName = null)
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
            public static void MaxLength(string value, int maxLength,
                                         [CallerArgumentExpression("value")]
                                         [InvokerParameterName]
                                         string paramName = null)
            {
                if (value != null && value.Length > maxLength)
                    throw new ArgumentOutOfRangeException(
                        paramName,
                        value.Length,
                        $"Argument '{paramName}' exceeds maximum length of {maxLength} characters.");
            }

            /// <summary>
            /// Ensures that the string length does not fall below the minimum length.
            /// </summary>
            /// <param name="value">The <see cref="string"/> argument.</param>
            /// <param name="minLength">The required minimum length.</param>
            /// <param name="paramName">The parameter name.</param>
            /// <exception cref="ArgumentOutOfRangeException">The string length <paramref name="value"/> is below the minimum length.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void MinLength(string value, int minLength,
                                         [CallerArgumentExpression("value")]
                                         [InvokerParameterName]
                                         string paramName = null)
            {
                if (value != null && value.Length < minLength)
                    throw new ArgumentOutOfRangeException(
                        paramName,
                        value.Length,
                        $"Argument '{paramName}' length is below the minimum length of {minLength} characters.");
            }

            /// <summary>
            /// Ensures that the type argument is of the expected type. Supports testing for
            /// open generic types.
            /// </summary>
            /// <param name="type">The <see cref="Type"/> argument.</param>
            /// <param name="expectedType">The expected <see cref="Type"/>.</param>
            /// <param name="paramName">The parameter name.</param>
            /// <exception cref="ArgumentException">The type argument is not of the expected type.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void OfType(Type type, Type expectedType,
                                      [CallerArgumentExpression("type")]
                                      [InvokerParameterName]
                                      string paramName = null)
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
            public static void OfType<TExpected>(Type type,
                                                 [CallerArgumentExpression("type")]
                                                 [InvokerParameterName]
                                                 string paramName = null)
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
                {
                    return true;
                }

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
