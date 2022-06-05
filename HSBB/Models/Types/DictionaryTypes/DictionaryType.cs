using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    public class DictionaryType
    {
        public List<SexType> SexTypes { get; set; }
        public List<NationType> NationTypes { get; set; }
        public List<SpecimenCollectionType> SpecimenCollectionTypes { get; set; }
        public List<CategoryType> CategoryTypes { get; set; }
        public List<DetectionType> DetectionTypes { get; set; }
        public List<EntityCertificateType> EntityCertificateTypes { get; set; }
        public List<ElectronicCertificateType> ElectronicCertificateTypes { get; set; }
        public List<IDCardReaderType> IDCardReaderTypes { get; set; }
        public List<DataBaseServiceType> DataBaseServiceTypes { get; set; }
    }
}
