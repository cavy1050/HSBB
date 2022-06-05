using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Mvvm;
using HSBB.Models;
using HSBB.Services;
using System.Diagnostics;

namespace HSBB.Models
{
    public class SynchronizeModel : BindableBase
    {
        public double processingProgress;
        public double ProcessingProgress
        {
            get => processingProgress;
            set => SetProperty(ref processingProgress, value);
        }

        IContainerProvider containerProvider;
        IAppConfigController appConfigController;
        NativeDataBaseController nativeDataBaseController;
        NetWorkDataBaseController networkDataBaseController;

        int totalQueueCount, currentQueueCount;

        public SynchronizeModel(IContainerProvider containerProviderArgs)
        {
            this.containerProvider = containerProviderArgs;
            this.appConfigController = containerProviderArgs.Resolve<IAppConfigController>();
        }

        public void SynchronizeData()
        {
            totalQueueCount = appConfigController.AppEnvironmentSetting.WaitForSynchronizeTypes.Count;
            int retVal = 0;

            for (int i = 0; i < totalQueueCount; i++)
            {
                WaitForSynchronizeType synchronizeType = appConfigController.AppEnvironmentSetting.WaitForSynchronizeTypes.Dequeue();

                networkDataBaseController = (NetWorkDataBaseController)containerProvider.Resolve<IDataBaseController>("NetWork");
                nativeDataBaseController = (NativeDataBaseController)containerProvider.Resolve<IDataBaseController>("Native");

                if (networkDataBaseController.SynchronizeData(synchronizeType, ref retVal))
                {
                    if (retVal != 0)
                    {
                        nativeDataBaseController.ConfirmSynchronizeResult(synchronizeType.SerialNumber, retVal);

                        currentQueueCount = appConfigController.AppEnvironmentSetting.WaitForSynchronizeTypes.Count;
                        ProcessingProgress = 100 - currentQueueCount / totalQueueCount * 100;
                    }
                }
            }
        }
    }
}