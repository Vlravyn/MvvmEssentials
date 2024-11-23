using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmEssentials.Core.Commands
{
    /// <summary>
    /// Asynchronous implementation of <see cref="RelayCommand{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of parameter to pass to the methods</typeparam>
    public class RelayCommandAsync<T> : RelayCommandBaseAsync
    {
        private readonly Func<T, CancellationToken, Task> executeMethod;

        /// <summary>
        /// Creates an instance of <see cref="RelayCommandAsync{T}"/>
        /// </summary>
        /// <param name="execute">the method to execute</param>
        /// <param name="allowMultipleExecutions">
        /// Allows using the method multiple times before waiting to finish the existing commands. The class must hold a single instance instead of getting
        /// a new instance every time to use this property.
        /// </param>
        /// <param name="token">the cancellation token</param>
        public RelayCommandAsync(Func<T, CancellationToken, Task> execute, bool allowMultipleExecutions = false, CancellationToken token = default)
            : this(execute, (_) => true, allowMultipleExecutions, token)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="RelayCommandAsync{T}"/>
        /// </summary>
        /// <param name="execute">the method to execute</param>
        /// <param name="canExecute">the method to check if the <paramref name="execute"/> should be executed</param>
        /// <param name="allowMultipleExecutions">
        /// Allows using the method multiple times before waiting to finish the existing commands. The class must hold a single instance instead of getting
        /// a new instance every time to use this property.
        /// </param>
        /// <param name="token">the cancellation token</param>
        public RelayCommandAsync(Func<T, CancellationToken, Task> execute, Func<T, bool> canExecute, bool allowMultipleExecutions = false, CancellationToken token = default)
            : base(canExecute, allowMultipleExecutions, token)
        {
            executeMethod = execute;
        }

        public override bool CanExecute(object? parameter)
        {
            if (TryGetCommandParameter(parameter, out T? commandParameter) is false)
                throw new ArgumentException("Invalid type passed in the RelayCommand");

            return base.CanExecute(commandParameter);
        }
 
        protected override async Task ExecuteAsync(object? parameter, CancellationToken token = default)
        {
            if (TryGetCommandParameter(parameter, out T? commandParameter) is false)
                throw new ArgumentException("Invalid type passed in the RelayCommand");

            if (parameter is T p)
                await executeMethod.Invoke(p, cancellationToken);
        }

        /// <summary>
        /// Tries to convert the parameter to it's original type.
        /// </summary>
        /// <param name="parameter">the parameter to convert</param>
        /// <param name="convertedParameter">the converted parameter that is sent back</param>
        /// <returns> <see langword="true"/> if the parameter was converted succesfully.</returns>
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