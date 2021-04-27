using System.Collections.Generic;
using System.IO;

namespace oop.Executors
{
    public class FileNamesCollectionExecutor : IFileExecutable<List<string>>
    {
        public List<string> Result { get; } = new();

        public void Execute(string path)
        {
            if (File.Exists(path))
                Result.Add(Path.GetFileName(path));
        }
    }
}