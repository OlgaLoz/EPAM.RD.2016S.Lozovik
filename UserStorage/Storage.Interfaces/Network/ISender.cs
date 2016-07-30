namespace Storage.Interfaces.Network
{
    public interface ISender
    {
        void Send<T>(T message);
    }
}