using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.Debugging
{
    public interface IDebugger : IDisposable, INotifyPropertyChanged
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
        /// The current state of the debugger.
        /// Expected to announce its change via the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        EDebugState State { get; }

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
        SqfValue Evaluate(string text);

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
        /// <summary>
        /// Changes the variable with the <paramref name="variableName"/> to the provided
        /// <paramref name="data"/>. Data will be changed to whatever real data it is.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="data"></param>
        /// <param name="namespace"></param>
        /// <returns></returns>
        bool SetVariable(string variableName, string data, ENamespace @namespace);
        /// <summary>
        /// Returns the desired variable or NULL if no variable is available.
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="namespace"></param>
        /// <returns></returns>
        VariableInfo GetVariable(string variableName, ENamespace @namespace);

        /// <summary>
        /// Supposed to return the different <see cref="HaltInfo"/>s available at given time.
        /// </summary>
        /// <returns></returns>
        IEnumerable<HaltInfo> GetHaltInfos();

        /// <summary>
        /// Should return the currently deemed "local" variables at curren point of execution
        /// </summary>
        /// <returns></returns>
        IEnumerable<VariableInfo> GetLocalVariables();
    }
}
