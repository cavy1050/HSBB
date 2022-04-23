using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using Prism.Mvvm;

namespace HSBB.Models
{
    public class IDCardModel : BindableBase
    {
        string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        string sex;
        public string Sex
        {
            get => sex;
            set
            {
                sex = value;
                this.CurrentSexType = SexTypes.FirstOrDefault(x => x.SexCode == sex);
            }
        }

        ObservableCollection<SexType> sexTypes;
        public ObservableCollection<SexType> SexTypes
        {
            get => sexTypes;
            set => SetProperty(ref sexTypes, value);
        }

        SexType currentSexType;
        public SexType CurrentSexType
        {
            get => currentSexType;
            set => SetProperty(ref currentSexType, value);
        }

        string nation;
        public string Nation
        {
            get => nation;
            set
            {
                nation = value;
                this.CurrentNationType = NationTypes.FirstOrDefault(x => x.NationCode == nation);
            }
        }

        ObservableCollection<NationType> nationTypes;
        public ObservableCollection<NationType> NationTypes
        {
            get => nationTypes;
            set => SetProperty(ref nationTypes, value);
        }

        NationType currentNationType;
        public NationType CurrentNationType
        {
            get => currentNationType;
            set => SetProperty(ref currentNationType, value);
        }

        string birthDay;
        public string BirthDay
        {
            get => birthDay;
            set
            {
                birthDay = value;

                DateTime dateTimeBirthDay = DateTime.ParseExact(birthDay, "yyyyMMdd", CultureInfo.CurrentCulture);
                DateTime dateTimeNow = DateTime.Now;

                this.Age = dateTimeNow.Year - dateTimeBirthDay.Year;
            }
        }

        int age;
        public int Age
        {
            get => age;
            set => SetProperty(ref age, value);
        }

        string address;
        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        string idNumber;
        public string IDNumber
        {
            get => idNumber;
            set => SetProperty(ref idNumber, value);
        }

        string department;
        public string Department
        {
            get => department;
            set => SetProperty(ref department, value);
        }

        string expireStartDay;
        public string ExpireStartDay
        {
            get => expireStartDay;
            set => SetProperty(ref expireStartDay, value);
        }

        string expireEndDay;
        public string ExpireEndDay
        {
            get => expireEndDay;
            set => SetProperty(ref expireEndDay, value);
        }

        string photo;
        public string Photo
        {
            get => photo;
            set => SetProperty(ref photo, value);
        }

        public IDCardModel()
        {
            this.SexTypes = new ObservableCollection<SexType>
            {
                new SexType { SexCode = "1", SexName = "男" },
                new SexType { SexCode = "2", SexName = "女" }
            };

            this.nationTypes = new ObservableCollection<NationType>
            {
                new NationType{ NationCode="01", NationName= "汉族"},
                new NationType{ NationCode="02", NationName="蒙古族"},
                new NationType{ NationCode="03", NationName="回族"},
                new NationType{ NationCode="04", NationName="藏族"},
                new NationType{ NationCode="05", NationName="维吾尔族"},
                new NationType{ NationCode="06", NationName="苗族"},
                new NationType{ NationCode="07", NationName= "彝族"},
                new NationType{ NationCode="08", NationName= "壮族"},
                new NationType{ NationCode="09", NationName="布依族"},
                new NationType{ NationCode="10", NationName= "朝鲜族"},
                new NationType{ NationCode="11", NationName= "满族"},
                new NationType{ NationCode="12", NationName= "侗族"},
                new NationType{ NationCode="13", NationName= "瑶族"},
                new NationType{ NationCode="14", NationName="白族"},
                new NationType{ NationCode="15", NationName= "土家族"},
                new NationType{ NationCode="16", NationName="哈尼族"},
                new NationType{ NationCode="17", NationName= "哈萨克族"},
                new NationType{ NationCode="18", NationName="傣族"},
                new NationType{ NationCode="19", NationName= "黎族"},
                new NationType{ NationCode="20", NationName="傈僳族"},
                new NationType{ NationCode="21", NationName="佤族"},
                new NationType{ NationCode="22", NationName= "畲族"},
                new NationType{ NationCode="23", NationName="高山族"},
                new NationType{ NationCode="24", NationName="拉祜族"},
                new NationType{ NationCode="25", NationName="水族"},
                new NationType{ NationCode="26", NationName= "东乡族"},
                new NationType{ NationCode="27", NationName= "纳西族"},
                new NationType{ NationCode="28", NationName="景颇族"},
                new NationType{ NationCode="29", NationName= "柯尔克孜族"},
                new NationType{ NationCode="30", NationName= "土族"},
                new NationType{ NationCode="31", NationName= "达翰尔族"},
                new NationType{ NationCode="32", NationName="仫佬族"},
                new NationType{ NationCode="33", NationName="羌族"},
                new NationType{ NationCode="34", NationName= "布朗族"},
                new NationType{ NationCode="35", NationName= "撒拉族"},
                new NationType{ NationCode="36", NationName="毛南族"},
                new NationType{ NationCode="37", NationName="仡佬族"},
                new NationType{ NationCode="38", NationName= "锡伯族"},
                new NationType{ NationCode="39", NationName="阿昌族"},
                new NationType{ NationCode="40", NationName="普米族"},
                new NationType{ NationCode="41", NationName="塔吉克族"},
                new NationType{ NationCode="42", NationName="怒族"},
                new NationType{ NationCode="43", NationName="乌孜别克族"},
                new NationType{ NationCode="44", NationName= "俄罗斯族"},
                new NationType{ NationCode="45", NationName= "鄂温克族"},
                new NationType{ NationCode="46", NationName="德昂族"},
                new NationType{ NationCode="47", NationName="保安族"},
                new NationType{ NationCode="48", NationName= "裕固族"},
                new NationType{ NationCode="49", NationName= "京族"},
                new NationType{ NationCode="50", NationName= "塔塔尔族"},
                new NationType{ NationCode="51", NationName= "独龙族"},
                new NationType{ NationCode="52", NationName= "鄂伦春族"},
                new NationType{ NationCode="53", NationName="赫哲族"},
                new NationType{ NationCode="54", NationName= "门巴族"},
                new NationType{ NationCode="55", NationName= "珞巴族"},
                new NationType{ NationCode="56", NationName="基诺族"},
                new NationType{ NationCode="57", NationName= "其它"},
                new NationType{ NationCode="98", NationName="外国人入籍"}
            };
        }
    }
}