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