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
        double progress;

        public ViewModel()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            model = new Model();

            model.ProgressChanged += OnModelProgressChanged;
        }

        public double Progress
        {
            get { return progress; }
            set
            {
                if( progress == value ) return;
                progress = value;
                RaisePropertyChanged("Progress");
            }
        }

        void OnModelProgressChanged(double newValue)
        {
            dispatcher.BeginInvoke((Action)delegate() { Progress = newValue; });
        }

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
