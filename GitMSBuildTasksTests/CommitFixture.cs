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
            var tag = _commitInfo.GetMostRecentTagByEnumeration(_commitInfo.CurrentRepository.Commits, _commitInfo.CurrentRepository.Tags);
            string tagName = "0.1.0.0";
            Assert.AreEqual(tagName, tag.Annotation.Name);
            Assert.AreEqual("54ada48803737d8bd3a1f851263dd745a6a83cfd", tag.Annotation.TargetId.Sha);
        }

        [Test]
        public void FindNumberOfCommitsSinceLastTag()
        {
            var commit = _commitInfo.CurrentRepository.Lookup<Commit>("4c52af060209691691eae90463a8428b38d2cf43");
            var tagListFromCommit = _commitInfo.CurrentRepository.Commits.StartingAt("4c52af060209691691eae90463a8428b38d2cf43");
            var tag = _commitInfo.GetMostRecentTagByEnumeration(tagListFromCommit, _commitInfo.CurrentRepository.Tags);
            var revisionCount = _commitInfo.GetRevisionCountToClosestTag(commit);

            Assert.AreEqual(1, revisionCount);
        }

        [Test]
        public void GetShortHashOfCommit()
        {
            var commit = _commitInfo.CurrentRepository.Lookup<Commit>("4c52af060209691691eae90463a8428b38d2cf43");

            // Short hash is the first 7 characters of Sha hash
            var shortHash = commit.Sha.Substring(0, 7);

            Assert.AreEqual(shortHash, _commitInfo.GetCommitShortHash(commit));
        }
    }
}
