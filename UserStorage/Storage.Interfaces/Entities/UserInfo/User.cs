using System;

namespace Storage.Interfaces.Entities.UserInfo
{
    [Serializable]
    public class User : IEquatable<User>
    {
        public int PersonalId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirdth { get; set; }

        public Gender Gender { get; set; }

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