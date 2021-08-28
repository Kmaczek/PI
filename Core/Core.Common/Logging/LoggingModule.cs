using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving.Pipeline;
using System.Linq;

namespace Core.Common.Logging
{
    public class LoggingModule : Autofac.Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            registration.PipelineBuilding += RegistrationPipelineBuilding;
        }

        private void RegistrationPipelineBuilding(object sender, IResolvePipelineBuilder pipeline)
        {
            pipeline.Use(PipelinePhase.Activation, MiddlewareInsertionMode.EndOfPhase, (c, next) =>
            {
                c.ChangeParameters(c.Parameters.Union(new[]
                {
                    new ResolvedParameter(
                        (p, i) => p.ParameterType == typeof(ILogger),
                        (p, i) => new Log4Net(p.Member.DeclaringType)
                    )
                }));

                next(c);
            });
        }
    }
}
