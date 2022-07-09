using System.Collections;
using System;

namespace OVAWORKT
{
    public static class CollectionUtility
    {
        public static bool IsNullOrEmpty(this Array item)
        {
            if (item == null) return true;
            if (item.Length <= 0) return true;
            return false;
        }

        public static bool IsNullOrEmpty(this ICollection item)
        {
            if (item == null) return true;
            if (item.Count <= 0) return true;
            return false;
        }
    }
}