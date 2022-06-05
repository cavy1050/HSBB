using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Commands;
using Prism.Regions;
using HSBB.Views;

namespace HSBB.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        IContainerProvider containerProvider;
        IRegionManager regionManager;

        public DelegateCommand WindowLoadedCommand { get; private set; }

        public ShellViewModel(IContainerProvider containerProviderArgs)
        {
            this.containerProvider = containerProviderArgs;       
            WindowLoadedCommand = new DelegateCommand(OnWindowLoaded);
        }

        private void OnWindowLoaded()
        {
            this.regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("MainRegion", typeof(RegisterView));
        }
    }
}
