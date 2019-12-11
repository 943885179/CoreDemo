using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace CoreDemo_GrpcService.Services
{
    public class UserService : User.UserBase
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override Task<UserResult> GetUser(UserRequest request, ServerCallContext context)
        {
            var Id = request.Message;
           return Task.FromResult(new UserResult()
           {
               Name = "weixiao",
               Age=21
           });
        }

        public override Task<UserResult> GetUserDetail(UserDetail request, ServerCallContext context)
        {
            return base.GetUserDetail(request, context);
        }

        public override Task<UserResult> GetUsers(Empty request, ServerCallContext context)
        {
            return base.GetUsers(request, context);
        }

        public override Task<UserType> GetUserType(UserType request, ServerCallContext context)
        {
            return Task.FromResult(request);
            // return base.GetUserType(request, context);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
