using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace ArmA.Studio.Data.IntelliSense
{
    public interface IIntelliSenseHost
    {
        /// <summary>
        /// Function to fill the IntelliSense panel.
        /// </summary>
        /// <param name="document">Related <see cref="TextDocument"/>.</param>
        /// <param name="caret">Related <see cref="Caret"/>.</param>
        IEnumerable<IntelliSenseItem> Generate(TextDocument document, Caret caret);
    }
}
