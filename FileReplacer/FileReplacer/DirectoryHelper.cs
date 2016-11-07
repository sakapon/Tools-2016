using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileReplacer
{
    public static class DirectoryHelper
    {
        public static void ReplaceDirectoryNames(DirectoryInfo rootDir, string oldValue, string newValue)
        {
            var targetDirs = rootDir.EnumerateDirectories($"*{oldValue}*", SearchOption.AllDirectories)
                .Reverse()
                .ToArray();
            foreach (var dir in targetDirs)
            {
                ReplaceDirectoryName(dir, oldValue, newValue);
                Console.WriteLine($"Renamed directory: {dir.FullName}");
            }
        }

        public static void ReplaceFileNames(DirectoryInfo rootDir, string oldValue, string newValue)
        {
            var targetFiles = rootDir.EnumerateFiles($"*{oldValue}*", SearchOption.AllDirectories)
                //.Reverse()
                .ToArray();
            foreach (var file in targetFiles)
            {
                ReplaceFileName(file, oldValue, newValue);
                Console.WriteLine($"Renamed file: {file.FullName}");
            }
        }

        public static void ReplaceFileContents(DirectoryInfo rootDir, string oldValue, string newValue)
        {
            var files = rootDir.GetFiles("*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                FileHelper.ReplaceContent(file, oldValue, newValue);
            }
        }

        internal static void ReplaceDirectoryName(DirectoryInfo dir, string oldValue, string newValue)
        {
            var newPath = Path.Combine(dir.Parent.FullName, dir.Name.Replace(oldValue, newValue));
            dir.MoveTo(newPath);
        }

        internal static void ReplaceFileName(FileInfo file, string oldValue, string newValue)
        {
            var newPath = Path.Combine(file.Directory.FullName, file.Name.Replace(oldValue, newValue));
            file.MoveTo(newPath);
        }
    }
}
