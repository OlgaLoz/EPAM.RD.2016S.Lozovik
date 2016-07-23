using System;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Interfaces;

namespace Storage.Validator
{
    [Serializable]
    public class UserValidator : IValidator 
    {
        public bool IsValid(User user)
        {
            return true;
        }
    }
}