using System;
using System.Collections.Generic;

namespace Platypus
{
    public static class LinqExtensions
    {
        public static void Fold<T>(this List<T> input, Action<T> action)
        {
            input.ForEach(action);
        } 
    }
}