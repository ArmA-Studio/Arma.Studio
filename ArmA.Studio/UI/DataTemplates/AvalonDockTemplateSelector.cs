using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Markup;

namespace ArmA.Studio.UI.DataTemplates
{
    public class AvalonDockTemplateSelector : DataTemplateSelector
    {
        public static DataTemplate SolutionExplorerPane { get; private set; }
        public static DataTemplate PropertiesPane { get; private set; }
        public static DataTemplate DocumentBase { get; private set; }
        public static DataTemplate OutputPane { get; private set; }
        public static DataTemplate ErrorListPane { get; private set; }
        static AvalonDockTemplateSelector()
        {
            var ass = System.Reflection.Assembly.GetExecutingAssembly();
            var resNames = from name in ass.GetManifestResourceNames() where name.EndsWith(".xaml") select name;
            foreach (var res in resNames)
            {
                var resSplit = res.Split('.');
                var header = resSplit[resSplit.Count() - 2];
                DataTemplate template;
                using (var stream = ass.GetManifestResourceStream(res))
                {
                    try
                    {
                        var obj = XamlReader.Load(stream);
                        if (!(obj is DataTemplate))
                        {
                            continue;
                        }
                        template = obj as DataTemplate;
                    }
                    catch
                    {
                        continue;
                    }
                }
                switch (res)
                {
                    case "ArmA.Studio.UI.DataTemplates.SolutionExplorer.xaml":
                        SolutionExplorerPane = template;
                        break;
                    case "ArmA.Studio.UI.DataTemplates.Properties.xaml":
                        PropertiesPane = template;
                        break;
                    case "ArmA.Studio.UI.DataTemplates.DocumentBase.xaml":
                        DocumentBase = template;
                        break;
                    case "ArmA.Studio.UI.DataTemplates.Output.xaml":
                        OutputPane = template;
                        break;
                    case "ArmA.Studio.UI.DataTemplates.ErrorList.xaml":
                        ErrorListPane = template;
                        break;
                }
            }
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SolutionUtil.Solution)
                return SolutionExplorerPane;
            if (item is DataContext.PropertiesPane)
                return PropertiesPane;
            if (item is DocumentBase)
                return DocumentBase;
            if (item is DataContext.OutputPane)
                return OutputPane;
            if (item is DataContext.ErrorListPane)
                return ErrorListPane;

            return base.SelectTemplate(item, container);
        }
    }
}
