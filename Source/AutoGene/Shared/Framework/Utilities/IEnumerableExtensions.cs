﻿using System.Collections.Generic;
using System.Linq;

namespace Shared.Framework.Utilities
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<List<T>> InSetsOf<T>(
            this IEnumerable<T> source, int max)
        {
            var toReturn = new List<T>(max);
            foreach (T item in source)
            {
                toReturn.Add(item);
                if (toReturn.Count == max)
                {
                    yield return toReturn;
                    toReturn = new List<T>(max);
                }
            }
            if (toReturn.Any())
            {
                yield return toReturn;
            }
        }
    }
}