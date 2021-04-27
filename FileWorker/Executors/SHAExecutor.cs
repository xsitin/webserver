using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace oop.Executors
{
    public class SHAExecutor : IFileExecutable<IEnumerable<byte>>
    {
        private readonly SHA512CryptoServiceProvider _sha = new();
        private readonly byte[] buffer = new byte[4096];

        public IEnumerable<byte> Result
        {
            get
            {
                _sha.TransformFinalBlock(buffer, 0, 0);
                return (_sha.Hash ?? throw new EvaluateException("invalid hash value")).ToArray();
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
                length = fs.Read(buffer, 0, buffer.Length);
                _sha.TransformBlock(buffer, 0, length, buffer, 0);
            } while (length > 0);
        }
    }
}