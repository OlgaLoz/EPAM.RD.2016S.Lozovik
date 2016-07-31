using System;
using System.Runtime.Serialization;

namespace Storage.Interfaces.Entities.UserInfo
{
    [DataContract]
    public class User : IEquatable<User>
    {
        [DataMember]
        public int PersonalId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public DateTime DateOfBirdth { get; set; }

        [DataMember]
        public Gender Gender { get; set; }

        [DataMember]
        public Visa[] Visas { get; set; }

        public bool Equals(User other)
        {
            if (other == null)
            {
                return false;
            }

            return PersonalId == other.PersonalId &&
                FirstName == other.FirstName &&
                LastName == other.LastName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj.GetType() == typeof(User) && Equals((User)obj);
        }

        public override int GetHashCode()
        {
            int hash = PersonalId.GetHashCode();
            hash ^= FirstName?.GetHashCode() ?? 0;
            hash ^= LastName?.GetHashCode() ?? 0;
            hash ^= DateOfBirdth.GetHashCode();
            hash ^= Gender.GetHashCode();

            return hash;
        }
    }
}