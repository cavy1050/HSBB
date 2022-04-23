using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public struct NationType
    {
        string nationCode;
        public string NationCode
        {
            get { return nationCode; }
            set { nationCode = value; }
        }

        string nationName;
        public string NationName
        {
            get { return nationName; }
            set { nationName = value; }
        }

        public NationType(string nationCodeArgs, string nationNameArgs)
        {
            this.nationCode = nationCodeArgs;
            this.nationName = nationNameArgs;
        }
    }
}
