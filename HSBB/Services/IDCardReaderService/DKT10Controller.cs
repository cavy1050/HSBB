using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Prism.Ioc;
using HSBB.Models;

namespace HSBB.Services
{
    public class DKT10Controller : IIDCardController
    {
        [DllImport("dcrf32.dll")]
        private static extern int dc_init(short port, int baud);

        [DllImport("dcrf32.dll")]
        private static extern short dc_exit(int icdev);

        [DllImport("dcrf32.dll")]
        private static extern short dc_beep(int icdev, ushort baud);

        [DllImport("dcrf32.dll")]
        private static extern short dc_find_i_d(int icdev);

        [DllImport("dcrf32.dll")]
        private static extern short dc_SamAReadCardInfo(int icdev, int type,
                                                        ref int text_len, byte[] text,
                                                        ref int photo_len, byte[] photo,
                                                        ref int fingerprint_len, byte[] fingerprint,
                                                        ref int extra_len, byte[] extra);

        [DllImport("dcrf32.dll")]
        private static extern short dc_ParseTextInfo(int icdev,
                                                     int charset,
                                                     int info_len,
                                                     byte[] info,
                                                     byte[] name,
                                                     byte[] sex,
                                                     byte[] nation,
                                                     byte[] birth_day,
                                                     byte[] address,
                                                     byte[] id_number,
                                                     byte[] department,
                                                     byte[] expire_start_day,
                                                     byte[] expire_end_day,
                                                     byte[] reserved);

        [DllImport("dcrf32.dll")]
        private static extern short dc_ParsePhotoInfo(int icdev,
                                                      int type,
                                                      int info_len,
                                                      byte[] info,
                                                      ref int photo_len,
                                                      byte[] photo);

        int icdev;
        int retVal = -1;
        bool isValidateSucceed = false;

        ILogController logController;

        public DKT10Controller(IContainerProvider containerProviderArgs)
        {
            logController = containerProviderArgs.Resolve<ILogController>();

            Validate();
        }

        private bool InitIDCardReader()
        {
            try
            {
                icdev = dc_init(100, 0);
            }
            catch (Exception ex)
            {
                logController.WriteLog(ex.Message);
            }

            if (icdev < 0)
            {
                logController.WriteLog("调用身份证读取函数失败![dc_init]");
                return false;
            }
            else
                return true;

        }

        private bool FindIDCard()
        {
            retVal = dc_find_i_d(icdev);

            if (retVal != 0)
            {
                logController.WriteLog("调用身份证读取函数失败![dc_find_i_d]");
                return false;
            }
            else
                return true; 
        }

        private void Validate()
        {
            if (InitIDCardReader())
            {
                if (FindIDCard())
                    isValidateSucceed = true;
            }
        }

        public bool Exit()
        {
            retVal = dc_exit(icdev);

            if (retVal != 0)
                return false;
            else
                return true;
        }

        public bool  Load(out IDCardTransferredType idCardTransferredTypeArgs)
        {
            bool ret = false;
            idCardTransferredTypeArgs = default(IDCardTransferredType);

            if (isValidateSucceed)
            {
                IDCardTransferredType value = new IDCardTransferredType();

                int ret_text_len = 0, ret_photo_len = 0, ret_fingerprint_len = 0, ret_extra_len = 0, ret_anls_photo_len = 65536;

                byte[] ret_text = new byte[256],
                       ret_photo = new byte[1024],
                       ret_fingerprint = new byte[1024],
                       ret_extra = new byte[70];

                byte[] ret_name = new byte[64],
                       ret_sex = new byte[8],
                       ret_nation = new byte[12],
                       ret_birth_day = new byte[36],
                       ret_address = new byte[144],
                       ret_id_number = new byte[76],
                       ret_department = new byte[64],
                       ret_expire_start_day = new byte[36],
                       ret_expire_end_day = new byte[36],
                       ret_reserved = new byte[76];

                byte[] ret_anls_photo = new byte[65536];

                retVal = dc_SamAReadCardInfo(icdev, 1, ref ret_text_len, ret_text, ref ret_photo_len, ret_photo, ref ret_fingerprint_len, ret_fingerprint, ref ret_extra_len, ret_extra);

                if (retVal != 0)
                    logController.WriteLog("调用身份证读取函数失败![dc_SamAReadCardInfo]");
                else
                {
                    retVal = dc_ParseTextInfo(icdev, 0, ret_text_len, ret_text, ret_name, ret_sex, ret_nation, ret_birth_day, ret_address, ret_id_number, ret_department, ret_expire_start_day, ret_expire_end_day, ret_reserved);

                    if (retVal != 0)
                        logController.WriteLog("调用身份证读取函数失败![dc_ParseTextInfo]");
                    else
                    {
                        value.Name = Encoding.Default.GetString(ret_name).Replace("\0", string.Empty).Trim();
                        value.Sex = Encoding.Default.GetString(ret_sex).Replace("\0", string.Empty).Trim();
                        value.Nation = Encoding.Default.GetString(ret_nation).Replace("\0", string.Empty).Trim();
                        value.BirthDay = Encoding.Default.GetString(ret_birth_day).Replace("\0", string.Empty).Trim();
                        value.Address = Encoding.Default.GetString(ret_address).Replace("\0", string.Empty).Trim();
                        value.IDNumber = Encoding.Default.GetString(ret_id_number).Replace("\0", string.Empty).Trim();
                        value.Department = Encoding.Default.GetString(ret_department).Replace("\0", string.Empty).Trim();
                        value.ExpireStartDay = Encoding.Default.GetString(ret_expire_start_day).Replace("\0", string.Empty).Trim();
                        value.ExpireEndDay = Encoding.Default.GetString(ret_expire_end_day).Replace("\0", string.Empty).Trim();

                        retVal = dc_ParsePhotoInfo(icdev, 2, ret_photo_len, ret_photo, ref ret_anls_photo_len, ret_anls_photo);

                        if (retVal != 0)
                            logController.WriteLog("调用身份证读取函数[dc_ParsePhotoInfo]失败!");
                        else
                        {
                            value.Photo = Encoding.Default.GetString(ret_anls_photo).Substring(0, ret_anls_photo_len);

                            logController.WriteLog("获取身份证信息成功![姓名:" + value.Name + ",性别:" + value.Sex + ",国籍:" + value.Nation + ",出生日期:" + value.BirthDay +
                                        ",证件地址:" + value.Address + ",身份证号:" + value.IDNumber + ",发证部门:" + value.Department + ",开始日期:" + value.ExpireStartDay + ",结束日期:" + value.ExpireEndDay +
                                        ",照片:" + value.Photo);

                            idCardTransferredTypeArgs = value;
                            dc_beep(icdev, 10);

                            ret = true;
                        }
                    }
                }
            }

            return ret;
        }
    }
}