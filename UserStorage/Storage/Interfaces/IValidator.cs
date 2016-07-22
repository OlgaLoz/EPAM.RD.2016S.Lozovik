using Storage.Entities.UserInfo;

namespace Storage.Interfaces
{
    public interface IValidator
    {
        bool IsValid(User user);
    }
}