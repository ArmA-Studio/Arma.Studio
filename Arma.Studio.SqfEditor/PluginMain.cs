using Arma.Studio.Data;
using Arma.Studio.Data.Dockable;
using Arma.Studio.Data.TextEditor;
using Arma.Studio.Data.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Arma.Studio.SqfEditor
{
    public class PluginMain : IPlugin, ITextEditorProvider
    {
        public const string SqfDefinitionsFileName = "sqf_definitions.xml";

        public static SqfDefinitionsFile SqfDefinitionsFile = null;

        #region IPlugin
        public Version Version => new Version(1, 0, 0);
        public string Name => Properties.Language.SqfEditor_Name;
        public Task<IUpdateInfo> CheckForUpdate(CancellationToken cancellationToken) => Task.Run(() => default(IUpdateInfo));
        public async Task Initialize(string pluginPath, CancellationToken cancellationToken)
        {
            var sqfdefinitions = System.IO.Path.Combine(pluginPath, SqfDefinitionsFileName);
            if (System.IO.File.Exists(sqfdefinitions))
            {
                using (var file = new System.IO.StreamReader(sqfdefinitions))
                {
                    var serializer = new XmlSerializer(typeof(SqfDefinitionsFile));
                    SqfDefinitionsFile = await Task.Run(() => serializer.Deserialize(file) as SqfDefinitionsFile);
                }
            }
            else
            {
                SqfDefinitionsFile = new SqfDefinitionsFile()
                {
                    Binaries =
                    {
                        new SqfDefinitionsFile.Binary { Name = "select", Left = "scalar", Right = "scalar" }
                    },
                    Unaries =
                    {
                        new SqfDefinitionsFile.Unary { Name = "floor", Right = "scalar" }
                    },
                    Nulars =
                    {
                        new SqfDefinitionsFile.Nular { Name = "player" }
                    },
                    Groups =
                    {
                        new SqfDefinitionsFile.Group() { Name = "group", Blue = 0, Green = 127, Red = 255, IsBold = false }
                    }
                };
                using (var file = new System.IO.StreamWriter(sqfdefinitions))
                {
                    var serializer = new XmlSerializer(typeof(SqfDefinitionsFile));
                    await Task.Run(() => serializer.Serialize(file, SqfDefinitionsFile));
                }
            }
        }
        #endregion
        #region ITextEditorProvider
        public IEnumerable<TextEditorInfo> TextEditorInfos => new TextEditorInfo[] {
            TextEditorInfo.Create(Properties.Language.SqfEditor_Document, () => new SqfEditor())
        };
        #endregion
    }
}
