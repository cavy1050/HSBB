using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Commands;
using MaterialDesignThemes.Wpf;
using HSBB.Models;
using HSBB.Services;
using System.Diagnostics;

namespace HSBB.ViewModels
{
    public class SynchronizeViewModel : BindableBase
    {
        SynchronizeModel synchronizeModel;
        public SynchronizeModel SynchronizeModel
        {
            get => synchronizeModel;
            set => SetProperty(ref synchronizeModel, value);
        }

        ISnackbarMessageQueue messageQueue;

        public DelegateCommand WindowLoadedCommand { get; private set; }
        public DelegateCommand<object> ProgressBarValueChangedCommand { get; private set; }

        IContainerProvider containerProvider;

        Timer timer;

        public SynchronizeViewModel(IContainerProvider containerProviderArgs)
        {
            this.containerProvider = containerProviderArgs;
            this.SynchronizeModel = new SynchronizeModel(containerProviderArgs);
            this.messageQueue = containerProviderArgs.Resolve<ISnackbarMessageQueue>();

            WindowLoadedCommand = new DelegateCommand(OnWindowLoaded);
            ProgressBarValueChangedCommand = new DelegateCommand<object>(OnProgressBarValueChanged);        

            timer = new Timer();
        }

        private void OnWindowLoaded()
        {
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = false;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SynchronizeModel.SynchronizeData();
        }

        private void OnProgressBarValueChanged(object obj)
        {
            if (SynchronizeModel.ProcessingProgress == 100)
            {
                Window window = obj as Window;
                window.Dispatcher.Invoke(new Action(delegate
                {
                    DialogHost.CloseDialogCommand.Execute(null, null);
                }));

                messageQueue.Enqueue("同步成功!");
            }
        }
    }
}