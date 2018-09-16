using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Linq;
using System.Diagnostics;

namespace appGameBoardTest.Extensions
{
    public static class DispatchTimerExtensions
    {
        public static long RunGameLoop(this DispatcherTimer timer, Stopwatch watch, Func<int> ExecGameLoop)
        {
            if (watch == null)
            {
                watch = new Stopwatch();
            }

            long msPrevLoop = watch.ElapsedMilliseconds;

            watch.Start();
            int NotUsed = ExecGameLoop();
            watch.Stop();

            long msLoopDuration = watch.ElapsedMilliseconds;
            timer.Interval = TimeSpan.FromMilliseconds(msLoopDuration);
            watch.Reset();

            return msLoopDuration;
        }
    }
}
