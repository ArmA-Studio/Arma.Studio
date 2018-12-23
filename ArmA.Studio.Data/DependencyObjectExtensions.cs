using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArmA.Studio.Data
{
    public static class DependencyObjectExtensions
    {
        public static DependencyObject FindParent<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            while (dependencyObject != null && !(dependencyObject is T))
            {
                dependencyObject = LogicalTreeHelper.GetParent(dependencyObject);
            }
            return dependencyObject;
        }
    }
}
