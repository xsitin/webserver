using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace oop.Executors
{
    public class Md5Executor : IFileExecutable<IEnumerable<byte>>
    {
        private readonly byte[] _buffer = new byte[4096];
        private readonly MD5CryptoServiceProvider _md5 = new();

        public IEnumerable<byte> Result
        {
            get
            {
                _md5.TransformFinalBlock(_buffer, 0, 0);
                return (_md5.Hash ?? throw new EvaluateException("invalid hash value")).ToArray();
            }
        }


        public void Execute(string path)
        {
            int length;
            if (!File.Exists(path))
                return;
            using var fs = File.OpenRead(path);
            do
            {
                length = fs.Read(_buffer, 0, _buffer.Length);
                _md5.TransformBlock(_buffer, 0, length, _buffer, 0);
            } while (length > 0);
        }
    }
}