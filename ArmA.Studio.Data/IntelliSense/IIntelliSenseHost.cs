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
        /// Tells wether the <see cref="IIntelliSenseHost"/> is currently shown or not.
        /// </summary>
        bool IsDisplayed { get; }

        /// <summary>
        /// Function to fill/update the <see cref="IIntelliSenseHost"/>.
        /// </summary>
        /// <param name="document">Related <see cref="TextDocument"/>.</param>
        /// <param name="caret">Related <see cref="Caret"/>.</param>
        void Update(TextDocument document, Caret caret);
        /// <summary>
        /// Callen to display the <see cref="IIntelliSenseHost"/>.
        /// Might be callen with the <see cref="IIntelliSenseHost"/> already being displayed.
        /// </summary>
        /// <param name="editorInstance">The <see cref="TextEditor"/> instance this <see cref="IIntelliSenseHost"/> is displayed for.</param>
        /// <param name="pos">The <see cref="Point"/> where the text currently is located on screen relative to the <paramref name="editorInstance"/>.</param>
        void Display(TextEditor editorInstance, Point pos);
        /// <summary>
        /// Callen to hide the <see cref="IIntelliSenseHost"/>.
        /// Might be callen when the <see cref="IIntelliSenseHost"/> is not displayed.
        /// </summary>
        void Hide();
    }
}
