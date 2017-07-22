using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PostSharp.Samples.WeakEvent
{
    internal static class DelegateReferenceKeeper
    {
        private static readonly object staticObj = new object();
        static readonly ConditionalWeakTable<object,List<Delegate>> table = new ConditionalWeakTable<object, List<Delegate>>();

        public static void AddReference(Delegate handler)
        {
            List<Delegate> list = table.GetOrCreateValue(handler.Target ?? staticObj);
            lock (list)
            {
                list.Add(handler);
            }
        }

        public static void RemoveReference(Delegate handler)
        {
            List<Delegate> list;
            if ( table.TryGetValue(handler.Target ?? staticObj, out list ) )
            {
                lock (list)
                {
                    list.Remove(handler);
                }
            }
        }

    }
}