using Data.Repository.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace Pi.Api.HealthChecks
{
    public class EfContextHealthCheck(IPriceRepository _priceRepository) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            _ = await _priceRepository.HealthCheck();
            return new HealthCheckResult(HealthStatus.Healthy);
        }
    }
}
