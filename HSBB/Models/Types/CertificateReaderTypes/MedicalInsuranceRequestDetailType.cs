using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public class MedicalInsuranceRequestDetailType
    {
        public string orgId { get; set; }
        public string businessType { get; set; }
        public string operatorId { get; set; }
        public string operatorName { get; set; }
        public string officeId { get; set; }
        public string officeName { get; set; }
        public string deviceType { get; set; }

        public MedicalInsuranceRequestDetailType(SerialNnumberEnum serialNnumberEnumArgs)
        {
            orgId = "H50010603655";
            businessType = "01101";
            operatorId = "800020";
            operatorName = "杨迪";
            officeId = "01";
            officeName = "医保办";
            deviceType = "";
        }
    }
}
