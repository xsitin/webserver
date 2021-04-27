using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using webServer;

namespace CliWebserver
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new IPAddress(new byte[] {127, 0, 0, 1}), 8081);
                var text = Console.ReadLine();
                socket.Send(Encoding.UTF8.GetBytes(text));
                Console.WriteLine(socket.GetAllUtf8Text());
            }
        }
    }
}