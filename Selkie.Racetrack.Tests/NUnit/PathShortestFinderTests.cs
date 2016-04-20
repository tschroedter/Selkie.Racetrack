using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Racetrack.Interfaces;

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
        public void Find_SetsShortestPath_ForPaths()
        {
            // Arrange
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

            // Act
            finder.Find();

            // Assert
            Assert.AreEqual(m_Shortest,
                            finder.ShortestPath,
                            "ShortestPath");
            Assert.False(finder.ShortestPath.IsUnknown,
                         "IsUnknown");
        }

        [Test]
        public void Find_SetsShortestPathToEmpty_ForEmptyPaths()
        {
            // Arrange
            var paths = new List <IPath>();

            var finder = new PathShortestFinder
                         {
                             Paths = paths
                         };

            // Act
            finder.Find();

            // Assert
            Assert.True(finder.ShortestPath.IsUnknown);
        }

        [Test]
        public void Paths_ReturnsEmpty_ByDefault()
        {
            // Arrange
            // Act
            var finder = new PathShortestFinder();

            // Assert
            Assert.AreEqual(0,
                            finder.Paths.Count());
        }

        [Test]
        public void Paths_Roundtrip()
        {
            // Arrange
            var paths = new List <IPath>
                        {
                            m_Shortest,
                            m_Medium,
                            m_Longest
                        };

            // Act
            var finder = new PathShortestFinder
                         {
                             Paths = paths
                         };

            // Assert
            Assert.AreEqual(paths,
                            finder.Paths);
        }

        [Test]
        public void ShortestPath_ReturnsEmpty_ByDefault()
        {
            // Arrange
            // Act
            var finder = new PathShortestFinder();

            // Assert
            Assert.True(finder.ShortestPath.IsUnknown);
        }
    }
}