using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Method3
{
    class DelegateCommand : ICommand
    {
        readonly Action<object> executeDelegate;
        readonly Func<object, bool> canExecuteDelegate;

        public DelegateCommand(Action<object> executeDelegate, Func<object, bool> canExecuteDelegate=null)
        {
            if (executeDelegate == null)
                throw new ArgumentNullException("executeDelegate");

            this.executeDelegate = executeDelegate;
            this.canExecuteDelegate = canExecuteDelegate;
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteDelegate == null || canExecuteDelegate(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            executeDelegate(parameter);
        }
    }
}
