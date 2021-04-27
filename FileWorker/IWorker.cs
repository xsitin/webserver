namespace oop
{
    public interface IWorker
    {
        T Execute<T>(ICommand command);
    }
}