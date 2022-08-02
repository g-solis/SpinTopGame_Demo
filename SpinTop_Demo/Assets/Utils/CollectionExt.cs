using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CollectionExt
{
    public static void AddXTimes<T>(this List<T> list, T element, int times)
    {
        if(times <= 0 || list == null)
        {
            return;
        }

        for(int i=0;i<times;i++)
        {
            list.Add(element);
        }
    }

    public static void AddRangeXTimes<T>(this List<T> list, IEnumerable<T> range, int times)
    {
        if(times <= 0 || list == null)
        {
            return;
        }

        for(int i=0;i<times;i++)
        {
            list.AddRange(range);
        }
    }

    public static T Random<T>(this T[] value)
    {
        if (value.Length == 0)
        {
            return default(T);
        }

        return value[UnityEngine.Random.Range(0, value.Length)];
    }

    public static T Random<T>(this List<T> value)
    {
        if (value == null)
        {
            return default(T);
        }

        if (value.Count == 0)
        {
            return default(T);
        }

        return value[UnityEngine.Random.Range(0, value.Count)];
    }

    public static T Random<T>(this IEnumerable<T> value)
    {
        if (value == null)
        {
            return default(T);
        }

        if (value.Count() == 0)
        {
            return default(T);
        }

        return value.ToList()[UnityEngine.Random.Range(0, value.Count())];
    }

    public static T Random<T>(this T[] value, System.Predicate<T> predicate)
    {
        if (value == null)
        {
            return default(T);
        }

        if (value.Length == 0)
        {
            return default(T);
        }

        if (predicate == null)
        {
            return Random(value);
        }

        var filter = new T[value.Length];
        var filterCount = 0;
        foreach (var item in value)
        {
            if (item == null)
            {
                continue;
            }
            
            if (!predicate(item))
            {
                continue;
            }

            filter[filterCount++] = item;
        }

        if (filterCount == 0)
        {
            return default(T);
        }

        return filter[UnityEngine.Random.Range(0, filterCount)];
    }

    public static T Random<T>(this List<T> value, System.Predicate<T> predicate)
    {
        if (value == null)
        {
            return default(T);
        }

        if (value.Count == 0)
        {
            return default(T);
        }

        if (predicate == null)
        {
            return Random(value);
        }

        var filter = new T[value.Count];
        var filterCount = 0;
        foreach (var item in value)
        {
            if (item == null)
            {
                continue;
            }
            
            if (!predicate(item))
            {
                continue;
            }

            filter[filterCount++] = item;
        }

        if (filterCount == 0)
        {
            return default(T);
        }

        return filter[UnityEngine.Random.Range(0, filterCount)];
    }

    public static T Random<T>(this IEnumerable<T> value, System.Predicate<T> predicate)
    {
        if (value == null)
        {
            return default(T);
        }

        if (value.Count() == 0)
        {
            return default(T);
        }

        if (predicate == null)
        {
            return Random(value);
        }

        var filter = new T[value.Count()];
        var filterCount = 0;
        foreach (var item in value)
        {
            if (item == null)
            {
                continue;
            }
            
            if (!predicate(item))
            {
                continue;
            }

            filter[filterCount++] = item;
        }

        if (filterCount == 0)
        {
            return default(T);
        }

        return filter[UnityEngine.Random.Range(0, filterCount)];
    }

    public static T[] Filter<T>(this T[] value, System.Predicate<T> predicate)
    {
        if (value.Length == 0)
        {
            return null;
        }

        if (predicate == null)
        {
            return value;
        }

        var filter = new T[value.Length];
        var filterCount = 0;
        foreach (var item in value)
        {
            if (!predicate(item))
            {
                continue;
            }

            filter[filterCount++] = item;
        }

        if (filterCount == 0)
        {
            return null;
        }

        return filter;
    }

    public static List<T> Filter<T>(this List<T> value, System.Predicate<T> predicate)
    {
        if (value == null)
        {
            return null;
        }

        if (value.Count == 0)
        {
            return null;
        }

        if (predicate == null)
        {
            return value;
        }

        var filter = new T[value.Count];
        var filterCount = 0;
        foreach (var item in value)
        {
            if (!predicate(item))
            {
                continue;
            }

            filter[filterCount++] = item;
        }

        if (filterCount == 0)
        {
            return null;
        }

        return filter.ToList();
    }

    public static IEnumerable<T> Filter<T>(this IEnumerable<T> value, System.Predicate<T> predicate)
    {

        if (value == null)
        {
            return null;
        }

        int count = value.Count();
        if (count == 0)
        {
            return null;
        }

        if (predicate == null)
        {
            return value;
        }

        var filter = new T[count];
        var filterCount = 0;
        foreach (var item in value)
        {
            if (!predicate(item))
            {
                continue;
            }

            filter[filterCount++] = item;
        }

        if (filterCount == 0)
        {
            return null;
        }

        return filter.ToList();
    }
}
