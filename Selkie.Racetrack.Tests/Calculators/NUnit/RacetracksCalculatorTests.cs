using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.NUnit.Extensions;
using Selkie.Racetrack.Calculators;

namespace Selkie.Racetrack.Tests.Calculators.NUnit
{
    // ReSharper disable ClassTooBig
    // ReSharper disable MethodTooLong
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class RacetracksCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            var pathOne = Substitute.For <IPath>();
            var pathTwo = Substitute.For <IPath>();

            m_Paths = new[]
                      {
                          new[]
                          {
                              pathOne,
                              pathTwo
                          },
                          new[]
                          {
                              pathOne,
                              pathTwo
                          }
                      };

            m_Lines = new ILine[0];
            m_Radius = new Distance(100.0);
            m_BaseRacetracksCalculator = Substitute.For <IBaseLinesToLinesRacetracksCalculator>();
            m_BaseRacetracksCalculator.Paths.Returns(m_Paths);

            m_ForwardToForward = Substitute.For <ILinesToLinesForwardToForwardRacetrackCalculator>();
            m_ForwardToReverse = Substitute.For <ILinesToLinesForwardToReverseRacetrackCalculator>();
            m_ReverseToForward = Substitute.For <ILinesToLinesReverseToForwardRacetrackCalculator>();
            m_ReverseToReverse = Substitute.For <ILinesToLinesReverseToReverseRacetrackCalculator>();

            m_ForwardToForward.Paths.Returns(m_Paths);
            m_ForwardToReverse.Paths.Returns(m_Paths);
            m_ReverseToForward.Paths.Returns(m_Paths);
            m_ReverseToReverse.Paths.Returns(m_Paths);

            m_Calculator = new RacetracksCalculator(m_ForwardToForward,
                                                    m_ForwardToReverse,
                                                    m_ReverseToForward,
                                                    m_ReverseToReverse);
        }

        private IBaseLinesToLinesRacetracksCalculator m_BaseRacetracksCalculator;
        private RacetracksCalculator m_Calculator;
        private ILinesToLinesForwardToForwardRacetrackCalculator m_ForwardToForward;
        private ILinesToLinesForwardToReverseRacetrackCalculator m_ForwardToReverse;
        private ILine[] m_Lines;
        private IPath[][] m_Paths;
        private Distance m_Radius;
        private ILinesToLinesReverseToForwardRacetrackCalculator m_ReverseToForward;
        private ILinesToLinesReverseToReverseRacetrackCalculator m_ReverseToReverse;

        [Test]
        public void CalculateGeneralCallsCalculateTest()
        {
            m_Calculator.CalculateGeneral(m_BaseRacetracksCalculator,
                                          m_Lines,
                                          m_Radius);

            m_BaseRacetracksCalculator.Received().Calculate();
        }

        [Test]
        public void CalculateGeneralReturnsPathsTest()
        {
            IPath[][] actual = m_Calculator.CalculateGeneral(m_BaseRacetracksCalculator,
                                                             m_Lines,
                                                             m_Radius);

            Assert.AreEqual(m_Paths,
                            actual);
        }

        [Test]
        public void CalculateGeneralSetsIsPortTurnAllowedTest()
        {
            m_Calculator.CalculateGeneral(m_BaseRacetracksCalculator,
                                          m_Lines,
                                          m_Radius);

            Assert.AreEqual(m_Calculator.IsPortTurnAllowed,
                            m_BaseRacetracksCalculator.IsPortTurnAllowed);
        }

        [Test]
        public void CalculateGeneralSetsIsStarboardTurnAllowedTest()
        {
            m_Calculator.CalculateGeneral(m_BaseRacetracksCalculator,
                                          m_Lines,
                                          m_Radius);

            Assert.AreEqual(m_Calculator.IsStarboardTurnAllowed,
                            m_BaseRacetracksCalculator.IsStarboardTurnAllowed);
        }

        [Test]
        public void CalculateGeneralSetsLinesTest()
        {
            m_Calculator.CalculateGeneral(m_BaseRacetracksCalculator,
                                          m_Lines,
                                          m_Radius);

            Assert.AreEqual(m_Lines,
                            m_BaseRacetracksCalculator.Lines);
        }

        [Test]
        public void CalculateGeneralSetsRadiusTest()
        {
            m_Calculator.CalculateGeneral(m_BaseRacetracksCalculator,
                                          m_Lines,
                                          m_Radius);

            Assert.AreEqual(m_Radius,
                            m_BaseRacetracksCalculator.Radius);
        }

        [Test]
        public void CalculateSetsForwardToForwardTest()
        {
            m_Calculator.Calculate();

            Assert.AreEqual(m_Paths,
                            m_Calculator.ForwardToForward);
        }

        [Test]
        public void CalculateSetsForwardToReverseTest()
        {
            m_Calculator.Calculate();

            Assert.AreEqual(m_Paths,
                            m_Calculator.ForwardToReverse);
        }

        [Test]
        public void CalculateSetsReverseToForwardTest()
        {
            m_Calculator.Calculate();

            Assert.AreEqual(m_Paths,
                            m_Calculator.ReverseToForward);
        }

        [Test]
        public void CalculateSetsReverseToReverseTest()
        {
            m_Calculator.Calculate();

            Assert.AreEqual(m_Paths,
                            m_Calculator.ReverseToReverse);
        }

        [Test]
        public void ForwardToForwardDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Calculator.ForwardToForward.Length);
        }

        [Test]
        public void ForwardToReverseDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Calculator.ForwardToReverse.Length);
        }

        [Test]
        public void IsPortTurnAllowedDefaultTest()
        {
            Assert.True(m_Calculator.IsPortTurnAllowed);
        }

        [Test]
        public void IsPortTurnAllowedRoundtripTest()
        {
            m_Calculator.IsPortTurnAllowed = false;

            Assert.False(m_Calculator.IsPortTurnAllowed);
        }

        [Test]
        public void IsStarboardTurnAllowedDefaultTest()
        {
            Assert.True(m_Calculator.IsStarboardTurnAllowed);
        }

        [Test]
        public void IsStarboardTurnAllowedRoundtripTest()
        {
            m_Calculator.IsStarboardTurnAllowed = false;

            Assert.False(m_Calculator.IsStarboardTurnAllowed);
        }

        [Test]
        public void IsUnknownTest()
        {
            Assert.False(m_Calculator.IsUnknown);
        }

        [Test]
        public void LinesDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Calculator.Lines.Count());
        }

        [Test]
        public void LinesRoundtripTest()
        {
            var lines = new ILine[0];

            m_Calculator.Lines = lines;

            Assert.AreEqual(lines,
                            m_Calculator.Lines);
        }

        [Test]
        public void RadiusDefaultTest()
        {
            NUnitHelper.AssertIsEquivalent(30.0,
                                           m_Calculator.Radius.Length);
        }

        [Test]
        public void RadiusRoundtripTest()
        {
            var radius = new Distance(123.0);

            m_Calculator.Radius = radius;

            Assert.AreEqual(radius,
                            m_Calculator.Radius);
        }

        [Test]
        public void ReverseToForwardDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Calculator.ReverseToForward.Length);
        }

        [Test]
        public void ReverseToReverseDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Calculator.ReverseToReverse.Length);
        }
    }
}