using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using oop;
using oop.Executors;
using webServer.MyTP;

namespace webServer
{
    public class FileServerConnection : IThreadedTask
    {
        private readonly Socket _socket;
        private readonly string _path;

        public FileServerConnection(Socket socket, string path)
        {
            _socket = socket;
            _path = path;
        }

        public void Run()
        {
            try
            {
                var url = GetUrl();
                var filepath = Path.Combine(_path, url[1..]);
                if (url[0] != '/' || (!File.Exists(filepath) && !Directory.Exists(filepath)))
                    Send404HTTP();
                else if (File.Exists(filepath))
                    SendFile(filepath);
                else
                    SendHTTPAnswer(GetHtmlAnswer(url));
            }
            catch
            {
                Console.WriteLine("error");
            }
            finally
            {
                _socket.Close();
                _socket.Dispose();
            }
        }

        private void SendFile(string filepath)
        {
            var file = new FileInfo(filepath);
            var fileBody = new byte[file.Length];
            file.OpenRead().Read(fileBody);
            _socket.Send(Encoding.UTF8
                .GetBytes($"HTTP/1.1 200 OK\nContent-Length: {file.Length}\nConnection: close\n\n")
                .Concat(fileBody).ToArray());
        }

        private string GetUrl()
        {
            var bufferedText = _socket.GetAllUtf8Text();
            var url = bufferedText[..bufferedText.IndexOf('\n')].Split()[1];
            return url;
        }

        private string GetHtmlAnswer(string url)
        {
            return File.ReadAllText("htmlContent/Header.html") +
                   (url == "/"
                       ? ""
                       : "<a class=\"btn btn-secondary\" href=" +
                         $"{Path.GetRelativePath(_path, Directory.GetParent(url)?.FullName ?? string.Empty)}>" +
                         "<-" +
                         "</a>") +
                   GetHtmlBodyForPath(url) +
                   File.ReadAllText("htmlContent/bottom.html");
        }

        private string GetHtmlBodyForPath(string path)
        {
            var fullpath = Path.Combine(_path, path[1..]);
            var files = new HtmlResulter(path);
            var dirs = new HtmlResulter(path);
            try
            {
                var fac = new CommandFactory(fullpath, false);
                fac.CreateFileCommand(new FileNamesCollectionExecutor(), files)
                    .Execute();
                fac.CreateFileCommand(new DirectoriesNameCollectorExecutable(), dirs)
                    .Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return dirs.Result + "\n" + files.Result;
        }

        private void SendHTTPAnswer(string body)
        {
            _socket.Send(
                Encoding.UTF8.GetBytes(
                    "HTTP/1.1 200 OK\n" + "Content-Type: text/html; charset=UTF-8\n\n" + body));
        }

        private void Send404HTTP()
        {
            _socket.Send(Encoding.UTF8.GetBytes("HTTP/1.1 404 Error\n" +
                                                "Content-Type: text/html; charset=UTF-8\n\n not exist"));
        }
    }
}