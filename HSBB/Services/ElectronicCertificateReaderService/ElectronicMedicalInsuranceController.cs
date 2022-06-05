using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Prism.Ioc;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using HSBB.Models;

namespace HSBB.Services
{
    public class ElectronicMedicalInsuranceController : IElectronicCertificateController
    {
        [DllImport("SiInterface_hsf.dll", EntryPoint = "INIT")]
        public static extern int INIT(StringBuilder pErrMsg);

        [DllImport("SiInterface_hsf.dll", EntryPoint = "BUSINESS_HANDLE")]
        public static extern int BUSINESS_HANDLE(StringBuilder inputData, StringBuilder outputData);

        ISnackbarMessageQueue messageQueue;
        IAppConfigController appConfigController;
        ILogController logController;

        int ret;
        StringBuilder inputStr, outputStr;

        bool isValidateSucceed;

        public ElectronicMedicalInsuranceController(IContainerProvider containerProviderArgs)
        {
            this.appConfigController = containerProviderArgs.Resolve<IAppConfigController>();
            this.messageQueue = containerProviderArgs.Resolve<ISnackbarMessageQueue>();
            this.logController = containerProviderArgs.Resolve<ILogController>();

            inputStr = new StringBuilder(1024 * 1024);
            outputStr = new StringBuilder(1024 * 1024);

            Validate();
        }

        private void Validate()
        {
            isValidateSucceed = false;

            if (appConfigController.IsValidateSucceed)
            {
                ret = INIT(inputStr);
                if (ret == 0)
                {
                    isValidateSucceed = true;
                    logController.WriteLog("电子医保卡环境检查成功!");
                }
                else
                {
                    messageQueue.Enqueue("医保环境检查错误!");
                }
            }
        }

        public T Load<T>(string electronicCertificateStringArgs) where T : class
        {
            MedicalInsuranceTransferredType medicalInsuranceTransferredType = new MedicalInsuranceTransferredType();

            if (isValidateSucceed)
            {
                MedicalInsuranceRequestHeaderType medicalInsuranceRequestHeaderType = new MedicalInsuranceRequestHeaderType(SerialNnumberEnum.ReadElectronicCertificate);
                string jsonRequestHeaderString = JsonConvert.SerializeObject(medicalInsuranceRequestHeaderType);

                inputStr.Append(jsonRequestHeaderString);
                logController.WriteLog("调用读卡交易开始:" + inputStr.ToString());

                ret = BUSINESS_HANDLE(inputStr, outputStr);

                if (ret != 0)
                {
                    messageQueue.Enqueue("医保接口交易失败!");
                    logController.WriteLog("调用读卡交易失败:ret=" + ret.ToString() + ", outputStr=" + outputStr.ToString());
                }            
                else
                {
                    logController.WriteLog("调用读卡交易结束:" + outputStr.ToString());
                    MedicalInsuranceResponseHeaderType medicalInsuranceResponseHeaderType = JsonConvert.DeserializeObject<MedicalInsuranceResponseHeaderType>(outputStr.ToString());
                    if (medicalInsuranceResponseHeaderType.infcode != "0")
                        messageQueue.Enqueue("医保接口交易错误," + medicalInsuranceResponseHeaderType.err_msg);
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
