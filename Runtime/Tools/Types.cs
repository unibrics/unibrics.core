namespace Unibrics.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core;

    public static class Types
    {
        private static List<Type> cachedTypes;

        private static List<Type> CachedTypes()
        {
            return cachedTypes ??= AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.GetCustomAttribute<UnibricsDiscoverableAttribute>() != null)
                .SelectMany(assembly => assembly.GetTypes()).ToList();
        }

        public static IEnumerable<(TAttribute attribute, Type type)> AnnotatedWith<TAttribute>()
            where TAttribute : Attribute
        {
            return CachedTypes().Select(type => ((TAttribute) type.GetCustomAttribute(typeof(TAttribute)), type))
                .Where(tuple => tuple.Item1 != null);
        }

        public static IEnumerable<(TAttribute attribute, Type type)> WithParent<TAttribute>(
            this IEnumerable<(TAttribute attribute, Type type)> typesAndAttributes, Type type) where TAttribute : Attribute
        {
            return typesAndAttributes.Where(tuple => type.IsAssignableFrom(tuple.type));
            ;
        }
        
        public static (TAttribute attribute, Type type) EnsuredSingle<TAttribute>(
            this IEnumerable<(TAttribute attribute, Type type)> typesAndAttributes) where TAttribute : Attribute
        {
            var types = typesAndAttributes.ToList();
            if (types.Count > 1)
            {
                throw new Exception(
                    $"Expected single type of {types.FirstOrDefault().type} with attribute {typeof(TAttribute)}" +
                    $" in application, but {string.Join(",", types)} found. ");
            }

            return types.FirstOrDefault();
        }

        public static Type Type<TAttribute>(this (TAttribute attribute, Type type) tuple) where TAttribute : Attribute
        {
            return tuple.type;
        }

        public static IEnumerable<Type> TypesOnly<TAttribute>(
            this IEnumerable<(TAttribute attribute, Type type)> typesAndAttributes) where TAttribute : Attribute
        {
            return typesAndAttributes.Select(tuple => tuple.type);
        }
        
        public static IEnumerable<T> CreateInstances<T>(
            this IEnumerable<Type> types)
        {
            return types.Select(type => (T)Activator.CreateInstance(type));
        }
    }
}