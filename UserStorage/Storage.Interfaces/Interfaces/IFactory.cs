namespace Storage.Interfaces.Interfaces
{
    public interface IFactory
    {
        T GetInstance<T>(); 
    }
}