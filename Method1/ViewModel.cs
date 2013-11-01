using MvvmHighFrequency;
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
    class ViewModel : INotifyPropertyChanged
    {
        readonly Model model;
        readonly Dispatcher dispatcher;
        double progress, frequency;

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

        #region Public properties

        public double Frequency
        {
            get { return frequency; }
            set
            {
                if (frequency == value) return;
                frequency = value;
                RaisePropertyChanged("Frequency");
            }
        }

        public double Progress
        {
            get { return progress; }
            set
            {
                if (progress == value) return;
                progress = value;
                RaisePropertyChanged("Progress");
            }
        }

        #endregion
        
        #region INotifyPropertyChanged

        void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
