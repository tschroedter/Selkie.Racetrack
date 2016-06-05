using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Surveying;
using Selkie.NUnit.Extensions;
using Selkie.Racetrack.Calculators;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Tests.Calculators
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

            m_Features = new ISurveyFeature[0];
            m_TurnRadiusForPort = new Distance(30.0);
            m_TurnRadiusForStarboard = new Distance(30.0);
            m_BaseRacetracksCalculator = Substitute.For <IBaseFeaturesToFeaturesRacetracksCalculator>();
            m_BaseRacetracksCalculator.Paths.Returns(m_Paths);

            m_ForwardToForward = Substitute.For <IFeaturesToFeaturesForwardToForwardRacetrackCalculator>();
            m_ForwardToReverse = Substitute.For <IFeaturesToFeaturesForwardToReverseRacetrackCalculator>();
            m_ReverseToForward = Substitute.For <IFeaturesToFeaturesReverseToForwardRacetrackCalculator>();
            m_ReverseToReverse = Substitute.For <IFeaturesToFeaturesReverseToReverseRacetrackCalculator>();

            m_ForwardToForward.Paths.Returns(m_Paths);
            m_ForwardToReverse.Paths.Returns(m_Paths);
            m_ReverseToForward.Paths.Returns(m_Paths);
            m_ReverseToReverse.Paths.Returns(m_Paths);

            m_Sut = new RacetracksCalculator(m_ForwardToForward,
                                             m_ForwardToReverse,
                                             m_ReverseToForward,
                                             m_ReverseToReverse);
        }

        private IBaseFeaturesToFeaturesRacetracksCalculator m_BaseRacetracksCalculator;
        private RacetracksCalculator m_Sut;
        private IFeaturesToFeaturesForwardToForwardRacetrackCalculator m_ForwardToForward;
        private IFeaturesToFeaturesForwardToReverseRacetrackCalculator m_ForwardToReverse;
        private ISurveyFeature[] m_Features;
        private IPath[][] m_Paths;
        private Distance m_TurnRadiusForPort;
        private IFeaturesToFeaturesReverseToForwardRacetrackCalculator m_ReverseToForward;
        private IFeaturesToFeaturesReverseToReverseRacetrackCalculator m_ReverseToReverse;
        private Distance m_TurnRadiusForStarboard;

        [Test]
        public void CalculateGeneralCallsCalculateTest()
        {
            m_Sut.CalculateGeneral(m_BaseRacetracksCalculator,
                                   m_Features,
                                   m_TurnRadiusForPort,
                                   m_TurnRadiusForStarboard);

            m_BaseRacetracksCalculator.Received().Calculate();
        }

        [Test]
        public void CalculateGeneralReturnsPathsTest()
        {
            IPath[][] actual = m_Sut.CalculateGeneral(m_BaseRacetracksCalculator,
                                                      m_Features,
                                                      m_TurnRadiusForPort,
                                                      m_TurnRadiusForStarboard);

            Assert.AreEqual(m_Paths,
                            actual);
        }

        [Test]
        public void CalculateGeneralSetsIsPortTurnAllowedTest()
        {
            m_Sut.CalculateGeneral(m_BaseRacetracksCalculator,
                                   m_Features,
                                   m_TurnRadiusForPort,
                                   m_TurnRadiusForStarboard);

            Assert.AreEqual(m_Sut.IsPortTurnAllowed,
                            m_BaseRacetracksCalculator.IsPortTurnAllowed);
        }

        [Test]
        public void CalculateGeneralSetsIsStarboardTurnAllowedTest()
        {
            m_Sut.CalculateGeneral(m_BaseRacetracksCalculator,
                                   m_Features,
                                   m_TurnRadiusForPort,
                                   m_TurnRadiusForStarboard);

            Assert.AreEqual(m_Sut.IsStarboardTurnAllowed,
                            m_BaseRacetracksCalculator.IsStarboardTurnAllowed);
        }

        [Test]
        public void CalculateGeneralSetsLinesTest()
        {
            m_Sut.CalculateGeneral(m_BaseRacetracksCalculator,
                                   m_Features,
                                   m_TurnRadiusForPort,
                                   m_TurnRadiusForStarboard);

            Assert.AreEqual(m_Features,
                            m_BaseRacetracksCalculator.Features);
        }

        [Test]
        public void CalculateGeneralSetsTurnRadiusForPortTurnTest()
        {
            m_Sut.CalculateGeneral(m_BaseRacetracksCalculator,
                                   m_Features,
                                   m_TurnRadiusForPort,
                                   m_TurnRadiusForStarboard);

            Assert.AreEqual(m_TurnRadiusForPort,
                            m_BaseRacetracksCalculator.TurnRadiusForPort);
        }

        [Test]
        public void CalculateGeneralSetsTurnRadiusForStarboardTurnTest()
        {
            m_Sut.CalculateGeneral(m_BaseRacetracksCalculator,
                                   m_Features,
                                   m_TurnRadiusForPort,
                                   m_TurnRadiusForStarboard);

            Assert.AreEqual(m_TurnRadiusForPort,
                            m_BaseRacetracksCalculator.TurnRadiusForStarboard);
        }

        [Test]
        public void CalculateSetsForwardToForwardTest()
        {
            m_Sut.Calculate();

            Assert.AreEqual(m_Paths,
                            m_Sut.ForwardToForward);
        }

        [Test]
        public void CalculateSetsForwardToReverseTest()
        {
            m_Sut.Calculate();

            Assert.AreEqual(m_Paths,
                            m_Sut.ForwardToReverse);
        }

        [Test]
        public void CalculateSetsReverseToForwardTest()
        {
            m_Sut.Calculate();

            Assert.AreEqual(m_Paths,
                            m_Sut.ReverseToForward);
        }

        [Test]
        public void CalculateSetsReverseToReverseTest()
        {
            m_Sut.Calculate();

            Assert.AreEqual(m_Paths,
                            m_Sut.ReverseToReverse);
        }

        [Test]
        public void FeaturesRoundtripTest()
        {
            var features = new ISurveyFeature[0];

            m_Sut.Features = features;

            Assert.AreEqual(features,
                            m_Sut.Features);
        }

        [Test]
        public void ForwardToForwardDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Sut.ForwardToForward.Length);
        }

        [Test]
        public void ForwardToReverseDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Sut.ForwardToReverse.Length);
        }

        [Test]
        public void IsPortTurnAllowedDefaultTest()
        {
            Assert.True(m_Sut.IsPortTurnAllowed);
        }

        [Test]
        public void IsPortTurnAllowedRoundtripTest()
        {
            m_Sut.IsPortTurnAllowed = false;

            Assert.False(m_Sut.IsPortTurnAllowed);
        }

        [Test]
        public void IsStarboardTurnAllowedDefaultTest()
        {
            Assert.True(m_Sut.IsStarboardTurnAllowed);
        }

        [Test]
        public void IsStarboardTurnAllowedRoundtripTest()
        {
            m_Sut.IsStarboardTurnAllowed = false;

            Assert.False(m_Sut.IsStarboardTurnAllowed);
        }

        [Test]
        public void IsUnknownTest()
        {
            Assert.False(m_Sut.IsUnknown);
        }

        [Test]
        public void LinesDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Sut.Features.Count());
        }

        [Test]
        public void ReverseToForwardDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Sut.ReverseToForward.Length);
        }

        [Test]
        public void ReverseToReverseDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Sut.ReverseToReverse.Length);
        }

        [Test]
        public void TurnRadiusForPortTurnDefaultTest()
        {
            NUnitHelper.AssertIsEquivalent(RacetracksCalculator.DefaultTurnRadius.Length,
                                           m_Sut.TurnRadiusForPort.Length);
        }

        [Test]
        public void TurnRadiusForPortTurnRoundtripTest()
        {
            var radius = new Distance(123.0);

            m_Sut.TurnRadiusForPort = radius;

            Assert.AreEqual(radius,
                            m_Sut.TurnRadiusForPort);
        }

        [Test]
        public void TurnRadiusForStarboardTurnDefaultTest()
        {
            NUnitHelper.AssertIsEquivalent(RacetracksCalculator.DefaultTurnRadius.Length,
                                           m_Sut.TurnRadiusForStarboard.Length);
        }

        [Test]
        public void TurnRadiusForStarboardTurnRoundtripTest()
        {
            var radius = new Distance(123.0);

            m_Sut.TurnRadiusForStarboard = radius;

            Assert.AreEqual(radius,
                            m_Sut.TurnRadiusForStarboard);
        }
    }
}