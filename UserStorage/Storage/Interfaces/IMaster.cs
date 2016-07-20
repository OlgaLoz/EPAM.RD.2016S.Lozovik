using System;
using Storage.Entities.UserEventArgs;

namespace Storage.Interfaces
{
    public interface IMaster : IUserService
    {
        event EventHandler<UserEventArgs> AddUser;
        event EventHandler<UserEventArgs> DeleteUser;

        void Save();
        void Load();
    }
}