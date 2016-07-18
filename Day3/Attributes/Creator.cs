using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Attributes
{
    public class Creator
    {
        public List<User> CreateUsers()
        {
            var users = new List<User>();
            var attributes = typeof(User).GetCustomAttributes<InstantiateUserAttribute>();

            foreach (var attribute in attributes)
            {
                if (attribute.Id == null)
                {
                    attribute.Id = MatchParametr(typeof(User), "id");
                }

                var constructorInfo = typeof(User).GetConstructor(new[] {typeof(int)});
                if (constructorInfo != null)
                {
                    var user = (User)constructorInfo.Invoke(new object[] {attribute.Id.Value});
                    user.FirstName = attribute.FirstName;
                    user.LastName = attribute.LastName;
                    users.Add(user);
                }
            }

            return users;
        }

        public List<AdvancedUser> CreateAdvanceUsers()
        {
            var users = new List<AdvancedUser>();

            var assembly = Assembly.GetExecutingAssembly();  
            var attributes = assembly.GetCustomAttributes<InstantiateAdvancedUserAttribute>();

            foreach (var attribute in attributes)
            {
                if (attribute.Id == null)
                {
                    attribute.Id = MatchParametr(typeof(AdvancedUser), "id");
                }
                if (attribute.ExternalId == null)
                {
                    attribute.ExternalId = MatchParametr(typeof(AdvancedUser), "externalId");
                }

                var constructorInfo = typeof(AdvancedUser).GetConstructor(new[] { typeof(int), typeof(int) });
                if (constructorInfo != null)
                {
                    var user = (AdvancedUser)constructorInfo.Invoke(new object[] { attribute.Id.Value, attribute.ExternalId.Value });
                    user.FirstName = attribute.FirstName;
                    user.LastName = attribute.LastName;
                    users.Add(user);
                }
            }

            return users;
        }

        private int MatchParametr(Type type, string paramName)
        {
            var ctors = type.GetConstructors();
            var ctorWithAttribute = 
                ctors.FirstOrDefault(ctor=>ctor.GetCustomAttributes<MatchParameterWithPropertyAttribute>() != null);
            var attribute =
                    ctorWithAttribute?.GetCustomAttributes<MatchParameterWithPropertyAttribute>()
                    .FirstOrDefault(attr=>attr.Parametr == paramName);
            if (attribute == null)
            {
                throw new InvalidOperationException();
            }
            return (int)type.GetProperties().FirstOrDefault(prop => prop.Name == attribute.Property)
                .GetCustomAttribute<DefaultValueAttribute>().Value;
        }
    }
}