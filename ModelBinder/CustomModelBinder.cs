using conan_master_server.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace conan_master_server.ModelBinder;

public class CustomModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        Console.WriteLine(bindingContext);
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var bodyStream = bindingContext.HttpContext.Request.Body;
        using var reader = new StreamReader(bodyStream);
        var wrappedBody = await reader.ReadToEndAsync();
        
        try
        {
            var body = JObject.Parse(wrappedBody)["FunctionParameter"].ToString();
            var model = JsonConvert.DeserializeObject<TokenRequest>(body);
            bindingContext.Result = ModelBindingResult.Success(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            bindingContext.Result = ModelBindingResult.Failed();
        }
    }
}