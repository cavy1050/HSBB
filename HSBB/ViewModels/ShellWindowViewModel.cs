using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Commands;
using MaterialDesignThemes.Wpf;
using HSBB.Models;
using HSBB.Services;

namespace HSBB.ViewModels
{
    public class ShellWindowViewModel : BindableBase
    {
        ShellModel shellModel;
        public ShellModel ShellModel
        {
            get => shellModel;
            set => SetProperty(ref shellModel, value);
        }

        ISnackbarMessageQueue messageQueue;
        public ISnackbarMessageQueue MessageQueue
        {
            get => messageQueue;
            set => SetProperty(ref messageQueue, value);
        }

        IContainerProvider containerProvider;
        IConfigService configService;
        IIDCardReaderService idCardReaderService;
        IDataBaseService dataBaseService;

        public DelegateCommand ReadIDCardCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CleanUpCommand { get; private set; }
        public DelegateCommand<object> CloseCommand { get; private set; }
        public DelegateCommand ConfigSaveCommand { get; private set; }

        public DelegateCommand WindowLoadedCommand { get; private set; }
        public DelegateCommand WindowClosedCommand { get; private set; }

        public DelegateCommand<object[]> DetectionTypesAddedItemsCommand { get; private set; }
        public DelegateCommand<object[]> DetectionTypesRemovedItemsCommand { get; private set; }

        public ShellWindowViewModel(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;

            this.configService = containerProvider.Resolve<IConfigService>();

            this.ShellModel = new ShellModel(containerProvider);
            this.ShellModel.PropertyChanged += ShellModel_PropertyChanged;

            this.idCardReaderService = containerProvider.Resolve<IIDCardReaderService>(configService.DefaultIDCardReaderType);
            this.dataBaseService = new DataBaseService();

            MessageQueue = containerProvider.Resolve<ISnackbarMessageQueue>();

            ReadIDCardCommand = new DelegateCommand(OnReadIDCard, OnCanReadIDCard);
            SaveCommand = new DelegateCommand(OnSave,OnCanSave);
            CleanUpCommand = new DelegateCommand(OnCleanUp, OnCanCleanUp);
            CloseCommand = new DelegateCommand<object>(OnClose);

            WindowLoadedCommand = new DelegateCommand(OnWindowLoaded);
            WindowClosedCommand = new DelegateCommand(OnWindowClosed);

            DetectionTypesAddedItemsCommand = new DelegateCommand<object[]>(OnDetectionTypesAddedItems);
            DetectionTypesRemovedItemsCommand = new DelegateCommand<object[]>(OnDetectionTypesRemovedItems);

            ConfigSaveCommand=new DelegateCommand(OnConfigSave);
        }

        private void OnConfigSave()
        {
            if(configService.SaveConfigInfo(ShellModel.RetryTimes, ShellModel.CurrentIDCardReaderType.IDCardReaderCode))
                MessageQueue.Enqueue("保存设置成功!");
        }

        private void OnClose(object obj)
        {
            SystemCommands.CloseWindow((Window)obj);
        }

        private bool OnCanCleanUp()
        {
            return !string.IsNullOrEmpty(shellModel.Name) && !string.IsNullOrEmpty(shellModel.IDNumber);
        }

        private void ShellModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name"|| e.PropertyName == "IDNumber")
            {
                SaveCommand.RaiseCanExecuteChanged();
                CleanUpCommand.RaiseCanExecuteChanged();
            }    
        }

        private bool OnCanSave()
        {
            return !string.IsNullOrEmpty(shellModel.Name)&& !string.IsNullOrEmpty(shellModel.IDNumber);
        }

        private void OnDetectionTypesAddedItems(object[] addedItems)
        {
            if (addedItems != null && addedItems.Count() > 0)
            {
                foreach (DetectionType item in addedItems)
                {
                    ShellModel.CurrentDetectionType.Add(item);
                }
            }
        }

        private void OnDetectionTypesRemovedItems(object[] removedItems)
        {
            if (removedItems != null && removedItems.Count() > 0)
            {
                foreach (DetectionType item in removedItems)
                {
                    ShellModel.CurrentDetectionType.Remove(item);
                }
            }
        }

        private bool OnCanReadIDCard()
        {
            return idCardReaderService.InitIDCardReader();
        }

        private void OnWindowLoaded()
        {
            if (!idCardReaderService.InitIDCardReader())
                MessageQueue.Enqueue("初始化身份证设备失败!");
        }

        private void OnWindowClosed()
        {
            if (!idCardReaderService.ExitIDCardReader())
                MessageQueue.Enqueue("释放身份证资源失败!");
        }

        private void OnReadIDCard()
        {
            IDCardModel retIDCardModel = idCardReaderService.LoadIDCardReader();

            if (retIDCardModel!=null)
            {
                ShellModel.Name = retIDCardModel.Name;
                ShellModel.Sex = retIDCardModel.Sex;
                ShellModel.Nation = retIDCardModel.Nation;
                ShellModel.BirthDay = retIDCardModel.BirthDay;
                ShellModel.Address = retIDCardModel.Address;
                ShellModel.IDNumber = retIDCardModel.IDNumber;
                ShellModel.Department = retIDCardModel.Department;
                ShellModel.Photo = retIDCardModel.Photo;
            }
        }

        private void OnSave()
        {
            if (dataBaseService.SaveInfo(this.ShellModel))
            {
                MessageQueue.Enqueue("保存成功!");
                ShellModel.ClearUp();
            }      
            else
                MessageQueue.Enqueue("保存失败!");
        }

        private void OnCleanUp()
        {
            ShellModel.ClearUp();
        }
    }
}
