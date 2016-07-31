using Storage.Interfaces.Services;

namespace Storage.Interfaces.ServiceInfo
{
    public interface IWcfHelper
    {
        void Open(string host);

        void Close();
    }
}