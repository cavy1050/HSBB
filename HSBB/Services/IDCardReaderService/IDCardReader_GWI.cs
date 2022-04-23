using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using HSBB.Models;

namespace HSBB.Services
{
    public class IDCardReader_GWI : IIDCardReaderService
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

		public bool ExitIDCardReader()
        {
			byte[] pszRcCode = new byte[2000];
			int RetI = IDCard_CloseDevice(pszRcCode);

			bool flag = RetI == 0;

			return flag;
		}

		public bool InitIDCardReader()
		{
			string devType = "GWI";
			string devPort = "USB";
			string devPortParam = "19200,n,8,1";
			byte[] pszRcCode = new byte[2000];
			int RetI = IDCard_SetDeviceParam(devType, devPort, devPortParam, pszRcCode);
			bool flag = RetI == 0;
			bool result;
			if (flag)
			{
				RetI = IDCard_OpenDevice(pszRcCode);
				bool flag2 = RetI == 0;
				result = flag2;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public IDCardModel LoadIDCardReader()
        {
			IDCardModel retIDCardModel = new IDCardModel();
			byte[] pszRcCode = new byte[2000];
			byte[] msg = new byte[4000];
			int RetI = IDCard_ReadIDCardMsg(5000u, msg, pszRcCode);
			bool flag = RetI == 0;
			string result;
			if (flag)
			{
				string str = Encoding.Default.GetString(msg);
				if (!string.IsNullOrEmpty(str))
                {
					string[] idmsg = str.Split(new char[]{'|'});
					retIDCardModel.IDNumber = idmsg[4].Trim();
					retIDCardModel.Name = idmsg[0].Trim();
					retIDCardModel.Address = idmsg[5].Trim();
					retIDCardModel.Sex = idmsg[1].Trim();
					retIDCardModel.BirthDay = idmsg[2].Trim();
				}
			}

			return retIDCardModel;
		}
    }
}
