using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArmA.Studio.Data
{
    public static class DragEventArgsExtensions
    {
        /// <summary>
        /// Shorthand for calling the <see cref="IDataObject.GetData(string)"/> from <see cref="DragEventArgs.Data"/>
        /// with the type of a <see cref="DragDropInfo"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static DragDropInfo GetInfo(this DragEventArgs e) => e.Data.GetData(typeof(DragDropInfo)) as DragDropInfo;
    }
}
