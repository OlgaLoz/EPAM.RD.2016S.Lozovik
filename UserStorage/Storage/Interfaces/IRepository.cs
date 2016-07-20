using Storage.Entities.ServiceState;
using Storage.Service;

namespace Storage.Interfaces
{
    public interface IRepository
    {
        void Save(ServiceState state);
        ServiceState Load();
    }
}