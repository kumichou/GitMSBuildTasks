using System;
using System.Collections.Generic;
using GitMSBuildTasks;
using NUnit.Framework;

namespace GitMSBuildTasksTests
{
    [TestFixture]
    public class AlphaNumberComparerFixture
    {
        [Test]
        public void ComparerShouldPutMixedAlphaNumInLogicalOrder()
        {
            List<string> versionTags = new List<string>();

            versionTags.Add("build-1.0.3.4");
            versionTags.Add("build-0.1.0.0");
            versionTags.Add("build-1.0.0.0");

            versionTags.Sort(new AlphanumComparator());

            Assert.AreEqual("build-1.0.0.0", versionTags[1]);
            Assert.AreEqual("build-1.0.3.4", versionTags[2]);
        }
    }
}
