using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Calculators;

namespace Selkie.Racetrack.Tests.Calculators
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class LinePointDirectionForHorizontalOrVerticalLineCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_DefaultTurnDirection = Constants.TurnDirection.Clockwise;

            var line = new Line(-10.0,
                                -10.0,
                                -10.0,
                                10.0);
            var point = new Point(5.0,
                                  5.0);

            m_Calculator = new LinePointDirectionForHorizontalOrVerticalLineCalculator(line,
                                                                                       point,
                                                                                       m_DefaultTurnDirection);
        }

        private LinePointDirectionForHorizontalOrVerticalLineCalculator m_Calculator;
        private Constants.TurnDirection m_DefaultTurnDirection;

        [Test]
        public void DefaultTurnDirectionTest()
        {
            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            m_Calculator.TurnDirection);
        }

        [Test]
        public void DetermineDirectionReturnsClockwiseForHorizontalLineTest()
        {
            const double ax = 0.0;
            const double ay = 10.0;
            const double bx = 100.0;
            const double by = 10.0;
            const double cx = 5.0;
            const double cy = 20.0;

            var line = new Line(ax,
                                ay,
                                bx,
                                by);
            var point = new Point(cx,
                                  cy);

            Constants.TurnDirection actual = m_Calculator.DetermineDirection(line,
                                                                             point);

            Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                            actual);
        }

        [Test]
        public void DetermineDirectionReturnsClockwiseForVerticalLineTest()
        {
            const double ax = 10.0;
            const double ay = 10.0;
            const double bx = 10.0;
            const double by = 10.0;
            const double cx = 5.0;
            const double cy = 10.0;

            var line = new Line(ax,
                                ay,
                                bx,
                                by);
            var point = new Point(cx,
                                  cy);

            Constants.TurnDirection actual = m_Calculator.DetermineDirection(line,
                                                                             point);

            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            actual);
        }

        [Test]
        public void DetermineDirectionReturnsUnknownForPointOnLineTest()
        {
            const double ax = 10.0;
            const double ay = 100.0;
            const double bx = 100.0;
            const double by = 200.0;
            const double cx = 100.0;
            const double cy = 200.0;

            var line = new Line(ax,
                                ay,
                                bx,
                                by);
            var point = new Point(cx,
                                  cy);

            Constants.TurnDirection actual = m_Calculator.DetermineDirection(line,
                                                                             point);

            Assert.AreEqual(Constants.TurnDirection.Unknown,
                            actual);
        }

        [Test]
        public void FindSideForHorizontalLineReturnsClockwiseForCyGreaterByAndBxLessAxTest()
        {
            const double ax = 10.0;
            const double bx = -10.0;
            const double by = -5.0;
            const double cy = 5.0;

            Constants.TurnDirection actual = m_Calculator.FindSideForHorizontalLine(ax,
                                                                                    bx,
                                                                                    by,
                                                                                    cy);

            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            actual);
        }

        [Test]
        public void FindSideForHorizontalLineReturnsCounterclockwiseForCyGreaterByAndBxGreaterAxTest()
        {
            const double ax = -10.0;
            const double bx = 10.0;
            const double by = -5.0;
            const double cy = 5.0;

            Constants.TurnDirection actual = m_Calculator.FindSideForHorizontalLine(ax,
                                                                                    bx,
                                                                                    by,
                                                                                    cy);

            Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                            actual);
        }

        [Test]
        public void FindSideForHorizontalLineReturnsCounterclockwiseForCyLessByAndBxLessAxTest()
        {
            const double ax = 10.0;
            const double bx = -10.0;
            const double by = 5.0;
            const double cy = -5.0;

            Constants.TurnDirection actual = m_Calculator.FindSideForHorizontalLine(ax,
                                                                                    bx,
                                                                                    by,
                                                                                    cy);

            Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                            actual);
        }

        [Test]
        public void FindSideForHorizontalLineReturnsForCyLessByAndBxGreaterAxTest()
        {
            const double ax = -10.0;
            const double bx = 10.0;
            const double by = 5.0;
            const double cy = -5.0;

            Constants.TurnDirection actual = m_Calculator.FindSideForHorizontalLine(ax,
                                                                                    bx,
                                                                                    by,
                                                                                    cy);

            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            actual);
        }

        [Test]
        public void FindSideForHorizontalLineReturnsUnknownForCyEqualsByTest()
        {
            const double ax = 10.0;
            const double bx = -10.0;
            const double by = -5.0;
            const double cy = -5.0;

            Constants.TurnDirection actual = m_Calculator.FindSideForHorizontalLine(ax,
                                                                                    bx,
                                                                                    by,
                                                                                    cy);

            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            actual);
        }

        [Test]
        public void FindSideForVerticalLineReturnsUnknownForCxEqualsBxTest()
        {
            const double ay = 10.0;
            const double by = -10.0;
            const double bx = -5.0;
            const double cx = -5.0;

            Constants.TurnDirection actual = m_Calculator.FindSideForVerticalLine(ay,
                                                                                  bx,
                                                                                  by,
                                                                                  cx);

            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            actual);
        }

        [Test]
        public void FindSideForVerticalLineReturnsUnknownForCxGreaterBxAndByGreaterAyTest()
        {
            const double ay = -10.0;
            const double by = 10.0;
            const double bx = -5.0;
            const double cx = 5.0;

            Constants.TurnDirection actual = m_Calculator.FindSideForVerticalLine(ay,
                                                                                  bx,
                                                                                  by,
                                                                                  cx);

            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            actual);
        }

        [Test]
        public void FindSideForVerticalLineReturnsUnknownForCxGreaterBxAndByLessAyTest()
        {
            const double ay = 10.0;
            const double by = -10.0;
            const double bx = -5.0;
            const double cx = 5.0;

            Constants.TurnDirection actual = m_Calculator.FindSideForVerticalLine(ay,
                                                                                  bx,
                                                                                  by,
                                                                                  cx);

            Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                            actual);
        }

        [Test]
        public void FindSideForVerticalLineReturnsUnknownForCxLessBxAndByGreaterAyTest()
        {
            const double ay = -10.0;
            const double by = 10.0;
            const double bx = 5.0;
            const double cx = -5.0;

            Constants.TurnDirection actual = m_Calculator.FindSideForVerticalLine(ay,
                                                                                  bx,
                                                                                  by,
                                                                                  cx);

            Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                            actual);
        }

        [Test]
        public void FindSideForVerticalLineReturnsUnknownForCxLessBxAndByLessAyTest()
        {
            const double ay = 10.0;
            const double by = -10.0;
            const double bx = 5.0;
            const double cx = -5.0;

            Constants.TurnDirection actual = m_Calculator.FindSideForVerticalLine(ay,
                                                                                  bx,
                                                                                  by,
                                                                                  cx);

            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            actual);
        }
    }
}