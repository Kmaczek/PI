using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Linq;

namespace Jobs.OldScheduler.HealthChecks
{
    public static class AddHealthCheckEndpoint
    {
        public static void AddHealthCheckEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        Status = report.Status.ToString(),
                        Components = report.Entries.Select(e => new
                        {
                            Component = e.Key,
                            Status = e.Value.Status.ToString(),
                            Description = e.Value.Description,
                            Duration = e.Value.Duration.ToString()
                        })
                    };

                    await context.Response.WriteAsJsonAsync(response);
                }
            });
        }
    }
}
