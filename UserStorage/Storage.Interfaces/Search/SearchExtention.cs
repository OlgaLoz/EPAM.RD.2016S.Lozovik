using System;
using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Interfaces.Search
{
    public static class SearchExtention 
    {
        public static Predicate<User>[] ConvertToPredicate(this User entity)
        {
            if (entity == null)
            {
                return new Predicate<User>[] { u => true };
            }

            return new Predicate<User>[]
            {
                u => u.PersonalId == entity.PersonalId,
                u => entity.FirstName != null && u.FirstName == entity.FirstName,
                u => entity.LastName != null && u.LastName == entity.LastName
            };
        }
    }
}