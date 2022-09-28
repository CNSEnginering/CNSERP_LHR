using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ERP.Web.Swagger
{
    public class SwaggerEnumSchemaFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaFilterContext context)
        {
            if (schema.Properties != null)
            {
                var enumProperties = schema.Properties.Where(p => p.Value.Enum != null)
                    .Union(schema.Properties.Where(p => p.Value.Items?.Enum != null)).ToList();

                var enums = context.SystemType.GetProperties()
                    .Select(p => Nullable.GetUnderlyingType(p.PropertyType) ??
                                 p.PropertyType.GetElementType() ??
                                 p.PropertyType.GetGenericArguments().FirstOrDefault() ??
                                 p.PropertyType)
                    .Where(p => p.IsEnum)
                    .Distinct()
                    .ToList();

                foreach (var enumProperty in enumProperties)
                {
                    ModifyEnumPropertySchema(schema, context, enumProperty, enums);
                }
            }

            var type = Nullable.GetUnderlyingType(context.SystemType) ?? context.SystemType;
            if (!type.IsEnum)
            {
                return;
            }
            if (!context.SchemaRegistry.Definitions.ContainsKey(type.Name) || context.SchemaRegistry.Definitions[type.Name] != null)
            {
                schema.Ref = context.SchemaRegistry.GetOrRegister(type).Ref;
            }
            else
            {
                schema.Extensions.Add("x-enumNames", Enum.GetNames(type));
            }
        }

        private void ModifyEnumPropertySchema(Schema schema, SchemaFilterContext context, KeyValuePair<string, Schema> enumProperty, List<Type> enums)
        {
            var enumPropertyValue = enumProperty.Value.Enum != null ? enumProperty.Value : enumProperty.Value.Items;

            var enumValues = enumPropertyValue.Enum.Select(e => $"{e}").ToList();
            Type enumType = enums.SingleOrDefault(p =>
            {
                var enumNames = Enum.GetNames(p);
                if (enumNames.Except(enumValues, StringComparer.InvariantCultureIgnoreCase).Any())
                {
                    return false;
                }

                if (enumValues.Except(enumNames, StringComparer.InvariantCultureIgnoreCase).Any())
                {
                    return false;
                }

                return true;
            });

            if (enumType == null)
            {
                throw new Exception($"Property {enumProperty} not found in {context.SystemType.Name} Type.");
            }

            if (context.SchemaRegistry.Definitions.ContainsKey(enumType.Name) == false)
            {
                context.SchemaRegistry.Definitions.Add(enumType.Name, enumPropertyValue);
            }

            var newSchema = new Schema
            {
                Ref = $"#/definitions/{enumType.Name}"
            };

            if (enumProperty.Value.Enum != null)
            {
                schema.Properties[enumProperty.Key] = newSchema;
            }
            else if (enumProperty.Value.Items?.Enum != null)
            {
                enumProperty.Value.Items = newSchema;
            }
        }
    }
}