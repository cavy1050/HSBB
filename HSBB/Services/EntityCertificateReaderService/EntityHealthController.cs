using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Services
{
    public class EntityHealthController : IEntityCertificateController
    {
        public bool Load<T>(out T t) where T : class
        {
            t = default(T);
            return false;
        }
    }
}
