using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public class MedicalInsuranceRequestHeaderType
    {
        public string infno { get; set; }
        public string msgid { get; set; }
        public string mdtrtarea_admvs { get; set; }
        public string insuplc_admdvs { get; set; }
        public string recer_sys_code { get; set; }
        public string dev_no { get; set; }
        public string dev_safe_info { get; set; }
        public string cainfo { get; set; }
        public string signtype { get; set; }
        public string infver { get; set; }
        public string opter_type { get; set; }
        public string opter { get; set; }
        public string opter_name { get; set; }
        public string inf_time { get; set; }
        public string fixmedins_code { get; set; }
        public string fixmedins_name { get; set; }
        public string sign_no { get; set; }
        public IMedicalInsuranceRequestContentType input { get; set; }
        public string serv_code { get; set; }
        public string serv_sign { get; set; }

        DateTime dateTime;
        Random random;

        public MedicalInsuranceRequestHeaderType(SerialNnumberEnum  serialNnumberEnumArgs)
        {
            dateTime = DateTime.Now;
            random = new Random();

            infno = Convert.ToInt32(serialNnumberEnumArgs).ToString();
            msgid = "H50010603655" + dateTime.ToString("yyyyMMddHHmmss") + random.Next(10000).ToString();
            mdtrtarea_admvs = "500000";
            insuplc_admdvs = "500000";
            recer_sys_code = "YBXT";
            dev_no = "";
            dev_safe_info = "";
            cainfo = "";
            signtype = "";
            infver = "V1.0";
            opter_type = "1";
            opter = "800020";
            opter_name = "杨迪";
            inf_time = dateTime.ToString("G");
            fixmedins_code = "H50010603655";
            fixmedins_name = "重庆医科大学附属大学城医院";
            sign_no = "5425421";
            serv_code = "HH00004";
            serv_sign = "FA61C2E1C5F4C635D8";

            if (serialNnumberEnumArgs == SerialNnumberEnum.ReadEntityCertificate)
                input = new MedicalInsuranceRequestEmptyContentType();
            else
                input = new MedicalInsuranceRequestContentType(serialNnumberEnumArgs);
        }
    }
}
