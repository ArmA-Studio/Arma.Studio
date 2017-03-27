using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArmA.Studio.Data;
using ArmA.Studio.Data.Configuration;
using ArmA.Studio.Data.UI;

namespace ArmA.Studio.Plugin
{
    public interface IDocumentProviderPlugin : IPlugin
    {
        IEnumerable<FileType> FileTypes { get; }
        IEnumerable<DocumentBase.DocumentDescribor> Documents { get; }
        IEnumerable<DataTemplate> DocumentDataTemplates { get; }

        DocumentBase CreateDocument(FileType type);
        DocumentBase CreateDocument(DocumentBase.DocumentDescribor describor);
    }
}
