using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Calculators;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Tests.Calculators.NUnit
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class LinePairToRacetrackCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_PathCalculator = Substitute.For <IPathCalculator>();

            m_Calculator = new LinePairToRacetrackCalculator(m_PathCalculator);
        }

        private LinePairToRacetrackCalculator m_Calculator;
        private IPathCalculator m_PathCalculator;

        [Test]
        public void CalculateCallsCalculateTest()
        {
            var fromLine = Substitute.For <ILine>();
            var line = Substitute.For <ILine>();

            m_Calculator.FromLine = fromLine;
            m_Calculator.ToLine = line;
            m_Calculator.IsPortTurnAllowed = true;
            m_Calculator.IsStarboardTurnAllowed = true;

            m_Calculator.Calculate();

            m_PathCalculator.Received().Calculate();
        }

        // ReSharper disable once MethodTooLong
        [Test]
        public void CalculateSetsSettingsTest()
        {
            var fromLine = Substitute.For <ILine>();
            var line = Substitute.For <ILine>();

            m_Calculator.FromLine = fromLine;
            m_Calculator.ToLine = line;
            m_Calculator.RadiusForPortTurn = new Distance(123.0);
            m_Calculator.RadiusForStarboardTurn = new Distance(456.0);
            m_Calculator.IsPortTurnAllowed = true;
            m_Calculator.IsStarboardTurnAllowed = true;

            m_Calculator.Calculate();

            ISettings actual = m_Calculator.Settings;

            Assert.AreEqual(fromLine.EndPoint,
                            actual.StartPoint,
                            "StartPoint");
            Assert.AreEqual(fromLine.AngleToXAxis,
                            actual.StartAzimuth,
                            "StartAzimuth");
            Assert.AreEqual(line.StartPoint,
                            actual.FinishPoint,
                            "FinishPoint");
            Assert.AreEqual(line.AngleToXAxis,
                            actual.FinishAzimuth,
                            "FinishAzimuth");
            Assert.AreEqual(123.0,
                            actual.RadiusForPortTurn.Length,
                            "RadiusForPortTurn");
            Assert.AreEqual(456.0,
                            actual.RadiusForStarboardTurn.Length,
                            "RadiusForStarboardTurn");
            Assert.True(actual.IsPortTurnAllowed,
                        "IsPortTurnAllowed");
            Assert.True(actual.IsStarboardTurnAllowed,
                        "IsStarboardTurnAllowed");
        }

        [Test]
        public void FromLineDefaultTest()
        {
            Assert.True(m_Calculator.FromLine.IsUnknown);
        }

        [Test]
        public void FromLineRoundtripTest()
        {
            var line = Substitute.For <ILine>();

            m_Calculator.FromLine = line;

            Assert.AreEqual(line,
                            m_Calculator.FromLine);
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
        public void RacetrackTest()
        {
            Assert.AreEqual(m_PathCalculator.Path,
                            m_Calculator.Racetrack);
        }

        [Test]
        public void RadiusForPortTurnDefaultTest()
        {
            Assert.AreEqual(0.0,
                            m_Calculator.RadiusForPortTurn.Length,
                            "Length");
            Assert.True(m_Calculator.RadiusForPortTurn.IsUnknown,
                        "IsUnknown");
        }

        [Test]
        public void RadiusForPortTurnRoundtripTest()
        {
            m_Calculator.RadiusForPortTurn = new Distance(123.0);

            Assert.AreEqual(123.0,
                            m_Calculator.RadiusForPortTurn.Length);
        }

        [Test]
        public void RadiusForStarboardTurnDefaultTest()
        {
            Assert.AreEqual(0.0,
                            m_Calculator.RadiusForStarboardTurn.Length,
                            "Length");
            Assert.True(m_Calculator.RadiusForStarboardTurn.IsUnknown,
                        "IsUnknown");
        }

        [Test]
        public void RadiusForStarboardTurnForPortTurnRoundtripTest()
        {
            m_Calculator.RadiusForStarboardTurn = new Distance(123.0);

            Assert.AreEqual(123.0,
                            m_Calculator.RadiusForStarboardTurn.Length);
        }

        [Test]
        public void ToLineDefaultTest()
        {
            Assert.True(m_Calculator.FromLine.IsUnknown);
        }

        [Test]
        public void ToLineRoundtripTest()
        {
            var line = Substitute.For <ILine>();

            m_Calculator.ToLine = line;

            Assert.AreEqual(line,
                            m_Calculator.ToLine);
        }
    }
}