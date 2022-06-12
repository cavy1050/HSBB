using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Linq;
using HSBB.Models;

namespace HSBB.Services
{
    public class NativeDataBaseController : IDataBaseController
    {
        IApplictionController applictionController;
        ILogController logController;

        bool isValidateSucceed;
        string sqlStatement;
        SQLiteConnection sqliteConnection;

        public NativeDataBaseController(IContainerProvider containerProviderArgs)
        {
            this.applictionController = containerProviderArgs.Resolve<IApplictionController>();
            logController = containerProviderArgs.Resolve<ILogController>();

            Validate();
        }

        private void Validate()
        {
            isValidateSucceed = false;
            string sqliteConnectionString = "Data Source= " + applictionController.EnvironmentSetting.NativeDataBaseFilePath;

            try
            {
                sqliteConnection = new SQLiteConnection(sqliteConnectionString);
                sqliteConnection.Open();          
            }
            catch 
            {
                logController.WriteLog("本地数据库连接错误,本地数据库连接字符串:" + sqliteConnectionString);
            }
            finally
            {
                sqliteConnection.Close();
            }

            isValidateSucceed = true;
        }

        public DictionaryType LoadDictionaryData()
        {
            DictionaryType retDictionaryType = new DictionaryType();
            retDictionaryType.SexTypes = new List<SexType>();
            retDictionaryType.NationTypes = new List<NationType>();
            retDictionaryType.SpecimenCollectionTypes = new List<SpecimenCollectionType>();
            retDictionaryType.CategoryTypes = new List<CategoryType>();
            retDictionaryType.DetectionTypes = new List<DetectionType>();
            retDictionaryType.IDCardReaderTypes = new List<IDCardReaderType>();
            retDictionaryType.ElectronicCertificateTypes = new List<ElectronicCertificateType>();
            retDictionaryType.EntityCertificateTypes = new List<EntityCertificateType>();
            retDictionaryType.DataBaseServiceTypes = new List<DataBaseServiceType>();

            if (isValidateSucceed)
            {
                sqliteConnection.Open();

                sqlStatement = "SELECT SexCode,SexName FROM SexTypes";
                SQLiteCommand sqliteCommand = new SQLiteCommand(sqlStatement, sqliteConnection);
                SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    retDictionaryType.SexTypes.Add(new SexType
                    {
                        SexCode = sqliteDataReader[0].ToString(),
                        SexName = sqliteDataReader[1].ToString()
                    });
                }
                sqliteDataReader.Close();

                sqlStatement = "SELECT NationCode,NationName FROM NationTypes";
                sqliteCommand.CommandText = sqlStatement;
                sqliteDataReader = sqliteCommand.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    retDictionaryType.NationTypes.Add(new NationType
                    {
                        NationCode = sqliteDataReader[0].ToString(),
                        NationName = sqliteDataReader[1].ToString()
                    });
                }
                sqliteDataReader.Close();

                sqlStatement = "SELECT SpecimenCollectionCode,SpecimenCollectionName FROM SpecimenCollectionTypes";
                sqliteCommand.CommandText = sqlStatement;
                sqliteDataReader = sqliteCommand.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    retDictionaryType.SpecimenCollectionTypes.Add(new SpecimenCollectionType
                    {
                        SpecimenCollectionCode = sqliteDataReader[0].ToString(),
                        SpecimenCollectionName = sqliteDataReader[1].ToString()
                    });
                }
                sqliteDataReader.Close();

