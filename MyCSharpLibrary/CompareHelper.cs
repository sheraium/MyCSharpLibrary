using System;
using System.Collections.Generic;
using System.Text;

namespace MyCSharpLibrary
{
    public static class CompareHelper
    {
        public static bool IsA<T>(this object something)
        {
            return something is T;
        }

        public static bool IsNotA<T>(this object something)
        {
            return !(something is T);
        }

        public static bool IsNull(this object something)
        {
            return something == null;
        }

        public static bool IsNotNull(this object something)
        {
            return something != null;
        }

        public static bool IsTrue(this bool something)
        {
            return something == true;
        }

        public static bool IsFalse(this bool something)
        {
            return something == false;
        }
    }
}
