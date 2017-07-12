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
        private static bool CheckMethodParameters(this System.Reflection.MethodInfo m, params Type[] types)
        {
            var p = m.GetParameters();
            if (p.Length != types.Length)
                return false;
            for(int i = 0; i < p.Length; i++)
            {
                if (!p[i].ParameterType.IsEquivalentTo(types[i]))
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Virtual wrapper for <code>Application.Current.ShowOperationFailedMessageBox(Exception)</code>
        /// </summary>
        /// <param name="ex">Exception to display</param>
        public static void ShowOperationFailedMessageBox(Exception ex)
        {
            const string fncName = "ShowOperationFailedMessageBox";
            System.Windows.Application.Current.GetType().GetMethods().First((m) => m.Name.Equals(fncName) && m.CheckMethodParameters(typeof(Exception))).Invoke(null, new[] { ex });
        }
    }
}
