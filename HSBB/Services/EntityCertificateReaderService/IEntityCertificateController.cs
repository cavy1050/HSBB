﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBB.Services
{
    public interface IEntityCertificateController
    {
        bool Load<T>(out T t) where T : class;
    }
}
