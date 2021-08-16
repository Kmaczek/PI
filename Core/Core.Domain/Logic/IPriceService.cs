using Core.Model.PriceView;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic
{
    public interface IPriceService
    {
        IEnumerable<ProductVm> GetProducts();
    }
}
