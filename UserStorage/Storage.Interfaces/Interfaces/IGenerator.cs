namespace Storage.Interfaces.Interfaces
{
    public interface IGenerator
    {
        int CurrentId { get; }
        void LoadState(int currentState);
        int GetNextId();
    }
}