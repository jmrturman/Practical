﻿using Practical.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practical.Services.Interfaces
{
    public interface IServiceBase
    {
        Task<PracticalResult> LookUp(string ipAddress);
    }
}
