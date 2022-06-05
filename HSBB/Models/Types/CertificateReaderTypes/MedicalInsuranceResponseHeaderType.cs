using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public class MedicalInsuranceResponseHeaderType
    {
        public string infcode { get; set; }
        public string inf_refmsgid { get; set; }
        public string refmsg_time { get; set; }
        public string respond_time { get; set; }
        public string enctype { get; set; }
        public string signtype { get; set; }
        public string err_msg { get; set; }
        public MedicalInsuranceResponseContentType output { get; set; }
    }
}