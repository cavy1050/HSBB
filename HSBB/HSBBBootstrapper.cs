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
            return Container.Resolve<ShellWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ISnackbarMessageQueue, SnackbarMessageQueue>();

            containerRegistry.Register<IIDCardReaderService, IDCardReader_DKT10>("DK_T10");
            containerRegistry.Register<IIDCardReaderService, IDCardReader_GWI>("GWI");

            containerRegistry.RegisterSingleton<IConfigService, ConfigService>();
        }
    }
}
