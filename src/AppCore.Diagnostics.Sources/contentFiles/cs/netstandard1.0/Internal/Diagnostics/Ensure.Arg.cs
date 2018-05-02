// Copyright 2018 the AppCore project.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace AppCore.Diagnostics
{
    internal static partial class Ensure
    {
        internal static class Arg
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [ContractAnnotation("value:null=>halt")]
            public static void NotNull<T>([NoEnumeration] T value, [InvokerParameterName] [NotNull] string paramName)
            {
                if (value == null)
                    throw new ArgumentNullException(paramName);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [ContractAnnotation("value:null=>halt")]
            public static void NotEmpty(string value, [InvokerParameterName] [NotNull] string paramName)
            {
                NotNull(value, paramName);

                if (value.Trim()
                         .Length
                    == 0)
                    throw new ArgumentException($"Argument '{paramName}' must not be an empty string.", paramName);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void NotEmptyButNull(string value, [InvokerParameterName] [NotNull] string paramName)
            {
                if (value != null && value.Length == 0)
                    throw new ArgumentException($"Argument '{paramName}' must not be an empty string.", paramName);
            }

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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void MaxLength(string value, int maxLength, [InvokerParameterName] [NotNull] string paramName)
            {
                if (value.Length > maxLength)
                    throw new ArgumentOutOfRangeException(
                        paramName,
                        value.Length,
                        $"Argument '{paramName}' exceeds maximum length of {maxLength} characters.");
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void OfType(Type type, Type expectedType, [InvokerParameterName] [NotNull] string paramName)
            {
                if (!IsAssignableTo(type, expectedType))
                    throw new ArgumentException($"Argument '{paramName}' is of type '{type}' but expected to be of type '{expectedType}'.", paramName);
            }

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
