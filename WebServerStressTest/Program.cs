using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using webServer;
using webServer.MyTP;

namespace WebServerStressTest
{
    class Program
    {
        class Load : IThreadedTask
        {
            public void Run()
            {
                while (true)
                {
                    using var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
                        socket.Send(Encoding.UTF8.GetBytes("GET / \n"));
                        var text = socket.GetAllUtf8Text();
                    }
                    catch
                    {
                        Console.WriteLine("error");
                    }
                    finally
                    {
                        socket.Close();
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(18,18);
            ThreadPool.SetMaxThreads(18, 18);
            var tp = ThreadDispatcher.GetInstance();
            tp.Add(new Load());
            tp.Add(new Load());
            //tp.Run();
        }
    }
}