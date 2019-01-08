using Arma.Studio.Data;
using Arma.Studio.Data.TextEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Arma.Studio.SqfEditor
{
    public class PluginMain : IPlugin, ITextEditor
    {
        #region IPlugin
        public Version Version => new Version(1, 0, 0);
        public string Name => Properties.Language.SqfEditor_Name;
        public Task<IUpdateInfo> CheckForUpdate(CancellationToken cancellationToken) => Task.Run(() => default(IUpdateInfo));
        public Task Initialize(CancellationToken cancellationToken) => Task.CompletedTask;
        #endregion
        //asdasd
        /*asdasd*/
        #region ITextEditor
        public SyntaxFile SyntaxFile => new SyntaxFile(true, @"&><~!@$%^*()-+=|\#/{}[]:;""' , .?")
        {
            DigitsColor = Color.FromRgb(0xB5, 0xCE, 0xA8),
            Enclosures = 
            {
                new Enclosure(Color.FromRgb(0xD6, 0x9D, 0x85), "\"", "\""),
                new Enclosure(Color.FromRgb(0xD6, 0x9D, 0x85), "'", "'"),
                new Enclosure(Color.FromRgb(0x9B, 0x9B, 0x9B), "#"),
                new Enclosure(Color.FromRgb(0x57, 0xA6, 0x3A), "//"),
                new Enclosure(Color.FromRgb(0x57, 0xA6, 0x3A), "/*", "*/")
            },
            Keywords =
            {
                // ToDo: Add a way to get keywords
            }
        };
        public bool ShowLineNumbers => true;
        #endregion
    }
}
