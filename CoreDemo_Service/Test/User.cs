using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Service.Test
{
    public class User : IUser
    {
        public string GetName(string name)
        {
            return name;
        }
    }
}
