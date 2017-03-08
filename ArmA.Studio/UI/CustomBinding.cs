using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using System.Windows;

namespace ArmA.Studio.UI
{
    public class CustomBinding<TSource, TTarget> : IDisposable
                                   where TSource : INotifyPropertyChanged
                                   where TTarget : INotifyPropertyChanged
    {
        public enum EMode
        {
            TwoWay = 0,
            SourceToTarget,
            TargetToSource,
            OnceSourceToTarget
        }
        public PropertyInfo PropertySource { get; private set; }
        public PropertyInfo PropertyTarget { get; private set; }
        public TSource ValueSource { get; private set; }
        public TTarget ValueTarget { get; private set; }
        public EMode Mode { get; private set; }

        private bool IsCustomBindingSourceUpdate;
        private bool IsCustomBindingTargetUpdate;

        public CustomBinding(PropertyInfo propSource, TSource source, PropertyInfo propTarget, TTarget target, EMode mode)
        {
            this.IsCustomBindingSourceUpdate = false;
            this.IsCustomBindingTargetUpdate = false;
            this.PropertySource = propSource;
            this.PropertyTarget = propTarget;
            this.ValueSource = source;
            this.ValueTarget = target;
            this.Mode = mode;
            switch (this.Mode)
            {
                case EMode.OnceSourceToTarget:
                    this.UpdateTargetValueFromSource();
                    break;
                case EMode.SourceToTarget:
                    this.ValueSource.PropertyChanged += Source_PropertyChanged;
                    this.UpdateTargetValueFromSource();
                    break;
                case EMode.TargetToSource:
                    this.ValueTarget.PropertyChanged += Target_PropertyChanged;
                    this.UpdateSourceValueFromTarget();
                    break;
                case EMode.TwoWay:
                    this.ValueSource.PropertyChanged += Source_PropertyChanged;
                    this.ValueTarget.PropertyChanged += Target_PropertyChanged;
                    this.UpdateTargetValueFromSource();
                    break;
            }
        }
        public CustomBinding(PropertyInfo propSource, TSource source, PropertyInfo propTarget, TTarget target) : this(propSource, source, propTarget, target, EMode.TwoWay) { }

        public void UpdateTargetValueFromSource()
        {
            var mInfoGet = this.PropertySource.GetGetMethod(false);
            var value = mInfoGet?.Invoke(this.ValueSource, new object[0]);

            this.IsCustomBindingTargetUpdate = true;
            var mInfoSet = this.PropertyTarget.GetSetMethod(false);
            mInfoSet?.Invoke(this.ValueTarget, new object[] { Convert.ChangeType(value, this.PropertyTarget.PropertyType) });
            this.IsCustomBindingTargetUpdate = false;
        }
        public void UpdateSourceValueFromTarget()
        {
            var mInfoGet = this.PropertyTarget.GetGetMethod(false);
            var value = mInfoGet?.Invoke(this.ValueTarget, new object[0]);

            this.IsCustomBindingSourceUpdate = true;
            var mInfoSet = this.PropertySource.GetSetMethod(false);
            mInfoSet?.Invoke(this.ValueSource, new object[] { Convert.ChangeType(value, this.PropertySource.PropertyType) });
            this.IsCustomBindingSourceUpdate = false;
        }

        private void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.IsCustomBindingTargetUpdate)
                return;
            if (this.PropertyTarget.Name == e.PropertyName)
            {
                this.UpdateSourceValueFromTarget();
            }
        }

        private void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.IsCustomBindingSourceUpdate)
                return;
            if (this.PropertySource.Name == e.PropertyName)
            {
                this.UpdateTargetValueFromSource();
            }
        }

        public void Dispose()
        {
            switch (this.Mode)
            {
                case EMode.SourceToTarget:
                    this.ValueSource.PropertyChanged -= Source_PropertyChanged;
                    break;
                case EMode.TargetToSource:
                    this.ValueTarget.PropertyChanged -= Target_PropertyChanged;
                    break;
                case EMode.TwoWay:
                    this.ValueSource.PropertyChanged -= Source_PropertyChanged;
                    this.ValueTarget.PropertyChanged -= Target_PropertyChanged;
                    break;
            }
        }
    }
}
