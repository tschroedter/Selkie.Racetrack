using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Calculators;
using Core2.Selkie.Racetrack.Interfaces.Calculators;

namespace Core2.Selkie.Racetrack.Tests.Calculators.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class ReverseToForwardCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_ToLineReversed = Substitute.For <ILine>();
            m_FromLine = Substitute.For <ILine>();
            m_ToLine = Substitute.For <ILine>();
            m_ToLine.Reverse().Returns(m_ToLineReversed);
            m_RadiusForPortTurn = new Distance(123.0);
            m_RadiusForStarboardTurn = new Distance(456.0);

            m_RacetrackCalculator = Substitute.For <ILinePairToRacetrackCalculator>();

            m_Calculator = new ReverseToForwardCalculator(m_RacetrackCalculator);
        }

        private ReverseToForwardCalculator m_Calculator;
        private ILine m_FromLine;
        private ILinePairToRacetrackCalculator m_RacetrackCalculator;
        private ILine m_ToLine;
        private ILine m_ToLineReversed;
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
        public void GetCalculatorSetRadiusForStarboardTurnTest()
        {
            m_Calculator.GetCalculator(m_FromLine,
                                       m_ToLine,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(m_RadiusForStarboardTurn,
                            m_RacetrackCalculator.RadiusForStarboardTurn);
        }

        [Test]
        public void GetCalculatorSetsFromLineTest()
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
        public void GetCalculatorSetsToLineForReverseToLineIsNullTest()
        {
            m_Calculator.GetCalculator(m_FromLine,
                                       m_ToLine,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(m_ToLine,
                            m_RacetrackCalculator.ToLine);
        }

        [Test]
        public void GetCalculatorSetsToLineTest()
        {
            m_Calculator.GetCalculator(m_FromLine,
                                       m_ToLine,
                                       m_RadiusForPortTurn,
                                       m_RadiusForStarboardTurn);

            Assert.AreEqual(m_ToLine,
                            m_RacetrackCalculator.ToLine);
        }
    }
}