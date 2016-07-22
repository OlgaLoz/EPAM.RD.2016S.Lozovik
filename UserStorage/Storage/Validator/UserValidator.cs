using System;
using Storage.Entities.UserInfo;
using Storage.Interfaces;

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