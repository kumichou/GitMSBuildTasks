using System;
using System.Collections.Generic;
using GitMSBuildTasks;
using GitMSBuildTasksTests.Utilities;
using NUnit.Framework;

namespace GitMSBuildTasksTests
{
    [TestFixture]
    public class RepositoryFixture
    {
        private GitCommitInfo _commitInfo;
        private string _repoDirectory;

        [SetUp]
        public void Setup()
        {
            _commitInfo = new GitCommitInfo();
            _repoDirectory = DirectoryUtil.GetGitRepositoryDirectory();
            _commitInfo.LocalPath = _repoDirectory;
            _commitInfo.OpenRepository();
        }

        [TearDown]
        public void TearDown()
        {
            _commitInfo.CloseRepository();
        }

        [Test]
        public void CommitTaskCanFindValidRepoGivenADirectory()
        {
            var currentRepoPath = _commitInfo.CurrentRepository.Info.Path;
            // libgit2 for some reason returns Info.Path with a trailing \ attached, that may be a bug in the managed wrapper
            // since when you create a Repository reference you can't have a trailing slash
            currentRepoPath = currentRepoPath.Trim(new char[] { '\\' });

            Assert.AreEqual(_repoDirectory, currentRepoPath);
        }
    }
}
