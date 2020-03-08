using Arma.Studio.Data;
using Arma.Studio.Data.UI;
using Arma.Studio.PropertiesWindow.PropertyContainers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Arma.Studio.PropertiesWindow
{
    public sealed class PropertiesWindowDataContext : DockableBase, IDisposable
    {
        public ObservableCollection<PropertyContainerGroup> Properties { get; }
        public override string Title { get => PropertiesWindow.Properties.Language.PropertiesWindow; set => throw new NotSupportedException(); }
        public PropertiesWindowDataContext()
        {
            this.Properties = new ObservableCollection<PropertyContainerGroup>();
            this.Properties.CollectionChanged += this.Properties_CollectionChanged;
            this.DisplayProperties(((Application.Current as IApp).MainWindow as IMainWindow).PropertyHost);
            (Application.Current as IApp).MainWindow.PropertyChanged += this.MainWindow_PropertyChanged;
        }

        private void Properties_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (PropertyContainerGroup it in e.OldItems ?? Array.Empty<PropertyContainerGroup>())
            {
                foreach (var property in it.Properties)
                {
                    property.Dispose();
                }
            }
        }

        private void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IMainWindow.PropertyHost))
            {
                this.DisplayProperties((sender as IMainWindow).PropertyHost);
            }
        }

        public void Dispose()
        {
            (Application.Current as IApp).MainWindow.PropertyChanged -= this.MainWindow_PropertyChanged;
            this.Properties.Clear();
            this.Properties.CollectionChanged -= this.Properties_CollectionChanged;
        }

        public static readonly Dictionary<string, bool> IsExpandedDictionary = new Dictionary<string, bool>();

        public void DisplayProperties(IPropertyHost propertyHost)
        {
            if (propertyHost is null)
            {
                var groups = Application.Current.Dispatcher.Invoke(() => this.Properties.ToArray());
                foreach (var it in groups)
                {
                    IsExpandedDictionary[it.Title] = it.IsExpanded;
                }
                Application.Current.Dispatcher.Invoke(() => this.Properties.Clear());
            }
            else
            {
                var type = propertyHost.GetType();
                var propertyInfos = type.GetProperties();
                var list = new List<Tuple<string, PropertyContainerBase>>();
                foreach (var propertyInfo in propertyInfos)
                {
                    var customAttributes = propertyInfo.GetCustomAttributes(typeof(PropertyAttribute), true);
                    if (customAttributes is null || customAttributes.Length == 0)
                    {
                        continue;
                    }
                    var propertyAttribute = (PropertyAttribute)customAttributes[0];
                    if (!propertyAttribute.Display)
                    {
                        continue;
                    }
                    if (propertyInfo.PropertyType.IsEquivalentTo(typeof(byte)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerByte.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(sbyte)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerSByte.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(int)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerInt32.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(uint)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerUInt32.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(short)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerInt16.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(ushort)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerUInt16.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(long)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerInt64.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(ulong)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerUInt64.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(float)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerSingle.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(double)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerDouble.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(char)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerChar.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(bool)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerBoolean.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(string)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerString.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(decimal)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerDecimal.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(System.Windows.Media.Color)))
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerColor.Create(propertyHost, propertyInfo))); }
                    else if (propertyInfo.PropertyType.IsEnum)
                    { list.Add(new Tuple<string, PropertyContainerBase>(propertyAttribute.Group, PropertyContainerEnum.Create(propertyHost, propertyInfo))); }
                    else
                    {
                        // Unknown Datatype
                        continue;
                    }
                }
                var groups = Application.Current.Dispatcher.Invoke(() => this.Properties.ToArray());
                foreach (var it in groups)
                {
                    IsExpandedDictionary[it.Title] = it.IsExpanded;
                }
                Application.Current.Dispatcher.Invoke(() => this.Properties.Clear());
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var items in list.GroupBy((it) => it.Item1).OrderBy((it) => it.Key))
                    {
                        var group = new PropertyContainerGroup(items.Key ?? PropertiesWindow.Properties.Language.Generic, items.Select((it) => it.Item2).OrderBy((it) => it.Title));
                        if (IsExpandedDictionary.TryGetValue(group.Title, out var flag))
                        {
                            group.IsExpanded = flag;
                        }
                        this.Properties.Add(group);
                    }
                });
            }
        }
    }
}
