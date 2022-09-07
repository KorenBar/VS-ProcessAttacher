using System;
using EnvDTE;
using EnvDTE80;
using EnvDTE90;
using EnvDTE100;
using System.Runtime.InteropServices;
using System.Collections;

namespace AttachDebugger
{
    internal static class Extensions
    {
        //public static T Item<T>(this IEnumerable objects, Predicate<T> predicate) where T : class
        //{
        //    foreach (T o in objects) // Element not found. (Exception from HRESULT: 0x8002802B (TYPE_E_ELEMENTNOTFOUND))
        //        if (predicate(o))
        //            return o;
        //    return null;
        //}

        public static Process2 Item(this Processes objects, Predicate<Process2> predicate)
        {
            foreach (Process2 o in objects)
                if (predicate(o))
                    return o;
            return null;
        }

        public static Engine Item(this Engines objects, Predicate<Engine> predicate)
        {
            foreach (Engine o in objects)
                if (predicate(o))
                    return o;
            return null;
        }

        public static Transport Item(this Transports objects, Predicate<Transport> predicate)
        {
            foreach (Transport o in objects)
                if (predicate(o))
                    return o;
            return null;
        }
    }
}
