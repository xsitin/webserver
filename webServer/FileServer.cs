using System.Net;
using System.Net.Sockets;
using webServer.MyTP;

namespace webServer
{
    public class FileServer : IThreadedTask
    {
        public readonly string _path;
        public bool isRunning = true;
        private int _port;
        private Socket _socket;

        public FileServer(string path, int port)
        {
            _path = path;
            _port = port;
        }

        public void Run()
        {
            var tq = TaskQueue.GetInstance();
            if (_socket is null)
            {
                _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                _socket.Bind(new IPEndPoint(IPAddress.Any, _port));
            }


            _socket.Listen();

            while (isRunning)
            {
                var socket = _socket.Accept();
                tq.AddTask(new FileServerConnection(socket, (string) _path.Clone()));
            }

            _socket.Close();
            _socket = null;
        }
    }
}