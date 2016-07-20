using System.Security.Cryptography.X509Certificates;

namespace FibbonacciGenerator.Interface
{
    public interface IGenerator
    {
        int CurrentId { get; }
        void LoadState(int currentState);
        int GetNextId();
    }
}