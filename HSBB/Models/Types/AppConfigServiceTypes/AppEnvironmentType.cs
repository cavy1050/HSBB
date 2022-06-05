using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public class AppEnvironmentType
    {
        public string AppConfigFilePath { get; set; }
        public string NativeDataBaseFilePath { get; set; }
        public string ExportXlsFilePath { get; set; }
        public string TextLogFilePath { get; set; }

        public Queue<WaitForSynchronizeType> WaitForSynchronizeTypes { get; set; }
    }
}
