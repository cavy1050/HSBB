using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public struct SexType
    {
        string sexCode;
        public string SexCode
        {
            get { return sexCode; }
            set { sexCode = value; }
        }

        string sexName;
        public string SexName
        {
            get { return sexName; }
            set { sexName = value; }
        }

        public SexType (string sexCodeArgs,string sexNameArgs)
        {
            this.sexCode = sexCodeArgs;
            this.sexName = sexNameArgs;
        }
    }
}
