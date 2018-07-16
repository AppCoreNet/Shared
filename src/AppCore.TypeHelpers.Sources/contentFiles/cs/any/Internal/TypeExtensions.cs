// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppCore
{
    /// <summary>
    /// Provides extension methods for the <see cref="Type"/> class.
    /// </summary>
    internal static class TypeExtensions
    {
        public static string GetDisplayName(this Type type)
        {
            return type.FullName;
        }

        /// <summary>
        /// Gets all types assignable from the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>An <see cref="IEnumerable{T}"/> of types assignable from the specified type.</returns>
        public static IEnumerable<Type> GetTypesAssignableFrom(this Type type)
        {
            return GetBagOfTypesAssignableFrom(type)
                .Distinct();
        }

        private static IEnumerable<Type> GetBagOfTypesAssignableFrom(Type type)
        {
            yield return type;

            if (type.GetTypeInfo().BaseType != null)
            {
                yield return type.GetTypeInfo().BaseType;
                foreach (Type fromBase in GetBagOfTypesAssignableFrom(type.GetTypeInfo().BaseType))
                    yield return fromBase;
            }
            else
            {
                if (type != typeof(object))
                    yield return typeof(object);
            }

            foreach (Type ifce in type.GetTypeInfo().ImplementedInterfaces)
            {
                if (ifce != type)
                {
                    yield return ifce;
                    foreach (Type fromIfce in GetBagOfTypesAssignableFrom(ifce))
                        yield return fromIfce;
                }
            }
        }

        /// <summary>
        /// Gets the closed generic type of <paramref name="openGeneric"/> implemented by <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="openGeneric">The open generic type which must be implemented.</param>
        /// <returns>The closed generic type.</returns>
        /// <exception cref="InvalidCastException">The specified type does not implement the generic type.</exception>
        public static Type GetClosedTypeOf(this Type type, Type openGeneric)
        {
            Type result = FindClosedTypeOf(type, openGeneric);
            if (result == null)
                throw new InvalidCastException($"{type.GetDisplayName()} does not implement {openGeneric.GetDisplayName()}");

            return result;
        }

        /// <summary>
        /// Finds the closed generic type of <paramref name="openGeneric"/> implemented by <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="openGeneric">The open generic type which should be implemented.</param>
        /// <returns>The closed generic type or <c>null</c> if the type does not implement the generic type..</returns>
        public static Type FindClosedTypeOf(this Type type, Type openGeneric)
        {
            if (type.GetTypeInfo().ContainsGenericParameters)
                return null;

            return type.GetTypesAssignableFrom()
                .FirstOrDefault(t => t.GetTypeInfo().IsGenericType
                                     && t.GetGenericTypeDefinition() == openGeneric);
        }

        /// <summary>
        /// Gets a value indicating whether the closed generic type of <paramref name="openGeneric"/> is implemented.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="openGeneric">The open generic type which should be implemented.</param>
        /// <returns><c>true</c> if the open generic type is implemented; <c>false</c> otherwise.</returns>
        public static bool IsClosedTypeOf(this Type type, Type openGeneric)
        {
            return type.FindClosedTypeOf(openGeneric) != null;
        }
    }
}