                sqlStatement = "SELECT CategoryCode,CategoryName FROM CategoryTypes";
                sqliteCommand.CommandText = sqlStatement;
                sqliteDataReader = sqliteCommand.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    retDictionaryType.CategoryTypes.Add(new CategoryType
                    {
                        CategoryCode = sqliteDataReader[0].ToString(),
                        CategoryName = sqliteDataReader[1].ToString()
                    });
                }
                sqliteDataReader.Close();

                sqlStatement = "SELECT DetectionCode,DetectionName FROM DetectionTypes";
                sqliteCommand.CommandText = sqlStatement;
                sqliteDataReader = sqliteCommand.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    retDictionaryType.DetectionTypes.Add(new DetectionType
                    {
                        DetectionCode = sqliteDataReader[0].ToString(),
                        DetectionName = sqliteDataReader[1].ToString()
                    });
                }
                sqliteDataReader.Close();

                sqlStatement = "SELECT IDCardReaderCode,IDCardReaderName FROM IDCardReaderTypes";
                sqliteCommand.CommandText = sqlStatement;
                sqliteDataReader = sqliteCommand.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    retDictionaryType.IDCardReaderTypes.Add(new IDCardReaderType
                    {
                        IDCardReaderCode = sqliteDataReader[0].ToString(),
                        IDCardReaderName = sqliteDataReader[1].ToString()
                    });
                }
                sqliteDataReader.Close();

                sqlStatement = "SELECT ElectronicCertificateCode,ElectronicCertificateName FROM ElectronicCertificateTypes";
                sqliteCommand.CommandText = sqlStatement;
                sqliteDataReader = sqliteCommand.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    retDictionaryType.ElectronicCertificateTypes.Add(new ElectronicCertificateType
                    {
                        ElectronicCertificateCode = sqliteDataReader[0].ToString(),
                        ElectronicCertificateName = sqliteDataReader[1].ToString()
                    });
                }
                sqliteDataReader.Close();

                sqlStatement = "SELECT EntityCertificateCode,EntityCertificateName FROM EntityCertificateTypes";
                sqliteCommand.CommandText = sqlStatement;
                sqliteDataReader = sqliteCommand.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    retDictionaryType.EntityCertificateTypes.Add(new EntityCertificateType
                    {
                        EntityCertificateCode = sqliteDataReader[0].ToString(),
                        EntityCertificateName = sqliteDataReader[1].ToString()
                    });
                }
                sqliteDataReader.Close();

                sqlStatement = "SELECT DataBaseServiceCode,DataBaseServiceName FROM DataBaseServiceTypes";
                sqliteCommand.CommandText = sqlStatement;
                sqliteDataReader = sqliteCommand.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    retDictionaryType.DataBaseServiceTypes.Add(new DataBaseServiceType
                    {
                        DataBaseServiceCode = sqliteDataReader[0].ToString(),
                        DataBaseServiceName = sqliteDataReader[1].ToString()
                    });
                }
                sqliteDataReader.Close();

                sqliteConnection.Close();
            }

            return retDictionaryType;
        }

        public bool Save(RegisterModel registerModelArgs)
        {
            bool ret = false;

            if (isValidateSucceed)
            {
                sqliteConnection.Open();

                sqlStatement = "INSERT INTO PatientInfo " +
                "(Name,SexCode,SexName,NationCode,NationName,Age,Address,IDNumber,CurrentAddress,PhoneNumber,SpecimenCollectionCode,SpecimenCollectionName," +
                  "CategoryCode,CategoryName,DetectionCode,DetectionName,OccupationName,TravelTrajectory,Remarks,SamplingLocation,SamplingPerson,CreateDateTime," +
                    "IsSynchronized,SynchronizeDateTime,NetWorkSerialNumber," +
                      "EntityCertificateCode,EntityCertificateName,ElectronicCertificateCode,ElectronicCertificateName,IDCardReaderCode,IDCardReaderName) " +
                 "VALUES (@Name, @SexCode, @SexName, @NationCode, @NationName, @Age, @Address, @IDNumber, @CurrentAddress, @PhoneNumber, @SpecimenCollectionCode, @SpecimenCollectionName, " +
                   "@CategoryCode, @CategoryName, @DetectionCode, @DetectionName, @OccupationName, @TravelTrajectory, @Remarks, @SamplingLocation, @SamplingPerson, @CreateDateTime," +
                     "@IsSynchronized, @SynchronizeDateTime, @NetWorkSerialNumber," +
                       "@EntityCertificateCode, @EntityCertificateName, @ElectronicCertificateCode, @ElectronicCertificateName, @IDCardReaderCode, @IDCardReaderName)";

                SQLiteCommand sqliteCommand = new SQLiteCommand(sqlStatement, sqliteConnection);

                sqliteCommand.Parameters.AddWithValue("Name", registerModelArgs.Name);
                sqliteCommand.Parameters.AddWithValue("SexCode", registerModelArgs.CurrentSexType.SexCode);
                sqliteCommand.Parameters.AddWithValue("SexName", registerModelArgs.CurrentSexType.SexName);
                sqliteCommand.Parameters.AddWithValue("NationCode", registerModelArgs.CurrentNationType.NationCode);
                sqliteCommand.Parameters.AddWithValue("NationName", registerModelArgs.CurrentNationType.NationName);
                sqliteCommand.Parameters.AddWithValue("Age", registerModelArgs.Age);
                sqliteCommand.Parameters.AddWithValue("Address", registerModelArgs.Address);
                sqliteCommand.Parameters.AddWithValue("IDNumber", registerModelArgs.IDNumber);
                sqliteCommand.Parameters.AddWithValue("CurrentAddress", registerModelArgs.CurrentAddress);
                sqliteCommand.Parameters.AddWithValue("PhoneNumber", registerModelArgs.PhoneNumber);
                sqliteCommand.Parameters.AddWithValue("SpecimenCollectionCode", registerModelArgs.CurrentSpecimenCollectionType.SpecimenCollectionCode);
                sqliteCommand.Parameters.AddWithValue("SpecimenCollectionName", registerModelArgs.CurrentSpecimenCollectionType.SpecimenCollectionName);
                sqliteCommand.Parameters.AddWithValue("CategoryCode", registerModelArgs.CurrentCategoryType.CategoryCode);
                sqliteCommand.Parameters.AddWithValue("CategoryName", registerModelArgs.CurrentCategoryType.CategoryName);

                string totalDetectionCode=string.Empty, totalDetectionName=string.Empty;
                foreach (DetectionType detectionType in registerModelArgs.CurrentDetectionType)
                {
                    totalDetectionCode += detectionType.DetectionCode + ",";
                    totalDetectionName += detectionType.DetectionName + ",";
                }
                totalDetectionCode = totalDetectionCode.Substring(0, totalDetectionCode.Length - 1);
                totalDetectionName = totalDetectionName.Substring(0, totalDetectionName.Length - 1);

                sqliteCommand.Parameters.AddWithValue("DetectionCode", totalDetectionCode);
                sqliteCommand.Parameters.AddWithValue("DetectionName", totalDetectionName);
                sqliteCommand.Parameters.AddWithValue("OccupationName", registerModelArgs.OccupationName);
                sqliteCommand.Parameters.AddWithValue("TravelTrajectory", registerModelArgs.TravelTrajectory);
                sqliteCommand.Parameters.AddWithValue("Remarks", registerModelArgs.Remarks);
                sqliteCommand.Parameters.AddWithValue("SamplingLocation", registerModelArgs.SamplingLocation);
                sqliteCommand.Parameters.AddWithValue("SamplingPerson", registerModelArgs.SamplingPerson);
                sqliteCommand.Parameters.AddWithValue("CreateDateTime", DateTime.Now.ToString("G"));
                sqliteCommand.Parameters.AddWithValue("IsSynchronized", 0);
                sqliteCommand.Parameters.AddWithValue("SynchronizeDateTime", string.Empty);
                sqliteCommand.Parameters.AddWithValue("NetWorkSerialNumber", 0);
                sqliteCommand.Parameters.AddWithValue("EntityCertificateCode", registerModelArgs.CurrentEntityCertificateType.EntityCertificateCode);
                sqliteCommand.Parameters.AddWithValue("EntityCertificateName", registerModelArgs.CurrentEntityCertificateType.EntityCertificateName);
                sqliteCommand.Parameters.AddWithValue("ElectronicCertificateCode", registerModelArgs.CurrentElectronicCertificateType.ElectronicCertificateCode);
                sqliteCommand.Parameters.AddWithValue("ElectronicCertificateName", registerModelArgs.CurrentElectronicCertificateType.ElectronicCertificateName);
                sqliteCommand.Parameters.AddWithValue("IDCardReaderCode", registerModelArgs.CurrentIDCardReaderType.IDCardReaderCode);
                sqliteCommand.Parameters.AddWithValue("IDCardReaderName", registerModelArgs.CurrentIDCardReaderType.IDCardReaderName);

                int retVal = sqliteCommand.ExecuteNonQuery();

                sqliteConnection.Close();

                if (retVal != 0)
                    ret = true;
            }

            return ret;
        }

        public List<WaitForSynchronizeType> FetchSynchronizeData()
        {
            List<WaitForSynchronizeType> retWaitForSynchronizeType = new List<WaitForSynchronizeType>();

            if (isValidateSucceed)
            {
                sqliteConnection.Open();

                sqlStatement = "SELECT SerialNumber,Name,SexCode,NationCode,Age,Address,IDNumber,CurrentAddress,PhoneNumber,SpecimenCollectionCode,CategoryCode," +
                                 "DetectionCode,OccupationName,TravelTrajectory,Remarks,SamplingLocation,SamplingPerson FROM PatientInfo where IsSynchronized = 0";
                SQLiteCommand sqliteCommand = new SQLiteCommand(sqlStatement, sqliteConnection);
                SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    retWaitForSynchronizeType.Add(new WaitForSynchronizeType
                    {
                        SerialNumber = Convert.ToInt64(sqliteDataReader[0]),
                        Name = sqliteDataReader[1].ToString(),
                        SexCode = sqliteDataReader[2].ToString(),
                        NationCode = sqliteDataReader[3].ToString(),
                        Age = (int)sqliteDataReader[4],
                        Address = sqliteDataReader[5].ToString(),
                        IDNumber = sqliteDataReader[6].ToString(),
                        CurrentAddress = sqliteDataReader[7].ToString(),
                        PhoneNumber = sqliteDataReader[8].ToString(),
                        SpecimenCollectionCode = sqliteDataReader[9].ToString(),
                        CategoryCode = sqliteDataReader[10].ToString(),
                        DetectionCode = sqliteDataReader[11].ToString(),
                        OccupationName = sqliteDataReader[12].ToString(),
                        TravelTrajectory = sqliteDataReader[13].ToString(),
                        Remarks = sqliteDataReader[14].ToString(),
                        SamplingLocation = sqliteDataReader[15].ToString(),
                        SamplingPerson = sqliteDataReader[16].ToString()
                    });
                }
                sqliteDataReader.Close();

                sqliteConnection.Close();
            }

            return retWaitForSynchronizeType;
        }

        public bool ConfirmSynchronizeResult(Int64 sourceSerialNumberArgs,int TargetSerialNumberArgs)
        {
            bool ret = false;

            if (isValidateSucceed)
            {
                sqliteConnection.Open();

                sqlStatement = "UPDATE PatientInfo SET IsSynchronized=1,SynchronizeDateTime=@SynchronizeDateTime,NetWorkSerialNumber=@NetWorkSerialNumber " +
                         "WHERE SerialNumber=@SerialNumber";
                SQLiteCommand sqliteCommand = new SQLiteCommand(sqlStatement, sqliteConnection);
                sqliteCommand.Parameters.AddWithValue("SynchronizeDateTime", DateTime.Now.ToString("G"));
                sqliteCommand.Parameters.AddWithValue("NetWorkSerialNumber", TargetSerialNumberArgs);
                sqliteCommand.Parameters.AddWithValue("SerialNumber", sourceSerialNumberArgs);
                int retVal = sqliteCommand.ExecuteNonQuery();
                if (retVal != 0)
                    ret = true;

                sqliteConnection.Close();
            }

            return ret;
        }

        public IEnumerable<T> Query<T>(string beginDateStringArgs, string endDateStringArgs) where T : class
        {
            List<QueryNativeRegisterDataType> ret = new List<QueryNativeRegisterDataType>();

            if (isValidateSucceed)
            {
                sqliteConnection.Open();

                sqlStatement = "SELECT SerialNumber,Name,SexName,NationName,Age,Address,IDNumber,CurrentAddress,PhoneNumber,SpecimenCollectionName," +
                    "CategoryName,DetectionName,OccupationName,TravelTrajectory,Remarks,SamplingLocation,SamplingPerson,CreateDateTime,case when IsSynchronized=1 then '是' else '否' end IsSynchronized,SynchronizeDateTime," +
                      "NetWorkSerialNumber  FROM PatientInfo WHERE CreateDateTime BETWEEN '" + beginDateStringArgs + "' AND '" + endDateStringArgs + "'";

                logController.WriteLog(sqlStatement);
                SQLiteCommand sqliteCommand = new SQLiteCommand(sqlStatement, sqliteConnection);
                SQLiteDataReader sqliteDataReader = sqliteCommand.ExecuteReader();
                while (sqliteDataReader.Read())
                {
                    ret.Add(new  QueryNativeRegisterDataType
                    {
                        SerialNumber = Convert.ToInt64(sqliteDataReader[0]),
                        Name = sqliteDataReader[1].ToString(),
                        SexName = sqliteDataReader[2].ToString(),
                        NationName = sqliteDataReader[3].ToString(),
                        Age = (int)sqliteDataReader[4],
                        Address = sqliteDataReader[5].ToString(),
                        IDNumber = sqliteDataReader[6].ToString(),
                        CurrentAddress = sqliteDataReader[7].ToString(),
                        PhoneNumber = sqliteDataReader[8].ToString(),
                        SpecimenCollectionName = sqliteDataReader[9].ToString(),
                        CategoryName = sqliteDataReader[10].ToString(),
                        DetectionName = sqliteDataReader[11].ToString(),
                        OccupationName = sqliteDataReader[12].ToString(),
                        TravelTrajectory = sqliteDataReader[13].ToString(),
                        Remarks = sqliteDataReader[14].ToString(),
                        SamplingLocation = sqliteDataReader[15].ToString(),
                        SamplingPerson = sqliteDataReader[16].ToString(),
                        CreateDateTime = sqliteDataReader[17].ToString(),
                        IsSynchronized = sqliteDataReader[18].ToString(),
                        SynchronizeDateTime = sqliteDataReader[19].ToString(),
                        NetWorkSerialNumber = Convert.ToInt64(sqliteDataReader[20])
                    });
                }
            }

            foreach (var v in ret)
            {
                yield return (T)(object)v;
            }
        }
    }
}