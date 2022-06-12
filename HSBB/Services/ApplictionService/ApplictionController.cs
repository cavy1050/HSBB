using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using Prism.Ioc;
using MaterialDesignThemes.Wpf;
using HSBB.Models;

namespace HSBB.Services
{
    public class ApplictionController : IApplictionController
    {
        Configuration configuration;
        ISnackbarMessageQueue messageQueue;

        public bool IsValidateSucceed { get; set; }
        public EnvironmentType EnvironmentSetting { get; set; }
        public ConfigSet ConfigSettings { get; set; }

        public ApplictionController(IContainerProvider containerProviderArgs)
        {
            EnvironmentSetting = new EnvironmentType();
            ConfigSettings = new ConfigSet();

            messageQueue = containerProviderArgs.Resolve<ISnackbarMessageQueue>();

            if (Validate())
            {
                Load();
            }
        }

        private bool Validate()
        {
            IsValidateSucceed = false;

            EnvironmentSetting.ApplictionConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "HSBB.config";
            if (!File.Exists(EnvironmentSetting.ApplictionConfigFilePath))
            {
                messageQueue.Enqueue("当前程序环境缺少配置文件,请检查配置并重启程序!");
                return false;
            }

            EnvironmentSetting.NativeDataBaseFilePath = AppDomain.CurrentDomain.BaseDirectory + "HSBB.db";
            if (!File.Exists(EnvironmentSetting.NativeDataBaseFilePath))
            {
                messageQueue.Enqueue("当前程序环境缺少本地数据库文件,请检查配置并重启程序!");
                return false;
            }

            EnvironmentSetting.TextLogFilePath= AppDomain.CurrentDomain.BaseDirectory + "HSBB.log";
            EnvironmentSetting.ExportXlsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\HSBB.xls";

            IsValidateSucceed = true;
            return true;
        }

        private void Load()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = EnvironmentSetting.ApplictionConfigFilePath;
            configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            KeyValueConfigurationCollection keyValueConfigurationCollection = configuration.AppSettings.Settings;
            foreach (KeyValueConfigurationElement keyValueConfigurationElement in keyValueConfigurationCollection)
            {
                ConfigSettings.Add(new ConfigType
                {
                    ConfigCode = keyValueConfigurationElement.Key,
                    ConfigValue = keyValueConfigurationElement.Value
                });
            }
        }

        public void Save(ConfigSet configSetArgs)
        {
            if (IsValidateSucceed)
            {
                foreach (ConfigType configType in configSetArgs)
                {
                    if (configuration.AppSettings.Settings.AllKeys.Contains(configType.ConfigCode))
                        configuration.AppSettings.Settings[configType.ConfigCode].Value = configType.ConfigValue;
                    else
                        configuration.AppSettings.Settings.Add(configType.ConfigCode, configType.ConfigValue);
                }

                configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
}