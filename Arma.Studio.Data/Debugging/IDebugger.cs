using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Debugging
{
    public interface IDebugger : IDisposable
    {
        /// <summary>
        /// Supposed to return all actions that are supported by <see cref="Execute(EDebugAction)"/>.
        /// </summary>
        IEnumerable<EDebugAction> SupportedActions { get; }

        /// <summary>
        /// Called when the user is clicking a button.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>Awaitable <see cref="Task"/> or <see cref="Task.CompletedTask"/>.</returns>
        Task Execute(EDebugAction action);

        /// <summary>
        /// Called whenever a user sets a breakpoint.
        /// To detect changes, use the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <remarks>
        /// When the Debugger instance was created, the current existing breakpoints will be send
        /// via this method.
        /// </remarks>
        /// <param name="breakpoint">The breakpoint that got set.</param>
        /// <returns>Awaitable <see cref="Task"/> or <see cref="Task.CompletedTask"/>.</returns>
        Task SetBreakpoint(IBreakpoint breakpoint);
        /// <summary>
        /// Called when a user removes a breakpoint.
        /// If you subscribed to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event,
        /// then you should unsubscribe here.
        /// </summary>
        /// <remarks>
        /// When the Debugger gets detached, this is not callen.
        /// </remarks>
        /// <param name="breakpoint">The breakpoint that got set.</param>
        /// <returns>Awaitable <see cref="Task"/> or <see cref="Task.CompletedTask"/>.</returns>
        Task RemoveBreakpoint(IBreakpoint breakpoint);
    }
}
