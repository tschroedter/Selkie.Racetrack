using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;

namespace Selkie.Racetrack.Tests.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class PathShortestFinderTests
    {
        [SetUp]
        public void Setup()
        {
            m_Shortest = Substitute.For <IPath>();
            m_Shortest.Distance.Returns(new Distance(10.0));

            m_Medium = Substitute.For <IPath>();
            m_Medium.Distance.Returns(new Distance(20.0));

            m_Longest = Substitute.For <IPath>();
            m_Longest.Distance.Returns(new Distance(30.0));
        }

        private IPath m_Longest;
        private IPath m_Medium;
        private IPath m_Shortest;

        [Test]
        public void PathsDefaultTest()
        {
            var finder = new PathShortestFinder();

            Assert.AreEqual(0,
                            finder.Paths.Count());
        }

        [Test]
        public void PathsRoundtripTest()
        {
            var paths = new List <IPath>
                        {
                            m_Shortest,
                            m_Medium,
                            m_Longest
                        };

            var finder = new PathShortestFinder
                         {
                             Paths = paths
                         };

            Assert.AreEqual(paths,
                            finder.Paths);
        }

        [Test]
        public void ShortestPathDefaultTest()
        {
            var finder = new PathShortestFinder();

            Assert.True(finder.ShortestPath.IsUnknown);
        }

        [Test]
        public void ShortestPathForEmptyListTest()
        {
            var paths = new List <IPath>();

            var finder = new PathShortestFinder
                         {
                             Paths = paths
                         };
            finder.Find();

            Assert.True(finder.ShortestPath.IsUnknown);
        }

        [Test]
        public void ShortestPathTest()
        {
            var paths = new List <IPath>
                        {
                            m_Shortest,
                            m_Medium,
                            m_Longest
                        };

            var finder = new PathShortestFinder
                         {
                             Paths = paths
                         };
            finder.Find();

            Assert.AreEqual(m_Shortest,
                            finder.ShortestPath,
                            "ShortestPath");
            Assert.False(finder.ShortestPath.IsUnknown,
                         "IsUnknown");
        }
    }
}