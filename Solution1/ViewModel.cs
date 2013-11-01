using System;
using System.Reactive.Linq;
using System.Windows.Threading;
using MvvmHighFrequency.Common;

namespace MvvmHighFrequency.Solution1
{
    class ViewModel : ViewModelBase
    {
    public ViewModel()
    {
        var dispatcher = Dispatcher.CurrentDispatcher;
        var model = new Model();

        Observable.FromEventPattern<double>(model, "ProgressChanged")
                .Sample(TimeSpan.FromMilliseconds(5))
                .ObserveOn(dispatcher)
                .Subscribe(x => Progress = x.EventArgs);

        Observable.FromEventPattern<double>(model, "FrequencyChanged")
                .Sample(TimeSpan.FromMilliseconds(5))
                .ObserveOn(dispatcher)
                .Subscribe(x => Frequency = x.EventArgs);
    }
    }
}
