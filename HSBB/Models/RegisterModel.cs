using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;
using Prism.Mvvm;
using Prism.Ioc;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using HSBB.Services;

namespace HSBB.Models
{
    public class RegisterModel : BindableBase, IDataErrorInfo
    {
        ObservableCollection<EntityCertificateType> entityCertificateTypes;
        public ObservableCollection<EntityCertificateType> EntityCertificateTypes
        {
            get => entityCertificateTypes;
            set => SetProperty(ref entityCertificateTypes, value);
        }

        EntityCertificateType currentEntityCertificateType;
        public EntityCertificateType CurrentEntityCertificateType
        {
            get => currentEntityCertificateType;
            set => SetProperty(ref currentEntityCertificateType, value);
        }

        ObservableCollection<ElectronicCertificateType> electronicCertificateTypes;
        public ObservableCollection<ElectronicCertificateType> ElectronicCertificateTypes
        {
            get => electronicCertificateTypes;
            set => SetProperty(ref electronicCertificateTypes, value);
        }

        ElectronicCertificateType currentElectronicCertificateType;
        public ElectronicCertificateType CurrentElectronicCertificateType
        {
            get => currentElectronicCertificateType;
            set => SetProperty(ref currentElectronicCertificateType, value);
        }

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

        ObservableCollection<DataBaseServiceType> dataBaseServiceTypes;
        public ObservableCollection<DataBaseServiceType> DataBaseServiceTypes
        {
            get => dataBaseServiceTypes;
            set => SetProperty(ref dataBaseServiceTypes, value);
        }

        DataBaseServiceType currentDataBaseServiceType;
        public DataBaseServiceType CurrentDataBaseServiceType
        {
            get => currentDataBaseServiceType;
            set => SetProperty(ref currentDataBaseServiceType, value);
        }

        string name;
        [NotNullValidator(MessageTemplate = "姓名不能为空!")]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        string sex;
        public string Sex
        {
            get => sex;
            set
            {
                sex = value;
                this.CurrentSexType = SexTypes.FirstOrDefault(x => x.SexCode == sex);
            }
        }

        ObservableCollection<SexType> sexTypes;
        public ObservableCollection<SexType> SexTypes
        {
            get => sexTypes;
            set => SetProperty(ref sexTypes, value);
        }

        SexType currentSexType;
        public SexType CurrentSexType
        {
            get => currentSexType;
            set => SetProperty(ref currentSexType, value);
        }

        string nation;
        public string Nation
        {
            get => nation;
            set
            {
                nation = value;
                this.CurrentNationType = NationTypes.FirstOrDefault(x => x.NationCode == nation);
            }
        }

        ObservableCollection<NationType> nationTypes;
        public ObservableCollection<NationType> NationTypes
        {
            get => nationTypes;
            set => SetProperty(ref nationTypes, value);
        }

        NationType currentNationType;
        public NationType CurrentNationType
        {
            get => currentNationType;
            set => SetProperty(ref currentNationType, value);
        }

        string birthDay;
        public string BirthDay
        {
            get => birthDay;
            set
            {
                birthDay = value;

                DateTime dateTimeBirthDay = DateTime.ParseExact(birthDay, "yyyy-MM-dd", CultureInfo.CurrentCulture);
                DateTime dateTimeNow = DateTime.Now;

                this.Age = dateTimeNow.Year - dateTimeBirthDay.Year;
            }
        }

        int age;
        public int Age
        {
            get => age;
            set => SetProperty(ref age, value);
        }

