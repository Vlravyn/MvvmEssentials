using System.Windows.Input;

namespace MvvmEssentials.Core.Commands
{
    public abstract class RelayCommandBaseAsync : ObservableObject, ICommand
    {
        private bool _isExecuting;
        private bool _allowMultipleExecutions;
        protected object canExecuteMethod;
        protected CancellationToken cancellationToken = default;

        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Specifies if the method is allowed to run multiple times before finishing the executing tasks.
        /// Use a single relay command instance to make use of this property. By default the command only allows one execution at a time.
        /// </summary>
        public bool AllowMultipleExecutions
        {
            get => _allowMultipleExecutions;
            set => SetProperty(ref _allowMultipleExecutions, value);
        }

        /// <summary>
        /// Specifies if the method is executing
        /// </summary>
        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                SetProperty(ref _isExecuting, value);
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected RelayCommandBaseAsync(object canExecuteMethod, bool allowMultipleExecutions = false, CancellationToken token = default)
        {
            this.canExecuteMethod = canExecuteMethod;
            AllowMultipleExecutions = allowMultipleExecutions;
            cancellationToken = token;
            IsExecuting = false;
        }

        /// <summary>
        /// The method which performs the task.
        /// Put the Command logic inside this method for your custom async relay command.
        /// </summary>
        /// <param name="token">the cancellation token</param>
        protected abstract Task ExecuteAsync(object? parameter, CancellationToken token = default);

        public virtual bool CanExecute(object? parameter)
        {
            bool? canExecuteResult = false;
            if (canExecuteMethod is Delegate d)
            {
                if (d.Method.GetParameters().Length != 0)
                    canExecuteResult = (bool?)d.DynamicInvoke(parameter);
                else
                    canExecuteResult = (bool?)d.DynamicInvoke();
            }
            if (canExecuteResult != null && (bool)canExecuteResult)
            {
                if (AllowMultipleExecutions)
                    return true;

                //execute if it not executing and vice versa
                return !IsExecuting;
            }

            return false;
        }

        public async void Execute(object? parameter)
        {
            IsExecuting = true;
            await ExecuteAsync(parameter, cancellationToken);
            IsExecuting = false;
        }
    }
}