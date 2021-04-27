using System.Collections.Generic;
using System.IO;

namespace oop.Executors
{
    public class DirectoriesNameCollectorExecutable:IFileExecutable<List<string>>
    {
        public List<string> Result { get; } = new List<string>();
        public void Execute(string path)
        {
            if (Directory.Exists(path))
            {
                Result.Add(path);   
            }

            
        }
    }
}