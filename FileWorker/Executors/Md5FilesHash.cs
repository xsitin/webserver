using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using NetBox.Extensions;

namespace oop.Executors
{
    public class Md5FilesHash : IFileExecutable<List<(string, string)>>
    {  
        private readonly MD5 _md5 = MD5.Create();
        public List<(string, string)> Result { get; } = new();

        public void Execute(string path)
        {
            if (File.Exists(path))
                Result.Add((Path.GetFileName(path), _md5
                    .ComputeHash(File.OpenRead(path))
                    .ToHexString()
                    .ToUpper()));
        }
    }
}