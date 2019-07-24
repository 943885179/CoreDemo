using Interfaces;
using System;

namespace BLLS
{
    public class UserService : IUserService
    {
        public string GetName(string name)
        {
            return name;
        }
    }
}
