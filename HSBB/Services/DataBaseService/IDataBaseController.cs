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
    }
}
