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
    public struct IntelliSenseItem
    {
        public string ImageSource { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public readonly Action<TextDocument> Apply;

        public IntelliSenseItem(Action<TextDocument> apply, string name)
        {
            this.Apply = apply;
            this.ImageSource = @"/ArmA.Studio;component/Resources/Pictograms/Method/Method.ico";
            this.Name = name;
            this.Description = string.Empty;
        }

    }
}
