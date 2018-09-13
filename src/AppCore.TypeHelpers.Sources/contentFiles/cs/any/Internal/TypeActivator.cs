using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AppCore
{
    [ExcludeFromCodeCoverage]
    internal static class TypeActivator
    {
        private static Delegate GetFactoryDelegate(Type type, params Type[] argTypes)
        {
            ConstructorInfo constructor = type.GetTypeInfo()
                                              .DeclaredConstructors.FirstOrDefault(
                                                  ci => ci.IsPublic
                                                        && ci.GetParameters()
                                                             .Select(p => p.ParameterType)
                                                             .SequenceEqual(argTypes));

            if (constructor == null)
            {
                throw new NotImplementedException(
                    $"Type '{type.GetDisplayName()}' does not have a constructor with arguments of type "
                    + $"'{string.Join(",", argTypes.Select(t => t.GetDisplayName()))}'.");
            }

            ParameterExpression[] parameters =
                argTypes.Select((t, i) => Expression.Parameter(t, "arg" + i))
                        .ToArray();

            return Expression.Lambda(Expression.New(constructor, parameters), parameters)
                             .Compile();
        }

        private static class TypeFactory
        {
            public static class ForType<T>
            {
                public static class WithoutArgs
                {
                    public static readonly Func<T> Create =
                        (Func<T>) GetFactoryDelegate(typeof(T));
                }

                public static class WithArgs<TArg1>
                {
                    public static readonly Func<TArg1, T> Create =
                        (Func<TArg1, T>) GetFactoryDelegate(typeof(T), typeof(TArg1));
                }

                public static class WithArgs<TArg1, TArg2>
                {
                    public static readonly Func<TArg1, TArg2, T> Create =
                        (Func<TArg1, TArg2, T>) GetFactoryDelegate(typeof(T), typeof(TArg1), typeof(TArg2));
                }

                public static class WithArgs<TArg1, TArg2, TArg3>
                {
                    public static readonly Func<TArg1, TArg2, TArg3, T> Create =
                        (Func<TArg1, TArg2, TArg3, T>) GetFactoryDelegate(
                            typeof(T),
                            typeof(TArg1),
                            typeof(TArg2),
                            typeof(TArg3));
                }

                public static class WithArgs<TArg1, TArg2, TArg3, TArg4>
                {
                    public static readonly Func<TArg1, TArg2, TArg3, TArg4, T> Create =
                        (Func<TArg1, TArg2, TArg3, TArg4, T>) GetFactoryDelegate(
                            typeof(T),
                            typeof(TArg1),
                            typeof(TArg2),
                            typeof(TArg3),
                            typeof(TArg4));
                }
            }

            public class NonGenericTypeFactory
            {
                private struct ArgTypes
                {
                    public Type Arg1;
                    public Type Arg2;
                    public Type Arg3;
                    public Type Arg4;
                }

                private readonly Type _type;
                private Func<object> _noArgsFactory;

                private readonly Dictionary<ArgTypes, Delegate> _argsFactories =
                    new Dictionary<ArgTypes, Delegate>();

                public NonGenericTypeFactory(Type type)
                {
                    _type = type;
                }

                public Func<object> WithoutArgs()
                {
                    return _noArgsFactory
                           ?? (_noArgsFactory = (Func<object>) GetFactoryDelegate(_type));
                }

                private Delegate GetOrAddFactory(ArgTypes key, Func<ArgTypes, Delegate> func)
                {
                    lock (_argsFactories)
                    {
                        if (!_argsFactories.TryGetValue(key, out Delegate factory))
                        {
                            factory = func(key);
                            _argsFactories.Add(key, factory);
                        }

                        return factory;
                    }
                }

                public Func<TArg1, object> WithArgs<TArg1>()
                {
                    return (Func<TArg1, object>) GetOrAddFactory(
                        new ArgTypes
                        {
                            Arg1 = typeof(TArg1)
                        },
                        t => GetFactoryDelegate(_type, t.Arg1));
                }

                public Func<TArg1, TArg2, object> WithArgs<TArg1, TArg2>()
                {
                    return (Func<TArg1, TArg2, object>) GetOrAddFactory(
                        new ArgTypes
                        {
                            Arg1 = typeof(TArg1),
                            Arg2 = typeof(TArg2)
                        },
                        t => GetFactoryDelegate(_type, t.Arg1, t.Arg2));
                }

                public Func<TArg1, TArg2, TArg3, object> WithArgs<TArg1, TArg2, TArg3>()
                {
                    return (Func<TArg1, TArg2, TArg3, object>) GetOrAddFactory(
                        new ArgTypes
                        {
                            Arg1 = typeof(TArg1),
                            Arg2 = typeof(TArg2),
                            Arg3 = typeof(TArg3)
                        },
                        t => GetFactoryDelegate(_type, t.Arg1, t.Arg2, t.Arg3));
                }

                public Func<TArg1, TArg2, TArg3, TArg4, object> WithArgs<TArg1, TArg2, TArg3, TArg4>()
                {
                    return (Func<TArg1, TArg2, TArg3, TArg4, object>) GetOrAddFactory(
                        new ArgTypes
                        {
                            Arg1 = typeof(TArg1),
                            Arg2 = typeof(TArg2),
                            Arg3 = typeof(TArg3),
                            Arg4 = typeof(TArg4)
                        },
                        t => GetFactoryDelegate(_type, t.Arg1, t.Arg2, t.Arg3, t.Arg4));
                }
            }

            private static readonly Dictionary<Type, NonGenericTypeFactory> _factories =
                new Dictionary<Type, NonGenericTypeFactory>();

            public static NonGenericTypeFactory ForReflectedType(Type type)
            {
                lock (_factories)
                {
                    if (!_factories.TryGetValue(type, out NonGenericTypeFactory factory))
                    {
                        factory = new NonGenericTypeFactory(type);
                        _factories.Add(type, factory);
                    }

                    return factory;
                }
            }
        }

        public static T CreateInstance<T>()
        {
            return TypeFactory.ForType<T>.WithoutArgs.Create();
        }

        public static object CreateInstance(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return TypeFactory.ForReflectedType(type)
                              .WithoutArgs();
        }

        public static T CreateInstance<T, TArg1>(TArg1 arg1)
        {
            return TypeFactory.ForType<T>.WithArgs<TArg1>.Create(arg1);
        }

        public static object CreateInstance<TArg1>(this Type type, TArg1 arg1)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return TypeFactory.ForReflectedType(type)
                              .WithArgs<TArg1>()(arg1);
        }

        public static T CreateInstance<T, TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
        {
            return TypeFactory.ForType<T>.WithArgs<TArg1, TArg2>.Create(arg1, arg2);
        }

        public static object CreateInstance<TArg1, TArg2>(this Type type, TArg1 arg1, TArg2 arg2)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return TypeFactory.ForReflectedType(type)
                              .WithArgs<TArg1, TArg2>()(arg1, arg2);
        }

        public static T CreateInstance<T, TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return TypeFactory.ForType<T>.WithArgs<TArg1, TArg2, TArg3>.Create(arg1, arg2, arg3);
        }

        public static object CreateInstance<TArg1, TArg2, TArg3>(this Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return TypeFactory.ForReflectedType(type)
                              .WithArgs<TArg1, TArg2, TArg3>()(arg1, arg2, arg3);
        }

        public static T CreateInstance<T, TArg1, TArg2, TArg3, TArg4>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
        {
            return TypeFactory.ForType<T>.WithArgs<TArg1, TArg2, TArg3, TArg4>.Create(arg1, arg2, arg3, arg4);
        }

        public static object CreateInstance<TArg1, TArg2, TArg3, TArg4>(
            this Type type,
            TArg1 arg1,
            TArg2 arg2,
            TArg3 arg3,
            TArg4 arg4)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return TypeFactory.ForReflectedType(type)
                              .WithArgs<TArg1, TArg2, TArg3, TArg4>()(arg1, arg2, arg3, arg4);
        }
    }
}
