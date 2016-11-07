using System;
using System.IO;
using FileReplacer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class DirectoryHelperTest
    {
        [TestMethod]
        public void ReplaceDirectoryName()
        {
            if (Directory.Exists("Root"))
                Directory.Delete("Root", true);
            Directory.CreateDirectory(@"Root\OldDir");

            var dir = new DirectoryInfo(@"Root\OldDir");
            DirectoryHelper.ReplaceDirectoryName(dir, "Old", "New");

            Assert.AreEqual(false, Directory.Exists(@"Root\OldDir"));
            Assert.AreEqual(true, Directory.Exists(@"Root\NewDir"));
        }

        [TestMethod]
        public void ReplaceFileName()
        {
            if (Directory.Exists("Root"))
                Directory.Delete("Root", true);
            Directory.CreateDirectory("Root");
            File.WriteAllText(@"Root\OldFile.txt", "Text");

            var file = new FileInfo(@"Root\OldFile.txt");
            DirectoryHelper.ReplaceFileName(file, "Old", "New");

            Assert.AreEqual(false, File.Exists(@"Root\OldFile.txt"));
            Assert.AreEqual(true, File.Exists(@"Root\NewFile.txt"));
        }
    }
}
