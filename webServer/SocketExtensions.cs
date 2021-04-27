using System.Net.Sockets;
using System.Text;

namespace webServer
{
    public static class SocketExtensions
    {
        public static string GetAllUtf8Text(this Socket socket)
        {
            var buff = new byte[4096];
            var textBuff = new StringBuilder();
            var count = 4096;
            while (count == 4096)
            {
                count = socket.Receive(buff);
                var text = Encoding.UTF8.GetString(buff);
                textBuff.Append(text);
            }

            return textBuff.ToString();
        }
    }
}