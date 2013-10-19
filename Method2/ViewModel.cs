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
    class ViewModel : INotifyPropertyChanged
    {
        readonly Model model;
        double progress;

        public ViewModel()
        {
            model = new Model();

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(5);
            timer.Tick += OnTimerTick;
            timer.Start();
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

        void OnTimerTick(object sender, EventArgs e)
        {
            Progress = model.Progress;
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
