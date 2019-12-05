using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace AlbionMarketPriceDiff.Util
{
    public class RelayCommand : ICommand
    {
        private Action execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private Action<T> execute;
        private Func<T, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(T parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(T parameter)
        {
            this.execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }

}
