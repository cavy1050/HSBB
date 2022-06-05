﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Prism.Ioc;
using MaterialDesignThemes.Wpf;
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
        int ret = -1;
        bool isValidateSucceed;

        ISnackbarMessageQueue messageQueue;
        IAppConfigController appConfigController;

        public DKT10Controller(IContainerProvider containerProviderArgs)
        {
            this.appConfigController = containerProviderArgs.Resolve<IAppConfigController>();
            this.messageQueue = containerProviderArgs.Resolve<ISnackbarMessageQueue>();

            if (appConfigController.IsValidateSucceed)
                Validate();
        }

        private bool InitIDCardReader()
        {
            icdev = dc_init(100, 0);

            if (icdev < 0)
            {
                messageQueue.Enqueue("初始化身份证读卡设备失败!");
                return false;
            }
            else
                return true;
        }

        private bool FindIDCard()
        {
            ret = dc_find_i_d(icdev);

            if (ret < 0)
            {
                messageQueue.Enqueue("查找身份证失败!");
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
            ret = dc_exit(icdev);

            if (ret != 0)
                return false;
            else
                return true;
        }

        public IDCardTransferredType Load()
        {
            IDCardTransferredType retIDCardTransferredType = new IDCardTransferredType();

            if (isValidateSucceed)
            {
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

                ret = dc_SamAReadCardInfo(icdev, 1, ref ret_text_len, ret_text, ref ret_photo_len, ret_photo, ref ret_fingerprint_len, ret_fingerprint, ref ret_extra_len, ret_extra);

                if (ret == 0)
                {
                    ret = dc_ParseTextInfo(icdev, 0, ret_text_len, ret_text, ret_name, ret_sex, ret_nation, ret_birth_day, ret_address, ret_id_number, ret_department, ret_expire_start_day, ret_expire_end_day, ret_reserved);

                    if (ret == 0)
                    {
                        retIDCardTransferredType.Name = Encoding.Default.GetString(ret_name).Replace("\0", string.Empty).Trim();
                        retIDCardTransferredType.Sex = Encoding.Default.GetString(ret_sex).Replace("\0", string.Empty).Trim();
                        retIDCardTransferredType.Nation = Encoding.Default.GetString(ret_nation).Replace("\0", string.Empty).Trim();
                        retIDCardTransferredType.BirthDay = Encoding.Default.GetString(ret_birth_day).Replace("\0", string.Empty).Trim();
                        retIDCardTransferredType.Address = Encoding.Default.GetString(ret_address).Replace("\0", string.Empty).Trim();
                        retIDCardTransferredType.IDNumber = Encoding.Default.GetString(ret_id_number).Replace("\0", string.Empty).Trim();
                        retIDCardTransferredType.Department = Encoding.Default.GetString(ret_department).Replace("\0", string.Empty).Trim();
                        retIDCardTransferredType.ExpireStartDay = Encoding.Default.GetString(ret_expire_start_day).Replace("\0", string.Empty).Trim();
                        retIDCardTransferredType.ExpireEndDay = Encoding.Default.GetString(ret_expire_end_day).Replace("\0", string.Empty).Trim();

                        ret = dc_ParsePhotoInfo(icdev, 2, ret_photo_len, ret_photo, ref ret_anls_photo_len, ret_anls_photo);

                        if (ret == 0)
                        {
                            retIDCardTransferredType.Photo = Encoding.Default.GetString(ret_anls_photo).Substring(0, ret_anls_photo_len);

                            dc_beep(icdev, 10);
                        }
                    }
                }
            }

            return retIDCardTransferredType;
        }
    }
}