using System.Collections.Generic;
using System.IO;
using NetBox.Extensions;
using oop;

namespace webServer
{
    class HtmlResulter : IResulter
    {
        private readonly string _path;
        public string Result;

        public HtmlResulter(string path) => _path = path;

        public void WriteResult<T>(T result)
        {
            if (result is IEnumerable<string> enumerable)
                WriteResult(enumerable);
            else
                Result = result.JsonSerialise();
        }

        private void WriteResult(IEnumerable<string> result)
        {
            Result = "<ul>\n";
            foreach (var line in result)
                Result +=
                    $"<li><a href=\"{Path.Combine(_path, File.Exists(line) ? line : new DirectoryInfo(line).Name)}\" >" +
                    line +
                    "</a></li>\n";

            Result += "</ul>\n";
        }
    }
}