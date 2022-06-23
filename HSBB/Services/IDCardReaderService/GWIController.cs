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

		bool isValidateSucceed;
		ILogController logController;

		public GWIController(IContainerProvider containerProviderArgs)
		{
			logController = containerProviderArgs.Resolve<ILogController>();

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
				logController.WriteLog("调用身份证读取函数失败![IDCard_SetDeviceParam]");
			else
			{
				RetI = IDCard_OpenDevice(pszRcCode);
				if (RetI != 0)
					logController.WriteLog("调用身份证读取函数失败![IDCard_OpenDevice]");
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

		public bool Load(out IDCardTransferredType idCardTransferredTypeArgs)
        {
			bool ret = false;
			idCardTransferredTypeArgs = default(IDCardTransferredType);

			if (isValidateSucceed)
			{
				IDCardTransferredType value = new IDCardTransferredType();

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
						value.IDNumber = idmsg[4].Trim();
						value.Name = idmsg[0].Trim();
						value.Address = idmsg[5].Trim();
						value.Sex = idmsg[1].Trim();
						value.BirthDay = idmsg[2].Trim();

						idCardTransferredTypeArgs = value;
						ret = true;
					}
				}
			}

			return ret;
		}
    }
}
