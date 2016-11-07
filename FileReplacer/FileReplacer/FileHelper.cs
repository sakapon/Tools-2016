using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileReplacer
{
    public static class FileHelper
    {
        public static void ReplaceContent(FileInfo file, string oldValue, string newValue)
        {
            var encoding = Encoding.UTF8;

            var oldContent = File.ReadAllText(file.FullName, encoding);
            if (!oldContent.Contains(oldValue)) return;

            var newContent = oldContent.Replace(oldValue, newValue);
            File.WriteAllText(file.FullName, newContent, encoding);
            Console.WriteLine($"Replaced content: {file.FullName}");
        }
    }
}
