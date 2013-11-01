using MvvmHighFrequency;
using MvvmHighFrequency.Common;
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
    class ViewModel : ViewModelBase
    {
        readonly Model model;
        readonly Dispatcher dispatcher;

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
    }
}
