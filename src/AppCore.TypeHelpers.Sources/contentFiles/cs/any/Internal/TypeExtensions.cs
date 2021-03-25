// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using AppCore.Diagnostics;

namespace AppCore
{
    /// <summary>
    /// Provides extension methods for the <see cref="Type"/> class.
    /// </summary>
    #if !APPCORE_SHARED_TEST_SOURCES
    [ExcludeFromCodeCoverage]
    #endif
    internal static class TypeExtensions
    {
        public static string GetDisplayName(this Type type)
        {
            Ensure.Arg.NotNull(type, nameof(type));

            var typeNameBuilder = new StringBuilder();
            BuildDisplayName(typeNameBuilder, type);
            return typeNameBuilder.ToString();
        }

        private static void BuildDisplayName(StringBuilder builder, Type type)
        {
            void BuildTypeArguments(StringBuilder sb, Type[] typeArguments)
            {
                if (typeArguments.Length > 0)
                {
                    builder.Append("<");
                    for (int i = 0; i < typeArguments.Length; i++)
                    {
                        Type typeArgument = typeArguments[i];
                        BuildDisplayName(sb, typeArgument);
                        if (i + 1 < typeArguments.Length)
                            sb.Append(',');
                    }

                    builder.Append(">");
                }
            }

            void BuildTypeName(StringBuilder sb, Type type)
            {
                if (type.IsGenericType && type.Name.Contains("`"))
                {
                    string typeName = type.Name.Substring(0, type.Name.Length - 2);
                    sb.Append(typeName);
                    BuildTypeArguments(builder, type.GetGenericArguments());
                }
                else
                {
                    sb.Append(type.Name);
                }
            }

            if (!type.IsGenericParameter)
            {
                builder.Append(type.Namespace);
                builder.Append(".");
            }

            if (type.IsNested && !type.IsGenericParameter)
            {
                BuildTypeName(builder, type.DeclaringType);
                builder.Append(".");
            }

            BuildTypeName(builder, type);
        }

        /// <summary>
        /// Gets all types assignable from the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeGenericTypeDefinitions"></param>
        /// <returns>An <see cref="IEnumerable{T}"/> of types assignable from the specified type.</returns>
        public static IEnumerable<Type> GetTypesAssignableFrom(this Type type, bool includeGenericTypeDefinitions = false)
        {
            Ensure.Arg.NotNull(type, nameof(type));

            var assignableTypes = new List<Type>();
            GetBagOfTypesAssignableFrom(type, assignableTypes, includeGenericTypeDefinitions);
            return assignableTypes;
        }

        private static void GetBagOfTypesAssignableFrom(Type type, List<Type> assignableType, bool includeGenericTypeDefinitions = false)
        {
            if (assignableType.Contains(type))
                return;

            assignableType.Add(type);

            if (type == typeof(object))
                return;

            if (type.BaseType != null)
            {
                GetBagOfTypesAssignableFrom(type.BaseType, assignableType, includeGenericTypeDefinitions);
            }

            foreach (Type ifce in type.GetInterfaces())
            {
                GetBagOfTypesAssignableFrom(ifce, assignableType, includeGenericTypeDefinitions);
            }

            if (includeGenericTypeDefinitions && type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                GetBagOfTypesAssignableFrom(type.GetGenericTypeDefinition(), assignableType, true);
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
            Type result = type.FindClosedTypeOf(openGeneric);
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
            Ensure.Arg.NotNull(type, nameof(type));
            Ensure.Arg.NotNull(openGeneric, nameof(openGeneric));

            if (type.ContainsGenericParameters)
                return null;

            return type.GetTypesAssignableFrom()
                .FirstOrDefault(t => t.IsGenericType
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