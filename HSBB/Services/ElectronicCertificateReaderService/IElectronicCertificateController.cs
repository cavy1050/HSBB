﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSBB.Models;

namespace HSBB.Services
{
    public interface IElectronicCertificateController
    {
        bool Load<T>(out T t,string electronicCertificateStringArgs) where T : class;
    }
}