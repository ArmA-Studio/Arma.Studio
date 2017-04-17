using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace ArmA.Studio.Data.IntelliSense
{
    public static class ExtensionMethod
    {
        /// <summary>
        /// Function to fill the IntelliSense panel.
        /// </summary>
        /// <param name="document">Related <see cref="TextDocument"/>.</param>
        /// <param name="caret">Related <see cref="Caret"/>.</param>
        public static async Task<IEnumerable<IntelliSenseItem>> GenerateAsync(this IIntelliSenseHost _this, TextDocument document, Caret caret) => await Task.Run(() => _this.Generate(document, caret));
    }
}
