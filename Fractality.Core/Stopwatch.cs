using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractality.Core
{
    public struct Stopwatch
    {
        private readonly long current;

        private Stopwatch(long c) : this()
        {
            current = c;
        }

        public static Stopwatch Start()
        {
            return new Stopwatch(DateTime.Now.Ticks);
        }

        public TimeSpan Elapsed()
        {
            return TimeSpan.FromTicks(DateTime.Now.Ticks - current);
        }
    }
}
