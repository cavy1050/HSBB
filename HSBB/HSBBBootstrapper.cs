using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Unity;
using Prism.Ioc;
using MaterialDesignThemes.Wpf;
using HSBB.Views;
using HSBB.Services;

namespace HSBB
{
    public class HSBBBootstrapper: PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<RegisterView>("RegisterView");
            containerRegistry.RegisterForNavigation<QueryView>("QueryView");
            containerRegistry.RegisterSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>();

            containerRegistry.RegisterSingleton<IAppConfigController, AppConfigController>();
            containerRegistry.RegisterSingleton<ILogController, TextLogController>();

            containerRegistry.Register<IEntityCertificateController, EntityHealthController>("JKK");
            containerRegistry.Register<IEntityCertificateController, EntityMedicalInsuranceController>("YBK");

            containerRegistry.Register<IElectronicCertificateController, ElectronicYukangController>("YKM");
            containerRegistry.Register<IElectronicCertificateController, ElectronicHealthController>("JKM");
            containerRegistry.Register<IElectronicCertificateController, ElectronicMedicalInsuranceController>("YBM");

            containerRegistry.Register<IIDCardController, DKT10Controller>("DK_T10");
            containerRegistry.Register<IIDCardController, GWIController>("GWI");

            containerRegistry.Register<IDataBaseController, NetWorkDataBaseController>("NetWork");
            containerRegistry.Register<IDataBaseController, NativeDataBaseController>("Native");
        }
    }
}
