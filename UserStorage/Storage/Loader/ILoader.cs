using Storage.Service;

namespace Storage.Loader
{
    public interface ILoader
    {
        void Save(ServiceState state);
        ServiceState Load();
    }
}