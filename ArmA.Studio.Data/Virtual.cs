using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmA.Studio.Data
{
    ///Contains calls to functions without direct access (Reflection only)
    ///In case this crashes, check function name and access modifier, access modifier has to be public, function name has to match the fncName string.
    ///Param1 = null -> static function
    ///Param2 array -> 1:1 representation of the actual properties, has to be an object array
    public static class Virtual
    {
        /// <summary>
        /// Virtual wrapper for <code>Application.Current.ShowOperationFailedMessageBox(Exception)</code>
        /// </summary>
        /// <param name="ex">Exception to display</param>
        public static void ShowOperationFailedMessageBox(Exception ex)
        {
            const string fncName = "ShowOperationFailedMessageBox";
            System.Diagnostics.Debug.Assert(System.Windows.Application.Current.GetType().GetMethod(fncName) != null);
            System.Diagnostics.Debug.Assert(System.Windows.Application.Current.GetType().GetMethod(fncName)?.ContainsGenericParameters == false);
            System.Diagnostics.Debug.Assert(System.Windows.Application.Current.GetType().GetMethod(fncName)?.GetParameters().Count() == 1);
            System.Diagnostics.Debug.Assert(System.Windows.Application.Current.GetType().GetMethod(fncName)?.GetParameters().ElementAtOrDefault(0)?.ParameterType.IsEquivalentTo(typeof(Exception))??false);
            System.Windows.Application.Current.GetType().GetMethod(fncName).Invoke(null, new[] { ex });
        }
    }
}
