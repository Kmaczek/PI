using Core.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pi.Api.Models.Request
{
    public class ClassifyTransactionsRequest
    {
        public IEnumerable<RawTransaction> RawTransactions { get; set; }

        public IEnumerable<TransactionModel> ToTransactionModel()
        {
            return RawTransactions.Select(x => new TransactionModel()
            {
                Amount = x.Price,
                Category = x.Category,
                Name = x.Name,
            });
        }
    }

    public class RawTransaction
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
