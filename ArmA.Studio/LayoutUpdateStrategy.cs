using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace ArmA.Studio
{
    class LayoutUpdateStrategy : ILayoutUpdateStrategy
    {
        private bool BeforeInsert_(LayoutRoot layout, LayoutContent anchorableToShow)
        {
            var dockable = (DockableBase)anchorableToShow.Content;
            if (string.IsNullOrWhiteSpace(dockable.ContentId))
            {
                dockable.ContentId = Guid.NewGuid().ToString();
            }
            var layoutContent = layout.Descendents().OfType<LayoutContent>().FirstOrDefault(it =>
            {
                return it.ContentId == dockable.ContentId;
            });
            if (layoutContent == null)
            {
                layoutContent = layout.Hidden.FirstOrDefault(it => it.ContentId == dockable.ContentId);
                if(layoutContent == null)
                    return false;
            }
            layoutContent.Content = anchorableToShow.Content;


            var layoutContainer = layoutContent.GetType().GetProperty("PreviousContainer", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(layoutContent, null) as ILayoutContainer;
            if (layoutContainer == null)
            {
                return true;
            }
            else if (layoutContainer is LayoutAnchorablePane)
            {
                (layoutContainer as LayoutAnchorablePane).Children.Add(layoutContent as LayoutAnchorable);
                return true;
            }
            else if (layoutContainer is LayoutDocumentPane)
            {
                (layoutContainer as LayoutDocumentPane).Children.Add(layoutContent);
                return true;
            }
            else if(layoutContainer is LayoutAnchorGroup)
            {
                (layoutContainer as LayoutAnchorGroup).Children.Add(layoutContent as LayoutAnchorable);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {

        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {
            
        }

        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            ConfigHost.Instance.Save(ConfigHost.EIniSelector.Layout);
            return this.BeforeInsert_(layout, anchorableToShow);
        }

        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;//this.BeforeInsert_(layout, anchorableToShow);
        }
    }
}
