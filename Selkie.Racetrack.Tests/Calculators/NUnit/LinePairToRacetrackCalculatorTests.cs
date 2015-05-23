using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Common;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Calculators;

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
            m_Disposer = Substitute.For <IDisposer>();

            m_PathCalculator = Substitute.For <IPathCalculator>();
            m_Factory = Substitute.For <ICalculatorFactory>();
            m_Factory.Create <IPathCalculator>().Returns(m_PathCalculator);

            m_Calculator = new LinePairToRacetrackCalculator(m_Disposer,
                                                             m_Factory);
        }

        [TearDown]
        public void Teardown()
        {
            m_Calculator.Dispose();
        }

        private LinePairToRacetrackCalculator m_Calculator;
        private IDisposer m_Disposer;
        private ICalculatorFactory m_Factory;
        private IPathCalculator m_PathCalculator;

        [Test]
        public void CalculateCallsCalculateTest()
        {
            var fromLine = Substitute.For <ILine>();
            var line = Substitute.For <ILine>();

            m_Calculator.FromLine = fromLine;
            m_Calculator.ToLine = line;
            m_Calculator.Radius = new Distance(123.0);
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
            m_Calculator.Radius = new Distance(123.0);
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
                            actual.Radius.Length,
                            "Radius");
            Assert.True(actual.IsPortTurnAllowed,
                        "IsPortTurnAllowed");
            Assert.True(actual.IsStarboardTurnAllowed,
                        "IsStarboardTurnAllowed");
        }

        [Test]
        public void ConstructorAddsToDisposerTest()
        {
            m_Disposer.Received().AddResource(m_Calculator.ReleaseCalculator);
        }

        [Test]
        public void ConstructorCallsCreateTest()
        {
            m_Factory.Received().Create <IPathCalculator>();
        }

        [Test]
        public void DisposeCallsDisposeTest()
        {
            var disposer = Substitute.For <IDisposer>();

            var calculator = new LinePairToRacetrackCalculator(disposer,
                                                               m_Factory);

            calculator.Dispose();

            disposer.Received().Dispose();
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
        public void RadiusDefaultTest()
        {
            Assert.AreEqual(0.0,
                            m_Calculator.Radius.Length,
                            "Length");
            Assert.True(m_Calculator.Radius.IsUnknown,
                        "IsUnknown");
        }

        [Test]
        public void RadiusRoundtripTest()
        {
            m_Calculator.Radius = new Distance(123.0);

            Assert.AreEqual(123.0,
                            m_Calculator.Radius.Length);
        }

        [Test]
        public void ReleaseCalculatorCallsReleaseTest()
        {
            var disposer = Substitute.For <IDisposer>();

            var calculator = new LinePairToRacetrackCalculator(disposer,
                                                               m_Factory);

            calculator.ReleaseCalculator();

            m_Factory.Received().Release(m_PathCalculator);
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