using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public struct SpecimenCollectionType
    {
        string specimenCollectionCode;
        public string SpecimenCollectionCode
        {
            get { return specimenCollectionCode; }
            set { specimenCollectionCode = value; }
        }

        string specimenCollectionName;
        public string SpecimenCollectionName
        {
            get { return specimenCollectionName; }
            set { specimenCollectionName = value; }
        }

        public SpecimenCollectionType(string specimenCollectionCodeArgs, string specimenCollectionNameArgs)
        {
            this.specimenCollectionCode = specimenCollectionCodeArgs;
            this.specimenCollectionName = specimenCollectionNameArgs;
        }
    }
}
