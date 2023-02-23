using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace conan_master_server.ModelBinder;

public class CustomModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        // if (context.BindingInfo.BindingSource != null && context.BindingInfo.BindingSource.Id == "Body")
        if (context.BindingInfo.BindingSource != null && context.Metadata.Name == "tokenRequest")
        {
            // Return your custom model binder for the specific endpoint
            return new CustomModelBinder();
        }

        return null;
    }
}