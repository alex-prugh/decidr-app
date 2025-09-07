using Decidr.Infrastructure.EntityFramework.Models;
using Decidr.Operations.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decidr.Infrastructure.Users;

public static class UserExtensions
{
    public static User ToBusinessObject(this UserEntity userEntity)
    {
        return new User
        {
            Id = userEntity.Id,
            Username = userEntity.Username,
        };
    }
}
