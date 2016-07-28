using Storage.Interfaces.Entities.ServiceState;

namespace Storage.Interfaces.Interfaces
{
    public interface IRepository
    {
        void Save(ServiceState state);

        ServiceState Load();
    }
}