﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Services.Models.UserService
{
    public class UserModel
    {
        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }
    }
}
