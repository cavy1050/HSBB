using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;
using Prism.Ioc;
using Prism.Commands;
using Prism.Regions;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using MaterialDesignThemes.Wpf;
using HSBB.Views;
using HSBB.Models;
using HSBB.Services;

namespace HSBB.ViewModels
{
    /// <summary>
    /// 视图模型类设计原则
    /// 1.视图模型不直接使用LogService功能，对于程序错误只在Snackbar中提示出现错误，错误详细信息则记录在日志中，
    /// 相应的各种服务类实现中不直接使用SnackbarMessageQueue功能.
    /// 2.服务类实现都暴露公共的返回值为布尔类型的方法，方法参数中可以指定是否需要返回外部类型参数(out关键字).
    /// 3.各层之间交互关系如下:视图=>视图模型=>模型=>服务，视图模型不直接使用服务类方法获取数据，模型负责将服务类方法获取的数据进行初步加工交
    /// 由视图模型调用。
    /// </summary>
    public class RegisterViewModel : BindableBase,INavigationAware
    {
        RegisterModel registerModel;
        public RegisterModel RegisterModel
        {
            get => registerModel;
            set => SetProperty(ref registerModel, value);
        }

        ISnackbarMessageQueue messageQueue;
        public ISnackbarMessageQueue MessageQueue
        {
            get => messageQueue;
            set => SetProperty(ref messageQueue, value);
        }

        IContainerProvider containerProvider;
        IRegionManager regionManager;
        IDataBaseController dataBaseController;

        public DelegateCommand WindowLoadedCommand { get; private set; }
        public DelegateCommand NavigateToQueryCommand { get; private set; }
        public DelegateCommand ConfigSaveCommand { get; private set; }
        public DelegateCommand ReadEntityCertificateCommand { get; private set; }
        public DelegateCommand ReadEctronicCertificateCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CleanUpCommand { get; private set; }
        public DelegateCommand<object> CloseCommand { get; private set; }
        public DelegateCommand ReadIDCardCommand { get; private set; }
        public DelegateCommand<object[]> DetectionTypesAddedItemsCommand { get; private set; }
        public DelegateCommand<object[]> DetectionTypesRemovedItemsCommand { get; private set; }

        public RegisterViewModel(IContainerProvider containerProviderArgs)
        {
            this.containerProvider = containerProviderArgs;
            this.RegisterModel = new RegisterModel(containerProviderArgs);          
            this.regionManager = containerProviderArgs.Resolve<IRegionManager>();
            this.MessageQueue = containerProviderArgs.Resolve<ISnackbarMessageQueue>();

            WindowLoadedCommand = new DelegateCommand(OnWindowLoaded);
            NavigateToQueryCommand = new DelegateCommand(OnNavigateToQuery);
            ConfigSaveCommand = new DelegateCommand(OnConfigSave);
            ReadEntityCertificateCommand = new DelegateCommand(OnReadEntityCertificate);
            ReadEctronicCertificateCommand = new DelegateCommand(OnReadEctronicCertificate);
            SaveCommand = new DelegateCommand(OnSave);
            CleanUpCommand = new DelegateCommand(OnCleanUp);
            CloseCommand = new DelegateCommand<object>(OnClose);
            ReadIDCardCommand = new DelegateCommand(OnReadIDCard);
            DetectionTypesAddedItemsCommand = new DelegateCommand<object[]>(OnDetectionTypesAddedItems);
            DetectionTypesRemovedItemsCommand = new DelegateCommand<object[]>(OnDetectionTypesRemovedItems);
        }

        private void OnWindowLoaded()
        {
            RegisterModel.LoadDictionaryData();
        }

        private void OnReadEntityCertificate()
        {
            string currentEntityCertificateType = RegisterModel.CurrentEntityCertificateType.EntityCertificateCode;

            if (currentEntityCertificateType == "JKK")
                messageQueue.Enqueue("暂不支持当前实体凭证类型,请重新选择!");
            else
            {
                if (!RegisterModel.TransformServiceData(CertificateEnum.EntityCertificate))
                    messageQueue.Enqueue("读取实体医保卡信息错误,详细信息请查阅日志!");
            }
        }

        private async void OnReadEctronicCertificate()
        {
            string currentElectronicCertificateType = RegisterModel.CurrentElectronicCertificateType.ElectronicCertificateCode;

            if (currentElectronicCertificateType == "YBM")
            {
                if (!RegisterModel.TransformServiceData(CertificateEnum.ElectronicCertificate,string.Empty))
                    messageQueue.Enqueue("读取电子医保码信息错误,详细信息请查阅日志!");
            }             
            else
            {
                var view = new CertificateView();
                var result = await DialogHost.Show(view, "MainDialog", CertificateClosingEventHandler);
            }
        }

        private void CertificateClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            string currentElectronicCertificateString = (string)eventArgs.Parameter;

            if (!string.IsNullOrEmpty(currentElectronicCertificateString))
            {
                if (!RegisterModel.TransformServiceData(CertificateEnum.ElectronicCertificate, currentElectronicCertificateString))
                    messageQueue.Enqueue("读取电子凭证码信息错误,详细信息请查阅日志!");
            }
        }

        private void OnNavigateToQuery()
        {
            regionManager.RequestNavigate("MainRegion", "QueryView");
        }

        private async void OnConfigSave()
        {
            if (!registerModel.SaveConfigData())
                messageQueue.Enqueue("保存配置信息失败!");
            else
            {
                string currentDataBaseServiceType = registerModel.CurrentDataBaseServiceType.DataBaseServiceCode;
                if (!string.IsNullOrEmpty(currentDataBaseServiceType))
                {
                    if (currentDataBaseServiceType == "NetWork")
                    {
                        if (registerModel.FetchSynchronizeData() != 0)
                        {
                            var view = new SynchronizeView();
                            var result = await DialogHost.Show(view, "MainDialog");
                        }
                    }
                }
            }
        }

        private void OnReadIDCard()
        {
            if (!RegisterModel.TransformServiceData(CertificateEnum.IDCard))
                messageQueue.Enqueue("读取身份证信息错误,详细信息请查阅日志!");
        }

        private void OnClose(object obj)
        {
            SystemCommands.CloseWindow((Window)obj);
        }

        private void OnDetectionTypesAddedItems(object[] addedItems)
        {
            if (addedItems != null && addedItems.Count() > 0)
            {
                foreach (DetectionType item in addedItems)
                {
                    RegisterModel.CurrentDetectionType.Add(item);
                }
            }
        }

        private void OnDetectionTypesRemovedItems(object[] removedItems)
        {
            if (removedItems != null && removedItems.Count() > 0)
            {
                foreach (DetectionType item in removedItems)
                {
                    RegisterModel.CurrentDetectionType.Remove(item);
                }
            }
        }

        private void OnSave()
        {
            Validator<RegisterModel> registerModelValidator = ValidationFactory.CreateValidator<RegisterModel>();
            ValidationResults results = registerModelValidator.Validate(RegisterModel);
            if (!results.IsValid)
            {
                foreach (ValidationResult item in results)
                {
                    messageQueue.Enqueue(item.Message);
                }
            }
            else
            {
                if (!registerModel.SaveData(RegisterModel))
                    messageQueue.Enqueue("保存数据失败!");
                else
                    messageQueue.Enqueue("保存数据成功!");
            }
        }

        private void OnCleanUp()
        {
            RegisterModel.ClearUp();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}