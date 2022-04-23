using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Prism.Ioc;
using HSBB.Services;

namespace HSBB.Models
{
    public class ShellModel : IDCardModel
    {
        IContainerProvider containerProvider;

        IConfigService configService;

        ObservableCollection<IDCardReaderType> idCardReaderTypes;
        public ObservableCollection<IDCardReaderType> IDCardReaderTypes
        {
            get => idCardReaderTypes;
            set => SetProperty(ref idCardReaderTypes, value);
        }

        IDCardReaderType currentIDCardReaderType;
        public IDCardReaderType CurrentIDCardReaderType
        {
            get => currentIDCardReaderType;
            set => SetProperty(ref currentIDCardReaderType, value);
        }

        int retryTimes;
        public int RetryTimes
        {
            get => retryTimes;
            set => SetProperty(ref retryTimes, value);
        }

        string currentAddress;
        public string CurrentAddress
        {
            get => currentAddress;
            set => SetProperty(ref currentAddress, value);
        }

        string phoneNumber;
        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        ObservableCollection<SpecimenCollectionType> specimenCollectionTypes;
        public ObservableCollection<SpecimenCollectionType> SpecimenCollectionTypes
        {
            get => specimenCollectionTypes;
            set => SetProperty(ref specimenCollectionTypes, value);
        }

        SpecimenCollectionType currentSpecimenCollectionType;
        public SpecimenCollectionType CurrentSpecimenCollectionType
        {
            get => currentSpecimenCollectionType;
            set => SetProperty(ref currentSpecimenCollectionType, value);
        }

        ObservableCollection<PatientCategoryType> patientCategoryTypes;
        public ObservableCollection<PatientCategoryType> PatientCategoryTypes
        {
            get => patientCategoryTypes;
            set => SetProperty(ref patientCategoryTypes, value);
        }

        PatientCategoryType currentPatientCategoryType;
        public PatientCategoryType CurrentPatientCategoryType
        {
            get => currentPatientCategoryType;
            set => SetProperty(ref currentPatientCategoryType, value);
        }

        ObservableCollection<DetectionType> detectionTypes;
        public ObservableCollection<DetectionType> DetectionTypes
        {
            get => detectionTypes;
            set => SetProperty(ref detectionTypes, value);
        }

        ObservableCollection<DetectionType> currentDetectionType;
        public ObservableCollection<DetectionType> CurrentDetectionType
        {
            get => currentDetectionType;
            set => SetProperty(ref currentDetectionType, value);
        }

        string occupationName;
        public string OccupationName
        {
            get => occupationName;
            set => SetProperty(ref occupationName, value);
        }

        string travelTrajectory;
        public string TravelTrajectory
        {
            get => travelTrajectory;
            set => SetProperty(ref travelTrajectory, value);
        }

        string remarks;
        public string Remarks
        {
            get => remarks;
            set => SetProperty(ref remarks, value);
        }

        string samplingLocation;
        public string SamplingLocation
        {
            get => samplingLocation;
            set => SetProperty(ref samplingLocation, value);
        }

        string samplingPerson;
        public string SamplingPerson
        {
            get => samplingPerson;  
            set => SetProperty(ref samplingPerson, value);
        }

        public ShellModel(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
            configService = containerProvider.Resolve<IConfigService>();

            this.SpecimenCollectionTypes = new ObservableCollection<SpecimenCollectionType>
            {
                new SpecimenCollectionType { SpecimenCollectionCode = "1", SpecimenCollectionName = "鼻咽拭子" },
                new SpecimenCollectionType { SpecimenCollectionCode = "2", SpecimenCollectionName = "口咽拭子" }
            };

            this.PatientCategoryTypes = new ObservableCollection<PatientCategoryType>
            {
                new PatientCategoryType { PatientCategoryCode = "1", PatientCategoryName = "密切接触者" },
                new PatientCategoryType { PatientCategoryCode = "2", PatientCategoryName = "疑似患者" },
                new PatientCategoryType { PatientCategoryCode = "3", PatientCategoryName = "无" }
            };

            this.DetectionTypes = new ObservableCollection<DetectionType>
            {
                new DetectionType { DetectionCode = "1", DetectionName = "门急诊" },
                new DetectionType { DetectionCode = "2", DetectionName = "住院患者" },
                new DetectionType { DetectionCode = "3", DetectionName = "陪护" },
                new DetectionType { DetectionCode = "4", DetectionName = "体检(自检)" },
                new DetectionType { DetectionCode = "5", DetectionName = "NCP筛查" },
                new DetectionType { DetectionCode = "6", DetectionName = "发热患者" }
            };

            this.IDCardReaderTypes = new ObservableCollection<IDCardReaderType>
            {
                new IDCardReaderType { IDCardReaderCode = "DK_T10", IDCardReaderName = "德卡T10" },
                new IDCardReaderType { IDCardReaderCode = "GWI", IDCardReaderName = "长城自助医疗" }
            };

            this.CurrentDetectionType = new ObservableCollection<DetectionType>();

            this.CurrentIDCardReaderType = IDCardReaderTypes.FirstOrDefault(x => x.IDCardReaderCode == configService.DefaultIDCardReaderType);
            this.RetryTimes = configService.DefaultRetryTimes;
        }

        public void ClearUp()
        {
            this.Name = string.Empty;
            this.CurrentSexType = new SexType { SexCode = "1", SexName = "男" };
            this.Age = 0;
            this.Address = string.Empty;
            this.IDNumber = string.Empty;
            this.CurrentAddress = string.Empty;
            this.PhoneNumber = string.Empty;


            this.CurrentSpecimenCollectionType = new SpecimenCollectionType 
            { 
                SpecimenCollectionCode = "2", 
                SpecimenCollectionName = "口咽拭子" 
            };

            this.CurrentPatientCategoryType = new PatientCategoryType
            {
                PatientCategoryCode = "3",
                PatientCategoryName = "无"
            };

            this.OccupationName = string.Empty;
            this.TravelTrajectory = string.Empty;
            this.Remarks = string.Empty;
        }
    }
}
