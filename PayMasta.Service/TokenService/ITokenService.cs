﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.TokenService
{
    public interface ITokenService
    {
        bool IsSessionValid(string token);
    }
}