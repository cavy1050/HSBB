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
    public class AppConfigController : IAppConfigController
    {
        Configuration configuration;
        ISnackbarMessageQueue messageQueue;

        public bool IsValidateSucceed { get; set; }
        public AppEnvironmentType AppEnvironmentSetting { get; set; }
        public AppConfigSet AppConfigSetting { get; set; }

        public AppConfigController(IContainerProvider containerProviderArgs)
        {
            AppEnvironmentSetting = new AppEnvironmentType();
            AppConfigSetting = new AppConfigSet();

            messageQueue = containerProviderArgs.Resolve<ISnackbarMessageQueue>();

            if (Validate())
            {
                Load();
            }
        }

        private bool Validate()
        {
            IsValidateSucceed = false;

            AppEnvironmentSetting.AppConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "HSBB.config";
            if (!File.Exists(AppEnvironmentSetting.AppConfigFilePath))
            {
                messageQueue.Enqueue("当前程序环境缺少配置文件,请检查配置并重启程序!");
                return false;
            }

            AppEnvironmentSetting.NativeDataBaseFilePath = AppDomain.CurrentDomain.BaseDirectory + "HSBB.db";
            if (!File.Exists(AppEnvironmentSetting.NativeDataBaseFilePath))
            {
                messageQueue.Enqueue("当前程序环境缺少本地数据库文件,请检查配置并重启程序!");
                return false;
            }

            AppEnvironmentSetting.TextLogFilePath= AppDomain.CurrentDomain.BaseDirectory + "HSBB.log";
            AppEnvironmentSetting.ExportXlsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\HSBB.xls";

            IsValidateSucceed = true;
            return true;
        }

        private void Load()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = AppEnvironmentSetting.AppConfigFilePath;
            configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            KeyValueConfigurationCollection keyValueConfigurationCollection = configuration.AppSettings.Settings;
            foreach (KeyValueConfigurationElement keyValueConfigurationElement in keyValueConfigurationCollection)
            {
                AppConfigSetting.Add(new AppConfigType
                {
                    AppConfigCode = keyValueConfigurationElement.Key,
                    AppConfigValue = keyValueConfigurationElement.Value
                });
            }
        }

        public void Save(AppConfigSet appConfigSetArgs)
        {
            if (IsValidateSucceed)
            {
                foreach (AppConfigType appConfigType in appConfigSetArgs)
                {
                    if (configuration.AppSettings.Settings.AllKeys.Contains(appConfigType.AppConfigCode))
                        configuration.AppSettings.Settings[appConfigType.AppConfigCode].Value = appConfigType.AppConfigValue;
                    else
                        configuration.AppSettings.Settings.Add(appConfigType.AppConfigCode, appConfigType.AppConfigValue);
                }

                configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
}