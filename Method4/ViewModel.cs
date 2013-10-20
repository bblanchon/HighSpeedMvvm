using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Method4
{
    class ViewModel : INotifyPropertyChanged, IRefresh
    {
        readonly Model model;
        double progress;

        public ViewModel()
        {
            model = new Model();
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

        #region IRefresh

        public void Refresh()
        {
            Progress = model.Progress;
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
