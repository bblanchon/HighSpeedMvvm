using MvvmHighFrequency;
using MvvmHighFrequency.Common;
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
    class ViewModel : ViewModelBase, IRefresh
    {
        readonly Model model;

        public ViewModel()
        {
            model = new Model();
        }               

        #region IRefresh

        public void Refresh()
        {
            Progress = model.Progress;
            Frequency = model.Frequency;
        }

        #endregion
    }
}
