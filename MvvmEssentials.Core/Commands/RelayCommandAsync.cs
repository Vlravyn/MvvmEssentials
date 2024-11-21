using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmEssentials.Core.Commands
{
    /// <summary>
    /// Basic implementation Async Relay Command.
    /// </summary>
    public class RelayCommandAsync : RelayCommandBaseAsync
    {
        private readonly Func<CancellationToken, Task> executeMethod;

        /// <summary>
        /// Creates an instance of the <see cref="RelayCommandAsync"/>
        /// </summary>
        /// <param name="executeMethod">the method to execute. Must have parameter <see cref="CancellationToken"/> and return a <see cref="Task"/></param>
        /// <param name="allowMultipleExecutions">
        /// Allows using the method multiple times before waiting to finish the existing commands. The class must hold a single instance instead of getting
        /// a new instance every time to use this property.
        /// </param>
        /// <param name="token"></param>
        public RelayCommandAsync(Func<CancellationToken, Task> executeMethod, bool allowMultipleExecutions = false, CancellationToken token = default)
            : this(executeMethod, () => true, allowMultipleExecutions, token)
        {
        }

        /// <summary>
        /// Creates an instance of the <see cref="RelayCommandAsync"/>
        /// </summary>
        /// <param name="executeMethod">the method to execute. Must have parameter <see cref="CancellationToken"/> and return a <see cref="Task"/></param>
        /// <param name="canExecuteMethod">the method which checks if the <paramref name="executeMethod"/> can be executed.</param>
        /// <param name="allowMultipleExecutions">
        /// Allows using the method multiple times before waiting to finish the existing commands. The class must hold a single instance instead of getting
        /// a new instance every time to use this property.
        /// </param>
        /// <param name="token">the cancellation token</param>
        public RelayCommandAsync(Func<CancellationToken, Task> executeMethod, Func<bool> canExecuteMethod, bool allowMultipleExecutions = false, CancellationToken token = default)
            : base(canExecuteMethod, allowMultipleExecutions, token)
        {
            this.executeMethod = executeMethod;
        }

        protected override async Task ExecuteAsync(object? parameters, CancellationToken token = default)
        {
            await executeMethod.Invoke(token);
        }
    }
}