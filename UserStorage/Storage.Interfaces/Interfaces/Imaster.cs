namespace Storage.Interfaces.Interfaces
{
    public interface IMaster : IUserService
    {
        void Save();

        void Load();
    }
}