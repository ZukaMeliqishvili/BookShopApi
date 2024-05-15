using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;
namespace BookShopApi.Infrastructure
{
    public class AddFileUploadParams : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            // Add a file parameter to the operation if it accepts a file
            var attributes = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            var httpMethod = context.ApiDescription.HttpMethod;
            if (httpMethod.Equals("POST") || httpMethod.Equals("PUT")) // Adjust this according to your requirements
            {
                foreach (var attribute in attributes)
                {
                    if (attribute is ConsumesAttribute consumesAttribute)
                    {
                        foreach (var contentType in consumesAttribute.ContentTypes)
                        {
                            if (contentType.Equals("multipart/form-data"))
                            {
                                operation.Parameters.Add(new OpenApiParameter
                                {
                                    Name = "file",
                                    In = ParameterLocation.Query,
                                    Description = "Upload File",
                                    Required = true,
                                    Schema = new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary"
                                    }
                                });
                            }
                            else if (contentType.Equals("application/json"))
                            {
                                operation.RequestBody = new OpenApiRequestBody
                                {
                                    Required = true,
                                    Content = new Dictionary<string, OpenApiMediaType>
                                    {
                                        ["application/json"] = new OpenApiMediaType
                                        {
                                            Schema = context.SchemaGenerator.GenerateSchema(context.ApiDescription.ActionDescriptor.Parameters[0].ParameterType, context.SchemaRepository)
                                        }
                                    }
                                };
                            }
                        }
                    }
                }
            }
        }
    }
}
