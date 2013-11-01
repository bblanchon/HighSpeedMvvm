using System.ComponentModel;

namespace HighSpeedMvvm.Common
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        double progress, frequency;

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
