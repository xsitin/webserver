using System;
using System.IO;
using System.Linq;
using NetBox.Extensions;
using oop.Executors;

namespace oop
{
    public class FileWorker : IWorker
    {
        private readonly string _path;
        private readonly bool _recursive;

        public FileWorker(string path, bool isRecursive)
        {
            _path = path;
            _recursive = isRecursive;
        }

        public T Execute<T>(ICommand command)
        {
            if (command is IFileExecutable<T> executable)
                return Execute(executable);
            throw new ArgumentException("command should be IFileCommand");
        }

        public T Execute<T>(IFileExecutable<T> command)
        {
            if (!Directory.Exists(_path))
                throw new Exception("invalid path: " + _path);
            foreach (var file in Directory.EnumerateFileSystemEntries(_path, "",
                _recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToHashSet())
                command.Execute(file);
            return command.Result;
        }
    }
}