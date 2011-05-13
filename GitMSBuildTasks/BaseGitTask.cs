using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace GitMSBuildTasks 
{
    public abstract class BaseGitTask : Task
    {
        public string LocalPath { get; set; }
        public Repository CurrentRepository { get; protected set; }

        public BaseGitTask()
        {
            LocalPath = ".";
        }

        public void OpenRepository()
        {
            if (CurrentRepository == null)
                CurrentRepository = new Repository(LocalPath);
        }

        public void CloseRepository()
        {
            if (CurrentRepository != null)
                CurrentRepository.Dispose();
        }
    }
}
