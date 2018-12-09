using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DummyTestsCore
{
    [TestClass]
    public class MyCloudCommands
    {
        [TestMethod]
        public void Rename()
        {
            string path = @"\\WDEX4100\Public\Snimki\Planini\Rila\Rila 2018.11.17";

            var dir = new DirectoryInfo(path);
            var files = dir.GetFiles("*103_*");
            foreach (var file in files)
            {
                File.Move(file.FullName, file.FullName.Replace("103_", "100_"));
            }
        }

        [TestMethod]
        public void FingMissingPhoto()
        {
            var dir1 = new DirectoryInfo(@"D:\Output\E4 Google Photo");
            var files1 = dir1.GetFiles("*.jpg").Select(f => f.Name);

            var dir2 = new DirectoryInfo(@"D:\Junk\E4 2017");
            var files2 = dir2.GetFiles("*.jpg").Select(f => f.Name);

            var missingPhoto = files1.Except(files2);
        }

        [TestMethod]
        public void DeleteEmptyDirectories()
        {
            var path = @"C:\Users\Hades\AppData\Roaming\IDM\DwnlData\Hades";
            var root = new DirectoryInfo(path);

            foreach (var dir in root.GetDirectories())
            {
                if (dir.GetFiles().Length == 0)
                {
                    Directory.Delete(dir.FullName);
                }
                else
                {
                    var file = dir.GetFiles().Where(f => f.Extension != ".log").FirstOrDefault();
                    if (file != null && file.Length < 10 * 1024)
                    {
                        Directory.Delete(dir.FullName, true);
                    }
                }
            }
        }

        [TestMethod]
        public void CheckFiles()
        {
            var path = @"\\WDEX4100\Personal\Downloads1";
            var root = new DirectoryInfo(path);

            var files = root.GetDirectories()
                .Select(d => new Tuple<string, int, long>(d.Name, d.GetFiles().Count(), d.GetFiles().Sum(f => f.Length) / (1024 * 1024)))
                .OrderByDescending(t => t.Item3)
                .ToList();
        }

        [TestMethod]
        public void SeparateFiles()
        {
            var path = @"D:\Downloads";
            var destination = @"\\WDEX4100\Personal\Downloads1";

            var extensions = new HashSet<string>() { ".flv", ".mp4" };

            var root = new DirectoryInfo(path);
            var files = root.GetFiles().Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase)).ToList();

            foreach (var file in files)
            {
                string username = GetUserName(file.Name);
                if (!string.IsNullOrWhiteSpace(username))
                {
                    var folderName = Path.Combine(destination, username);
                    var dir = new DirectoryInfo(folderName);

                    int fileSufix = 0;
                    if (dir.Exists)
                    {
                        fileSufix = dir.GetFiles().Count();
                    }
                    else
                    {
                        Directory.CreateDirectory(folderName);
                    }

                    fileSufix++;

                    string newFilePath = Path.Combine(folderName, $"{username}_{fileSufix}{file.Extension}");
                    File.Move(file.FullName, newFilePath);
                }
            }
        }

        private string GetUserName(string fileName)
        {
            string username = string.Empty;

            if (fileName.StartsWith("Камера") && fileName.LastIndexOf("Cam4") > 0)
            {
                var parts = fileName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                username = parts[9];
            }
            else if (fileName.Count(c => c == '-') == 6)
            {
                var parts = fileName.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                username = parts[0];
            }
            else if (fileName.Count(c => c == '\'') == 1 && fileName.LastIndexOf("Cam4") > 0)
            {
                var parts = fileName.Split(new char[] { '\'' }, StringSplitOptions.RemoveEmptyEntries);
                username = parts[0];
            }

            return username;
        }
    }
}