        string address;
        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        string idNumber;
        [RegexValidator(@"(^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$)|(^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{2}$)", 
            MessageTemplate = "身份证号不符合15或18位规则.")]
        public string IDNumber
        {
            get => idNumber;
            set => SetProperty(ref idNumber, value);
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

        string photo;
        public string Photo
        {
            get => photo;
            set => SetProperty(ref photo, value);
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

        ObservableCollection<CategoryType> categoryTypes;
        public ObservableCollection<CategoryType> CategoryTypes
        {
            get => categoryTypes;
            set => SetProperty(ref categoryTypes, value);
        }

        CategoryType currentCategoryType;
        public CategoryType CurrentCategoryType
        {
            get => currentCategoryType;
            set => SetProperty(ref currentCategoryType, value);
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

        public string Error
        {
            get => string.Empty;
        }

        public string this[string propertyName]
        {
            get
            {
                if (propertyName == "Name" && string.IsNullOrEmpty(Name))
                    return "姓名不能为空!";
                else if (propertyName == "IDNumber" && string.IsNullOrEmpty(IDNumber))
                    return "身份证号不能为空!";

                return string.Empty;
            }
        }

        IContainerProvider containerProvider;
        IAppConfigController appConfigController;
        NativeDataBaseController nativeDataBaseController;

        public RegisterModel(IContainerProvider containerProviderArgs)
        {
            this.containerProvider = containerProviderArgs;
            appConfigController = containerProvider.Resolve<IAppConfigController>();
            nativeDataBaseController = (NativeDataBaseController)containerProvider.Resolve<IDataBaseController>("Native");

            CurrentDetectionType = new ObservableCollection<DetectionType>();
        }

        public void LoadDictionaryData()
        {
            DictionaryType dictionaryType = nativeDataBaseController.LoadDictionaryData();
            this.SexTypes = new ObservableCollection<SexType>(dictionaryType.SexTypes);
            this.NationTypes = new ObservableCollection<NationType>(dictionaryType.NationTypes);
            this.SpecimenCollectionTypes = new ObservableCollection<SpecimenCollectionType>(dictionaryType.SpecimenCollectionTypes);
            this.CategoryTypes = new ObservableCollection<CategoryType>(dictionaryType.CategoryTypes);
            this.DetectionTypes = new ObservableCollection<DetectionType>(dictionaryType.DetectionTypes);
            this.IDCardReaderTypes = new ObservableCollection<IDCardReaderType>(dictionaryType.IDCardReaderTypes);
            this.ElectronicCertificateTypes = new ObservableCollection<ElectronicCertificateType>(dictionaryType.ElectronicCertificateTypes);
            this.EntityCertificateTypes = new ObservableCollection<EntityCertificateType>(dictionaryType.EntityCertificateTypes);
            this.DataBaseServiceTypes = new ObservableCollection<DataBaseServiceType>(dictionaryType.DataBaseServiceTypes);

            CurrentEntityCertificateType = EntityCertificateTypes.FirstOrDefault(x => x.EntityCertificateCode == appConfigController.AppConfigSetting["defaultEntityCertificateReaderType"]);
            CurrentElectronicCertificateType = ElectronicCertificateTypes.FirstOrDefault(x => x.ElectronicCertificateCode == appConfigController.AppConfigSetting["defaultElectronicCertificateReaderType"]);
            CurrentIDCardReaderType = IDCardReaderTypes.FirstOrDefault(x => x.IDCardReaderCode == appConfigController.AppConfigSetting["defaultIDCardReaderType"]);
            CurrentDataBaseServiceType = DataBaseServiceTypes.FirstOrDefault(x => x.DataBaseServiceCode == appConfigController.AppConfigSetting["defaultDataBaseServiceType"]);
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
            this.Photo = null;
            this.CurrentSpecimenCollectionType = new SpecimenCollectionType { SpecimenCollectionCode = "2", SpecimenCollectionName = "口咽拭子" };
            this.CurrentCategoryType = new CategoryType { CategoryCode = "3",   CategoryName  = "无" };
            this.OccupationName = string.Empty;
            this.TravelTrajectory = string.Empty;
            this.Remarks = string.Empty;
        }

        public void TransformData<T>(T t) where T : class
        {
            if (t is GovernmentAffairsTransferredType)
            {
                GovernmentAffairsTransferredType value = t as GovernmentAffairsTransferredType;

                if (value != null)
                {
                    this.Name = value.Name;
                    this.Sex = SexTypes.FirstOrDefault(x => x.SexName == value.Sex).SexCode;
                    this.BirthDay = Convert.ToDateTime(value.BirthDay).Date.ToShortDateString();
                    this.Address = value.Address;
                    this.IDNumber = value.IDNumber;
                    this.PhoneNumber = value.PhoneNumber;
                }
            }
            else if (t is IDCardTransferredType)
            {
                IDCardTransferredType value = t as IDCardTransferredType;

                if (value != null)
                {
                    this.Name = value.Name;
                    this.Sex = value.Sex;
                    this.BirthDay = DateTime.ParseExact(value.BirthDay, "yyyyMMdd", CultureInfo.CurrentCulture).Date.ToShortDateString();
                    this.Address = value.Address;
                    this.IDNumber = value.IDNumber;
                    this.Photo = value.Photo;
                }
            }
            else if (t is MedicalInsuranceTransferredType)
            {
                MedicalInsuranceTransferredType value = t as MedicalInsuranceTransferredType;

                if (value != null)
                {
                    this.Name = value.Name;
                    this.Sex = value.Sex;
                    this.Nation = value.Nation;
                    this.BirthDay = value.BirthDay;
                    this.IDNumber = value.IDNumber;
                }
            }
        }

        public List<WaitForSynchronizeType> FetchSynchronizeData()
        {
            return nativeDataBaseController.FetchSynchronizeData();
        }
    }
}