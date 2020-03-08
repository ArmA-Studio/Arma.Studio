using Arma.Studio.Data;
using Arma.Studio.Data.UI;
using Arma.Studio.PropertiesWindow.PropertyContainers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Arma.Studio.PropertiesWindow
{
    public sealed class PropertiesWindowDataContext : DockableBase, IDisposable
    {
        public ObservableCollection<PropertyContainerBase> Properties { get; }
        public override string Title { get => PropertiesWindow.Properties.Language.PropertiesWindow; set => throw new NotSupportedException(); }
        public PropertiesWindowDataContext()
        {
            this.Properties = new ObservableCollection<PropertyContainerBase>();
            this.Properties.CollectionChanged += this.Properties_CollectionChanged;
            this.DisplayProperties(((Application.Current as IApp).MainWindow as IMainWindow).PropertyHost);
            (Application.Current as IApp).MainWindow.PropertyChanged += this.MainWindow_PropertyChanged;
        }

        private void Properties_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (PropertyContainerBase it in e.OldItems ?? Array.Empty<PropertyContainerBase>())
            {
                it.Dispose();
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

        public void DisplayProperties(IPropertyHost propertyHost)
        {
            if (propertyHost is null)
            {
                this.Properties.Clear();
                Application.Current.Dispatcher.Invoke(() => this.Properties.Clear());
            }
            else
            {
                var type = propertyHost.GetType();
                var propertyInfos = type.GetProperties();
                var list = new List<PropertyContainerBase>();
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
                    if (propertyInfo.PropertyType.IsEquivalentTo(typeof(byte))) { list.Add(PropertyContainerByte.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(sbyte))) { list.Add(PropertyContainerSByte.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(int))) { list.Add(PropertyContainerInt32.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(uint))) { list.Add(PropertyContainerUInt32.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(short))) { list.Add(PropertyContainerInt16.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(ushort))) { list.Add(PropertyContainerUInt16.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(long))) { list.Add(PropertyContainerInt64.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(ulong))) { list.Add(PropertyContainerUInt64.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(float))) { list.Add(PropertyContainerSingle.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(double))) { list.Add(PropertyContainerDouble.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(char))) { list.Add(PropertyContainerChar.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(bool))) { list.Add(PropertyContainerBoolean.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(string))) { list.Add(PropertyContainerString.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(decimal))) { list.Add(PropertyContainerDecimal.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEquivalentTo(typeof(System.Windows.Media.Color))) { list.Add(PropertyContainerColor.Create(propertyHost, propertyInfo)); }
                    else if (propertyInfo.PropertyType.IsEnum) { list.Add(PropertyContainerEnum.Create(propertyHost, propertyInfo)); }
                    else
                    {
                        // Unknown Datatype
                        continue;
                    }
                }
                Application.Current.Dispatcher.Invoke(() => list.ForEach((it) => this.Properties.Add(it)));
            }
        }
    }
}
