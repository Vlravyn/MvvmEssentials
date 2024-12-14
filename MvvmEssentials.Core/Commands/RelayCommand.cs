using System;
using System.Windows.Input;

namespace MvvmEssentials.Core.Commands
{
    /// <summary>
    /// Implementation of the <see cref="ICommand"/> interface
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        private readonly Action executeMethod;
        private readonly Func<bool> canExecuteMethod;

        /// <summary>
        /// Creates a new instance of RelayCommand
        /// </summary>
        /// <param name="method">the method to execute</param>
        public RelayCommand(Action method) : this(method, () => true)
        {
        }

        /// <summary>
        /// Creates a new instance of RelayCommand
        /// </summary>
        /// <param name="execute">the method to execute</param>
        /// <param name="canExecute">the method that decides whether to execute the method or not.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            executeMethod = execute;
            canExecuteMethod = canExecute;
        }

        /// <summary>
        /// Determines whether this command can be executed.
        /// </summary>
        /// <returns><see langword="true"/> if the command can be executed.</returns>
        public bool CanExecute(object? parameter)
        {
            return canExecuteMethod.Invoke();
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        public void Execute(object? parameter)
        {
            executeMethod.Invoke();
        }
    }
}