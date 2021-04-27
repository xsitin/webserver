using System;
using webServer.MyTP;

namespace webServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var tp = ThreadDispatcher.GetInstance();
            tp.Run();
            var fs = new FileServer(@"C:\Users\m9cko", 8080);
            tp.AddInQueue(fs);
            tp.AddInQueue(new CliServer(fs,8081));
            tp.SetPoolSize(1);
            tp.SetPoolSize(10);
        }
    }
}