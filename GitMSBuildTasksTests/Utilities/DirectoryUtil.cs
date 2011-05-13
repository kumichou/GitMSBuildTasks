using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GitMSBuildTasksTests.Utilities
{
    public static class DirectoryUtil
    {
        public static string AssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        public static string GetGitRepositoryDirectory()
        {
            DirectoryInfo currDirectory = new DirectoryInfo(AssemblyDirectory());

            while (!currDirectory.FullName.Contains(".git"))
            {
                currDirectory = FindGitRepoDirectory(currDirectory);
            }

            return currDirectory.FullName;
        }

        private static DirectoryInfo FindGitRepoDirectory(DirectoryInfo currDirectory)
        {
            List<DirectoryInfo> directoryInfos = currDirectory.Parent.GetDirectories().ToList();

            DirectoryInfo gitRepo = directoryInfos.Where(x => x.Name.ToLower() == ".git").FirstOrDefault();

            if (gitRepo == null)
            {
                // Not found, look up another directory
                return FindGitRepoDirectory(currDirectory.Parent);
            }
            else
            {
                return gitRepo;
            }
        }
    }
}
