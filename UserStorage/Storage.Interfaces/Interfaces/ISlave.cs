namespace Storage.Interfaces.Interfaces
{
    public interface ISlave : IUserService
    {
        void ListenForUpdate();
        void InitializeCollection();
    }
}