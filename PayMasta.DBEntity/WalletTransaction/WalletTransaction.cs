using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.DBEntity.WalletTransaction
{
    public class WalletTransaction : BaseEntity
    {
        public long WalletTransactionId { get; set; }
        public string TotalAmount { get; set; }
        public int CommisionId { get; set; }
        public string CommisionAmount { get; set; }
        public string WalletAmount { get; set; }
        public decimal ServiceTaxRate { get; set; }
        public string ServiceTax { get; set; }
        public int ServiceCategoryId { get; set; }
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public string AccountNo { get; set; }
        public string TransactionId { get; set; }
        public bool IsAdminTransaction { get; set; }
        public string Comments { get; set; }
        public string InvoiceNo { get; set; }
        public int TransactionStatus { get; set; }
        public string TransactionType { get; set; }
        public int TransactionTypeInfo { get; set; }
        public bool IsBankTransaction { get; set; }
        public string BankBranchCode { get; set; }
        public string BankTransactionId { get; set; }
        public string VoucherCode { get; set; }
        public decimal FlatCharges { get; set; }
        public decimal BenchmarkCharges { get; set; }
        public decimal CommisionPercent { get; set; }
        public string DisplayContent { get; set; }
        public bool IsUpcomingBillShow { get; set; }
        public int SubCategoryId { get; set; }
        public bool IsAmountPaid { get; set; }

    }
}
