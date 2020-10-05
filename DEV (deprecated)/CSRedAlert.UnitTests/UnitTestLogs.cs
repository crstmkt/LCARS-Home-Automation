using System;
using CSRedAlert;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CSRedAlert.UnitTests
{
    [TestClass]
    public class UnitTestLogs
    {
        [TestMethod]
        public void CreateLogentryWOParam()
        {
            var logentry = new Core.Classes.Log();

            Assert.AreEqual("No Logentry was given", logentry.Entry);
        }

        [TestMethod]
        public void CreateLogentryWParam()
        {
            var logentry = new Core.Classes.Log("This is a log Entry");

            Assert.AreEqual("This is a log Entry", logentry.Entry);
        }
    }
}
