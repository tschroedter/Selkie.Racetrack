using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Surveying;
using Core2.Selkie.Racetrack.Calculators.Surveying;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators.Surveying;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;

namespace Core2.Selkie.Racetrack.Tests.Calculators
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class BaseFeaturesToFeaturesRacetracksCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_FromFeature = Substitute.For <ISurveyFeature>();
            m_ToFeature = Substitute.For <ISurveyFeature>();
            m_ToFeatures = new[]
                           {
                               m_FromFeature,
                               m_ToFeature
                           };
            m_Radius = new Distance(100.0);
            m_PathOne = Substitute.For <IPath>();
            m_PathTwo = Substitute.For <IPath>();
            m_PathsOneTwo = new[]
                            {
                                m_PathOne,
                                m_PathTwo
                            };

            m_RacetrackCalculator = Substitute.For <IBaseRacetrackFeatureCalculator>();
            m_RacetrackCalculator.Paths.Returns(m_PathsOneTwo);

            m_Calculator = new TestBaseFeaturesToFeaturesRacetracksCalculator(m_RacetrackCalculator);
        }

        private BaseFeaturesToFeaturesRacetracksCalculator m_Calculator;
        private ISurveyFeature m_FromFeature;
        private IPath m_PathOne;
        private IPath[] m_PathsOneTwo;
        private IPath m_PathTwo;
        private IBaseRacetrackFeatureCalculator m_RacetrackCalculator;
        private Distance m_Radius;
        private ISurveyFeature m_ToFeature;
        private ISurveyFeature[] m_ToFeatures;

        private class TestBaseFeaturesToFeaturesRacetracksCalculator : BaseFeaturesToFeaturesRacetracksCalculator
        {
            public TestBaseFeaturesToFeaturesRacetracksCalculator([NotNull] IBaseRacetrackFeatureCalculator calculator)
                : base(calculator)
            {
            }

            protected override ISurveyFeature GetFromFeature(ISurveyFeature toFeature)
            {
                return toFeature;
            }
        }

        [Test]
        public void CalculateSetsPathsCountTest()
        {
            m_Calculator.Features = m_ToFeatures;

            m_Calculator.Calculate();

            IPath[][] actual = m_Calculator.Paths;

            Assert.AreEqual(2,
                            actual.Length);
        }

        [Test]
        public void CalculateSetsPathsForFirstTest()
        {
            m_Calculator.Features = m_ToFeatures;

            m_Calculator.Calculate();

            IPath[] actual = m_Calculator.Paths [ 0 ];

            Assert.AreEqual(2,
                            actual.Length);
        }

        [Test]
        public void CalculateSetsPathsForFirstValueTest()
        {
            m_Calculator.Features = m_ToFeatures;

            m_Calculator.Calculate();

            IPath[] actual = m_Calculator.Paths [ 0 ];

            Assert.AreEqual(m_PathOne,
                            actual [ 0 ],
                            "[0]");
            Assert.AreEqual(m_PathTwo,
                            actual [ 1 ],
                            "[1]");
        }

        [Test]
        public void CalculateSetsPathsForSecondTest()
        {
            m_Calculator.Features = m_ToFeatures;

            m_Calculator.Calculate();

            IPath[] actual = m_Calculator.Paths [ 1 ];

            Assert.AreEqual(2,
                            actual.Length);
        }

        [Test]
        public void CalculateSetsPathsForSecondValueTest()
        {
            m_Calculator.Features = m_ToFeatures;

            m_Calculator.Calculate();

            IPath[] actual = m_Calculator.Paths [ 1 ];

            Assert.AreEqual(m_PathOne,
                            actual [ 0 ],
                            "[0]");
            Assert.AreEqual(m_PathTwo,
                            actual [ 1 ],
                            "[1]");
        }

        [Test]
        public void CallCalculatorCallsCalculateTest()
        {
            m_Calculator.CallCalculator(m_FromFeature,
                                        m_ToFeatures);

            m_RacetrackCalculator.Received().Calculate();
        }

        [Test]
        public void CallCalculatorReturnsPathsTest()
        {
            IPath[] actual = m_Calculator.CallCalculator(m_FromFeature,
                                                         m_ToFeatures);

            Assert.AreEqual(m_PathsOneTwo,
                            actual);
        }

        [Test]
        public void CallCalculatorSetsFromFeatureTest()
        {
            m_Calculator.CallCalculator(m_FromFeature,
                                        m_ToFeatures);

            Assert.AreEqual(m_FromFeature,
                            m_RacetrackCalculator.FromFeature);
        }

        [Test]
        public void CallCalculatorSetsIsPortTurnAllowedTest()
        {
            m_Calculator.IsPortTurnAllowed = true;

            m_Calculator.CallCalculator(m_FromFeature,
                                        m_ToFeatures);

            Assert.True(m_RacetrackCalculator.IsPortTurnAllowed);
        }

        [Test]
        public void CallCalculatorSetsIsStarboardTurnAllowedTest()
        {
            m_Calculator.IsStarboardTurnAllowed = true;

            m_Calculator.CallCalculator(m_FromFeature,
                                        m_ToFeatures);

            Assert.True(m_RacetrackCalculator.IsStarboardTurnAllowed);
        }

        [Test]
        public void CallCalculatorSetsToFeaturesTest()
        {
            m_Calculator.CallCalculator(m_FromFeature,
                                        m_ToFeatures);

            Assert.AreEqual(m_ToFeatures,
                            m_RacetrackCalculator.ToFeatures);
        }

        [Test]
        public void CallCalculatorSetsTurnRadiusForPortTest()
        {
            m_Calculator.TurnRadiusForPort = m_Radius;

            m_Calculator.CallCalculator(m_FromFeature,
                                        m_ToFeatures);

            Assert.AreEqual(m_Radius,
                            m_RacetrackCalculator.TurnRadiusForPort);
        }

        [Test]
        public void CallCalculatorSetsTurnRadiusForStarboardTest()
        {
            m_Calculator.TurnRadiusForStarboard = m_Radius;

            m_Calculator.CallCalculator(m_FromFeature,
                                        m_ToFeatures);

            Assert.AreEqual(m_Radius,
                            m_RacetrackCalculator.TurnRadiusForStarboard);
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
        public void LinesDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Calculator.Features.Count());
        }

        [Test]
        public void LinesRoundtripTest()
        {
            var feature = Substitute.For <ISurveyFeature>();
            ISurveyFeature[] features =
            {
                feature
            };

            m_Calculator.Features = features;

            Assert.AreEqual(features,
                            m_Calculator.Features);
        }

        [Test]
        public void PathsDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Calculator.Paths.GetLength(0));
        }

        [Test]
        public void RadiusRoundtripTest()
        {
            var radius = new Distance(123.0);

            m_Calculator.TurnRadiusForStarboard = radius;

            Assert.AreEqual(radius,
                            m_Calculator.TurnRadiusForStarboard);
        }

        [Test]
        public void TurnRadiusForPortTurnInMetresDefaultTest()
        {
            Assert.True(m_Calculator.TurnRadiusForPort.IsUnknown);
        }

        [Test]
        public void TurnRadiusForPortTurnInMetresRoundtripTest()
        {
            var radius = new Distance(123.0);

            m_Calculator.TurnRadiusForPort = radius;

            Assert.AreEqual(radius,
                            m_Calculator.TurnRadiusForPort);
        }

        [Test]
        public void TurnRadiusForStarboardTurnInMetresDefaultTest()
        {
            Assert.True(m_Calculator.TurnRadiusForStarboard.IsUnknown);
        }
    }
}