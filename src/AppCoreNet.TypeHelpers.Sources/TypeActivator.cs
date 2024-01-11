﻿// <auto-generated />
// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AppCoreNet.Diagnostics;

#nullable enable

namespace AppCoreNet;

#if !APPCORENET_SHARED_SOURCES_TESTS
[ExcludeFromCodeCoverage]
#endif
internal static class TypeActivator
{
    public static TDelegate GetFactoryDelegate<TDelegate>(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
        Type type)
        where TDelegate : Delegate
    {
        Type[] argTypes = typeof(TDelegate).GetTypeInfo().GenericTypeArguments;
        argTypes = argTypes.Take(argTypes.Length - 1)
                           .ToArray();

        return GetFactoryDelegate<TDelegate>(type, argTypes);
    }

    public static TDelegate GetFactoryDelegate<TDelegate>(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
        Type type,
        params Type[] argTypes)
        where TDelegate : Delegate
    {
        Ensure.Arg.NotNull(type);

        ConstructorInfo? constructor = type.GetTypeInfo()
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

        Type[] passedArgTypes = typeof(TDelegate).GetTypeInfo().GenericTypeArguments;
        passedArgTypes = passedArgTypes.Take(passedArgTypes.Length - 1)
                                       .ToArray();

        ParameterExpression[] parameters =
            passedArgTypes.Select((t, i) => Expression.Parameter(t, "arg" + i))
                          .ToArray();

        NewExpression body = Expression.New(
            constructor,
            parameters.Select((_, i) => Expression.Convert(parameters[i], argTypes[i])));

        return (TDelegate)Expression.Lambda(body, parameters)
                                    .Compile();
    }

#if !APPCORENET_SHARED_SOURCES_TESTS
    [ExcludeFromCodeCoverage]
#endif
    private static class TypeFactory
    {
#if !APPCORENET_SHARED_SOURCES_TESTS
        [ExcludeFromCodeCoverage]
#endif
        public static class ForType<T>
        {
#if !APPCORENET_SHARED_SOURCES_TESTS
            [ExcludeFromCodeCoverage]
#endif
            public static class WithoutArgs
            {
                public static readonly Func<T> Create =
                    GetFactoryDelegate<Func<T>>(typeof(T));
            }

#if !APPCORENET_SHARED_SOURCES_TESTS
            [ExcludeFromCodeCoverage]
#endif
            public static class WithArgs<TArg1>
            {
                public static readonly Func<TArg1, T> Create =
                    GetFactoryDelegate<Func<TArg1, T>>(typeof(T), typeof(TArg1));
            }

#if !APPCORENET_SHARED_SOURCES_TESTS
            [ExcludeFromCodeCoverage]
#endif
            public static class WithArgs<TArg1, TArg2>
            {
                public static readonly Func<TArg1, TArg2, T> Create =
                    GetFactoryDelegate<Func<TArg1, TArg2, T>>(typeof(T), typeof(TArg1), typeof(TArg2));
            }

#if !APPCORENET_SHARED_SOURCES_TESTS
            [ExcludeFromCodeCoverage]
#endif
            public static class WithArgs<TArg1, TArg2, TArg3>
            {
                public static readonly Func<TArg1, TArg2, TArg3, T> Create =
                    GetFactoryDelegate<Func<TArg1, TArg2, TArg3, T>>(
                        typeof(T),
                        typeof(TArg1),
                        typeof(TArg2),
                        typeof(TArg3));
            }

#if !APPCORENET_SHARED_SOURCES_TESTS
            [ExcludeFromCodeCoverage]
#endif
            public static class WithArgs<TArg1, TArg2, TArg3, TArg4>
            {
                public static readonly Func<TArg1, TArg2, TArg3, TArg4, T> Create =
                    GetFactoryDelegate<Func<TArg1, TArg2, TArg3, TArg4, T>>(
                        typeof(T),
                        typeof(TArg1),
                        typeof(TArg2),
                        typeof(TArg3),
                        typeof(TArg4));
            }
        }

#if !APPCORENET_SHARED_SOURCES_TESTS
        [ExcludeFromCodeCoverage]
#endif
        public sealed class NonGenericTypeFactory
        {
            private struct ArgTypes
            {
                public Type Arg1;
                public Type Arg2;
                public Type Arg3;
                public Type Arg4;
            }

