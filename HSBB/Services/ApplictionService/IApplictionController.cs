using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSBB.Models;

namespace HSBB.Services
{
    public interface IApplictionController
    {
        bool IsValidateSucceed { get; set; }
        EnvironmentType EnvironmentSetting { get; set; }
        ConfigSet ConfigSettings { get; set; }

        void Save(ConfigSet configSetArgs);
    }
}