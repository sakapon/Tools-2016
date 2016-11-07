using System;
using System.IO;
using System.Text;
using FileReplacer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class FileHelperTest
    {
        [TestMethod]
        public void ReplaceContent_UTF8()
        {
            ReplaceContent("ReplaceContent_UTF8.txt", Encoding.UTF8);
        }

        [TestMethod]
        public void ReplaceContent_UTF8N()
        {
            ReplaceContent("ReplaceContent_UTF8N.txt", FileHelper.UTF8N);
        }

        static void ReplaceContent(string filePath, Encoding encoding)
        {
            File.WriteAllText(filePath, "A Old Name.\r\n123\r\n", encoding);

            var file = new FileInfo(filePath);
            FileHelper.ReplaceContent(file, "Old Name", "New Name");

            var actual = File.ReadAllBytes(filePath);
            var expected = ToBytes("A New Name.\r\n123\r\n", encoding);
            CollectionAssert.AreEqual(expected, actual);
        }

        static byte[] ToBytes(string content, Encoding encoding)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, encoding))
            {
                writer.Write(content);
                writer.Flush();
                return stream.ToArray();
            }
        }
    }
}
