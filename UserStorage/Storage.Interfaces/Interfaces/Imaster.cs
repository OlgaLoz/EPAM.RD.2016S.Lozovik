using System;
using Storage.Interfaces.Entities.UserEventArgs;

namespace Storage.Interfaces.Interfaces
{
    public interface IMaster : IUserService
    {
        event EventHandler<UserEventArgs> AddUser;
        event EventHandler<UserEventArgs> DeleteUser;

        void Save();
        void Load();
    }
}