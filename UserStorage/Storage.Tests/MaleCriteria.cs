using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Search;

namespace Storage.Tests
{
    public class MaleCriteria : SearchCriteria<User>
    {
        public MaleCriteria()
        {
            Entity = TestInfo.Users[0];
        }

        public override bool Compare(User entityToCompare)
            => (entityToCompare.Gender == Entity.Gender);
    }
}