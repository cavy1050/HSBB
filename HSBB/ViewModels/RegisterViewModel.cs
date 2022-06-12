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
        IApplictionController applictionController;
        IEntityCertificateController entityCertificateController;
        IElectronicCertificateController electronicCertificateController;
        IIDCardController idCardController;
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
            applictionController = containerProvider.Resolve<IApplictionController>();
            RegisterModel.LoadDictionaryData();
        }

        private void OnReadEntityCertificate()
        {
            string currentEntityCertificateType = RegisterModel.CurrentEntityCertificateType.EntityCertificateCode;

            if (currentEntityCertificateType == "JKK")
                messageQueue.Enqueue("暂不支持当前实体凭证类型,请重新选择!");
            else
            {
                this.entityCertificateController = containerProvider.Resolve<IEntityCertificateController>(currentEntityCertificateType);
                RegisterModel.TransformData<MedicalInsuranceTransferredType>(entityCertificateController.Load<MedicalInsuranceTransferredType>());
            }
        }

        private async void OnReadEctronicCertificate()
        {
            string currentElectronicCertificateType = RegisterModel.CurrentElectronicCertificateType.ElectronicCertificateCode;

            if (currentElectronicCertificateType == "YBM")
            {
                electronicCertificateController = containerProvider.Resolve<IElectronicCertificateController>(currentElectronicCertificateType);
                RegisterModel.TransformData<MedicalInsuranceTransferredType>(electronicCertificateController.Load<MedicalInsuranceTransferredType>(""));
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
                if (currentElectronicCertificateString.Contains("outTime") || currentElectronicCertificateString.Contains("5000A0003"))
                {
                    string currentElectronicCertificateType = RegisterModel.CurrentElectronicCertificateType.ElectronicCertificateCode;
                    if (!string.IsNullOrEmpty(currentElectronicCertificateType))
                    {
                        this.electronicCertificateController = containerProvider.Resolve<IElectronicCertificateController>(currentElectronicCertificateType);
                        RegisterModel.TransformData<GovernmentAffairsTransferredType>(electronicCertificateController.Load<GovernmentAffairsTransferredType>(currentElectronicCertificateString));
                    }   
                }     
            }
        }

        private void OnNavigateToQuery()
        {
            regionManager.RequestNavigate("MainRegion", "QueryView");
        }

        private async void OnConfigSave()
        {
            applictionController.ConfigSettings["defaultEntityCertificateReaderType"] = RegisterModel.CurrentEntityCertificateType.EntityCertificateCode;
            applictionController.ConfigSettings["defaultIDCardReaderType"] = RegisterModel.CurrentIDCardReaderType.IDCardReaderCode;
            applictionController.ConfigSettings["defaultElectronicCertificateReaderType"] = RegisterModel.CurrentElectronicCertificateType.ElectronicCertificateCode;
            applictionController.ConfigSettings["defaultDataBaseServiceType"] = RegisterModel.CurrentDataBaseServiceType.DataBaseServiceCode;

            applictionController.Save(applictionController.ConfigSettings);
            messageQueue.Enqueue("保存设置成功!");

            string currentDataBaseServiceType = registerModel.CurrentDataBaseServiceType.DataBaseServiceCode;
            if(!string.IsNullOrEmpty(currentDataBaseServiceType))
            {
                if (currentDataBaseServiceType== "NetWork")
                {
                    List<WaitForSynchronizeType> waitForSynchronizeTypes = registerModel.FetchSynchronizeData();

                    if (waitForSynchronizeTypes.Count!=0)
                    {
                        applictionController.EnvironmentSetting.WaitForSynchronizeTypes = new Queue<WaitForSynchronizeType>(waitForSynchronizeTypes);

                        var view = new SynchronizeView();
                        var result = await DialogHost.Show(view, "MainDialog");
                    }     
                }
            }
        }

        private void OnReadIDCard()
        {
            string currentIDCardReaderType = RegisterModel.CurrentIDCardReaderType.IDCardReaderCode;

            if (!string.IsNullOrEmpty(currentIDCardReaderType))
            {
                idCardController = containerProvider.Resolve<IIDCardController>(currentIDCardReaderType);

                RegisterModel.TransformData<IDCardTransferredType>(idCardController.Load());
            }
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
            ValidationResults results = registerModelValidator.Validate(this.RegisterModel);
            if (!results.IsValid)
            {
                foreach (ValidationResult item in results)
                {
                    messageQueue.Enqueue(item.Message);
                }
            }
            else
            {
                string currentDataBaseServiceType = registerModel.CurrentDataBaseServiceType.DataBaseServiceCode;

                if (!string.IsNullOrEmpty(currentDataBaseServiceType))
                {
                    dataBaseController = containerProvider.Resolve<IDataBaseController>(currentDataBaseServiceType);

                    if (dataBaseController.Save(this.RegisterModel))
                    {
                        messageQueue.Enqueue("保存成功!");
                        RegisterModel.ClearUp();
                    }
                    else
                        messageQueue.Enqueue("保存失败!");
                }
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