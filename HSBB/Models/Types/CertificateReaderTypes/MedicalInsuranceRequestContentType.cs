using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public class MedicalInsuranceRequestContentType : IMedicalInsuranceRequestContentType
    {
        public MedicalInsuranceRequestDetailType data { get; set; }

        public MedicalInsuranceRequestContentType(SerialNnumberEnum serialNnumberEnumArgs)
        {
            data = new MedicalInsuranceRequestDetailType(serialNnumberEnumArgs);
        }
    }
}
