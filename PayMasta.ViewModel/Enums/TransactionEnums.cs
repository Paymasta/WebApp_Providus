using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.Enums
{
    public enum WalletTransactionStatus
    {
        SUCCESS = 1,
        FAILED = 2,
        SENDER_NOT_EXIST = 3,
        RECEIVER_NOT_EXIST = 4,
        SELF_WALLET = 5,
        OTHER_ERROR = 6
    }
    public enum WalletTransactionDetailTypes
    {
        DEBIT = 1,
        CREDIT = 2
    }
    public enum TransactionDetailType
    {
        Debit = 1,
        Credit = 2,
    }
    public enum WalletTransactionType
    {

        PAID = 1,
        RECEIVED = 2,
        ADDED = 3,
        All = 4
    }
    public enum DownloadFileType
    {
        PDF = 1,
        EXCEL = 2
    }
    public enum TransactionTypeInfo
    {
        AddedByCard = 1,
        AddedByMobileMoney = 2,
        Receive = 3,
        PaidByPayServices = 4,
        EWalletToEwalletTransactionsPayMoney = 5,
        EWalletToEwalletTransactionsMakePaymentRequest = 6,
        EWalletToEwalletTransactionsMerchantPayment = 7,
        EWalletToBankTransactions = 8,
        AddByBankToWallet = 9,
        Payforfilgth = 10,
        LendingAppTransaction = 11,
        Resort,
        AfroBasket,
        CashIn,
        CashOut,
    }

    public enum TransactionStatus
    {
        NoResponse = 0,
        Completed = 1,
        Pending = 2,
        Rejected = 3,
        UnderProcess = 4,
        Failed = 5,
        Hold
    }
    public enum WalletTransactionSubTypes
    {
        AIRTIME = 1,
        INTERNET,
        TV,
        Power,
        DATABUNDLE

    }

    public enum ProvidusStatus
    {
        Success = 00,
        duplicate = 01,
        Rejected = 02
    }
}
