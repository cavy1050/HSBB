using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Data;
using System.Data.Common;
using Prism.Ioc;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using HSBB.Models;

namespace HSBB.Services
{
    public class NetWorkDataBaseController : IDataBaseController
    {
        Database HSDB;
        string sqlStatement;

        IApplictionController applictionController;
        ILogController logController;

        bool isValidateSucceed;

        public NetWorkDataBaseController(IContainerProvider containerProviderArgs)
        {
            this.applictionController = containerProviderArgs.Resolve<IApplictionController>();
            logController = containerProviderArgs.Resolve<ILogController>();

            Validate();
        }

        private void Validate()
        {
            isValidateSucceed = false;

            if (applictionController.IsValidateSucceed)
            {
                FileConfigurationSource fileConfigurationSource = new FileConfigurationSource(applictionController.EnvironmentSetting.ApplictionConfigFilePath);
                DatabaseProviderFactory databaseProviderFactory = new DatabaseProviderFactory(fileConfigurationSource);
                HSDB = databaseProviderFactory.Create("HSDB");
                DbConnection dbConnection = HSDB.CreateConnection();
                string defaultDataBaseConnectionString = dbConnection.ConnectionString;

                Regex IPAd = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
                MatchCollection MatchResult = IPAd.Matches(defaultDataBaseConnectionString);
                string dataBaseIPString = MatchResult[0].ToString();
                IPAddress ipAddress;

                if (IPAddress.TryParse(dataBaseIPString, out ipAddress))
                {
                    Ping ping = new Ping();

                    try
                    {
                        PingReply pingReply = ping.Send(ipAddress);
                    }
                    catch
                    {
                        logController.WriteLog("网络设置检查失败：" + dataBaseIPString);
                    }
                    finally
                    {
                        try
                        {
                            //TODO 异步连接中连接字符串错误仍不显示错误消息.
                            dbConnection.Open();
                        }
                        catch
                        {
                            logController.WriteLog("数据库连接检查失败,当前数据库连接字符串:" + defaultDataBaseConnectionString);
                        }
                        finally
                        {
                            isValidateSucceed = true;
                        }
                    }
                }
            }
        }

        public bool Save(RegisterModel registerModelArgs)
        {
            bool ret = false;

            if (isValidateSucceed)
            {
                sqlStatement = "Insert into dbo.hsBBData " +
                                "(cyDate,blz,ylz,name,sex,age,idCard,addr,jwfy,qtmjz,zyhz,ph,ncp,frhz,hzlb,remark,hzlb2,phone,cyr,cyAddr,addr1,tj) " +
                                 "values(@cyDate, @blz, @ylz, @name, @sex, @age, @idCard, @addr, @jwfy, @qtmjz, @zyhz, @ph, @ncp, @frhz, @hzlb, @remark, @hzlb2, @phone, @cyr, @cyAddr, @addr1, @tj)";

                using (DbCommand cmd = HSDB.GetSqlStringCommand(sqlStatement))
                {
                    HSDB.AddInParameter(cmd, "cyDate", DbType.DateTime, DateTime.Now);
                    HSDB.AddInParameter(cmd, "blz", DbType.Int32, registerModelArgs.CurrentSpecimenCollectionType.SpecimenCollectionCode == "1" ? 1 : 0);
                    HSDB.AddInParameter(cmd, "ylz", DbType.Int32, registerModelArgs.CurrentSpecimenCollectionType.SpecimenCollectionCode == "2" ? 1 : 0);
                    HSDB.AddInParameter(cmd, "name", DbType.String, registerModelArgs.Name);
                    HSDB.AddInParameter(cmd, "sex", DbType.String, registerModelArgs.CurrentSexType.SexName);
                    HSDB.AddInParameter(cmd, "age", DbType.Int32, registerModelArgs.Age);
                    HSDB.AddInParameter(cmd, "idCard", DbType.String, registerModelArgs.IDNumber);
                    HSDB.AddInParameter(cmd, "addr", DbType.String, registerModelArgs.Address);
                    HSDB.AddInParameter(cmd, "jwfy", DbType.String, registerModelArgs.TravelTrajectory);
                    HSDB.AddInParameter(cmd, "qtmjz", DbType.Int32, registerModelArgs.CurrentDetectionType.Contains(new DetectionType { DetectionCode = "1", DetectionName = "门急诊" }) ? 1 : 0);
                    HSDB.AddInParameter(cmd, "zyhz", DbType.Int32, registerModelArgs.CurrentDetectionType.Contains(new DetectionType { DetectionCode = "2", DetectionName = "住院患者" }) ? 1 : 0);
                    HSDB.AddInParameter(cmd, "ph", DbType.Int32, registerModelArgs.CurrentDetectionType.Contains(new DetectionType { DetectionCode = "3", DetectionName = "陪护" }) ? 1 : 0);
                    HSDB.AddInParameter(cmd, "ncp", DbType.Int32, registerModelArgs.CurrentDetectionType.Contains(new DetectionType { DetectionCode = "5", DetectionName = "NCP筛查" }) ? 1 : 0);
                    HSDB.AddInParameter(cmd, "frhz", DbType.Int32, registerModelArgs.CurrentDetectionType.Contains(new DetectionType { DetectionCode = "6", DetectionName = "发热患者" }) ? 1 : 0);
                    HSDB.AddInParameter(cmd, "hzlb", DbType.String, registerModelArgs.CurrentCategoryType.CategoryName);
                    HSDB.AddInParameter(cmd, "remark", DbType.String, registerModelArgs.Remarks);
                    HSDB.AddInParameter(cmd, "hzlb2", DbType.String, registerModelArgs.OccupationName);
                    HSDB.AddInParameter(cmd, "phone", DbType.String, registerModelArgs.PhoneNumber);
                    HSDB.AddInParameter(cmd, "cyr", DbType.String, registerModelArgs.SamplingPerson);
                    HSDB.AddInParameter(cmd, "cyAddr", DbType.String, registerModelArgs.SamplingLocation);
                    HSDB.AddInParameter(cmd, "addr1", DbType.String, registerModelArgs.CurrentAddress);
                    HSDB.AddInParameter(cmd, "tj", DbType.Int32, registerModelArgs.CurrentDetectionType.Contains(new DetectionType { DetectionCode = "4", DetectionName = "体检(自检)" }) ? 1 : 0);

                    int retVal = HSDB.ExecuteNonQuery(cmd);

                    if (retVal != 0)
                        ret = true;
                    else
                        ret = false;
                }
            }

            return ret;
        }

