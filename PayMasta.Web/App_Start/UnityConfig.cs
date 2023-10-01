using PayMasta.Service.Account;
using PayMasta.Service.Article;
using PayMasta.Service.BankTransfer;
using PayMasta.Service.Budget;
using PayMasta.Service.CallBack;
using PayMasta.Service.Common;
using PayMasta.Service.Earning;
using PayMasta.Service.Employer.CommonEmployerService;
using PayMasta.Service.Employer.Dashboard;
using PayMasta.Service.Employer.Employees;
using PayMasta.Service.Employer.EmployeesBankAccount;
using PayMasta.Service.Employer.EmployeeTransaction;
using PayMasta.Service.Employer.EmployerProfile;
using PayMasta.Service.Employer.EwaRequests;
using PayMasta.Service.Employer.PayToPayMasta;
using PayMasta.Service.FlutterWave;
using PayMasta.Service.ItexRechargeService;
using PayMasta.Service.ItexService;
using PayMasta.Service.ManageFinance;
using PayMasta.Service.Okra;
using PayMasta.Service.ProvidusExpresssWallet;
using PayMasta.Service.Support;
using PayMasta.Service.ThirdParty;
using PayMasta.Service.TokenService;
using PayMasta.Service.VerifyNin;
using PayMasta.Service.VirtualAccount;
using PayMasta.Service.ZealvendService;
using PayMasta.Service.ZealvendService.DataBundle;
using PayMasta.Service.ZealvendService.Electricity;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;
using Unity.Mvc5;

namespace PayMasta.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterType<IAccountService, AccountService>(new HierarchicalLifetimeManager());
            container.RegisterType<IThirdParty, ThirdPartyService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEarningService, EarningService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBankTransferService, BankTransferService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOkraService, OkraService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICallBackService, CallBackService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICommonEmployerService, CommonEmployerService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICommonService, CommonService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISupportService, SupportService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmployeesService, EmployeesService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmployerProfileService, EmployerProfileService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFlutterWaveService, FlutterWaveService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEwaRequestsServices, EwaRequestsServices>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmployeeTransactionService, EmployeeTransactionService>(new HierarchicalLifetimeManager());
            container.RegisterType<IDashboardService, DashboardService>(new HierarchicalLifetimeManager());
            container.RegisterType<IManageFinanceService, ManageFinanceService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITokenService, TokenService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmployeesBankAccountService, EmployeesBankAccountService>(new HierarchicalLifetimeManager());
            container.RegisterType<IVirtualAccountService, VirtualAccountService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPayToPayMastaService, PayToPayMastaService>(new HierarchicalLifetimeManager());
            container.RegisterType<IItexService, ItexService>(new HierarchicalLifetimeManager());
            container.RegisterType<IItexRechargeService, ItexRechargeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IVerifyNinService, VerifyNinService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProvidusExpresssWalletService, ProvidusExpresssWalletService>(new HierarchicalLifetimeManager());
            container.RegisterType<IZealvendBillService, ZealvendBillService>(new HierarchicalLifetimeManager());
            container.RegisterType<IZealvendDataService, ZealvendDataService>(new HierarchicalLifetimeManager());
            container.RegisterType<IElectricityService, ElectricityService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBudgetService, BudgetService>(new HierarchicalLifetimeManager());
            container.RegisterType<IArticleService, ArticleService>(new HierarchicalLifetimeManager());

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}