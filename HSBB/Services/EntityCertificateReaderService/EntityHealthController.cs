using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Services
{
    public class EntityHealthController : IEntityCertificateController
    {
        public T Load<T>() where T : class
        {
            return default(T);
        }
    }
}
