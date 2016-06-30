using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Reflection;
using PlatformManagement.Common;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(PlatformManagement.Startup))]
namespace PlatformManagement
{
    public class FilteredCamelCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public FilteredCamelCasePropertyNamesContractResolver()
        {
            AssembliesToInclude = new HashSet<Assembly>();
            TypesToInclude = new HashSet<Type>();
        }
        public HashSet<Assembly> AssembliesToInclude { get; set; }
        public HashSet<Type> TypesToInclude { get; set; }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);

            Type declaringType = member.DeclaringType;
            if (
                TypesToInclude.Contains(declaringType)
                || AssembliesToInclude.Contains(declaringType.Assembly))
            {
                jsonProperty.PropertyName = jsonProperty.PropertyName.ToCamelCase();
            }
            return jsonProperty;
        }     
    }

    public partial class Startup
    {
        private static readonly Lazy<JsonSerializer> JsonSerializerFactory = new Lazy<JsonSerializer>(GetJsonSerializer);

        private static JsonSerializer GetJsonSerializer()
        {
            return new JsonSerializer
            {
                ContractResolver = new FilteredCamelCasePropertyNamesContractResolver
                {

                    AssembliesToInclude =
                    {
                        typeof (Startup).Assembly
                    },

                    TypesToInclude =
                        {
                            typeof(Hubs.Message),
                        }
                }
            };
        }

        public void Configuration(IAppBuilder app)
        {
           
            app.MapSignalR();

            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => JsonSerializerFactory.Value);
        }
    }
}
