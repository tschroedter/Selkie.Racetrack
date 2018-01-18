using System.Diagnostics.CodeAnalysis;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Calculators;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using NSubstitute;
using NUnit.Framework;

namespace Core2.Selkie.Racetrack.Tests.Calculators
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class ForwardToReverseCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_FromLine = Substitute.For <ILine>();
            m_ToLine = new Line(0,
                                1,
                                2,
                                3,
                                4);
            m_RadiusForPortTurn = new Distance(123.0);
            m_RadiusForStarboardTurn = new Distance(456.0);

            m_RacetrackCalculator = Substitute.For <ILinePairToRacetrackCalculator>();

            m_Calculator = new ForwardToReverseCalculator(m_RacetrackCalculator);
        }

        private ForwardToReverseCalculator m_Calculator;
        private ILine m_FromLine;
        private ILinePairToRacetrackCalculator m_RacetrackCalculator;
        private ILine m_ToLine;
        private Distance m_RadiusForPortTurn;
        private Distance m_RadiusForStarboardTurn;

        [Test]
        public void GetCalculatorCallsCalculateTest()
        {
            m_Calculator.GetCalculator(m_FromLine,
                                       m_ToLine,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            m_RacetrackCalculator.Received().Calculate();
        }

        [Test]
        public void GetCalculatorReturnsCalculatorTest()
        {
            ILinePairToRacetrackCalculator actual = m_Calculator.GetCalculator(m_FromLine,
                                                                               m_ToLine,
                                                                               m_RadiusForPortTurn,
                                                                               m_RadiusForStarboardTurn);

            Assert.AreEqual(m_RacetrackCalculator,
                            actual);
        }

        [Test]
        public void GetCalculatorSetsFromFeatureTest()
        {
            m_Calculator.GetCalculator(m_FromLine,
                                       m_ToLine,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(m_FromLine,
                            m_RacetrackCalculator.FromLine);
        }

        [Test]
        public void GetCalculatorSetsIsPortTurnAllowedTest()
        {
            m_Calculator.GetCalculator(m_FromLine,
                                       m_ToLine,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.True(m_RacetrackCalculator.IsPortTurnAllowed);
        }

        [Test]
        public void GetCalculatorSetsIsStarboardTurnAllowedTest()
        {
            m_Calculator.GetCalculator(m_FromLine,
                                       m_ToLine,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.True(m_RacetrackCalculator.IsStarboardTurnAllowed);
        }

        [Test]
        public void GetCalculatorSetsRadiusForPortTurnTest()
        {
            m_Calculator.GetCalculator(m_FromLine,
                                       m_ToLine,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(m_RadiusForPortTurn,
                            m_RacetrackCalculator.RadiusForPortTurn);
        }

        [Test]
        public void GetCalculatorSetsRadiusForStarboardTurnTest()
        {
            m_Calculator.GetCalculator(m_FromLine,
                                       m_ToLine,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(m_RadiusForStarboardTurn,
                            m_RacetrackCalculator.RadiusForStarboardTurn);
        }

        [Test]
        public void GetCalculatorSetsToLineForReverseToLineIsNullTest()
        {
            var ToLine = Substitute.For <ILine>();
            ToLine.Reverse().Returns(( ILine ) null);

            m_Calculator.GetCalculator(m_FromLine,
                                       ToLine,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(ToLine,
                            m_RacetrackCalculator.ToLine);
        }

        [Test]
        public void GetCalculatorSetsToLineTest()
        {
            IPolylineSegment toLineReversed = m_ToLine.Reverse();

            m_Calculator.GetCalculator(m_FromLine,
                                       m_ToLine,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(toLineReversed,
                            m_RacetrackCalculator.ToLine);
        }
    }
}