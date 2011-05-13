using System;
using System.Collections.Generic;
using System.Linq;
using GitMSBuildTasks;
using GitMSBuildTasksTests.Utilities;
using LibGit2Sharp;
using NUnit.Framework;

namespace GitMSBuildTasksTests
{
    [TestFixture]
    public class CommitFixture
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
        public void CommitTaskCanFindMostRecentCommitInCurrentBranch()
        {
            string guid = _commitInfo.GetMostRecentCommit(_commitInfo.CurrentRepository).Sha;
            Assert.AreEqual(_commitInfo.CurrentRepository.Commits.First().Sha, guid);
        }

        [Test]
        public void CommitTaskCanFindMostRecentTagByEnumeration()
        {
            var tag = _commitInfo.GetMostRecentTagByEnumeration(_commitInfo.CurrentRepository);
            string tagName = "0.1.0.0";
            Assert.AreEqual(tagName, tag.Annotation.Name);
        }
    }
}
