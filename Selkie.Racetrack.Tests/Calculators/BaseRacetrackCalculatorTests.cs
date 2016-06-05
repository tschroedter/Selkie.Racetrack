using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Calculators;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Tests.Calculators
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class BaseRacetrackCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_FeatureOne = Substitute.For <ISurveyFeature>();
            m_FeatureTwo = Substitute.For <ISurveyFeature>();

            m_Racetrack = Substitute.For <IPath>();

            m_FeaturePairToRacetrackCalculator = Substitute.For <IFeaturePairToRacetrackCalculator>();
            m_FeaturePairToRacetrackCalculator.Racetrack.Returns(m_Racetrack);

            m_Calculator = new TestBaseRacetrackCalculator(m_FeaturePairToRacetrackCalculator);
        }

        private class TestBaseRacetrackCalculator : BaseRacetrackCalculator
        {
            public TestBaseRacetrackCalculator([NotNull] IFeaturePairToRacetrackCalculator calculator)
                : base(calculator)
            {
            }

            internal override IFeaturePairToRacetrackCalculator GetCalculator(ISurveyFeature fromFeature,
                                                                              ISurveyFeature toFeature,
                                                                              Distance radiusForPortTurn,
                                                                              Distance radiusForStarboardTurn)
            {
                return Calculator;
            }
        }

        private TestBaseRacetrackCalculator m_Calculator;
        private IFeaturePairToRacetrackCalculator m_FeaturePairToRacetrackCalculator;
        private ISurveyFeature m_FeatureOne;
        private ISurveyFeature m_FeatureTwo;
        private IPath m_Racetrack;

        [Test]
        public void CalculateForLinesEmptyTest()
        {
            ISurveyFeature[] features =
            {
            };

            m_Calculator.ToFeatures = features;
            m_Calculator.FromFeature = m_FeatureOne;
            m_Calculator.TurnRadiusForPort = new Distance(30.0);
            m_Calculator.TurnRadiusForStarboard = new Distance(30.0);

            IPath[] actual = m_Calculator.Paths;

            Assert.AreEqual(0,
                            actual.Length);
        }

        [Test]
        public void CalculateForOneLineOnlyCountTest()
        {
            ISurveyFeature[] lines =
            {
                m_FeatureOne
            };

            m_Calculator.ToFeatures = lines;
            m_Calculator.FromFeature = m_FeatureOne;
            m_Calculator.TurnRadiusForPort = new Distance(30.0);
            m_Calculator.TurnRadiusForStarboard = new Distance(30.0);

            m_Calculator.Calculate();

            IPath[] actual = m_Calculator.Paths;

            Assert.AreEqual(1,
                            actual.Length);
        }

        [Test]
        public void CalculateForOneLineOnlyUnknownTest()
        {
            ISurveyFeature[] lines =
            {
                m_FeatureOne
            };

            m_Calculator.ToFeatures = lines;
            m_Calculator.FromFeature = m_FeatureOne;
            m_Calculator.TurnRadiusForPort = new Distance(30.0);
            m_Calculator.TurnRadiusForStarboard = new Distance(30.0);

            m_Calculator.Calculate();

            IPath actual = m_Calculator.Paths.First();

            Assert.True(actual.IsUnknown);
        }

        [Test]
        public void CalculateForTwoLinesCountTest()
        {
            ISurveyFeature[] lines =
            {
                m_FeatureOne,
                m_FeatureTwo
            };

            m_Calculator.ToFeatures = lines;
            m_Calculator.FromFeature = m_FeatureOne;
            m_Calculator.TurnRadiusForPort = new Distance(30.0);
            m_Calculator.TurnRadiusForStarboard = new Distance(30.0);

            m_Calculator.Calculate();

            IPath[] actual = m_Calculator.Paths;

            Assert.AreEqual(2,
                            actual.Length);
        }

        [Test]
        public void CalculateForTwoLinesPathOneTest()
        {
            ISurveyFeature[] lines =
            {
                m_FeatureOne,
                m_FeatureTwo
            };

            m_Calculator.ToFeatures = lines;
            m_Calculator.FromFeature = m_FeatureOne;
            m_Calculator.TurnRadiusForPort = new Distance(30.0);
            m_Calculator.TurnRadiusForStarboard = new Distance(30.0);

            m_Calculator.Calculate();

            IPath actual = m_Calculator.Paths.First();

            Assert.True(actual.IsUnknown);
        }

        [Test]
        public void CalculateForTwoLinesPathTwoTest()
        {
            ISurveyFeature[] lines =
            {
                m_FeatureOne,
                m_FeatureTwo
            };

            m_Calculator.ToFeatures = lines;
            m_Calculator.FromFeature = m_FeatureOne;
            m_Calculator.TurnRadiusForPort = new Distance(30.0);
            m_Calculator.TurnRadiusForStarboard = new Distance(30.0);

            m_Calculator.Calculate();

            IPath actual = m_Calculator.Paths.Last();

            Assert.AreEqual(m_Racetrack,
                            actual);
        }

        [Test]
        public void CalculatorDefaultTest()
        {
            Assert.AreEqual(m_FeaturePairToRacetrackCalculator,
                            m_Calculator.Calculator);
        }

        [Test]
        public void FromFeatureDefaultTest()
        {
            Assert.True(m_Calculator.FromFeature.IsUnknown);
        }

        [Test]
        public void FromFeatureRoundtripTest()
        {
            var feature = Substitute.For <ISurveyFeature>();

            m_Calculator.FromFeature = feature;

            Assert.AreEqual(feature,
                            m_Calculator.FromFeature);
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
        public void ToFeaturesDefaultTest()
        {
            Assert.AreEqual(0,
                            m_Calculator.ToFeatures.Length);
        }

        [Test]
        public void ToFeaturesRoundtripTest()
        {
            var feature = Substitute.For <ISurveyFeature>();
            ISurveyFeature[] features =
            {
                feature
            };

            m_Calculator.ToFeatures = features;

            Assert.AreEqual(features,
                            m_Calculator.ToFeatures);
        }

        [Test]
        public void TurnRadiusForPortDefaultTest()
        {
            Assert.True(m_Calculator.TurnRadiusForPort.IsUnknown);
        }

        [Test]
        public void TurnRadiusForPortRoundtripTest()
        {
            var radius = new Distance(123.0);

            m_Calculator.TurnRadiusForPort = radius;

            Assert.AreEqual(radius,
                            m_Calculator.TurnRadiusForPort);
        }

        [Test]
        public void TurnRadiusForStarboardDefaultTest()
        {
            Assert.True(m_Calculator.TurnRadiusForStarboard.IsUnknown);
        }

        [Test]
        public void TurnRadiusForStarboardRoundtripTest()
        {
            var radius = new Distance(123.0);

            m_Calculator.TurnRadiusForStarboard = radius;

            Assert.AreEqual(radius,
                            m_Calculator.TurnRadiusForStarboard);
        }
    }
}