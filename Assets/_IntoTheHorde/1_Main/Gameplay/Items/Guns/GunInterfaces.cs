namespace IntoTheHorde
{
    public interface IReloadable 
    {
        public void Reload();
    }

    public interface IScopeable
    {
        public void OnScope();
        public void OnUnscope();
    }
}
