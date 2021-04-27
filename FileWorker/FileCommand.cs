using oop.Executors;

namespace oop
{
    public class FileCommand<T> : ICommand
    {
        private readonly IFileExecutable<T> _executable;
        private readonly FileWorker _fileWorker;
        private readonly IResulter? _resulter;

        internal FileCommand(FileWorker fileWorker, IFileExecutable<T> executable, IResulter? resulter = null)
        {
            _fileWorker = fileWorker;
            _executable = executable;
            _resulter = resulter;
        }

        public void Execute()
        {
            var result = _fileWorker.Execute(_executable);
            _resulter?.WriteResult(result);
        }
    }
}