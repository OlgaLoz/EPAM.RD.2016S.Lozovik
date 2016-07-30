namespace Storage.Interfaces.Generator
{
    public interface IGenerator
    {
        int CurrentId { get; }

        void LoadState(int currentState);

        int GetNextId();
    }
}