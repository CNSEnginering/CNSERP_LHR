using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ERP.Web.Swagger
{
    public class SwaggerEnumParameterFilter : IParameterFilter
    {
        public void Apply(IParameter parameter, ParameterFilterContext context)
        {
            var type = Nullable.GetUnderlyingType(context.ApiParameterDescription.Type) ?? context.ApiParameterDescription.Type;
            if (type.IsEnum)
            {
                AddEnumParmSpec(parameter.Extensions, type, context.SchemaRegistry);
                parameter.Required = type == context.ApiParameterDescription.Type;
            }
            else if (parameter is NonBodyParameter nonBodyParam && (type.IsArray || (type.IsGenericType && type.GetInterfaces().Contains(typeof(IEnumerable)))))
            {
                var itemType = type.GetElementType() ?? type.GenericTypeArguments.First();
                AddEnumSpec(nonBodyParam.Items.Extensions, itemType, context.SchemaRegistry);
            }
        }

        private static void AddEnumSpec(Dictionary<string, object> extensions, Type type, ISchemaRegistry schemaRegistry)
        {
            var registeredType = schemaRegistry.GetOrRegister(type);
            if (registeredType.Ref == null)
            {
                return;
            }

            extensions.Add("x-schema", registeredType);
            extensions.Add("x-enumNames", Enum.GetNames(type));
        }

        private static void AddEnumParmSpec(Dictionary<string, object> extensions, Type type, ISchemaRegistry schemaRegistry)
        {
            var registeredType = schemaRegistry.GetOrRegister(type);
            if (registeredType.Ref == null)
            {
                return;
            }

            extensions.Add("x-schema", registeredType);
            extensions.Add("x-enumNames", Enum.GetNames(type));
        }
    }
}