using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;


namespace PlatformManagement.Models
{

    public class BaseEnity
    {
        public BaseEnity() { }
        public BaseEnity(bool isAutoGuid)
        {
            this.SetGuid();
        }

        public string Guid { get; set; }

        public BaseEnity SetGuid()
        {
            this.Guid = System.Guid.NewGuid().ToString();
            return this;
        }

    }

    public class Dyna : DynamicObject
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public object GetPropertyValue(string propertyName)
        {
            if (_values.ContainsKey(propertyName) == true)
            {
                return _values[propertyName];
            }
            return null;
        }

        public object this[string name]
        {
            get
            {
                return _values.ContainsKey(name) ? _values[name] : null;
            }

        }
        public void SetPropertyValue(string propertyName, object value)
        {
            if (_values.ContainsKey(propertyName) == true)
            {
                _values[propertyName] = value;
            }
            else
            {
                _values.Add(propertyName, value);
            }
        }

        public T ConvertToObject<T>()
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(this._values);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetPropertyValue(binder.Name);
            return result != null;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetPropertyValue(binder.Name, value);
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var theDelegateObj = GetPropertyValue(binder.Name) as DelegateObj;
            if (theDelegateObj == null || theDelegateObj.CallMethod == null)
            {
                result = null;
                return false;
            }
            result = theDelegateObj.CallMethod(this, args);
            return true;
        }
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return base.TryInvoke(binder, args, out result);
        }
    }

    public class DelegateObj
    {
        public delegate object DDelegate(dynamic sender, params object[] param);

        private DDelegate _delegate;
        public DDelegate CallMethod
        {
            get { return _delegate; }
        }
        private DelegateObj(DDelegate d)
        {
            _delegate = d;
        }
        public static DelegateObj Function(DDelegate d)
        {
            return new DelegateObj(d);
        }
    }
}