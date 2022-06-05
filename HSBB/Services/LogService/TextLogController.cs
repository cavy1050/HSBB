using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Prism.Ioc;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace HSBB.Services
{
    public class TextLogController : ILogController
    {
        IAppConfigController appConfigController;
        LogWriter logWriter;      

        public TextLogController(IContainerProvider containerProviderArgs)
        {
            appConfigController = containerProviderArgs.Resolve<IAppConfigController>();

            string textLogFilePath = appConfigController.AppEnvironmentSetting.TextLogFilePath;

            TextFormatter textFormatter = new TextFormatter("{timestamp(local)}{tab}{message}");
            FlatFileTraceListener flatFileTraceListener = new FlatFileTraceListener(textLogFilePath, null, null, textFormatter);
            List<TraceListener> traceListeners = new List<TraceListener>();
            traceListeners.Add(flatFileTraceListener);

            LogEnabledFilter logEnabledFilter = new LogEnabledFilter("TextFileFilter", true);
            List<ILogFilter> logFilters = new List<ILogFilter>();
            logFilters.Add(logEnabledFilter);

            LogSource generalLogSource = new LogSource("General", traceListeners, SourceLevels.All);
            LogSource allEventsLogSource = new LogSource("AllEvents", SourceLevels.All);
            LogSource UnprocessedLogSource = new LogSource("Unprocessed", SourceLevels.All);
            LogSource ErrorsLogSource = new LogSource("Errors", traceListeners,SourceLevels.All);
            List<LogSource> logSources = new List<LogSource>();
            logSources.Add(generalLogSource);
            logSources.Add(allEventsLogSource);
            logSources.Add(UnprocessedLogSource);
            logSources.Add(ErrorsLogSource);

            logWriter = new LogWriter(logFilters, logSources, ErrorsLogSource, "General");
        }

        public void WriteLog(string MessageArgs)
        {
            logWriter.Write(MessageArgs);
        }
    }
}