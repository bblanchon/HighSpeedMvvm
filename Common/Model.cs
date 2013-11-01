using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HighSpeedMvvm.Common
{
    public class Model
    {
        const long MAX_DURATION = 20000;

        public double Progress { get; private set; }
        public event EventHandler<double> ProgressChanged;

        public double Frequency { get; private set; }
        public event EventHandler<double> FrequencyChanged;        

        public Model()
        {
            Task.Run((Action)LongRunningBackgroundTask);
        }
        
        void LongRunningBackgroundTask()
        {
            long loopCount = 0;
            long elapsed = 0;

            var chrono = new Stopwatch();
            chrono.Start();

            while (elapsed < MAX_DURATION)
            {
                elapsed = chrono.ElapsedMilliseconds;

                SetProgress(100.0 * elapsed / MAX_DURATION);
                SetFrequency(1.0 * loopCount / elapsed);

                loopCount++;
            }
        }

        void SetProgress(double value)
        {
            Progress = value;

            if (ProgressChanged != null)
                ProgressChanged(this, value);
        }

        void SetFrequency(double value)
        {
            Frequency = value;

            if (FrequencyChanged != null)
                FrequencyChanged(this, value);
        }
    }
}
