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

namespace Method1
{
    class ViewModel : ViewModelBase
    {
        readonly Model model;
        readonly Dispatcher dispatcher;

        public ViewModel()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            model = new Model();

            model.ProgressChanged += OnModelProgressChanged;
            model.FrequencyChanged += OnModelFrequencyChanged;
        }

        void OnModelProgressChanged(object sender, double newValue)
        {
            dispatcher.BeginInvoke((Action)delegate() { Progress = newValue; });
        }

        void OnModelFrequencyChanged(object sender, double newValue)
        {
            dispatcher.BeginInvoke((Action)delegate() { Frequency = newValue; });
        }    
    }
}
