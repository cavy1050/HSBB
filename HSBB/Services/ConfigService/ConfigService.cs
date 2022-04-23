using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace HSBB.Services
{
    public class ConfigService : IConfigService
    {
        static string appConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "HSBB.config";

        public string DefaultIDCardReaderType { get; set; }
        public int DefaultRetryTimes { get; set; }

        Configuration configuration;

        public ConfigService()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = appConfigFilePath;
            configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            DefaultIDCardReaderType = configuration.AppSettings.Settings["defaultIDCardReaderType"].Value;
            DefaultRetryTimes = int.Parse(configuration.AppSettings.Settings["defaultRetryTimes"].Value);
        }

        public bool SaveConfigInfo(int defaultRetryTimesArs, string defaultIDCardReaderTypeArgs)
        {
            bool ret = false;

            configuration.AppSettings.Settings["defaultIDCardReaderType"].Value = defaultIDCardReaderTypeArgs;
            configuration.AppSettings.Settings["defaultRetryTimes"].Value = defaultRetryTimesArs.ToString();
            configuration.Save(ConfigurationSaveMode.Modified);

            ret = true;
            return ret;
        }
    }
}