using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Method1
{
    class Model
    {
        public Model()
        {
            Task.Run((Action)LongRunningBackgroundTask);
        }

        public double Progress { get; private set; }
        public event Action<double> ProgressChanged;

        void LongRunningBackgroundTask()
        {
            var duration = TimeSpan.FromSeconds(20);
            var chrono = new Stopwatch();

            chrono.Start();

            while( chrono.Elapsed < duration)
            {
                // With Method1, we need to slow down the notification
                // otherwise the GUI will reach a shortage
                Task.Delay(1).Wait(); 

                Progress = 100.0 * chrono.ElapsedTicks / duration.Ticks;

                if (ProgressChanged != null)
                    ProgressChanged(Progress);      
            }
        }
    }
}
