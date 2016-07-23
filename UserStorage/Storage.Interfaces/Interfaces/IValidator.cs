using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Interfaces.Interfaces
{
    public interface IValidator
    {
        bool IsValid(User user);
    }
}