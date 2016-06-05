using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Calculators;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Tests.Calculators
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

            m_Sut = new FeaturePairToRacetrackCalculator(m_PathCalculator);
        }

        private FeaturePairToRacetrackCalculator m_Sut;
        private IPathCalculator m_PathCalculator;

        [Test]
        public void Calculate_CallsCalculate_WhenCalled()
        {
            // Arrange
            var fromFeature = Substitute.For <ISurveyFeature>();
            var toFeature = Substitute.For <ISurveyFeature>();

            m_Sut.FromFeature = fromFeature;
            m_Sut.ToFeature = toFeature;
            m_Sut.IsPortTurnAllowed = true;
            m_Sut.IsStarboardTurnAllowed = true;

            // Act
            m_Sut.Calculate();

            // Assert
            m_PathCalculator.Received().Calculate();
        }

        // ReSharper disable once MethodTooLong
        [Test]
        public void Calculate_SetsSettings_WhenCalledt()
        {
            // Arrange
            var fromFeature = Substitute.For <ISurveyFeature>();
            var toFeature = Substitute.For <ISurveyFeature>();

            m_Sut.FromFeature = fromFeature;
            m_Sut.ToFeature = toFeature;
            m_Sut.RadiusForPortTurn = new Distance(123.0);
            m_Sut.RadiusForStarboardTurn = new Distance(456.0);
            m_Sut.IsPortTurnAllowed = true;
            m_Sut.IsStarboardTurnAllowed = true;

            // Act
            m_Sut.Calculate();

            // Assert
            ISettings actual = m_Sut.Settings;

            Assert.AreEqual(fromFeature.EndPoint,
                            actual.StartPoint,
                            "StartPoint");
            Assert.AreEqual(fromFeature.AngleToXAxisAtEndPoint,
                            actual.StartAzimuth,
                            "StartAzimuth");
            Assert.AreEqual(toFeature.StartPoint,
                            actual.FinishPoint,
                            "FinishPoint");
            Assert.AreEqual(toFeature.AngleToXAxisAtEndPoint,
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
        public void FromFeature_ReturnsDefault_WhenCalled()
        {
            // Arrange
            // Act
            // Assert
            Assert.True(m_Sut.FromFeature.IsUnknown);
        }

        [Test]
        public void FromFeature_ReturnsUnknown_WhenCalled()
        {
            // Arrange
            // Act
            // Assert
            Assert.True(m_Sut.FromFeature.IsUnknown);
        }

        [Test]
        public void FromFeature_Roundtrip_Test()
        {
            // Arrange
            var feature = Substitute.For <ISurveyFeature>();

            // Act
            m_Sut.FromFeature = feature;

            // Assert
            Assert.AreEqual(feature,
                            m_Sut.FromFeature);
        }

        [Test]
        public void IsPortTurnAllowed_ReturnsDefault_WhenCalled()
        {
            // Arrange
            // Act
            // Assert
            Assert.True(m_Sut.IsPortTurnAllowed);
        }

        [Test]
        public void IsPortTurnAllowed_Roundtrip_Test()
        {
            // Arrange
            // Act
            m_Sut.IsPortTurnAllowed = false;

            // Assert
            Assert.False(m_Sut.IsPortTurnAllowed);
        }

        [Test]
        public void IsStarboardTurnAllowed_ReturnsDefault_WhenCalled()
        {
            // Arrange
            // Act
            // Assert
            Assert.True(m_Sut.IsStarboardTurnAllowed);
        }

        [Test]
        public void IsStarboardTurnAllowed_Roundtrip_Test()
        {
            // Arrange
            // Act
            m_Sut.IsStarboardTurnAllowed = false;

            // Assert
            Assert.False(m_Sut.IsStarboardTurnAllowed);
        }

        [Test]
        public void Racetrack_ReturnsPath__WhenCalled()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(m_PathCalculator.Path,
                            m_Sut.Racetrack);
        }

        [Test]
        public void RadiusForPortTurn_ReturnsDefault_WhenCalled()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(0.0,
                            m_Sut.RadiusForPortTurn.Length,
                            "Length");
            Assert.True(m_Sut.RadiusForPortTurn.IsUnknown,
                        "IsUnknown");
        }

        [Test]
        public void RadiusForPortTurn_Roundtrip_Test()
        {
            // Arrange
            // Act
            m_Sut.RadiusForPortTurn = new Distance(123.0);

            // Assert
            Assert.AreEqual(123.0,
                            m_Sut.RadiusForPortTurn.Length);
        }

        [Test]
        public void RadiusForStarboardTurn_ForPortTurn_Roundtrip_Test()
        {
            // Arrange
            // Act
            m_Sut.RadiusForStarboardTurn = new Distance(123.0);

            // Assert
            Assert.AreEqual(123.0,
                            m_Sut.RadiusForStarboardTurn.Length);
        }

        [Test]
        public void RadiusForStarboardTurn_ReturnsDefault_WhenCalled()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(0.0,
                            m_Sut.RadiusForStarboardTurn.Length,
                            "Length");
            Assert.True(m_Sut.RadiusForStarboardTurn.IsUnknown,
                        "IsUnknown");
        }

        [Test]
        public void ToFeature_ReturnsDefault_WhenCalled()
        {
            // Arrange
            // Act
            // Assert
            Assert.True(m_Sut.ToFeature.IsUnknown);
        }

        [Test]
        public void ToFeature_Roundtrip_Test()
        {
            // Arrange
            var feature = Substitute.For <ISurveyFeature>();

            // Act
            m_Sut.ToFeature = feature;

            // Assert
            Assert.AreEqual(feature,
                            m_Sut.ToFeature);
        }
    }
}