using System.Windows.Input;

namespace MvvmEssentials.Core.Commands
{
    /// <summary>
    /// An implementation of <see cref="ICommand"/> that can take in a parameter
    /// </summary>
    /// <typeparam name="T">the <see cref="Type"/> of parameter</typeparam>
    public class RelayCommand<T> : ICommand
        where T : Type
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action<T?> executeMethod;
        private readonly Func<T?, bool> canExecuteMethod;

        /// <summary>
        /// Creates an new instance of <see cref="RelayCommand{T}"/>
        /// </summary>
        /// <param name="execute">the method to execute</param>
        public RelayCommand(Action<T?> execute)
            : this(execute, (T) => true)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="RelayCommand{T}"/>
        /// </summary>
        /// <param name="execute">the method to execute</param>
        /// <param name="canExecute">the method that checks if the method should be executed.</param>
        public RelayCommand(Action<T?> execute, Func<T?, bool> canExecute)
        {
            executeMethod = execute;
            canExecuteMethod = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (TryGetCommandParameter(parameter, out T? commandParameter) is false)
                throw new ArgumentException("Invalid type passed in the RelayCommand");

            return canExecuteMethod.Invoke(commandParameter);
        }

        public void Execute(object? parameter)
        {
            if (TryGetCommandParameter(parameter, out T? commandParameter) is false)
                throw new ArgumentException("Invalid type passed in the RelayCommand");

            executeMethod?.Invoke(commandParameter);
        }

        /// <summary>
        /// Tries to convert the parameter to it's original type.
        /// </summary>
        /// <param name="parameter">the parameter to convert</param>
        /// <param name="convertedParameter">the converted parameter that is sent back</param>
        /// <returns> <see langword="true"/> if the parameter was converted successfully.</returns>
        private static bool TryGetCommandParameter(object? parameter, out T? convertedParameter)
        {
            convertedParameter = default;

            if (parameter is null && default(T) is null)
                return true;

            if (parameter is T argument)
            {
                convertedParameter = argument;
                return true;
            }

            convertedParameter = default;
            return false;
        }
    }
}