using PayMasta.API.Filters;
using PayMasta.API.Resolver;
using PayMasta.Service.Account;
using PayMasta.Service.BankTransfer;
using PayMasta.Service.CallBack;
using PayMasta.Service.Common;
using PayMasta.Service.Earning;
using PayMasta.Service.Employer.CommonEmployerService;
using PayMasta.Service.Employer.Employees;
using PayMasta.Service.Employer.EmployeeTransaction;
using PayMasta.Service.FlutterWave;
using PayMasta.Service.ItexRechargeService;
using PayMasta.Service.ItexService;
using PayMasta.Service.Lidya;
using PayMasta.Service.ManageFinance;
using PayMasta.Service.Okra;
using PayMasta.Service.Support;
using PayMasta.Service.TestInterView;
using PayMasta.Service.ThirdParty;
using PayMasta.Service.VerifyNin;
using PayMasta.Service.VirtualAccount;
using PayMasta.Service.ZealvendService;
//using PayMasta.Service.Account;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Unity;
using Unity.Lifetime;

namespace PayMasta.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = DI();
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
            name: "swagger_root",
            routeTemplate: "",
            defaults: null,
            constraints: null,
            handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger"));

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            //config.MessageHandlers.Add(new CustomLogHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
        private static UnityContainer DI()
        {
            var container = new UnityContainer();
            container.RegisterType<IAccountService, AccountService>(new HierarchicalLifetimeManager());
            container.RegisterType<IThirdParty, ThirdPartyService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEarningService, EarningService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBankTransferService, BankTransferService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOkraService, OkraService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICallBackService, CallBackService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICommonEmployerService, CommonEmployerService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISupportService, SupportService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFlutterWaveService, FlutterWaveService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICommonService, CommonService>(new HierarchicalLifetimeManager());
            container.RegisterType<IManageFinanceService, ManageFinanceService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmployeeTransactionService, EmployeeTransactionService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmployeesService, EmployeesService>(new HierarchicalLifetimeManager());
            container.RegisterType<IItexService, ItexService>(new HierarchicalLifetimeManager());
            container.RegisterType<IItexRechargeService, ItexRechargeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IVirtualAccountService, VirtualAccountService>(new HierarchicalLifetimeManager());
            container.RegisterType<IVerifyNinService, VerifyNinService>(new HierarchicalLifetimeManager());
            container.RegisterType<IZealvendBillService, ZealvendBillService>(new HierarchicalLifetimeManager());
            container.RegisterType<IInterView, InterView>(new HierarchicalLifetimeManager());
            container.RegisterType<ILidyaService, LidyaService>(new HierarchicalLifetimeManager());
            return container;
        }
    }
}