            private readonly Dictionary<ArgTypes, Delegate> _argsFactories = new ();
            [DynamicallyAccessedMembers(
                DynamicallyAccessedMemberTypes.PublicConstructors
                | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
            private readonly Type _type;
            private Func<object>? _noArgsFactory;

            public NonGenericTypeFactory(
                [DynamicallyAccessedMembers(
                    DynamicallyAccessedMemberTypes.PublicConstructors
                    | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
                Type type)
            {
                _type = type;
            }

            public Func<object> WithoutArgs()
            {
                return _noArgsFactory ??= GetFactoryDelegate<Func<object>>(_type);
            }

            private Delegate GetOrAddFactory(ArgTypes key, Func<ArgTypes, Delegate> func)
            {
                lock (_argsFactories)
                {
                    if (!_argsFactories.TryGetValue(key, out Delegate? factory))
                    {
                        factory = func(key);
                        _argsFactories.Add(key, factory);
                    }

                    return factory;
                }
            }

            public Func<TArg1, object> WithArgs<TArg1>()
            {
                return (Func<TArg1, object>)GetOrAddFactory(
                    new ArgTypes
                    {
                        Arg1 = typeof(TArg1),
                    },
                    t => GetFactoryDelegate<Func<TArg1, object>>(_type, t.Arg1));
            }

            public Func<TArg1, TArg2, object> WithArgs<TArg1, TArg2>()
            {
                return (Func<TArg1, TArg2, object>)GetOrAddFactory(
                    new ArgTypes
                    {
                        Arg1 = typeof(TArg1),
                        Arg2 = typeof(TArg2),
                    },
                    t => GetFactoryDelegate<Func<TArg1, TArg2, object>>(_type, t.Arg1, t.Arg2));
            }

            public Func<TArg1, TArg2, TArg3, object> WithArgs<TArg1, TArg2, TArg3>()
            {
                return (Func<TArg1, TArg2, TArg3, object>)GetOrAddFactory(
                    new ArgTypes
                    {
                        Arg1 = typeof(TArg1),
                        Arg2 = typeof(TArg2),
                        Arg3 = typeof(TArg3),
                    },
                    t => GetFactoryDelegate<Func<TArg1, TArg2, TArg3, object>>(_type, t.Arg1, t.Arg2, t.Arg3));
            }

            public Func<TArg1, TArg2, TArg3, TArg4, object> WithArgs<TArg1, TArg2, TArg3, TArg4>()
            {
                return (Func<TArg1, TArg2, TArg3, TArg4, object>)GetOrAddFactory(
                    new ArgTypes
                    {
                        Arg1 = typeof(TArg1),
                        Arg2 = typeof(TArg2),
                        Arg3 = typeof(TArg3),
                        Arg4 = typeof(TArg4),
                    },
                    t => GetFactoryDelegate<Func<TArg1, TArg2, TArg3, TArg4, object>>(
                        _type,
                        t.Arg1,
                        t.Arg2,
                        t.Arg3,
                        t.Arg4));
            }
        }

        private static readonly Dictionary<Type, NonGenericTypeFactory> Factories = new ();

        public static NonGenericTypeFactory ForReflectedType(
            [DynamicallyAccessedMembers(
                DynamicallyAccessedMemberTypes.PublicConstructors
                | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
            Type type)
        {
            lock (Factories)
            {
                if (!Factories.TryGetValue(type, out NonGenericTypeFactory? factory))
                {
                    factory = new NonGenericTypeFactory(type);
                    Factories.Add(type, factory);
                }

                return factory;
            }
        }
    }

    public static T CreateInstance<T>()
    {
        return TypeFactory.ForType<T>.WithoutArgs.Create();
    }

    public static object CreateInstance(
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicConstructors
            | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
        this Type type)
    {
        Ensure.Arg.NotNull(type, nameof(type));

        return TypeFactory.ForReflectedType(type)
                          .WithoutArgs();
    }

    public static T CreateInstance<T, TArg1>(TArg1 arg1)
    {
        return TypeFactory.ForType<T>.WithArgs<TArg1>.Create(arg1);
    }

    public static object CreateInstance<TArg1>(
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicConstructors
            | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
        this Type type,
        TArg1 arg1)
    {
        Ensure.Arg.NotNull(type, nameof(type));

        return TypeFactory.ForReflectedType(type)
                          .WithArgs<TArg1>()(arg1);
    }

    public static T CreateInstance<T, TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
    {
        return TypeFactory.ForType<T>.WithArgs<TArg1, TArg2>.Create(arg1, arg2);
    }

    public static object CreateInstance<TArg1, TArg2>(
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicConstructors
            | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
        this Type type,
        TArg1 arg1,
        TArg2 arg2)
    {
        Ensure.Arg.NotNull(type, nameof(type));

        return TypeFactory.ForReflectedType(type)
                          .WithArgs<TArg1, TArg2>()(arg1, arg2);
    }

    public static T CreateInstance<T, TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
    {
        return TypeFactory.ForType<T>.WithArgs<TArg1, TArg2, TArg3>.Create(arg1, arg2, arg3);
    }

    public static object CreateInstance<TArg1, TArg2, TArg3>(
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicConstructors
            | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
        this Type type,
        TArg1 arg1,
        TArg2 arg2,
        TArg3 arg3)
    {
        Ensure.Arg.NotNull(type, nameof(type));

        return TypeFactory.ForReflectedType(type)
                          .WithArgs<TArg1, TArg2, TArg3>()(arg1, arg2, arg3);
    }

    public static T CreateInstance<T, TArg1, TArg2, TArg3, TArg4>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
    {
        return TypeFactory.ForType<T>.WithArgs<TArg1, TArg2, TArg3, TArg4>.Create(arg1, arg2, arg3, arg4);
    }

    public static object CreateInstance<TArg1, TArg2, TArg3, TArg4>(
        [DynamicallyAccessedMembers(
            DynamicallyAccessedMemberTypes.PublicConstructors
            | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
        this Type type,
        TArg1 arg1,
        TArg2 arg2,
        TArg3 arg3,
        TArg4 arg4)
    {
        Ensure.Arg.NotNull(type, nameof(type));

        return TypeFactory.ForReflectedType(type)
                          .WithArgs<TArg1, TArg2, TArg3, TArg4>()(arg1, arg2, arg3, arg4);
    }
}