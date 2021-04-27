using System;
using NetBox.Extensions;

namespace oop
{
    public class ConsoleResulter : IResulter
    {
        public void WriteResult<T>(T result)
        {
            Console.WriteLine(result.JsonSerialise());
        }
    }
}