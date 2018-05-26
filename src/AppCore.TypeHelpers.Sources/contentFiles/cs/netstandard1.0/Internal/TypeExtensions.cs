// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppCore
{
    internal static class TypeExtensions
    {
        public static string GetDisplayName(this Type type)
        {
            return type.FullName;
        }

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

        public static Type GetClosedTypeOf(this Type @this, Type openGeneric)
        {
            Type result = FindClosedTypeOf(@this, openGeneric);
            if (result == null)
                throw new InvalidCastException($"{@this.GetDisplayName()} does not implement {openGeneric.GetDisplayName()}");

            return result;
        }

        public static Type FindClosedTypeOf(this Type @this, Type openGeneric)
        {
            if (@this.GetTypeInfo().ContainsGenericParameters)
                return null;

            return @this.GetTypesAssignableFrom()
                .FirstOrDefault(t => t.GetTypeInfo().IsGenericType
                                     && t.GetGenericTypeDefinition() == openGeneric);
        }

        public static bool IsClosedTypeOf(this Type @this, Type openGeneric)
        {
            return @this.FindClosedTypeOf(openGeneric) != null;
        }
    }
}