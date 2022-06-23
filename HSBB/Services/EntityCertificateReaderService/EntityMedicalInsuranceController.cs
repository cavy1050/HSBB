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

        ILogController logController;

        int retVal=-1;
        StringBuilder inputStr, outputStr;
        bool isValidateSucceed=false;

        public EntityMedicalInsuranceController(IContainerProvider containerProviderArgs)
        {
            this.logController = containerProviderArgs.Resolve<ILogController>();

            inputStr = new StringBuilder(1024 * 1024);
            outputStr = new StringBuilder(1024 * 1024);

            Validate();
        }

        private void Validate()
        {
            retVal = INIT(inputStr);
            if (retVal!=0)
                logController.WriteLog("初始化实体医保卡环境错误!");
            else
            {
                logController.WriteLog("初始化实体医保卡环境成功!");
                isValidateSucceed = true;
            }
        }

        public bool Load<T>(out T t) where T : class
        {
            bool ret = false;
            t = default(T);

            if (isValidateSucceed)
            {
                MedicalInsuranceTransferredType medicalInsuranceTransferredType = new MedicalInsuranceTransferredType();

                MedicalInsuranceRequestHeaderType medicalInsuranceRequestHeaderType = new MedicalInsuranceRequestHeaderType(SerialNnumberEnum.ReadEntityCertificate);
                string jsonRequestHeaderString = JsonConvert.SerializeObject(medicalInsuranceRequestHeaderType);

                inputStr.Append(jsonRequestHeaderString);
                logController.WriteLog("调用读卡交易开始:" + inputStr.ToString());

                retVal = BUSINESS_HANDLE(inputStr, outputStr);

                if (retVal != 0)
                { 
                    logController.WriteLog("调用读卡交易失败:ret="+ret.ToString()+", outputStr="+outputStr.ToString());
                }
                else
                {
                    logController.WriteLog("调用读卡交易结束:" + outputStr.ToString());
                    MedicalInsuranceResponseHeaderType medicalInsuranceResponseHeaderType = JsonConvert.DeserializeObject<MedicalInsuranceResponseHeaderType>(outputStr.ToString());
                    if (medicalInsuranceResponseHeaderType.infcode != "0")
                    {
                        logController.WriteLog("医保接口交易错误," + medicalInsuranceResponseHeaderType.err_msg);
                    }
                    else
                    {
                        MedicalInsuranceResponseDetailBaseinfoType medicalInsuranceResponseDetailBaseinfoType = medicalInsuranceResponseHeaderType.output.baseinfo;

                        medicalInsuranceTransferredType.Name = medicalInsuranceResponseDetailBaseinfoType.psn_name;
                        medicalInsuranceTransferredType.Sex = medicalInsuranceResponseDetailBaseinfoType.gend;
                        medicalInsuranceTransferredType.Nation = medicalInsuranceResponseDetailBaseinfoType.naty;
                        medicalInsuranceTransferredType.BirthDay = medicalInsuranceResponseDetailBaseinfoType.brdy;
                        medicalInsuranceTransferredType.IDNumber = medicalInsuranceResponseDetailBaseinfoType.certno;

                        if (typeof(T) == typeof(MedicalInsuranceTransferredType))
                            t= (T)(object)medicalInsuranceTransferredType;

                        ret = true;
                    }
                }
            }

            return ret;
        }
    }
}
