using System;
using System.Threading.Tasks;

namespace Core.Domain.Logic.Inflation
{
    public interface ICalculateInflation
    {
        Task<decimal> CalculateInflation(DateTime date);
    }
}
