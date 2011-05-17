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

        [Output]
        public int RevisionCount { get; set; }

        [Output]
        public string LastTagName { get; set; }

        [Output]
        public string LastTagMessage { get; set; }

        [Output]
        public string LastTagCommitHash { get; set; }

        public string CommitHash { get; set; }

        public override bool Execute()
        {
            try
            {
                //if (LocalPath == ".")
                //{
                //    // User didn't define a Path, find the Git path by expecting this Task to be contained in a Git repo
                //    LocalPath =
                //}

                using (var repo = new Repository(LocalPath))
                {
                    LastCommitHash = LastCommitShortHash = LastCommitMessage = LastTagName = LastTagMessage = LastTagCommitHash = String.Empty;
                    RevisionCount = 0;
                    Commit commit = null;
                    CommitCollection commits = repo.Commits;

                    if (!String.IsNullOrEmpty(CommitHash))
                    {
                        // We're looking for a from a particular Commit on back
                        commit = repo.Lookup<Commit>(CommitHash);
                        commits = commits.StartingAt(CommitHash);
                    }
                    else
                    {
                        // We're looking from the most recent Commit
                        commit = repo.Commits.First();
                    }

                    if (null == commit)
                    {
                        Log.LogError("Can't find a Git Commit with that hash.");
                        return false;
                    }

                    LastCommitShortHash = GetCommitShortHash(commit);
                    LastCommitHash = commit.Sha;
                    LastCommitMessage = commit.Message;

                    Tag tag = GetMostRecentTagByEnumeration(commits, repo.Tags);

                    if (null != tag)
                    {
                        LastTagName = tag.Annotation.Name;
                        LastTagMessage = tag.Annotation.Message;
                        LastTagCommitHash = tag.Annotation.TargetId.Sha;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.LogMessage("StackTrace: " + exception.StackTrace);
                Log.LogErrorFromException(exception);
                return false;
            }

            return true;
        }

        public Commit GetMostRecentCommit(Repository repo)
        {
            return repo.Commits.First();
        }

        public Tag GetMostRecentTagByEnumeration(CommitCollection commits, TagCollection tags)
        {
            Tag recentTag = null;

            foreach (var commit in commits)
            {
                // Commits are actually ordered already in reverse chronological order
                var tag = tags.Where(x => x.Annotation.TargetId.Sha == commit.Sha).FirstOrDefault();

                if (tag != null)
                {
                    recentTag = tag;
                    break;
                }
            }

            return recentTag;
        }

        public int GetRevisionCountToClosestTag(Commit commit)
        {
            var commits = CurrentRepository.Commits.StartingAt(commit.Sha);
            var tag = GetMostRecentTagByEnumeration(commits, CurrentRepository.Tags);
            int count = 0;

            foreach (var commitToCheck in commits)
            {
                if (commitToCheck.Sha != tag.Annotation.TargetId.Sha)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        public string GetCommitShortHash(Commit commit)
        {
            return commit.Sha.Substring(0, 7);
        }
    }
}
