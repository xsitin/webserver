using System.IO;
using NetBox.Extensions;

namespace oop
{
    public class FileResulter : IResulter
    {
        private readonly string _path;

        public FileResulter(string path)
        {
            _path = path;
        }

        public void WriteResult<T>(T result)
        {
            var writer = new StreamWriter(File.OpenWrite(_path));
            writer.Write(result.JsonSerialise());
            writer.Close();
        }
    }
}