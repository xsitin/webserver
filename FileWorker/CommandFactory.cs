using System.IO;
using oop.Executors;

namespace oop
{
    public class CommandFactory
    {
        private FileWorker _fileWorker;

        public CommandFactory(string path, bool isRecursive)
        {
            _fileWorker = new FileWorker(path, isRecursive);
        }
        
        
        public ICommand CreateFileCommand<T>(IFileExecutable<T> executable,
            IResulter? resulter)
        {
            return new FileCommand<T>(_fileWorker, executable, resulter);
        }
        
        
        public static ICommand CreateFileCommand<T>(FileWorker fileWorker, IFileExecutable<T> executable,
            IResulter? resulter)
        {
            return new FileCommand<T>(fileWorker, executable, resulter);
        }

        public static ICommand CreateFileCommand<T>(string path, bool isRecursive, IFileExecutable<T> executable,
            IResulter? resulter)
        {
            return new FileCommand<T>(new FileWorker(path, isRecursive), executable, resulter);
        }
    }
}