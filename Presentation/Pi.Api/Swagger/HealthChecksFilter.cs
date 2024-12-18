using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Pi.Api.Swagger
{
    public class HealthChecksFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var (path, description) in GetHealthCheckPaths())
            {
                var pathItem = new OpenApiPathItem
                {
                    Operations = new Dictionary<OperationType, OpenApiOperation>
                    {
                        [OperationType.Get] = new OpenApiOperation
                        {
                            Tags = new List<OpenApiTag> { new() { Name = "Health Checks" } },
                            Description = description,
                            Responses = new OpenApiResponses
                            {
                                ["200"] = new OpenApiResponse { Description = "Healthy" },
                                ["503"] = new OpenApiResponse { Description = "Unhealthy" }
                            }
                        }
                    }
                };

                swaggerDoc.Paths.Add(path, pathItem);
            }
        }

        private static IEnumerable<(string Path, string Description)> GetHealthCheckPaths()
        {
            return new[]
            {
                ("/health", "Basic health check"),
            };
        }
    }
}
