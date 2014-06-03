using System.Windows;
using System.Windows.Controls;
using TimeLegendSpike.ViewModels;

namespace TimeLegendSpike.Helpers
{
    public abstract class DataTemplateSelector : ContentControl
    {
        public virtual DataTemplate SelectTemplate(
            object item, DependencyObject container)
        {
            return null;
        }

        protected override void OnContentChanged(
            object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }

    public class DrawTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CanvasTemplate
        {
            get;
            set;
        }

        public DataTemplate ItemsTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(
            object item, DependencyObject container)
        {
            var itemAux = item as MainViewModel;
            if (itemAux != null)
            {
                if (itemAux.UseCanvas)
                {
                    return CanvasTemplate;
                }
                else
                {
                    return ItemsTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }

}
