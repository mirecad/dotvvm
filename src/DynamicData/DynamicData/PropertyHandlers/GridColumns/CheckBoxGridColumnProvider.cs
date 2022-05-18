using DotVVM.AutoUI.Controls;
using DotVVM.AutoUI.Metadata;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Utils;

namespace DotVVM.AutoUI.PropertyHandlers.GridColumns
{
    public class CheckBoxGridColumnProvider : GridColumnProviderBase
    {
        public override bool CanHandleProperty(PropertyDisplayMetadata property, DynamicDataContext context)
        {
            return property.Type.UnwrapNullableType() == typeof(bool);
        }

        protected override GridViewColumn CreateColumnCore(PropertyDisplayMetadata property, DynamicGridColumn.Props props, DynamicDataContext context)
        {
            var column = new GridViewCheckBoxColumn();
            column.SetBinding(GridViewCheckBoxColumn.ValueBindingProperty, context.CreateValueBinding(property));
            return column;
        }
    }
}
