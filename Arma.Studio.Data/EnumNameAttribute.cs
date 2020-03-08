using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumNameAttribute : Attribute
    {
        /// <summary>
        /// An always valid, fallback name that should be used
        /// in the cases where the <see cref="ResourceName"/> is
        /// either not existing (<see cref="string.IsNullOrWhiteSpace(string)"/>) or
        /// was not found.
        /// </summary>
        public string FallbackName { get; private set; }
        /// <summary>
        /// The resource name from the resource dictionary.
        /// </summary>
        public string ResourceName { get; set; }

        public EnumNameAttribute(string fallback)
        {
            this.FallbackName = fallback;
        }
    }
}
