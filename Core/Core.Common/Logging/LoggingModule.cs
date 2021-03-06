﻿using Autofac.Core;
using Autofac.Core.Registration;
using System.Linq;

namespace Core.Common.Logging
{
    public class LoggingModule : Autofac.Module
    {
        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            e.Parameters = e.Parameters.Union(new[]
            {
                new ResolvedParameter(
                    (p, i) => p.ParameterType == typeof(ILogger),
                    (p, i) => new Log4Net(p.Member.DeclaringType)
                ),
            });
        }

        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }
    }
}
