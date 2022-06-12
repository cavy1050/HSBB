using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.NetworkInformation;
using Prism.Ioc;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using KYSharp.SM;
using HSBB.Models;

namespace HSBB.Services
{
    public class ElectronicYukangController : IElectronicCertificateController
    {
        readonly string sm4Key = "037985f760b5c3d7";
        string defaultGovernmentCertificateUriString;

        IApplictionController applictionController;
        ISnackbarMessageQueue messageQueue;

        bool isValidateSucceed;

        public ElectronicYukangController(IContainerProvider containerProviderArgs)
        {
            this.applictionController = containerProviderArgs.Resolve<IApplictionController>();
            this.messageQueue = containerProviderArgs.Resolve<ISnackbarMessageQueue>();

            Validate();
        }

        private void Validate()
        {
            isValidateSucceed = false;

            if (applictionController.IsValidateSucceed)
            {
                defaultGovernmentCertificateUriString = applictionController.ConfigSettings["defaultGovernmentCertificateUriString"];

                string remoteHostUriString = defaultGovernmentCertificateUriString;
                Regex IPAd = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
                MatchCollection MatchResult = IPAd.Matches(remoteHostUriString);
                string remoteHostIPString = MatchResult[0].ToString();
                IPAddress ipAddress;

                if (IPAddress.TryParse(remoteHostIPString, out ipAddress))
                {
                    Ping ping = new Ping();

                    try
                    {
                        PingReply pingReply = ping.Send(ipAddress);
                    }
                    catch
                    {
                        messageQueue.Enqueue("网络设置检查失败!");
                    }
                    finally
                    {
                        isValidateSucceed = true;
                    }
                }
            }
        }

        public T Load<T>(string electronicCertificateStringArgs) where T : class
        {
            GovernmentAffairsTransferredType governmentAffairsTransferredType= new GovernmentAffairsTransferredType();

            if (isValidateSucceed && electronicCertificateStringArgs.Contains("outTime"))
            {
                Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(electronicCertificateStringArgs);
                GovernmentAffairsRequstJosnStringType governmentAffairsRequstJosnStringType = new GovernmentAffairsRequstJosnStringType();

                governmentAffairsRequstJosnStringType.codeId = dic["codeId"];
                governmentAffairsRequstJosnStringType.lastReportTime = dic["lastReportTime"];
                governmentAffairsRequstJosnStringType.outTime = dic["outTime"];
                governmentAffairsRequstJosnStringType.zoning = dic["zoning"];

                string electronicCertificateReRequstJosnString = JsonConvert.SerializeObject(governmentAffairsRequstJosnStringType);
                SM4Utils sm4Utils = new SM4Utils();
                sm4Utils.secretKey = sm4Key;

                string electronicCertificateCipheredRequestString = sm4Utils.Encrypt_ECB(electronicCertificateReRequstJosnString);

                HttpClient httpClient = new HttpClient { BaseAddress = new Uri(defaultGovernmentCertificateUriString) };

                string httpResponse = httpClient.GetStringAsync(new Uri(httpClient.BaseAddress, "appointment-do/getHealthCardInfo/ykm" + "?ciphertext=" + electronicCertificateCipheredRequestString.ToUpper()).OriginalString).Result;
                if (!string.IsNullOrEmpty(httpResponse))
                {
                    GovernmentAffairsResponseCipherStringType governmentAffairsResponseCipherStringType = JsonConvert.DeserializeObject<GovernmentAffairsResponseCipherStringType>(httpResponse);
                    if (governmentAffairsResponseCipherStringType != null)
                    {
                        string receiveJosnString = sm4Utils.Decrypt_ECB(governmentAffairsResponseCipherStringType.CipherText);
                        if (!string.IsNullOrEmpty(receiveJosnString))
                        {
                            GovernmentAffairsResponseJsonStringType governmentAffairsResponseJsonStringType = JsonConvert.DeserializeObject<GovernmentAffairsResponseJsonStringType>(receiveJosnString);
                            if (governmentAffairsResponseJsonStringType != null)
                            {
                                governmentAffairsTransferredType.BirthDay = governmentAffairsResponseJsonStringType.birthday;
                                governmentAffairsTransferredType.PhoneNumber = governmentAffairsResponseJsonStringType.lxdh;
                                governmentAffairsTransferredType.Address = governmentAffairsResponseJsonStringType.dz;
                                governmentAffairsTransferredType.Name = governmentAffairsResponseJsonStringType.aac003;
                                governmentAffairsTransferredType.Sex = governmentAffairsResponseJsonStringType.aac004;
                                governmentAffairsTransferredType.IDNumber = governmentAffairsResponseJsonStringType.zjhm;
                            }
                        }
                    }
                }
            }

            if (typeof(T) == typeof(GovernmentAffairsTransferredType))
                return (T)(object)governmentAffairsTransferredType;
            else
                return default(T);
        }
    }
}