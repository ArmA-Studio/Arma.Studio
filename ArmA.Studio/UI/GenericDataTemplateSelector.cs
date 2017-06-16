using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace ArmA.Studio.UI
{
    public class GenericDataTemplateSelector : DataTemplateSelector
    {
        public IEnumerable<DataTemplate> Templates => this._Templates;
        private List<DataTemplate> _Templates;

        public GenericDataTemplateSelector()
        {
            this._Templates = new List<DataTemplate>();
        }
        public GenericDataTemplateSelector(Assembly useAssembly, Func<string, bool> cond = null)
        {
            this._Templates = new List<DataTemplate>();
            this.AddAllDataTemplatesInAssembly(useAssembly, cond);
        }

        private static bool FindAllDataTemplatesInAssembly_defaultCondition(string s) { return true; }

        public void AddDataTemplate(DataTemplate dt)
        {
            if (this._Templates.Contains(dt))
                return;
            dt.Seal();
            this._Templates.Add(dt);
        }

        public void AddAllDataTemplatesInAssembly(Assembly ass, Func<string, bool> cond = null)
        {
            var resNames = from name in ass.GetManifestResourceNames() where name.EndsWith(".xaml") select name;
            foreach (var res in resNames)
            {
                var resSplit = res.Split('.');
                var header = resSplit[resSplit.Count() - 2];
                DataTemplate template;
                if (cond != null && !cond.Invoke(res))
                {
                    continue;
                }
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
                if (template.DataType is Type)
                    this.AddDataTemplate(template);
            }
        }

        private static IEnumerable<Type> GetBaseTypes(Type t)
        {
            var objectType = typeof(object);
            do
            {
                yield return t;
                t = t.BaseType;
            } while (!t.IsEquivalentTo(objectType));
        }
        private static int ParentRank(Type target, Type derived)
        {
            var derivedTypes = GetBaseTypes(derived).Concat(derived.GetInterfaces());
            var count = 0;

            foreach(var t in derivedTypes)
            {
                if(t.IsEquivalentTo(target))
                {
                    return count;
                }
                count++;
            }
            return -1;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return base.SelectTemplate(item, container);
            var itemType = item.GetType();
            var dtCollection = this._Templates.FindAll((d) => ((Type)d.DataType).IsAssignableFrom(itemType));
            DataTemplate dt = dtCollection.FirstOrDefault();
            var dtRank = int.MaxValue;
            foreach (var it in dtCollection)
            {
                var parentRank = ParentRank((Type)it.DataType, itemType);
                if (parentRank == -1)
                    continue;
                if (parentRank == 0)
                    return dt;
                if (parentRank < dtRank)
                {
                    dt = it;
                    dtRank = parentRank;
                }
            }
            if (dt != null || dtRank == -1)
                return dt;
            return base.SelectTemplate(item, container);
        }
    }
}
