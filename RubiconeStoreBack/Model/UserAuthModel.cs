﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Shared.Model;

namespace RubiconeStoreBack.Model
{
    public class UserAuthModel
    {
        public User User { get; set; }
        public UserSession UserSession { get; set; }
    }
}
