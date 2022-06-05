using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Services
{
    public interface IEntityCertificateController
    {
        T Load<T>() where T : class;
    }
}
