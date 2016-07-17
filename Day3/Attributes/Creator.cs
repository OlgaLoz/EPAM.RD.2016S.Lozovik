using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Attributes
{
    public class Creator
    {
        public IEnumerable<User> CreateUsers()
        {
            var users = new List<User>();
            var attributes = typeof(User).GetCustomAttributes<InstantiateUserAttribute>();

            foreach (var attribute in attributes)
            {
                if (attribute.Id == null)
                {
                    attribute.Id = MatchParametr(typeof(User), "id");
                }
                users.Add(new User(attribute.Id.Value) {FirstName = attribute.FirstName, LastName = attribute.LastName});
            }

            return users;
        }

        public IEnumerable<AdvancedUser> CreateAdvanceUsers()
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
                users.Add(new AdvancedUser(attribute.Id.Value, attribute.ExternalId.Value) { FirstName = attribute.FirstName, LastName = attribute.LastName });
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