using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public class MedicalInsuranceResponseContentType
    {
        public List<MedicalInsuranceResponseDetailIdetinfoItemType> idetinfo { get; set; }
        public MedicalInsuranceResponseDetailBaseinfoType baseinfo { get; set; }
        public MedicalInsuranceResponseDetailCardecinfoType cardecinfo { get; set; }
        public List<MedicalInsuranceResponseDetailInsuinfoItemType> insuinfo { get; set; }
    }
}
