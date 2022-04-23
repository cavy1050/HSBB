using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using HSBB.Models;

namespace HSBB.Services
{
    public class DataBaseService: IDataBaseService
    {
        static string appConfigFilePath=AppDomain.CurrentDomain.BaseDirectory + "HSBB.config";
        Database HSDB;
        string sqlStatement;

        public DataBaseService()
        {
            FileConfigurationSource fileConfigurationSource = new FileConfigurationSource(appConfigFilePath);

            DatabaseProviderFactory databaseProviderFactory = new DatabaseProviderFactory(fileConfigurationSource);
            HSDB = databaseProviderFactory.Create("HSDB");
        }

        public bool SaveInfo(ShellModel shellModelArgs)
        {
            sqlStatement = "Insert into hsBBData "+
                            "(cyDate,blz,ylz,name,sex,age,idCard,addr,jwfy,qtmjz,zyhz,ph,ncp,frhz,hzlb,remark,hzlb2,phone,cyr,cyAddr,addr1,tj) " +
                             "values(@cyDate, @blz, @ylz, @name, @sex, @age, @idCard, @addr, @jwfy, @qtmjz, @zyhz, @ph, @ncp, @frhz, @hzlb, @remark, @hzlb2, @phone, @cyr, @cyAddr, @addr1, @tj)";

            using (DbCommand cmd = HSDB.GetSqlStringCommand(sqlStatement))
            {
                HSDB.AddInParameter(cmd, "cyDate", DbType.DateTime, DateTime.Now);
                HSDB.AddInParameter(cmd, "blz", DbType.Int32, shellModelArgs.CurrentSpecimenCollectionType.SpecimenCollectionCode == "1" ? 1 : 0);
                HSDB.AddInParameter(cmd, "ylz", DbType.Int32, shellModelArgs.CurrentSpecimenCollectionType.SpecimenCollectionCode == "2" ? 1 : 0);
                HSDB.AddInParameter(cmd, "name", DbType.String, shellModelArgs.Name);
                HSDB.AddInParameter(cmd, "sex", DbType.String, shellModelArgs.CurrentSexType.SexName);
                HSDB.AddInParameter(cmd, "age", DbType.Int32, shellModelArgs.Age);
                HSDB.AddInParameter(cmd, "idCard", DbType.String, shellModelArgs.IDNumber);
                HSDB.AddInParameter(cmd, "addr", DbType.String, shellModelArgs.Address);
                HSDB.AddInParameter(cmd, "jwfy", DbType.String, shellModelArgs.TravelTrajectory);
                HSDB.AddInParameter(cmd, "qtmjz", DbType.Int32, shellModelArgs.CurrentDetectionType.Contains(new DetectionType { DetectionCode = "1", DetectionName = "门急诊" }) ? 1 : 0);
                HSDB.AddInParameter(cmd, "zyhz", DbType.Int32, shellModelArgs.CurrentDetectionType.Contains(new DetectionType { DetectionCode = "2", DetectionName = "住院患者" }) ? 1 : 0);
                HSDB.AddInParameter(cmd, "ph", DbType.Int32, shellModelArgs.CurrentDetectionType.Contains(new DetectionType { DetectionCode = "3", DetectionName = "陪护" }) ? 1 : 0);
                HSDB.AddInParameter(cmd, "ncp", DbType.Int32, shellModelArgs.CurrentDetectionType.Contains(new DetectionType { DetectionCode = "5", DetectionName = "NCP筛查" }) ? 1 : 0);
                HSDB.AddInParameter(cmd, "frhz", DbType.Int32, shellModelArgs.CurrentDetectionType.Contains(new DetectionType { DetectionCode = "6", DetectionName = "发热患者" }) ? 1 : 0);
                HSDB.AddInParameter(cmd, "hzlb", DbType.String, shellModelArgs.CurrentPatientCategoryType.PatientCategoryName);
                HSDB.AddInParameter(cmd, "remark", DbType.String, shellModelArgs.Remarks);
                HSDB.AddInParameter(cmd, "hzlb2", DbType.String, shellModelArgs.OccupationName);
                HSDB.AddInParameter(cmd, "phone", DbType.String, shellModelArgs.PhoneNumber);
                HSDB.AddInParameter(cmd, "cyr", DbType.String, shellModelArgs.SamplingPerson);
                HSDB.AddInParameter(cmd, "cyAddr", DbType.String, shellModelArgs.SamplingLocation);
                HSDB.AddInParameter(cmd, "addr1", DbType.String, shellModelArgs.CurrentAddress);
                HSDB.AddInParameter(cmd, "tj", DbType.Int32, shellModelArgs.CurrentDetectionType.Contains(new DetectionType { DetectionCode = "4", DetectionName = "体检(自检)" }) ? 1 : 0);

                int ret = HSDB.ExecuteNonQuery(cmd);

                if (ret!=0)
                    return true;
                else
                    return false;
            }
        }
    }
}
