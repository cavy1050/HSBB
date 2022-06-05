using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Commands;
using MaterialDesignThemes.Wpf;
using HSBB.Models;
using HSBB.Services;

namespace HSBB.ViewModels
{
    public class CertificateViewModel : BindableBase
    {
        CertificateModel certificateModel;
        public CertificateModel CertificateModel
        {
            get => certificateModel;
            set => SetProperty(ref certificateModel, value);
        }

        IContainerProvider containerProvider;

        public DelegateCommand<object> ElectronicCertificateTextChangedCommand { get; private set; }

        object currentWindow;
        Timer timer;

        public CertificateViewModel(IContainerProvider containerProviderArgs)
        {
            this.containerProvider = containerProviderArgs;
            this.CertificateModel = new CertificateModel(containerProviderArgs);
            ElectronicCertificateTextChangedCommand = new DelegateCommand<object>(OnElectronicCertificateTextChangedCommand);

            currentWindow = new object();
            timer = new Timer();
        }

        private void OnElectronicCertificateTextChangedCommand(object obj)
        {
            string currentElectronicCertificateString = CertificateModel.ElectronicCertificateString;

            if (!string.IsNullOrEmpty(currentElectronicCertificateString))
            {
                timer.Interval = 5000;
                timer.Elapsed += Timer_Elapsed;
                timer.AutoReset = false;
                timer.Start();

                currentWindow = obj;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string currentElectronicCertificateString = CertificateModel.ElectronicCertificateString;

            if (!string.IsNullOrEmpty(currentElectronicCertificateString))
            {
                timer.Stop();

                Window window = currentWindow as Window;
                window.Dispatcher.Invoke(new Action(delegate
                {
                    DialogHost.CloseDialogCommand.Execute(currentElectronicCertificateString, null);
                }));
            }
        }
    }
}