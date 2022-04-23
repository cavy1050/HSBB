using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Services
{
    public interface IConfigService
    {
        string DefaultIDCardReaderType { get; set; }

        int DefaultRetryTimes { get; set; }

        bool SaveConfigInfo(int defaultRetryTimesArs, string defaultIDCardReaderTypeArgs);
    }
}