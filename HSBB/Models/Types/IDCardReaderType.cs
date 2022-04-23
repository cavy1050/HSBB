using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public struct IDCardReaderType
    {
        string idCardReaderCode;
        public string IDCardReaderCode
        {
            get { return idCardReaderCode; }
            set { idCardReaderCode = value; }
        }

        string idCardReaderName;
        public string IDCardReaderName
        {
            get { return idCardReaderName; }
            set { idCardReaderName = value; }
        }

        public IDCardReaderType(string idCardReaderCodeArgs, string idCardReaderNameArgs)
        {
            this.idCardReaderCode = idCardReaderCodeArgs;
            this.idCardReaderName = idCardReaderNameArgs;
        }
    }
}
