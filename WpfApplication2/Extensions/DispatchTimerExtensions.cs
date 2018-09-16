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

            long ticksPrevLoop = watch.ElapsedTicks;

            watch.Start();
            int NotUsed = ExecGameLoop();
            watch.Stop();

            long ticksLoopDuration = watch.ElapsedTicks;
            timer.Interval = TimeSpan.FromTicks(ticksLoopDuration);
            watch.Reset();

            return ticksLoopDuration;
        }
    }
}
