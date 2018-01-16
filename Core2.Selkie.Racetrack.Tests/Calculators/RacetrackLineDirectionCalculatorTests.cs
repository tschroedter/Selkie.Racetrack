using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Calculators;

namespace Core2.Selkie.Racetrack.Tests.Calculators
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class RacetrackLineDirectionCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_DefaultTurnDirection = Constants.TurnDirection.Clockwise;

            var line = new Line(-10.0,
                                -10.0,
                                10.0,
                                10.0);
            var point = new Point(5.0,
                                  0.0);

            m_Calculator = new RacetrackLineDirectionCalculator(line,
                                                                point,
                                                                m_DefaultTurnDirection);
        }

        private RacetrackLineDirectionCalculator m_Calculator;
        private Constants.TurnDirection m_DefaultTurnDirection;

        [Test]
        public void CalulateReturnsClockwiseForPointRightTest()
        {
            const double ax = -10.0;
            const double ay = -10.0;
            const double bx = 10.0;
            const double by = 10.0;
            const double cx = 5.0;
            const double cy = 0.0;

            var line = new Line(ax,
                                ay,
                                bx,
                                by);
            var point = new Point(cx,
                                  cy);

            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            m_Calculator.Calculate(line,
                                                   point));
        }

        [Test]
        public void CalulateReturnsCounterclockwiseForPointLeftTest()
        {
            const double ax = -10.0;
            const double ay = -10.0;
            const double bx = 10.0;
            const double by = 10.0;
            const double cx = -5.0;
            const double cy = 0.0;

            var line = new Line(ax,
                                ay,
                                bx,
                                by);
            var point = new Point(cx,
                                  cy);

            Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                            m_Calculator.Calculate(line,
                                                   point));
        }

        [Test]
        public void CalulateReturnsUnknownForPointOnLineTest()
        {
            const double ax = -10.0;
            const double ay = -10.0;
            const double bx = 10.0;
            const double by = 10.0;
            const double cx = -5.0;
            const double cy = -5.0;

            var line = new Line(ax,
                                ay,
                                bx,
                                by);
            var point = new Point(cx,
                                  cy);

            Assert.AreEqual(Constants.TurnDirection.Unknown,
                            m_Calculator.Calculate(line,
                                                   point));
        }

        [Test]
        public void FindSideDependingOnSlopeForSlopeIsZeroTest()
        {
            const double ax = 0.0;
            const double ay = 0.0;
            const double bx = 0.0;
            const double by = 0.0;
            const double cx = 50.0;
            const double cy = 50.0;

            RacetrackLineDirectionCalculator.Side actual = m_Calculator.FindSideDependingOnSlope(ax,
                                                                                                 ay,
                                                                                                 bx,
                                                                                                 by,
                                                                                                 cx,
                                                                                                 cy);

            Assert.AreEqual(RacetrackLineDirectionCalculator.Side.Unknown,
                            actual);
        }

        [Test]
        public void FindSideReturnsLeftForNegativeSlopeAndPointLeftTest()
        {
            const double ax = 0.0;
            const double ay = 0.0;
            const double bx = -100.0;
            const double by = -100.0;
            const double cx = 50.0;
            const double cy = -100.0;

            RacetrackLineDirectionCalculator.Side actual = m_Calculator.FindSide(ax,
                                                                                 ay,
                                                                                 bx,
                                                                                 by,
                                                                                 cx,
                                                                                 cy);

            Assert.AreEqual(RacetrackLineDirectionCalculator.Side.Left,
                            actual);
        }

        [Test]
        public void FindSideReturnsLeftForNegativeSlopeAndPointRightTest()
        {
            const double ax = 0.0;
            const double ay = 0.0;
            const double bx = -100.0;
            const double by = -100.0;
            const double cx = -50.0;
            const double cy = 0.0;

            RacetrackLineDirectionCalculator.Side actual = m_Calculator.FindSide(ax,
                                                                                 ay,
                                                                                 bx,
                                                                                 by,
                                                                                 cx,
                                                                                 cy);

            Assert.AreEqual(RacetrackLineDirectionCalculator.Side.Right,
                            actual);
        }

        [Test]
        public void FindSideReturnsLeftForPositiveSlopeAndPointLeftTest()
        {
            const double ax = 0.0;
            const double ay = 0.0;
            const double bx = 100.0;
            const double by = 100.0;
            const double cx = 0.0;
            const double cy = 50.0;

            RacetrackLineDirectionCalculator.Side actual = m_Calculator.FindSide(ax,
                                                                                 ay,
                                                                                 bx,
                                                                                 by,
                                                                                 cx,
                                                                                 cy);

            Assert.AreEqual(RacetrackLineDirectionCalculator.Side.Left,
                            actual);
        }

        [Test]
        public void FindSideReturnsLeftForPositiveSlopeAndPointRightTest()
        {
            const double ax = 0.0;
            const double ay = 0.0;
            const double bx = 100.0;
            const double by = 100.0;
            const double cx = 50.0;
            const double cy = 0.0;

            RacetrackLineDirectionCalculator.Side actual = m_Calculator.FindSide(ax,
                                                                                 ay,
                                                                                 bx,
                                                                                 by,
                                                                                 cx,
                                                                                 cy);

            Assert.AreEqual(RacetrackLineDirectionCalculator.Side.Right,
                            actual);
        }

        [Test]
        public void FindSideReturnsUnknownForBxEqualsAxTest()
        {
            const double ax = 10.0;
            const double ay = 0.0;
            const double bx = 10.0;
            const double by = 10.0;
            const double cx = 5.0;
            const double cy = 0.0;

            RacetrackLineDirectionCalculator.Side actual = m_Calculator.FindSide(ax,
                                                                                 ay,
                                                                                 bx,
                                                                                 by,
                                                                                 cx,
                                                                                 cy);

            Assert.AreEqual(RacetrackLineDirectionCalculator.Side.Unknown,
                            actual);
        }

        [Test]
        public void FindSideReturnsUnknownForByEqualsAyTest()
        {
            const double ax = 0.0;
            const double ay = 10.0;
            const double bx = 10.0;
            const double by = 10.0;
            const double cx = -5.0;
            const double cy = 0.0;

            RacetrackLineDirectionCalculator.Side actual = m_Calculator.FindSide(ax,
                                                                                 ay,
                                                                                 bx,
                                                                                 by,
                                                                                 cx,
                                                                                 cy);

            Assert.AreEqual(RacetrackLineDirectionCalculator.Side.Unknown,
                            actual);
        }

        [Test]
        public void FindSideReturnsUnknownForPointOnLineTest()
        {
            const double ax = 0.0;
            const double ay = 0.0;
            const double bx = 10.0;
            const double by = 10.0;
            const double cx = 5.0;
            const double cy = 5.0;

            RacetrackLineDirectionCalculator.Side actual = m_Calculator.FindSide(ax,
                                                                                 ay,
                                                                                 bx,
                                                                                 by,
                                                                                 cx,
                                                                                 cy);

            Assert.AreEqual(RacetrackLineDirectionCalculator.Side.Unknown,
                            actual);
        }

        [Test]
        public void FindSideReturnsUnknownForSlopeIsZeroTest()
        {
            const double ax = 0.0;
            const double ay = 0.0;
            const double bx = 0.0;
            const double by = 0.0;
            const double cx = 50.0;
            const double cy = 50.0;

            RacetrackLineDirectionCalculator.Side actual = m_Calculator.FindSide(ax,
                                                                                 ay,
                                                                                 bx,
                                                                                 by,
                                                                                 cx,
                                                                                 cy);

            Assert.AreEqual(RacetrackLineDirectionCalculator.Side.Unknown,
                            actual);
        }
    }
}