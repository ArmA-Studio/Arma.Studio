using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArmA.Studio.Data;
using ArmA.Studio.Data.Lint;
using ArmA.Studio.Data.UI;
using ArmA.Studio.Plugin;

namespace ArmA.Studio.DefaultPlugin
{
    internal sealed class PluginMain : IDocumentProviderPlugin
    {
        public string Description => Properties.Localization.DefaultPluginDescription;
        public string Name => "ArmA.Studio";

        public static readonly FileType SqfFileType = new FileType((ext) => ext.Equals(".sqf", StringComparison.InvariantCultureIgnoreCase), "SQF", ".sqf") { Linter = new SqfLintHelper() };
        public static readonly DocumentBase.DocumentDescribor SqfDocumentDescribor = new DocumentBase.DocumentDescribor(new[] { SqfFileType }, "SQF");

        public static readonly FileType ConfigFileType = new FileType((ext) => ext.Equals(".cpp", StringComparison.InvariantCultureIgnoreCase) || ext.Equals(".ext", StringComparison.InvariantCultureIgnoreCase), "Config", ".cpp") { Linter = new ConfigLintHelper() };
        public static readonly DocumentBase.DocumentDescribor ConfigDocumentDescribor = new DocumentBase.DocumentDescribor(new[] { ConfigFileType }, "Config");

        public IEnumerable<DataTemplate> DocumentDataTemplates => new[] { TextEditorBaseDataContext.TextEditorBaseDataTemplate };
        public IEnumerable<DocumentBase.DocumentDescribor> Documents => new[] { SqfDocumentDescribor, ConfigDocumentDescribor };
        public IEnumerable<FileType> FileTypes => new[] { SqfFileType, ConfigFileType };

        public DocumentBase CreateDocument(DocumentBase.DocumentDescribor describor)
        {
            if (describor == SqfDocumentDescribor)
            {
                return new SqfDocument();
            }
            else if (describor == ConfigDocumentDescribor)
            {
                return new ConfigDocument();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public DocumentBase CreateDocument(FileType type)
        {
            if (type == SqfFileType)
            {
                return new SqfDocument();
            }
            else if (type == ConfigFileType)
            {
                return new ConfigDocument();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
