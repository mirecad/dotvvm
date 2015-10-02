using System;
using System.Collections.Generic;
using System.Linq;
using DotVVM.Framework.Controls.Infrastructure;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;

namespace DotVVM.Framework.Runtime
{
    public interface IViewModelSerializer
    {
        void BuildViewModel(DotvvmRequestContext context);

        string SerializeViewModel(DotvvmRequestContext context);
        
        string SerializeModelState(IDotvvmRequestContext context);

        void PopulateViewModel(DotvvmRequestContext context, string serializedPostData);

        void ResolveCommand(DotvvmRequestContext context, DotvvmView view, string serializedPostData, out ActionInfo actionInfo);

        void AddPostBackUpdatedControls(DotvvmRequestContext context);
    }
}