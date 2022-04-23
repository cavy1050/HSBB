using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public struct DetectionType
    {
        string detectionCode;
        public string DetectionCode
        {
            get { return detectionCode; }
            set { detectionCode = value; }
        }

        string detectionName;
        public string DetectionName
        {
            get { return detectionName; }
            set { detectionName = value; }
        }

        public DetectionType(string detectionCodeArgs, string detectionNameArgs)
        {
            this.detectionCode = detectionCodeArgs;
            this.detectionName = detectionNameArgs;
        }
    }
}
