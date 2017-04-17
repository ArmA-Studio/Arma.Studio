using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ArmA.Studio.Data;
using ArmA.Studio.Data.Lint;
using ArmA.Studio.Data.UI;
using ArmA.Studio.Plugin;

namespace ArmA.Studio.DefaultPlugin
{
    internal sealed class PluginMain : IDocumentProviderPlugin, IHotKeyPlugin
    {
        public string Description => Properties.Localization.DefaultPluginDescription;
        public string Name => "ArmA.Studio";

        public static readonly FileType SqfFileType = new FileType((ext) => ext.Equals(".sqf", StringComparison.InvariantCultureIgnoreCase), "SQF", ".sqf") { Linter = new SqfLintHelper(), FileTemplate = @"/*
 * @Author: 
 * 
 * @Description: 
 * 
 * @Arguments: -/-
 * @Return: -/-
 */
params [];" };
        public static readonly DocumentBase.DocumentDescribor SqfDocumentDescribor = new DocumentBase.DocumentDescribor(new[] { SqfFileType }, "SQF");

        public static readonly FileType ConfigFileType = new FileType((ext) => ext.Equals(".cpp", StringComparison.InvariantCultureIgnoreCase) || ext.Equals(".ext", StringComparison.InvariantCultureIgnoreCase), "Config", ".cpp") { Linter = new ConfigLintHelper(), FileTemplate = @"/*
 * @Author: 
 * 
 * @Purpose: 
 *
 */" };
        public static readonly FileType ConfigCppFileType = new FileType((ext) => ext.Equals(".cpp", StringComparison.InvariantCultureIgnoreCase) || ext.Equals(".ext", StringComparison.InvariantCultureIgnoreCase), "Config.cpp", ".cpp") { Linter = new ConfigLintHelper(), StaticFileName = "config.cpp", FileTemplate = @"class CfgPatches
{
    class TAG_ModName
    {
        units[] = {};
        weapons[] = {};
        requiredVersion = 1.68;
        requiredAddons[] = {};
        author = "";
        mail = "";
        url = "";
    };
};" };
        public static readonly FileType DescriptionExtFileType = new FileType((ext) => ext.Equals(".cpp", StringComparison.InvariantCultureIgnoreCase) || ext.Equals(".ext", StringComparison.InvariantCultureIgnoreCase), "description.ext", ".ext") { Linter = new ConfigLintHelper(), StaticFileName = "description.ext" };
        public static readonly DocumentBase.DocumentDescribor ConfigDocumentDescribor = new DocumentBase.DocumentDescribor(new[] { ConfigFileType }, "Config");

        public IEnumerable<DataTemplate> DocumentDataTemplates => new[] { TextEditorBaseDataContext.TextEditorBaseDataTemplate };
        public IEnumerable<DocumentBase.DocumentDescribor> Documents => new[] { SqfDocumentDescribor, ConfigDocumentDescribor };
        public IEnumerable<FileType> FileTypes => new[] { SqfFileType, ConfigFileType, ConfigCppFileType, DescriptionExtFileType };

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
            else if (type == ConfigFileType || type == ConfigCppFileType || type == DescriptionExtFileType)
            {
                return new ConfigDocument();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<KeyContainer> GetGlobalHotKeys()
        {
            yield return new KeyContainer(Properties.Localization.Hotkey_SaveCurrentDocument, new Key[] { Key.LeftCtrl, Key.S }, (p) => Workspace.Instance.CmdSave.Execute(null));
        }
    }
}
