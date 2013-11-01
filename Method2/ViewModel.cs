using System;
using System.Windows.Threading;
using MvvmHighFrequency.Common;

namespace MvvmHighFrequency.Solution2
{
    class ViewModel : ViewModelBase
    {
        readonly Model model;

        public ViewModel()
        {
            model = new Model();

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(5);
            timer.Tick += OnTimerTick;
            timer.Start();
        }

        void OnTimerTick(object sender, EventArgs e)
        {
            Progress = model.Progress;
            Frequency = model.Frequency;
        }
    }
}
