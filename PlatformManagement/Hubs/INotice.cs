using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformManagement.Hubs
{
   public interface INotice
   {
       Task<bool> Receive(Message message);
   }
}
