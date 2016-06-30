using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformManagement.Filter
{
    public class UserLoginException : Exception
    {
        public UserLoginException(string mesage) : base(mesage)
        {
            
        }
    }
}
