namespace IntoTheHorde
{
    public interface IRegisterable<T>
    {
        void Register(T obj);
        void Unregister(T obj);
    }
}
