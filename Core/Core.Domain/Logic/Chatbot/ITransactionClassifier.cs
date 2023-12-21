using Core.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Logic.Chatbot
{
    public interface ITransactionClassifier
    {
        void AssignCategoryInBatch(IEnumerable<TransactionModel> records);
    }
}
