using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Interfaces.Entities.CustomSerialization.Mappers
{
    public static class UserMappers
    {
        public static SerializableUser ToSerializableUser(this User inputUser)
        {
            var user = new SerializableUser
            {
                PersonalId = inputUser.PersonalId,
                FirstName = inputUser.FirstName,
                LastName = inputUser.LastName,
                DateOfBirdth = inputUser.DateOfBirdth,
                Gender = inputUser.Gender,
                Visas = new SerializableVisa[inputUser.Visas?.Length ?? 0],
            };

            for (int i = 0; i < inputUser.Visas?.Length; i++)
            {
                user.Visas[i] = new SerializableVisa
                {
                    Country = inputUser.Visas[i].Country,
                    Start = inputUser.Visas[i].Start,
                    End = inputUser.Visas[i].End
                };
            }

            return user;
        }

        public static User ToUser(this SerializableUser inputUser)
        {
            var user = new User
            {
                PersonalId = inputUser.PersonalId,
                FirstName = inputUser.FirstName,
                LastName = inputUser.LastName,
                DateOfBirdth = inputUser.DateOfBirdth,
                Gender = inputUser.Gender,
                Visas = new Visa[inputUser.Visas?.Length ?? 0],
            };

            for (int i = 0; i < inputUser.Visas?.Length; i++)
            {
                user.Visas[i] = new Visa
                {
                    Country = inputUser.Visas[i].Country,
                    Start = inputUser.Visas[i].Start,
                    End = inputUser.Visas[i].End
                };
            }

            return user;
        }
    }
}