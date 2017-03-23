using System.Collections.Generic;

namespace Platypus.Domain
{
    public abstract class ValueObject
    {
        public virtual bool Equals<T>(IEqualityComparer<T> comparer, T right) where T : ValueObject
        {
            return comparer.Equals((T)this, right);
        }
    }
}