using Storage.Interfaces.Entities.ServiceState;

namespace Storage.Interfaces.Repository
{
    public interface IRepository
    {
        void Save(ServiceState state);

        ServiceState Load();
    }
}