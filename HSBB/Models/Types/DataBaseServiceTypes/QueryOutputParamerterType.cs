using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npoi.Mapper.Attributes;

namespace HSBB.Models
{
    public class QueryOutputParamerterType
    {
        [Ignore]
        public int PageID { get; set; }

        [Ignore]
        public int RowID { get; set; }

        [Column("序号")]
        public int ID { get; set; }

        [Column("姓名")]
        public string Name { get; set; }

        [Column("性别")]
        public string Sex { get; set; }

        [Column("年龄")]
        public int Age { get; set; }

        [Column("证件地址")]
        public string Address { get; set; }

        [Column("身份证号")]
        public string IDNumber { get; set; }

        [Column("采样时间")]
        public string cyDate { get; set; }

        [Column("现住址")]
        public string CurrentAddress { get; set; }

        [Column("手机号")]
        public string PhoneNumber { get; set; }

        [Column("采集方式")]
        public string CurrentSpecimenCollectionType { get; set; }

        [Column("患者类别")]
        public string CurrentPatientCategoryType { get; set; }

        [Column("检测类型")]
        public string CurrentDetectionType { get; set; }

        [Column("职业名称")]
        public string OccupationName { get; set; }

        [Column("行程轨迹")]
        public string TravelTrajectory { get; set; }

        [Column("备注")]
        public string Remarks { get; set; }

        [Column("采样地点")]
        public string SamplingLocation { get; set; }

        [Column("采样人")]
        public string SamplingPerson { get; set; }
    }
}
