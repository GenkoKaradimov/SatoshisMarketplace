using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoshisMarketplace.Entities
{
    public enum UserLogType
    {
        [Description("Account Created")]
        AccountCreated,

        [Description("User Logged In")]
        Login,

        [Description("User Logged Out")]
        Logout,

        [Description("Profile Picture Set")]
        ProfilePictureSet,

        [Description("Password Changed")]
        PasswordChanged
    }
}
