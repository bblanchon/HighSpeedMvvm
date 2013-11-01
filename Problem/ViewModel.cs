using System;
using System.Windows.Threading;
using HighSpeedMvvm.Common;

namespace HighSpeedMvvm.Problem
{
    class ViewModel : ViewModelBase
    {
        readonly Dispatcher dispatcher;

        public ViewModel()
        {
            dispatcher = Dispatcher.CurrentDispatcher;

            var model = new Model();

            model.ProgressChanged += OnModelProgressChanged;
            model.FrequencyChanged += OnModelFrequencyChanged;
        }

        void OnModelProgressChanged(object sender, double newValue)
        {
            dispatcher.BeginInvoke((Action)delegate { Progress = newValue; });
        }

        void OnModelFrequencyChanged(object sender, double newValue)
        {
            dispatcher.BeginInvoke((Action)delegate { Frequency = newValue; });
        }    
    }
}
