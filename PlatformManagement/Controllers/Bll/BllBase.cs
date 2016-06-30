using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlatformManagement.Controllers.Bll
{
    public abstract class BllBase
    {
        protected readonly string _key;

        public BllBase(string key)
        {
            this._key = key;
        }
    }
}