        public IEnumerable<T> Query<T>(string beginDateStringArgs, string endDateStringArgs) where T : class
        {
            List<QueryNetworkRegisterDataType> ret = new List<QueryNetworkRegisterDataType>();

            if (isValidateSucceed)
            {
                sqlStatement = "SELECT PageID,RowID,ID,Name,Sex," +
                            "Age,Address,IDNumber,cyDate,CurrentAddress," +
                              "PhoneNumber,CurrentSpecimenCollectionType,CurrentPatientCategoryType,CurrentDetectionType,OccupationName," +
                                "TravelTrajectory,Remarks,SamplingLocation,SamplingPerson FROM dbo.TF_GetRegisterInfo('" + beginDateStringArgs + "','" + endDateStringArgs + "')";

                ret = HSDB.ExecuteSqlStringAccessor<QueryNetworkRegisterDataType>(sqlStatement).ToList();
            }

            foreach(var v in ret)
            {
                yield return (T)(object)v;
            }
        }

        public bool SynchronizeData(WaitForSynchronizeType waitForSynchronizeTypeArgs,ref int retValArgs)
        {
            bool ret = false;
            retValArgs = 0;

            if (isValidateSucceed)
            {
                sqlStatement = "exec usp_SynchronizeData @Name,@SexCode,@NationCode,@Age,@Address,@IDNumber,@CurrentAddress,@PhoneNumber,@SpecimenCollectionCode,@CategoryCode," +
                    "@DetectionCode,@OccupationName,@TravelTrajectory,@Remarks,@SamplingLocation,@SamplingPerson";

                using (DbCommand cmd = HSDB.GetSqlStringCommand(sqlStatement))
                {
                    HSDB.AddInParameter(cmd, "Name", DbType.String, waitForSynchronizeTypeArgs.Name);
                    HSDB.AddInParameter(cmd, "SexCode", DbType.String, waitForSynchronizeTypeArgs.SexCode);
                    HSDB.AddInParameter(cmd, "NationCode", DbType.String, waitForSynchronizeTypeArgs.NationCode);
                    HSDB.AddInParameter(cmd, "Age", DbType.Int32, waitForSynchronizeTypeArgs.Age);
                    HSDB.AddInParameter(cmd, "Address", DbType.String, waitForSynchronizeTypeArgs.Address);
                    HSDB.AddInParameter(cmd, "IDNumber", DbType.String, waitForSynchronizeTypeArgs.IDNumber);
                    HSDB.AddInParameter(cmd, "CurrentAddress", DbType.String, waitForSynchronizeTypeArgs.CurrentAddress);
                    HSDB.AddInParameter(cmd, "PhoneNumber", DbType.String, waitForSynchronizeTypeArgs.PhoneNumber);
                    HSDB.AddInParameter(cmd, "SpecimenCollectionCode", DbType.String, waitForSynchronizeTypeArgs.SpecimenCollectionCode);
                    HSDB.AddInParameter(cmd, "CategoryCode", DbType.String, waitForSynchronizeTypeArgs.CategoryCode);
                    HSDB.AddInParameter(cmd, "DetectionCode", DbType.String, waitForSynchronizeTypeArgs.DetectionCode);
                    HSDB.AddInParameter(cmd, "OccupationName", DbType.String, waitForSynchronizeTypeArgs.OccupationName);
                    HSDB.AddInParameter(cmd, "TravelTrajectory", DbType.String, waitForSynchronizeTypeArgs.TravelTrajectory);
                    HSDB.AddInParameter(cmd, "Remarks", DbType.String, waitForSynchronizeTypeArgs.Remarks);
                    HSDB.AddInParameter(cmd, "SamplingLocation", DbType.String, waitForSynchronizeTypeArgs.SamplingLocation);
                    HSDB.AddInParameter(cmd, "SamplingPerson", DbType.String, waitForSynchronizeTypeArgs.SamplingPerson);

                    retValArgs = (int)HSDB.ExecuteScalar(cmd);

                    if (retValArgs != 0)
                        ret = true;
                    else
                        ret = false;
                }
            }

            return ret;
        }
    }
}
