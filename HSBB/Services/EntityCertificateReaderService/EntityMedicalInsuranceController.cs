using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Prism.Ioc;
using Newtonsoft.Json;
using HSBB.Models;

namespace HSBB.Services
{
    public class EntityMedicalInsuranceController : IEntityCertificateController
    {
        [DllImport("SiInterface_hsf.dll", EntryPoint = "INIT")]
        public static extern int INIT(StringBuilder pErrMsg);

        [DllImport("SiInterface_hsf.dll", EntryPoint = "BUSINESS_HANDLE")]
        public static extern int BUSINESS_HANDLE(StringBuilder inputData, StringBuilder outputData);

        IApplictionController applictionController;
        ILogController logController;

        int ret=-1;
        StringBuilder inputStr, outputStr;
        bool isValidateSucceed;

        public EntityMedicalInsuranceController(IContainerProvider containerProviderArgs)
        {
            this.applictionController = containerProviderArgs.Resolve<IApplictionController>();
            this.logController = containerProviderArgs.Resolve<ILogController>();

            inputStr = new StringBuilder(1024 * 1024);
            outputStr = new StringBuilder(1024 * 1024);

            logController.WriteLog("实体医保卡服务调用成功!");

            Validate();
        }

        private void Validate()
        {
            isValidateSucceed = false;

            if (applictionController.IsValidateSucceed)
            {
                try
                {
                    ret = INIT(inputStr);
                }
                catch (Exception ex)
                {
                    logController.WriteLog("初始化实体医保卡环境错误," + ex.Message);
                }
                finally
                {
                    if (ret == 0)
                    {
                        isValidateSucceed = true;
                        logController.WriteLog("初始化实体医保卡环境成功!");
                    }
                    else
                    {
                        logController.WriteLog("初始化实体医保卡环境错误," + inputStr.ToString());
                    }
                }
            }
        }

        public T Load<T>() where T : class
        {
            MedicalInsuranceTransferredType medicalInsuranceTransferredType = new MedicalInsuranceTransferredType();

            if (isValidateSucceed)
            {
                MedicalInsuranceRequestHeaderType medicalInsuranceRequestHeaderType = new MedicalInsuranceRequestHeaderType(SerialNnumberEnum.ReadEntityCertificate);
                string jsonRequestHeaderString = JsonConvert.SerializeObject(medicalInsuranceRequestHeaderType);

                inputStr.Append(jsonRequestHeaderString);
                logController.WriteLog("调用读卡交易开始:" + inputStr.ToString());

                ret = BUSINESS_HANDLE(inputStr, outputStr);

                if (ret != 0)
                { 
                    logController.WriteLog("调用读卡交易失败:ret="+ret.ToString()+", outputStr="+outputStr.ToString());
                }
                else
                {
                    logController.WriteLog("调用读卡交易结束:" + outputStr.ToString());
                    MedicalInsuranceResponseHeaderType medicalInsuranceResponseHeaderType = JsonConvert.DeserializeObject<MedicalInsuranceResponseHeaderType>(outputStr.ToString());
                    if (medicalInsuranceResponseHeaderType.infcode != "0")
                        logController.WriteLog("医保接口交易错误," + medicalInsuranceResponseHeaderType.err_msg);
                    else
                    {
                        MedicalInsuranceResponseDetailBaseinfoType medicalInsuranceResponseDetailBaseinfoType = medicalInsuranceResponseHeaderType.output.baseinfo;

                        medicalInsuranceTransferredType.Name = medicalInsuranceResponseDetailBaseinfoType.psn_name;
                        medicalInsuranceTransferredType.Sex = medicalInsuranceResponseDetailBaseinfoType.gend;
                        medicalInsuranceTransferredType.Nation = medicalInsuranceResponseDetailBaseinfoType.naty;
                        medicalInsuranceTransferredType.BirthDay = medicalInsuranceResponseDetailBaseinfoType.brdy;
                        medicalInsuranceTransferredType.IDNumber = medicalInsuranceResponseDetailBaseinfoType.certno;
                    }
                }
            }

            if (typeof(T) == typeof(MedicalInsuranceTransferredType))
                return (T)(object)medicalInsuranceTransferredType;
            else
                return default(T);
        }
    }
}
