using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using Microsoft.Build.Framework;

namespace GitMSBuildTasks
{
    public class GitCommitInfo : BaseGitTask
    {
        [Output]
        public string LastCommitHash { get; set; }

        [Output]
        public string LastCommitShortHash { get; set; }

        [Output]
        public string LastCommitMessage { get; set; }

        public override bool Execute()
        {
            try
            {
                using (var repo = new Repository(LocalPath))
                {
                    Commit recent = GetMostRecentCommit(repo);
                    repo.Commits.
                    //var tag = repo.Tags.OrderByDescending(x => x.Name, new AlphanumComparatorFast()).First();
                    //var refs = repo.Refs.ToList();
                    //var lastTagShortHash = tag.Annotation.TargetId.Sha.Substring(0, 7);
                    //var lastCommitShortHash = headCommit.Sha.Substring(0, 7);
                    //var lastCommitMessage = headCommit.Message;
                }
            }
            catch (Exception exception)
            {
                
                throw;
            }

            return true;
        }

        public Commit GetMostRecentCommit(Repository repo)
        {
            return repo.Commits.First();
        }

        public Tag GetMostRecentTagByEnumeration(Repository repo)
        {
            var tagList = repo.Tags;

            foreach (var commit in repo.Commits)
            {
                // Commits are 
            }

            return null;
        }
    }
}
