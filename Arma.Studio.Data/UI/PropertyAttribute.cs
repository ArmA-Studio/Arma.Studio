using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.UI
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyAttribute : Attribute
    {
        /// <summary>
        /// The display title of this property.
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// The description of this property.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Allows to group properties together.
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// If True, the property may be displayed.
        /// If False, it should not be displayed.
        /// Defaults to True.
        /// </summary>
        public bool Display { get; set; }
        /// <summary>
        /// The Default value the property is supposed to have.
        /// </summary>
        public object Default { get; set; }

        /// <summary>
        /// UI-Configuration.
        /// The Minimum value the property is supposed to have.
        /// </summary>
        public double MinValue { get; set; }
        /// <summary>
        /// UI-Configuration.
        /// The Maximum value the property is supposed to have.
        /// </summary>
        public double MaxValue { get; set; }
        /// <summary>
        /// UI-Configuration.
        /// Stepsize for numeric up-down.
        /// </summary>
        public double Stepsize { get; set; }

        public PropertyAttribute(string title)
        {
            this.Title = title;
            this.Display = true;
            this.Description = string.Empty;
            this.Group = null;
            this.Default = null;
            this.MinValue = double.MinValue;
            this.MaxValue = double.MaxValue;
            this.Stepsize = 1;
        }
    }
}
