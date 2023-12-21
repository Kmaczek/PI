using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.Transaction
{
    public class TransactionModel
    {
        public string Date { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public string Category { get; set; }

        public override string ToString()
        {
            return $"[Nazwa: {Name}] [Kwota: {Amount}] [Kategoria: {Category}]";
        }

        public string ToFullString()
        {
            return $"[Date: {Date}] [Name: {Name}] [Amount: {Amount}] [Category: {Category}]";
        }
    }
}
