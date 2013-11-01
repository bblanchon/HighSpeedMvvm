using MvvmHighFrequency;
using MvvmHighFrequency.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Method2
{
    class ViewModel : ViewModelBase
    {
        readonly Model model;
        double progress, frequency;

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
