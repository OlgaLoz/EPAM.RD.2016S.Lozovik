using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Interfaces.Validator
{
    public interface IValidator
    {
        bool IsValid(User user);
    }
}