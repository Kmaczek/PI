using Autofac;
using System.Collections.Generic;
using System.Linq;

namespace Core.Common
{
    public static class AutofacContainerValidation
    {
        public static List<string> Verify(this IContainer container)
        {
            var errors = new List<string>();
            var registrations = container.ComponentRegistry.Registrations;

            using (var scope = container.BeginLifetimeScope())
            {
                foreach (var registration in registrations.SelectMany(x => x.Services))
                {
                    try
                    {
                        if (registration is Autofac.Core.TypedService)
                        {
                            var tSvc = registration as Autofac.Core.TypedService;
                            scope.Resolve(tSvc.ServiceType);
                        }
                        else if (registration is Autofac.Core.KeyedService)
                        {
                            var kSvc = registration as Autofac.Core.KeyedService;
                            scope.ResolveNamed(kSvc.ServiceKey.ToString(), kSvc.ServiceType);
                        }
                    }
                    catch (Autofac.Core.DependencyResolutionException ex)
                    {
                        errors.Add($"{ex.Message} {ex.InnerException}");
                    }
                }
            }

            return errors;
        }
    }
}
