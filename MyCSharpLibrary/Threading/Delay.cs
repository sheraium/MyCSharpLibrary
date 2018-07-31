using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyCSharpLibrary.Threading
{
    public static class Delay
    {
        public static void FromMilliseconds(int milliseconds)
        {
            if (milliseconds <= 0) return;
            SpinWait.SpinUntil(() => false, milliseconds);
        }

        public static void FromSeconds(int seconds)
        {
            if (seconds <= 0) return;
            SpinWait.SpinUntil(() => false, seconds * 1000);
        }

        public static void FromMinutes(int minutes)
        {
            if (minutes <= 0) return;
            SpinWait.SpinUntil(() => false, minutes * 60 * 1000);
        }
    }
}
