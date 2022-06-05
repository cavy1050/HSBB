using System;
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
    public class GWIController : IIDCardController
	{
		[DllImport("GWI_IDCard_Driver.dll")]
		private static extern int IDCard_OpenDevice(byte[] pszRcCode);

		[DllImport("GWI_IDCard_Driver.dll")]
		private static extern int IDCard_CloseDevice(byte[] pszRcCode);

		[DllImport("GWI_IDCard_Driver.dll")]
		private static extern int IDCard_SetDeviceParam(string devType, string devPort, string devPortParam, byte[] pszRcCode);

		[DllImport("GWI_IDCard_Driver.dll")]
		private static extern void IDCard_SetDeviceTraceLevel(int level);

		[DllImport("GWI_IDCard_Driver.dll")]
		private static extern int IDCard_GetDevid(long dwTimeOut, byte[] devid, byte[] pszRcCode);

		[DllImport("GWI_IDCard_Driver.dll")]
		private static extern int IDCard_ReadIDCardMsg(uint dwTimeOut, byte[] msg, byte[] pszRcCode);

		[DllImport("GWI_IDCard_Driver.dll")]
		private static extern int IDCard_GetCardid(long dwTimeOut, byte[] cardid, byte[] pszRcCode);

		[DllImport("GWI_IDCard_Driver.dll")]
		public static extern int IDCard_ReadIDCardMsgExt(string photoName, int dwTimeOut, byte[] msg, byte[] pszRcCode);

		ISnackbarMessageQueue messageQueue;
		IAppConfigController appConfigController;
		bool isValidateSucceed;

		public GWIController(IContainerProvider containerProviderArgs)
		{
			this.appConfigController = containerProviderArgs.Resolve<IAppConfigController>();
			this.messageQueue = containerProviderArgs.Resolve<ISnackbarMessageQueue>();

			if (appConfigController.IsValidateSucceed)
				Validate();
		}

		private void Validate()
		{
			string devType = "GWI";
			string devPort = "USB";
			string devPortParam = "19200,n,8,1";
			byte[] pszRcCode = new byte[2000];
			isValidateSucceed = false;

			int RetI = IDCard_SetDeviceParam(devType, devPort, devPortParam, pszRcCode);
			if (RetI != 0)
				messageQueue.Enqueue("初始化身份证读卡设备失败!");
			else
			{
				RetI = IDCard_OpenDevice(pszRcCode);
				if (RetI != 0)
					messageQueue.Enqueue("查找身份证失败!");
				else
					isValidateSucceed = true;
			}
		}

		public bool Exit()
		{
			byte[] pszRcCode = new byte[2000];
			int RetI = IDCard_CloseDevice(pszRcCode);

			bool flag = RetI == 0;

			return flag;
		}

		public IDCardTransferredType Load()
        {
			IDCardTransferredType retIDCardTransferredType = new IDCardTransferredType();

			if (isValidateSucceed)
			{
				byte[] pszRcCode = new byte[2000];
				byte[] msg = new byte[4000];
				int RetI = IDCard_ReadIDCardMsg(5000u, msg, pszRcCode);
				bool flag = RetI == 0;

				if (flag)
				{
					string str = Encoding.Default.GetString(msg);
					if (!string.IsNullOrEmpty(str))
					{
						string[] idmsg = str.Split(new char[] { '|' });
						retIDCardTransferredType.IDNumber = idmsg[4].Trim();
						retIDCardTransferredType.Name = idmsg[0].Trim();
						retIDCardTransferredType.Address = idmsg[5].Trim();
						retIDCardTransferredType.Sex = idmsg[1].Trim();
						retIDCardTransferredType.BirthDay = idmsg[2].Trim();
					}
				}
			}

			return retIDCardTransferredType;
		}
    }
}
