using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Calculators;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Tests.Calculators
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class ForwardToReverseCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_ToFeatureReversed = Substitute.For <ISurveyFeature>();
            m_FromFeature = Substitute.For <ISurveyFeature>();
            m_ToFeature = Substitute.For <ISurveyFeature>();
            m_ToFeature.Reverse().Returns(m_ToFeatureReversed);
            m_RadiusForPortTurn = new Distance(123.0);
            m_RadiusForStarboardTurn = new Distance(456.0);

            m_RacetrackCalculator = Substitute.For <IFeaturePairToRacetrackCalculator>();

            m_Calculator = new ForwardToReverseCalculator(m_RacetrackCalculator);
        }

        private ForwardToReverseCalculator m_Calculator;
        private ISurveyFeature m_FromFeature;
        private IFeaturePairToRacetrackCalculator m_RacetrackCalculator;
        private ISurveyFeature m_ToFeature;
        private ISurveyFeature m_ToFeatureReversed;
        private Distance m_RadiusForPortTurn;
        private Distance m_RadiusForStarboardTurn;

        [Test]
        public void GetCalculatorCallsCalculateTest()
        {
            m_Calculator.GetCalculator(m_FromFeature,
                                       m_ToFeature,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            m_RacetrackCalculator.Received().Calculate();
        }

        [Test]
        public void GetCalculatorReturnsCalculatorTest()
        {
            IFeaturePairToRacetrackCalculator actual = m_Calculator.GetCalculator(m_FromFeature,
                                                                                  m_ToFeature,
                                                                                  m_RadiusForPortTurn,
                                                                                  m_RadiusForStarboardTurn);

            Assert.AreEqual(m_RacetrackCalculator,
                            actual);
        }

        [Test]
        public void GetCalculatorSetsFromFeatureTest()
        {
            m_Calculator.GetCalculator(m_FromFeature,
                                       m_ToFeature,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(m_FromFeature,
                            m_RacetrackCalculator.FromFeature);
        }

        [Test]
        public void GetCalculatorSetsIsPortTurnAllowedTest()
        {
            m_Calculator.GetCalculator(m_FromFeature,
                                       m_ToFeature,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.True(m_RacetrackCalculator.IsPortTurnAllowed);
        }

        [Test]
        public void GetCalculatorSetsIsStarboardTurnAllowedTest()
        {
            m_Calculator.GetCalculator(m_FromFeature,
                                       m_ToFeature,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.True(m_RacetrackCalculator.IsStarboardTurnAllowed);
        }

        [Test]
        public void GetCalculatorSetsRadiusForPortTurnTest()
        {
            m_Calculator.GetCalculator(m_FromFeature,
                                       m_ToFeature,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(m_RadiusForPortTurn,
                            m_RacetrackCalculator.RadiusForPortTurn);
        }

        [Test]
        public void GetCalculatorSetsRadiusForStarboardTurnTest()
        {
            m_Calculator.GetCalculator(m_FromFeature,
                                       m_ToFeature,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(m_RadiusForStarboardTurn,
                            m_RacetrackCalculator.RadiusForStarboardTurn);
        }

        [Test]
        public void GetCalculatorSetsToFeatureForReverseToFeatureIsNullTest()
        {
            var toFeature = Substitute.For <ISurveyFeature>();
            toFeature.Reverse().Returns(( ISurveyFeature ) null);

            m_Calculator.GetCalculator(m_FromFeature,
                                       toFeature,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(toFeature,
                            m_RacetrackCalculator.ToFeature);
        }

        [Test]
        public void GetCalculatorSetsToFeatureTest()
        {
            m_Calculator.GetCalculator(m_FromFeature,
                                       m_ToFeature,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(m_ToFeatureReversed,
                            m_RacetrackCalculator.ToFeature);
        }
    }
}