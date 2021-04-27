using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using NetBox.Extensions;
using webServer.MyTP;

namespace webServer
{
    public class CliServer : IThreadedTask
    {
        private readonly FileServer _fs;
        private readonly int _port;


        public CliServer(FileServer fs, int port)
        {
            _fs = fs;
            _port = port;
        }

        public void Run()
        {
            var tq = TaskQueue.GetInstance();
            var sock = new Socket(SocketType.Stream, ProtocolType.Tcp);
            sock.Bind(new IPEndPoint(IPAddress.Any, _port));
            sock.Listen();

            while (true)
            {
                var socket = sock.Accept();
                var text = ReceiveAllText(socket);

                if (text.StartsWith("list"))
                    socket.Send(GetFileNames());
                else if (text.StartsWith("hash"))
                    socket.Send(GetHash(text));
                else if (text.StartsWith("size"))
                    socket.Send(GetFileSize(text));
                else if (text.StartsWith("status"))
                    socket.Send(_fs.isRunning
                        ? Encoding.UTF8.GetBytes("active")
                        : Encoding.UTF8.GetBytes("stopped"));
                else if (text.StartsWith("stop"))
                {
                    _fs.isRunning = false;
                    socket.Send(Encoding.UTF8.GetBytes("stopped"));
                }
                else if (text.StartsWith("start"))
                {
                    if (!_fs.isRunning)
                    {
                        _fs.isRunning = true;
                        ThreadDispatcher.GetInstance().AddInQueue(_fs);
                    }

                    socket.Send(Encoding.UTF8.GetBytes("started"));
                }
            }
        }

        private byte[] GetFileSize(string text)
        {
            return Encoding.UTF8.GetBytes(
                new FileInfo(
                        Path.Combine(_fs._path,
                            text.Split()[1]))
                    .Length
                    .ToString());
        }

        private byte[] GetHash(string text)
        {
            var hash = MD5.Create().ComputeHash(new FileInfo(
                Path.Combine(_fs._path,
                    text.Split()[1])).OpenRead());
            return Encoding.UTF8.GetBytes(
                hash.JsonSerialise());
        }

        private byte[] GetFileNames()
        {
            return Encoding.UTF8.GetBytes(Directory
                .EnumerateFileSystemEntries(_fs._path, "", SearchOption.TopDirectoryOnly).JsonSerialise());
        }

        private string ReceiveAllText(Socket socket)
        {
            var buff = new byte[4096];
            var textbuff = new StringBuilder();
            var count = 4096;
            while (count == 4096)
            {
                count = socket.Receive(buff);
                textbuff.Append(Encoding.UTF8.GetString(buff.Take(count).ToArray()));
            }

            return textbuff.ToString();
        }
    }
}
/*
Приложение занимает порт 8081, на котором поднимается cli, позволяющая получать диагностику сервера:
	- list - получить список файлов в директории, на которую смотрит FileWorker.
	- hash <filename> - получить хэш соответствующего файла.
	- size <filename> - размер файла
	- status - статус работы сервера(active, stoped)
	- stop - остановить именно http-сервер
	- start - запустить http-сервер
*/