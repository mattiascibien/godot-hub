using GodotHub.Online;
using Xunit;

namespace GodotHub.Tests.Online
{
    public class OnlineGodotVersionTests
    {
        [Fact]
        public void CorrectlyParsesGithubVersionTag()
        {
            var version = new OnlineGodotVersion("2.0.4.1-stable");
            Assert.Equal(2, version.Version.Major);
            Assert.Equal(0, version.Version.Minor);
            Assert.Equal(4, version.Version.Build);
            Assert.Equal(1, version.Version.Revision);
            Assert.Null(version.PostFix);
            Assert.True(version.IsStable);
        }

        [Fact]
        public void CorrectlyParsesDevVersionTag()
        {
            var version = new OnlineGodotVersion("4.0-dev.20210727");
            Assert.Equal(4, version.Version.Major);
            Assert.Equal(0, version.Version.Minor);
            Assert.Equal("dev.20210727", version.PostFix);
            Assert.False(version.IsStable);
        }

        [Fact]
        public void CorrectlyParsesRcVersionTag()
        {
            var version = new OnlineGodotVersion("3.3-rc6.2");
            Assert.Equal(3, version.Version.Major);
            Assert.Equal(3, version.Version.Minor);
            Assert.Equal("rc6.2", version.PostFix);
            Assert.False(version.IsStable);
        }

        [Fact]
        public void CorrectlyParsesStableVersionTagWithoutPosfix()
        {
            var version = new OnlineGodotVersion("4.0");
            Assert.Equal(4, version.Version.Major);
            Assert.Equal(0, version.Version.Minor);
            Assert.Null(version.PostFix);
            Assert.True(version.IsStable);
        }
    }
}