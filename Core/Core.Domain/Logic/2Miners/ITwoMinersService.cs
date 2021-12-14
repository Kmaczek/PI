using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Logic._2Miners
{
    public interface ITwoMinersService
    {
        Task AccountInfoAsync(string walletId);
    }
}
