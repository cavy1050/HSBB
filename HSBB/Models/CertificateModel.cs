using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Mvvm;

namespace HSBB.Models
{
    public class CertificateModel : BindableBase
    {
        string electronicCertificateString;
        public string ElectronicCertificateString
        {
            get => electronicCertificateString;
            set => SetProperty(ref electronicCertificateString, value);
        }

        IContainerProvider containerProvider;

        public CertificateModel (IContainerProvider containerProviderArgs)
        {
            this.containerProvider = containerProviderArgs;
        }
    }
}
