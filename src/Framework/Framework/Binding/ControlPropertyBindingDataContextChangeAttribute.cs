using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Compilation.ControlTree;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Utils;
using FastExpressionCompiler;

namespace DotVVM.Framework.Binding
{
    /// <summary> Sets data context type to the result type of binding the specified property. </summary>
    public class ControlPropertyBindingDataContextChangeAttribute : DataContextChangeAttribute
    {
        public string PropertyName { get; set; }

        public override int Order { get; }

        public bool AllowMissingProperty { get; set; }

        public ControlPropertyBindingDataContextChangeAttribute(string propertyName, int order = 0)
        {
            PropertyName = propertyName;
            Order = order;
        }

        public override ITypeDescriptor? GetChildDataContextType(ITypeDescriptor dataContext, IDataContextStack controlContextStack, IAbstractControl control, IPropertyDescriptor? property = null)
        {
            if (!control.Metadata.TryGetProperty(PropertyName, out var controlProperty))
            {
                throw new Exception($"The property '{PropertyName}' was not found on control '{control.Metadata.Type}'!");
            }

            if (control.TryGetProperty(controlProperty, out var setter))
            {
                return setter is IAbstractPropertyBinding binding
                    ? binding.Binding.ResultType ?? throw new Exception($"The '{controlProperty.Name}' property contains invalid data-binding")
                    : dataContext;
            }

            if (AllowMissingProperty)
            {
                return dataContext;
            }

            throw new Exception($"Property '{PropertyName}' is required on '{control.Metadata.Type.CSharpName}'.");
        }

        public override Type? GetChildDataContextType(Type dataContext, DataContextStack controlContextStack, DotvvmBindableObject control, DotvvmProperty? property = null)
        {
            var controlType = control.GetType();
            var controlProperty = DotvvmProperty.ResolveProperty(controlType, PropertyName);

            if (controlProperty == null)
            {
                throw new Exception($"The property '{PropertyName}' was not found on control '{controlType.ToCode()}'!");
            }

            if (control.properties.Contains(controlProperty))
            {
                return control.GetBinding(controlProperty) is IStaticValueBinding valueBinding
                    ? valueBinding.ResultType
                    : dataContext;
            }

            if (AllowMissingProperty)
            {
                return dataContext;
            }

            throw new Exception($"Property '{PropertyName}' is required on '{controlType.ToCode()}'.");
        }

        public override IEnumerable<string> PropertyDependsOn => new[] { PropertyName };

        public override bool? IsServerSideOnly(DataContextStack controlContextStack, DotvvmBindableObject control, DotvvmProperty? property = null)
        {
            var controlProperty = DotvvmProperty.ResolveProperty(control.GetType(), PropertyName);
            if (controlProperty is null)
                return null;

            var binding = control.GetBinding(controlProperty);
            return binding is not IValueBinding or null;
        }

        public override bool? IsServerSideOnly(IDataContextStack controlContextStack, IAbstractControl control, IPropertyDescriptor? property = null)
        {
            if (!control.Metadata.TryGetProperty(PropertyName, out var controlProperty))
                return null;
            if (!control.TryGetProperty(controlProperty, out var setter))
                return null;
            
            return
                setter is IAbstractPropertyBinding { Binding.BindingType: var bindingType } &&
                !typeof(IValueBinding).IsAssignableFrom(bindingType);

        }
    }
}
