using PostSharp.Patterns.Caching;
using System;
using PostSharp.Patterns.Caching.Dependencies;

namespace PostSharp.Samples.Caching
{
    [Serializable]
    class Account : ICacheDependency
    {
        public int AccountId;
        public string GetCacheKey() => $"Account:{AccountId}";

        public bool Equals(ICacheDependency other)
        {
            throw new NotImplementedException();
        }
    }

}
