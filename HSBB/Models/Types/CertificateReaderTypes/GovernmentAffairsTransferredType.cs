using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Models
{
    /// <summary>
    /// 渝康码与健康码完成http请求后的类型,交由shellmodel完成格式转换,取一个统一的名字:政务码
    /// </summary>
    public class GovernmentAffairsTransferredType
    {
        public string Name { get; set; }
        public string Sex { get; set; }
        public string BirthDay { get; set; }
        public string Address { get; set; }
        public string IDNumber { get; set; }
        public string PhoneNumber { get; set; }
    }
}

