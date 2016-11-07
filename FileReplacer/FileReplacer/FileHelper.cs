using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileReplacer
{
    public static class FileHelper
    {
        public static readonly Encoding UTF8N = new UTF8Encoding();

        public static void ReplaceContent(FileInfo file, string oldValue, string newValue)
        {
            string oldContent;
            Encoding encoding;

            using (var reader = new StreamReader(file.FullName, UTF8N, true))
            {
                oldContent = reader.ReadToEnd();
                encoding = reader.CurrentEncoding;
            }

            if (!oldContent.Contains(oldValue)) return;

            var newContent = oldContent.Replace(oldValue, newValue);
            File.WriteAllText(file.FullName, newContent, encoding);
            Console.WriteLine($"Replaced content: {file.FullName}");
        }
    }
}
