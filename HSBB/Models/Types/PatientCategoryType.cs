using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public struct PatientCategoryType
    {
        string patientCategoryCode;
        public string PatientCategoryCode
        {
            get { return patientCategoryCode; }
            set { patientCategoryCode = value; }
        }

        string patientCategoryName;
        public string PatientCategoryName
        {
            get { return patientCategoryName; }
            set { patientCategoryName = value; }
        }

        public PatientCategoryType(string patientCategoryCodeArgs, string patientCategoryNameArgs)
        {
            this.patientCategoryCode = patientCategoryCodeArgs;
            this.patientCategoryName = patientCategoryNameArgs;
        }
    }
}
