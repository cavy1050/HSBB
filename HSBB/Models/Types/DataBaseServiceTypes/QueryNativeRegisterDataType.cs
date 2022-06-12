using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public class QueryNativeRegisterDataType
    {
        public int PageID { get; set; }
        public int RowID { get; set; }
        public Int64 SerialNumber { get; set; }
        public string Name { get; set; }
        public string SexName { get; set; }
        public string NationName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string IDNumber { get; set; }
        public string CurrentAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string SpecimenCollectionName { get; set; }
        public string CategoryName { get; set; }
        public string DetectionName { get; set; }
        public string OccupationName { get; set; }
        public string TravelTrajectory { get; set; }
        public string Remarks { get; set; }
        public string SamplingLocation { get; set; }
        public string SamplingPerson { get; set; }
        public string CreateDateTime { get; set; }
        public string IsSynchronized { get; set; }
        public string SynchronizeDateTime { get; set; }
        public Int64 NetWorkSerialNumber { get; set; }
    }
}
