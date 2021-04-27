namespace oop.Executors
{
    public interface IFileExecutable<T>
    {
        T Result { get; }
        void Execute(string path);
    }
}