using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSBB.Models;

namespace HSBB.Services
{
    public interface IDataBaseController
    {
        bool Save(RegisterModel registerModelArgs);

        IEnumerable<T> Query<T>(string beginDateStringArgs, string endDateStringArgs) where T : class;
    }
}
