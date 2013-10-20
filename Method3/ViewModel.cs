using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Method3
{
    class ViewModel : INotifyPropertyChanged
    {
        readonly Model model;
        double progress;

        public ViewModel()
        {
            model = new Model();
            Refresh = new DelegateCommand(ExecuteRefresh);
        }

        public ICommand Refresh
        {
            get;
            private set;
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

        private void ExecuteRefresh(object obj)
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
