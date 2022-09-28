using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ERP.Web.Swagger
{
    public class SwaggerOperationFilter : IOperationFilter
    {
      
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                return;

            for (int i = 0; i < operation.Parameters.Count; ++i)
            {
                var parameter = operation.Parameters[i];

                var enumType = context.ApiDescription.ParameterDescriptions[i].ParameterDescriptor.ParameterType;
                if (!enumType.IsEnum)
                {
                    continue;
                }

                if (context.SchemaRegistry.Definitions.ContainsKey(enumType.Name) == false)
                    context.SchemaRegistry.Definitions.Add(enumType.Name, context.SchemaRegistry.GetOrRegister(enumType));


                var schema = new Schema
                {
                    Ref = $"#/definitions/{enumType.Name}"
                };
                parameter.Extensions.Add("x-schema", schema);
            }
        }
    }
}