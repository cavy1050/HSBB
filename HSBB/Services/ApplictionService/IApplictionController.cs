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
        EnvironmentType EnvironmentSetting { get; set; }
        ConfigSet ConfigSettings { get; set; }

        bool Load();
        bool Save(ConfigSet configSetArgs);
    }
}