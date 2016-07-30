namespace Storage.Interfaces.Factory
{
    public interface IFactory
    {
        T GetInstance<T>(); 
    }
}