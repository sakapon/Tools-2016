using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) return;

            if (args.Length == 1)
                Split(args[0]);
            else
                Split(args[0], int.Parse(args[1]));
        }

        static void Split(string sourceFilePath, int maxSizeInMegabyte = 1024)
        {
            using (var source = File.OpenRead(sourceFilePath))
            using (var reader = new BinaryReader(source))
            {
                for (var i = 0; ; i++)
                {
                    var targetFilePath = $"{sourceFilePath}.{i:D3}";

                    using (var target = File.Create(targetFilePath))
                    using (var writer = new BinaryWriter(target))
                    {
                        CopyBytes(reader, writer, maxSizeInMegabyte);
                    }

                    if (source.Position >= source.Length) break;
                }
            }
        }

        static void CopyBytes(BinaryReader reader, BinaryWriter writer, int maxSizeInMegabyte)
        {
            var buffer = new byte[1024 * 1024];
            var readSize = 0;

            for (var i = 0; i < maxSizeInMegabyte; i++)
            {
                readSize = reader.Read(buffer, 0, buffer.Length);
                writer.Write(buffer, 0, readSize);

                if (readSize < buffer.Length) break;
            }
        }
    }
}
