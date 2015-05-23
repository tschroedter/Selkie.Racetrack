using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry;
using Selkie.Racetrack.UTurn;

namespace Selkie.Racetrack.Tests.UTurn.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class UTurnCircleCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_PairCalculator = Substitute.For <IDetermineCirclePairCalculator>();

            m_Calculator = new UTurnCircleCalculator(m_PairCalculator);
        }

        private UTurnCircleCalculator m_Calculator;
        private IDetermineCirclePairCalculator m_PairCalculator;

        [Test]
        public void CircleDefaultTest()
        {
            Assert.True(m_Calculator.Circle.IsUnknown);
        }

        [Test]
        public void OneDefaultTest()
        {
            Assert.AreEqual(m_PairCalculator.One,
                            m_Calculator.One);
        }

        [Test]
        public void SettingsDefaultTest()
        {
            Assert.True(m_Calculator.Circle.IsUnknown);
        }

        [Test]
        public void SettingsRoundtripTest()
        {
            var settings = Substitute.For <ISettings>();

            m_Calculator.Settings = settings;

            Assert.AreEqual(settings,
                            m_Calculator.Settings);
        }

        [Test]
        public void TurnDirectionDefaultTest()
        {
            Assert.AreEqual(Constants.TurnDirection.Unknown,
                            m_Calculator.TurnDirection);
        }

        [Test]
        public void ZeroDefaultTest()
        {
            Assert.AreEqual(m_PairCalculator.Zero,
                            m_Calculator.Zero);
        }
    }
}