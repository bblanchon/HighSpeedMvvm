using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Method2
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
            while( Progress < 100)
            {
                // With Method2, we do not need to artificially slow down the events
                // so we can remove the Task.Delay

                // Note about this example Model:
                // You may need to have an even smaller increment on a fast computer
                // otherwise Progress would reach 100 too quickly
                Progress += 0.0000001; 

                if (ProgressChanged != null)
                    ProgressChanged(Progress);          
            }
        }
    }
}
