using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileReplacer
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine($"Usage: {nameof(FileReplacer)} dirPath oldValue newValue");
                return 101;
            }

            var dirPath = args[0];
            var oldValue = args[1];
            var newValue = args[2];

            if (!Directory.Exists(dirPath))
            {
                Console.WriteLine($"Usage: {nameof(FileReplacer)} dirPath oldValue newValue");
                return 102;
            }

            try
            {
                var rootDir = new DirectoryInfo(dirPath);

                DirectoryHelper.ReplaceDirectoryNames(rootDir, oldValue, newValue);
                DirectoryHelper.ReplaceFileNames(rootDir, oldValue, newValue);
                DirectoryHelper.ReplaceFileContents(rootDir, oldValue, newValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 100;
            }

            return 0;
        }
    }
}
