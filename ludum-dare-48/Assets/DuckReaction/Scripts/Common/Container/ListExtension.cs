using System.Collections;
using System.Collections.Generic;

namespace DuckReaction.Common.Container
{
    public static class ListExtension
    {
        public static T Shift<T>(this List<T> list)
        {
            return list.RemoveAndGet<T>(0);
        }

        public static T Pop<T>(this List<T> list)
        {
            return list.RemoveAndGet<T>(list.Count - 1);
        }

        public static T RemoveAndGet<T>(this List<T> list, int index)
        {
            T r = list[index];
            list.RemoveAt(index);
            return r;
        }
    }
}