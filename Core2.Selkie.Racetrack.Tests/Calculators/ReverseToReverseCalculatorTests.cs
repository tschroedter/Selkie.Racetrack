using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Racetrack.Calculators;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Geometry.Shapes;

namespace Core2.Selkie.Racetrack.Tests.Calculators
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class ReverseToReverseCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_ToFeaturReversed = Substitute.For <ILine>();
            m_FromFeature = Substitute.For <ILine>();
            m_ToFeature = Substitute.For <ILine>();
            m_ToFeature.Reverse().Returns(m_ToFeaturReversed);
            m_RadiusForPortTurn = new Distance(123.0);
            m_RadiusForStarboardTurn = new Distance(456.0);

            m_RacetrackCalculator = Substitute.For <ILinePairToRacetrackCalculator>();

            m_Calculator = new ReverseToReverseCalculator(m_RacetrackCalculator);
        }

        private ReverseToReverseCalculator m_Calculator;
        private ILine m_FromFeature;
        private ILinePairToRacetrackCalculator m_RacetrackCalculator;
        private ILine m_ToFeature;
        private ILine m_ToFeaturReversed;
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
            ILinePairToRacetrackCalculator actual = m_Calculator.GetCalculator(m_FromFeature,
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
                            m_RacetrackCalculator.FromLine);
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
        public void GetCalculatorSetsToFeaturesForReverseToFeaturesIsNullTest()
        {
            var toFeature = Substitute.For <ILine>();
            toFeature.Reverse().Returns(( ILine ) null);

            m_Calculator.GetCalculator(m_FromFeature,
                                       toFeature,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(toFeature,
                            m_RacetrackCalculator.ToLine);
        }

        [Test]
        public void GetCalculatorSetsToLineTest()
        {
            m_Calculator.GetCalculator(m_FromFeature,
                                       m_ToFeature,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(m_ToFeaturReversed,
                            m_RacetrackCalculator.ToLine);
        }
    }
}