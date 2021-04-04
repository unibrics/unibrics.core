﻿namespace Unibrics.Core.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AssemblyExtensions
    {
        private static List<Type> cachedTypes;

        private static void CheckCachedTypes()
        {
            cachedTypes ??= AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.GetCustomAttribute<UnibricsDiscoverableAttribute>() != null)
                .SelectMany(assembly => assembly.GetTypes()).ToList();
        }

        public static IEnumerable<(TAttribute attribute, Type type)> GetTypesWithAttributePairs<TAttribute>()
            where TAttribute : Attribute
        {
            CheckCachedTypes();

            return cachedTypes
                .Select(type => ((TAttribute) type.GetCustomAttribute(typeof(TAttribute)), type))
                .Where(tuple => tuple.Item1 != null);
        }

        public static IEnumerable<Type> GetTypesWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            CheckCachedTypes();

            return cachedTypes.Where(type => type.GetCustomAttribute(typeof(TAttribute)) != null);
        }

        public static IEnumerable<Type> GetTypesWithAttribute<TAttribute, TParentType>() where TAttribute : Attribute
        {
            return GetTypesWithAttribute<TAttribute>().Where(type => typeof(TParentType).IsAssignableFrom(type));
        }
    }
}