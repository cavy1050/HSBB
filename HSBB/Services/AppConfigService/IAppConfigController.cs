using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSBB.Models;

namespace HSBB.Services
{
    public interface IAppConfigController
    {
        bool IsValidateSucceed { get; set; }
        AppEnvironmentType AppEnvironmentSetting { get; set; }
        AppConfigSet AppConfigSetting { get; set; }

        void Save(AppConfigSet appConfigCollectionArs);
    }
}