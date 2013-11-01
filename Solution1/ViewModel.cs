using MvvmHighFrequency;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Solution1
{
    class ViewModel : INotifyPropertyChanged
    {
        readonly Model model;
        readonly Dispatcher dispatcher;
        double progress;
        double frequency;

        public ViewModel()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            model = new Model();

            Observable.FromEventPattern<double>(model, "ProgressChanged")
                 .Sample(TimeSpan.FromMilliseconds(20))
                 .ObserveOn(dispatcher)
                 .Subscribe(x => Progress = x.EventArgs);

            Observable.FromEventPattern<double>(model, "FrequencyChanged")
                 .Sample(TimeSpan.FromMilliseconds(20))
                 .ObserveOn(dispatcher)
                 .Subscribe(x => Frequency = x.EventArgs);
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
