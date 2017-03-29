using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ArmA.Studio.Data.UI.Commands;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;

namespace ArmA.Studio.Data.UI
{
    public abstract class CodeEditorBaseDataContext : TextEditorBaseDataContext
    {
        public CodeEditorBaseDataContext(ProjectFileFolder fileRef) : base(fileRef)
        {
        }
    }
}
