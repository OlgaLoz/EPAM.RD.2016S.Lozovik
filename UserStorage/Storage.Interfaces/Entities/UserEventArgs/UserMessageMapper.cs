using Storage.Interfaces.Entities.ConnectionInfo;

namespace Storage.Interfaces.Entities.UserEventArgs
{
    public static class UserMessageMapper
    {
        public static UserEventArgs ToUserEventArgs(this Message message)
        {
            return new UserEventArgs
            {
                User = message.User,
                Operation = message.Operation
            };
        }
    }
}