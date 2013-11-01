using MvvmHighFrequency.Common;

namespace MvvmHighFrequency.Solution3
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
