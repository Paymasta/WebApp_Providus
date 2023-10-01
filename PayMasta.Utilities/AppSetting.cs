using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities
{
    public class AppSetting
    {
        public static string GetImagePath = ConfigurationManager.AppSettings["GetImagePath"];

        public static string ConnectionStrings = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static string ForgotTemplate = "ForgotPasswordEmailTemplate.html";
        public static string AccountStatusTemplate = "AccountStatusTemplate.html";
        public static string RegistrationTemplate = "RegistrationTemplate.html";
        public static string EmailVerificationTemplate = "EmailVerificationTemplate.html";
        public static string NewCustomerTemplate = "NewCustomerTemplate.html";
        public static string ChangePassword = "ChangePasswordEmailTemplate.html";
        public static string Invoice = "Invoice.html";
        public static string RequestDemo = "RequestDemo.html";
        public static string LoginCreds = "LoginCredsAndWelcome.html";
        public static string EWAAlert = "EWA Alert.html";



        /// <summary>
        /// Lidya 
        /// </summary>
        public static string LidyaAuthUrl = ConfigurationManager.AppSettings["LidyaAuthUrl"];
        public static string LidyaAuthUsername = ConfigurationManager.AppSettings["LidyaAuthUsername"];
        public static string LidyaAuthPassword = ConfigurationManager.AppSettings["LidyaAuthPassword"];
        public static string LidyaAuthHeader = ConfigurationManager.AppSettings["LidyaAuthHeader"];
        public static string LidyaMandateCreateUrl = ConfigurationManager.AppSettings["LidyaMandateCreateUrl"];
        public static string TypeVariable = ConfigurationManager.AppSettings["TypeVariable"];

        /// <summary>
        /// Qoreid details
        /// </summary>
        public static string QoreIdAuthTokenUrl = ConfigurationManager.AppSettings["QoreIdAuthTokenUrl"];
        public static string QoreIdClientId = ConfigurationManager.AppSettings["QoreIdClientId"];
        public static string QoreIdSecretKey = ConfigurationManager.AppSettings["QoreIdSecretKey"];
        public static string QoreIdBvnNubanUrl = ConfigurationManager.AppSettings["QoreIdBvnNubanUrl"];
        public static string QoreIdVNninUrl = ConfigurationManager.AppSettings["QoreIdVNninUrl"];
        public static string BankListFile = ConfigurationManager.AppSettings["BankListFile"];
        public static string QoreIdDLVerificationURL = ConfigurationManager.AppSettings["QoreIdDLVerificationURL"];
        public static string QoreIdVoterIdVerificationURL = ConfigurationManager.AppSettings["QoreIdVoterIdVerificationURL"];
        public static string QoreIdPassportNumberVerificationURL = ConfigurationManager.AppSettings["QoreIdPassportNumberVerificationURL"];

        public static string SMS_Path = ConfigurationManager.AppSettings["SMS_Path"];
        public static string SMS_application = ConfigurationManager.AppSettings["SMS_application"];
        public static string SMS_source = ConfigurationManager.AppSettings["SMS_source"];
        public static string SMS_Password = ConfigurationManager.AppSettings["SMS_Password"];
        public static string SMS_mask = ConfigurationManager.AppSettings["SMS_mask"];

        public static string HostNameAdmin = ConfigurationManager.AppSettings["HostNameAdmin"];
        public static string VerifyMailLink = HostNameAdmin + "/" + ConfigurationManager.AppSettings["VerifyMailLink"];
        /// <summary>
        /// SMTP
        /// </summary>
        public static string SMTP_USERNAME = ConfigurationManager.AppSettings["SMTP_USERNAME"];
        public static string SMTP_PASSWORD = ConfigurationManager.AppSettings["SMTP_PASSWORD"];
        public static string CONFIGSET = ConfigurationManager.AppSettings["CONFIGSET"];
        public static string HOST = ConfigurationManager.AppSettings["HOST"];
        public static string FROM = ConfigurationManager.AppSettings["FROM"];
        public static string FROMNAME = ConfigurationManager.AppSettings["FROMNAME"];
        public static string PORT = ConfigurationManager.AppSettings["PORT"];

        /// <summary>
        /// Send Grid SMTP
        /// </summary>
        public static string sendgridKey = ConfigurationManager.AppSettings["sendgridKey"];
        public static string SendEmailFrom = ConfigurationManager.AppSettings["SendEmailFrom"];
        public static string DisplayName = ConfigurationManager.AppSettings["DisplayName"];
        public static string SupportEmailTo = ConfigurationManager.AppSettings["SupportEmailTo"];

        public static string AccountSidTwilio = ConfigurationManager.AppSettings["AccountSidTwilio"];
        public static string AuthTokenTwilio = ConfigurationManager.AppSettings["AuthTokenTwilio"];
        public static string MessagingServiceSid = ConfigurationManager.AppSettings["MessagingServiceSid"];

        /// <summary>
        /// JWT
        /// </summary>
        public static string JwtKey = ConfigurationManager.AppSettings["JwtKey"];
        public static string JwtExpireDays = ConfigurationManager.AppSettings["JwtExpireDays"];
        public static string JwtIssuer = ConfigurationManager.AppSettings["JwtIssuer"];

        public static string EmailForEWaAlertDomino = ConfigurationManager.AppSettings["EmailForEWaAlertDomino"];
        public static string EmailForEWaAlertGerald = ConfigurationManager.AppSettings["EmailForEWaAlertGerald"];

        /// <summary>
        /// NIN
        /// </summary>
        public static string NinVerifyUrl = ConfigurationManager.AppSettings["NinVerifyUrl"];
        public static string NinSecretKey = ConfigurationManager.AppSettings["NinSecretKey"];
        public static string vNinVerifyUrl = ConfigurationManager.AppSettings["vNinVerifyUrl"];

        /// <summary>
        /// flutter wave urls
        /// </summary>
        public static string FlutterWaveVertualAccountUrl = ConfigurationManager.AppSettings["FlutterWaveVertualAccountUrl"];
        public static string FlutterWaveSecretKey = ConfigurationManager.AppSettings["FlutterWaveSecretKey"];
        public static string FlutterWaveAirtimeOperator = ConfigurationManager.AppSettings["FlutterWaveAirtimeOperator"];
        public static string FlutterWaveElectricityOperator = ConfigurationManager.AppSettings["FlutterWaveElectricityOperator"];
        public static string FlutterWaveInternetOperator = ConfigurationManager.AppSettings["FlutterWaveInternetOperator"];
        public static string FlutterWaveInternetWifiOperator = ConfigurationManager.AppSettings["FlutterWaveInternetWifiOperator"];
        public static string FlutterWaveTvOperator = ConfigurationManager.AppSettings["FlutterWaveTvOperator"];
        public static string FlutterWaveBillsOperator = ConfigurationManager.AppSettings["FlutterWaveBillsOperator"];
        public static string FlutterWaveBillerCodefilter = ConfigurationManager.AppSettings["FlutterWaveBillerCodefilter"];
        public static string FlutterWaveBillPayMentUrl = ConfigurationManager.AppSettings["FlutterWaveBillPayMentUrl"];
        public static string FlutterWaveBulkBillPayMentUrl = ConfigurationManager.AppSettings["FlutterWaveBulkBillPayMentUrl"];
        public static string FlutterWaveVertualAccountBalance = ConfigurationManager.AppSettings["FlutterWaveVertualAccountBalance"];


        /// <summary>
        /// Pouchii Core Wallet Documentation
        /// </summary>
        public static string WalletExternal = ConfigurationManager.AppSettings["WalletExternal"];
        public static string schemeId = ConfigurationManager.AppSettings["schemeId"];
        public static string AuthenticatePouchii = ConfigurationManager.AppSettings["AuthenticatePouchii"];
        public static string CustomerWalletAccounts = ConfigurationManager.AppSettings["CustomerWalletAccounts"];
        public static string ChangeWalletPassword = ConfigurationManager.AppSettings["ChangeWalletPassword"];
        public static string billersOperator = ConfigurationManager.AppSettings["billersOperator"];
        public static string billersPlans = ConfigurationManager.AppSettings["billersPlans"];
        public static string meterValidate = ConfigurationManager.AppSettings["meterValidate"];
        public static string Bouquets = ConfigurationManager.AppSettings["Bouquets"];
        public static string Validatemultichoice = ConfigurationManager.AppSettings["Validatemultichoice"];
        public static string Validatestartimes = ConfigurationManager.AppSettings["Validatestartimes"];
        public static string InternetBunldes = ConfigurationManager.AppSettings["InternetBunldes"];
        public static string InternetValidation = ConfigurationManager.AppSettings["InternetValidation"];
        public static string PurchaseVtu = ConfigurationManager.AppSettings["PurchaseVtu"];
        public static string MultichoicePurchase = ConfigurationManager.AppSettings["MultichoicePurchase"];
        public static string ElectricityPurchase = ConfigurationManager.AppSettings["ElectricityPurchase"];
        public static string DataPurchase = ConfigurationManager.AppSettings["DataPurchase"];
        public static string SubscribeStartimes = ConfigurationManager.AppSettings["SubscribeStartimes"];
        public static string SubscribeInternet = ConfigurationManager.AppSettings["SubscribeInternet"];
        public static string SourceAccountNumberPouchii = ConfigurationManager.AppSettings["SourceAccountNumberPouchii"];
        public static string PINNUMBER = ConfigurationManager.AppSettings["PINNUMBER"];
        public static string Fundwallet = ConfigurationManager.AppSettings["Fundwallet"];
        public static string RetrieveNubanNumber = ConfigurationManager.AppSettings["RetrieveNubanNumber"];
        public static string BankListUrl = ConfigurationManager.AppSettings["BankListUrl"];
        public static string LatestBankListUrl = ConfigurationManager.AppSettings["LatestBankListUrl"];
        public static string SendMoneyToBank = ConfigurationManager.AppSettings["SendMoneyToBank"];
        public static string UpdatePin = ConfigurationManager.AppSettings["UpdatePin"];
        public static string CustomerWallets = ConfigurationManager.AppSettings["CustomerWallets"];
        public static string VerifyAccount = ConfigurationManager.AppSettings["VerifyAccount"];
        public static string CurrentBalanceAndNuban = ConfigurationManager.AppSettings["CurrentBalanceAndNuban"];
        /// <summary>
        /// providus 
        /// </summary>
        public static string ProvidusVertualAccountUrl = ConfigurationManager.AppSettings["ProvidusVertualAccountUrl"];
        public static string ProvidusClientId = ConfigurationManager.AppSettings["ProvidusClientId"];
        public static string ProvidusXAuthSignature = ConfigurationManager.AppSettings["XAuthSignature"];

        /// <summary>
        /// Providus Bank details
        /// </summary>
        public static string GetNIPBanks = ConfigurationManager.AppSettings["GetNIPBanks"];
        public static string BankUserName = ConfigurationManager.AppSettings["BankUserName"];
        public static string BankPassword = ConfigurationManager.AppSettings["BankPassword"];
        public static string GetNIPAccount = ConfigurationManager.AppSettings["GetNIPAccount"];
        public static string NIPFundTransfer = ConfigurationManager.AppSettings["NIPFundTransfer"];
        public static string ProvidusFundTransfer = ConfigurationManager.AppSettings["ProvidusFundTransfer"];
        public static string currencyCode = ConfigurationManager.AppSettings["currencyCode"];
        public static string narration = ConfigurationManager.AppSettings["narration"];
        public static string GetProvidusAccount = ConfigurationManager.AppSettings["GetProvidusAccount"];
        public static string GetBVNDetails = ConfigurationManager.AppSettings["GetBVNDetails"];
        public static string sourceAccountName = ConfigurationManager.AppSettings["sourceAccountName"];
        public static string PayMastaAccountNumber = ConfigurationManager.AppSettings["PayMastaAccountNumber"];

        /// <summary>
        /// okra details
        /// </summary>
        public static string OkraLinkUrl = ConfigurationManager.AppSettings["OkraLinkUrl"];
        public static string okracallback_url = ConfigurationManager.AppSettings["okracallback_url"];
        public static string Okralogo = ConfigurationManager.AppSettings["Okralogo"];
        public static string OkraUserName = ConfigurationManager.AppSettings["OkraUserName"];
        public static string Okrasupport_email = ConfigurationManager.AppSettings["Okrasupport_email"];
        public static string continue_cta = ConfigurationManager.AppSettings["continue_cta"];
        public static string OkraToken = ConfigurationManager.AppSettings["OkraToken"];
        public static string WidgetLink = ConfigurationManager.AppSettings["WidgetLink"];
        public static string OkraGetBankList = ConfigurationManager.AppSettings["OkraGetBankList"];

        /// <summary>
        /// fireBase detail
        /// </summary>
        public static string Bucket = ConfigurationManager.AppSettings["Bucket"];
        public static string FireBaseAPIKey = ConfigurationManager.AppSettings["FireBaseAPIKey"];
        public static string FirebaseUserName = ConfigurationManager.AppSettings["FirebaseUserName"];
        public static string FirebasePassword = ConfigurationManager.AppSettings["FirebasePassword"];

        /// <summary>
        /// push detail
        /// </summary>
        public static string FCM_ServerKey = ConfigurationManager.AppSettings["FCM_ServerKey"];
        public static string FCM_SenderId = ConfigurationManager.AppSettings["FCM_SenderId"];
        public static string FCMURL = ConfigurationManager.AppSettings["FCMURL"];

        /// <summary>
        /// Express wallet details
        /// </summary>
        public static string ExpressWalletSecretKey = ConfigurationManager.AppSettings["ExpressWalletSecretKey"];
        public static string ExpressWalletPublicKey = ConfigurationManager.AppSettings["ExpressWalletPublicKey"];
        public static string CreateExpressWallet = ConfigurationManager.AppSettings["CreateExpressWallet"];
        public static string GetExpressWallet = ConfigurationManager.AppSettings["GetExpressWallet"];
        public static string ExpressWalletBaseUrl = ConfigurationManager.AppSettings["ExpressWalletBaseUrl"];
        public static string ExpressWalletToWallet = ConfigurationManager.AppSettings["ExpressWalletToWallet"];
        public static string ExpressWalletToBankVerify = ConfigurationManager.AppSettings["ExpressWalletToBankVerify"];
        public static string ExpressWalletToBankTransfer = ConfigurationManager.AppSettings["ExpressWalletToBankTransfer"];
        public static string ExpressBankList = ConfigurationManager.AppSettings["ExpressBankList"];
        public static string ExpressWalletDebit = ConfigurationManager.AppSettings["ExpressWalletDebit"];

        /// <summary>
        /// Zealvend services for billpayment
        /// </summary>
        public static string ZealVendBaseurl = ConfigurationManager.AppSettings["ZealVendBaseurl"];
        public static string ZealVendAirtime = ZealVendBaseurl + ConfigurationManager.AppSettings["ZealVendAirtime"];
        public static string ZealVendAuthTokenEndpoint = ZealVendBaseurl + ConfigurationManager.AppSettings["ZealVendAuthTokenEndpoint"];
        public static string ZealEmail = ConfigurationManager.AppSettings["ZealEmail"];
        public static string ZealPassword = ConfigurationManager.AppSettings["ZealPassword"];
        public static string ZealPayTvVendEndpoint = ZealVendBaseurl + ConfigurationManager.AppSettings["ZealPayTvVendEndpoint"];
        public static string ZealPayTvTopupEndpoint = ZealVendBaseurl + ConfigurationManager.AppSettings["ZealPayTvTopupEndpoint"];
        public static string ZealPayTvProductEndpoint = ZealVendBaseurl + ConfigurationManager.AppSettings["ZealPayTvProductEndpoint"];
        public static string ZealPayTvVerifyEndpoint = ZealVendBaseurl + ConfigurationManager.AppSettings["ZealPayTvVerifyEndpoint"];
        public static string ZealPayElectricityEndpoint = ZealVendBaseurl + ConfigurationManager.AppSettings["ZealPayElectricityEndpoint"];
        public static string ZealPayElectricityMeterEndpoint = ZealVendBaseurl + ConfigurationManager.AppSettings["ZealPayElectricityMeterEndpoint"];
        public static string ZealPayDataNetworkEndpoint = ZealVendBaseurl + ConfigurationManager.AppSettings["ZealPayDataNetworkEndpoint"];
        public static string ZealPayDataPayEndpoint = ZealVendBaseurl + ConfigurationManager.AppSettings["ZealPayDataPayEndpoint"];

        public static string D2cEmployerEmail = ConfigurationManager.AppSettings["D2cEmployerEmail"];
        public static string D2cEmployerId = ConfigurationManager.AppSettings["D2cEmployerId"];
    }

    public static class AggregatorySTATUSCODES
    {
        public static string SUCCESSFUL = "00";
        public static string TRANSACTIONDOESNOTEXIST = "01";
        public static string REQUESTINPROCESS = "09";
        public static string FAILED = "32";
        public static string DEBITACCOUNTINVALID = "7701";
        public static string CREDITACCOUNTINVALID = "7702";
        public static string CREDITACCOUNTDORMANT = "7703";
        public static string INSUFFICIENTBALANCE = "7704";
        public static string INVALIDTRANSACTIONAMOUNT = "7706";
        public static string CURRENCYMISSMATCH = "7708";
        public static string TRANSACTIONREFERANCEEXISTS = "7709";
        public static string AUTHENTICATIONFAILED = "8004";
        public static string METHODNOTALLOWED = "8005";
        public static string NOTCONNECTION = "8803";
        public static string RESTRICTIONONACCOUNT = "8888";
        public static string ERRORDEBITINGACCOUNT = "7799";
    }
    public static class AggregatoryMESSAGE
    {
        public static string SUCCESSFUL = "APPROVED OR COMPLETED SUCCESSFULLY";
        public static string TRANSACTIONDOESNOTEXIST = "TRANSACTION DOES NOT EXIST";
        public static string REQUESTINPROCESS = "REQUEST IN PROCESS";
        public static string FAILED = "TRANSACTION NOT SUCCESSFULL";
        public static string DEBITACCOUNTINVALID = "DEBIT ACCOUNT INVALID";
        public static string CREDITACCOUNTINVALID = "CREDIT ACCOUNT INVALID";
        public static string CREDITACCOUNTDORMANT = "CREDIT ACCOUNT DORMANT";
        public static string INSUFFICIENTBALANCE = "INSUFFICIENT BALANCE";
        public static string INVALIDTRANSACTIONAMOUNT = "INVALID TRANSACTION AMOUNT";
        public static string CURRENCYMISSMATCH = "CURRENCY MISSMATCH";
        public static string TRANSACTIONREFERANCEEXISTS = "TRANSACTION REFERANCE EXISTS";
        public static string AUTHENTICATIONFAILED = "AUTHENTICATION FAILED";
        public static string METHODNOTALLOWED = "METHOD NOT ALLOWED";
        public static string NOTCONNECTION = "NOT CONNECTION";
        public static string RESTRICTIONONACCOUNT = "RESTRICTION ON ACCOUNT";
        public static string ERRORDEBITINGACCOUNT = "ERROR DEBITING ACCOUNT";
    }
    public static class OkraCallBackType
    {
        public static string TRANSACTIONS = "TRANSACTIONS";
        public static string INCOME = "INCOME";
        public static string AUTH = "AUTH";
        public static string IDENTITY = "IDENTITY";
        public static string BALANCE = "BALANCE";
        public static string ACCOUNTS = "ACCOUNTS";
        // public static string CREDITACCOUNTDORMANT = "CREDIT ACCOUNT DORMANT";
    }
    public class CommonSetting
    {
        public static string GetUniqueNumber()
        {
            Random random = new Random();
            const string numbers = "0123456789";

            return "PAY-MASTA" + (DateTime.UtcNow.Date.Year).ToString() + "-" + new string(Enumerable.Repeat(numbers, 4).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string AlphaNumericString(int stringLength)
        {
            Random random = new Random();


            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%&*()0123456789";
            return new string(Enumerable.Repeat(chars, stringLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
