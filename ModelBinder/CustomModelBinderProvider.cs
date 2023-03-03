using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace conan_master_server.ModelBinder;

public class CustomModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context.BindingInfo.BindingSource != null && context.Metadata.Name == "tokenRequest")
        {
            return new CustomModelBinder();
        }

        return null;
    }
}