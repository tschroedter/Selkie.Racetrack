using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NUnit.Framework;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Tests
{
    // ReSharper disable ClassTooBig
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class SettingsTests
    {
        [SetUp]
        public void Setup()
        {
            m_StartPoint = new Point(1.0,
                                     2.0);
            m_StartAzimuth = Angle.FromDegrees(45.0);
            m_FinishPoint = new Point(3.0,
                                      4.0);
            m_FinishAzimuth = Angle.FromDegrees(225.0);
            m_RadiusForPortTurn = new Distance(123.0);
            m_RadiusForStarboardTurn = new Distance(456.0);

            m_Sut = new Settings(m_StartPoint,
                                 m_StartAzimuth,
                                 m_FinishPoint,
                                 m_FinishAzimuth,
                                 m_RadiusForPortTurn,
                                 m_RadiusForStarboardTurn,
                                 true,
                                 true);
        }

        private Angle m_FinishAzimuth;
        private Point m_FinishPoint;
        private Settings m_Sut;
        private Angle m_StartAzimuth;
        private Point m_StartPoint;
        private Distance m_RadiusForPortTurn;
        private Distance m_RadiusForStarboardTurn;

        [NotNull]
        private Settings CreateSettings()
        {
            // ReSharper disable once IntroduceOptionalParameters.Local
            return CreateSettings(true,
                                  true);
        }

        [NotNull]
        private Settings CreateSettings(bool isPortTurnAllowed,
                                        bool isStarboardTurnAllowed)
        {
            var startPoint = new Point(1.0,
                                       2.0);
            Angle startAzimuth = Angle.FromDegrees(45.0);
            var finishPoint = new Point(3.0,
                                        4.0);
            Angle finischAzimuth = Angle.FromDegrees(225.0);

            var settings = new Settings(startPoint,
                                        startAzimuth,
                                        finishPoint,
                                        finischAzimuth,
                                        m_RadiusForPortTurn,
                                        m_RadiusForStarboardTurn,
                                        isPortTurnAllowed,
                                        isStarboardTurnAllowed);

            return settings;
        }

        [Test]
        public void DefaultTurnDirectionReturnsCounterclockwiseForPortOnlyTest()
        {
            // Arrange
            // Act
            // Assert
            Settings sut = CreateSettings(true,
                                          false);

            // Act
            // Assert
            Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                            sut.DefaultTurnDirection);
        }

        [Test]
        public void DefaultTurnDirectionReturnsCounterclockwiseForStarboardOnlyTest()
        {
            // Arrange
            Settings sut = CreateSettings(false,
                                          true);

            // Act
            // Assert
            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            sut.DefaultTurnDirection);
        }

        [Test]
        public void DefaultTurnDirectionReturnsCounterclockwiseTest()
        {
            // Arrange
            Settings sut = CreateSettings();

            // Act
            // Assert
            Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                            sut.DefaultTurnDirection);
        }

        [Test]
        public void EqualsOperatorReturnsFalseForDifferentValueTest()
        {
            // Arrange
            Settings settings1 = CreateSettings();
            var settings2 = new Settings(settings1.StartPoint.Move(10.0,
                                                                   10.0),
                                         settings1.StartAzimuth,
                                         settings1.FinishPoint,
                                         settings1.FinishAzimuth,
                                         m_RadiusForPortTurn,
                                         m_RadiusForStarboardTurn,
                                         settings1.IsPortTurnAllowed,
                                         settings1.IsStarboardTurnAllowed);

            // Act
            // Assert
            Assert.False(settings1 == settings2);
        }

        [Test]
        public void EqualsOperatorReturnsTrueForSameValuesTest()
        {
            // Arrange
            Settings settings1 = CreateSettings();
            var settings2 = new Settings(settings1.StartPoint,
                                         settings1.StartAzimuth,
                                         settings1.FinishPoint,
                                         settings1.FinishAzimuth,
                                         m_RadiusForPortTurn,
                                         m_RadiusForStarboardTurn,
                                         settings1.IsPortTurnAllowed,
                                         settings1.IsStarboardTurnAllowed);

            // Act
            // Assert
            Assert.True(settings1 == settings2);
        }

        [Test]
        public void EqualsOperatorReturnsTrueForSameValueTest()
        {
            // Arrange
            Settings settings1 = CreateSettings();
            Settings settings2 = CreateSettings();

            // Act
            // Assert
            Assert.True(settings1 == settings2);
        }

        [Test]
        public void EqualsReturnsFalseForNullTest()
        {
            // Arrange
            Settings sut = CreateSettings();

            // Act
            // Assert
            Assert.False(sut.Equals(null));
        }

        [Test]
        public void EqualsReturnsFalseForOtherClassTest()
        {
            // Arrange
            Settings sut = CreateSettings();

            // Act
            // Assert
            Assert.False(sut.Equals(new object()));
        }

        [Test]
        public void EqualsReturnsTrueForSameTest()
        {
            // Arrange
            Settings sut = CreateSettings();

            // Act
            // Assert
            Assert.True(sut.Equals(sut));
        }

        [Test]
        public void EqualsReturnsTrueForSameValueTest()
        {
            // Arrange
            Settings sut = CreateSettings();

            // Act
            // Assert
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.DoesNotThrow(() => sut.GetHashCode());
        }

        [Test]
        public void FinishAzimuthTest()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(m_FinishAzimuth,
                            m_Sut.FinishAzimuth);
        }

        [Test]
        public void FinishPointTest()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(m_FinishPoint,
                            m_Sut.FinishPoint);
        }

        [Test]
        public void IsPortTurnAllowedTest()
        {
            // Arrange
            Settings settings = CreateSettings();

            // Act
            // Assert
            Assert.True(settings.IsPortTurnAllowed);
        }

        [Test]
        public void IsStarboardTurnAllowedTest()
        {
            // Arrange
            Settings settings = CreateSettings();

            // Act
            // Assert
            Assert.True(settings.IsStarboardTurnAllowed);
        }

        [Test]
        public void IsUnknownDefaultTest()
        {
            // Arrange
            // Act
            // Assert
            Assert.False(m_Sut.IsUnknown);
        }

        [Test]
        public void IsUnknownReturnsTrueForUnknownTest()
        {
            // Arrange
            // Act
            // Assert
            Assert.True(Settings.Unknown.IsUnknown);
        }

        [Test]
        public void LargestRadiusForTurn_RetursLargestRadius_WhenCalled()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(m_RadiusForStarboardTurn,
                            m_Sut.LargestRadiusForTurn);
        }

        [Test]
        public void NotEqualsOperatorReturnsFalseForDifferentValueTest()
        {
            // Arrange
            Settings settings1 = CreateSettings();
            var settings2 = new Settings(settings1.StartPoint.Move(10.0,
                                                                   10.0),
                                         settings1.StartAzimuth,
                                         settings1.FinishPoint,
                                         settings1.FinishAzimuth,
                                         m_RadiusForPortTurn,
                                         m_RadiusForStarboardTurn,
                                         settings1.IsPortTurnAllowed,
                                         settings1.IsStarboardTurnAllowed);

            // Act
            // Assert
            Assert.True(settings1 != settings2);
        }

        [Test]
        public void NotEqualsOperatorReturnsFalseForSameValuesTest()
        {
            // Arrange
            Settings settings1 = CreateSettings();
            var settings2 = new Settings(settings1.StartPoint,
                                         settings1.StartAzimuth,
                                         settings1.FinishPoint,
                                         settings1.FinishAzimuth,
                                         m_RadiusForPortTurn,
                                         m_RadiusForStarboardTurn,
                                         settings1.IsPortTurnAllowed,
                                         settings1.IsStarboardTurnAllowed);

            // Act
            // Assert
            Assert.False(settings1 != settings2);
        }

        [Test]
        public void NotEqualsOperatorReturnsTrueForSameValuesTest()
        {
            // Arrange
            Settings settings1 = CreateSettings();
            Settings settings2 = CreateSettings();

            // Act
            // Assert
            Assert.False(settings1 != settings2);
        }

        [Test]
        public void RadiusForPortTurnTest()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(m_RadiusForPortTurn,
                            m_Sut.RadiusForPortTurn);
        }

        [Test]
        public void RadiusForStarboardTurnTest()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(m_RadiusForStarboardTurn,
                            m_Sut.RadiusForStarboardTurn);
        }

        [Test]
        public void StartAzimuthTest()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(m_StartAzimuth,
                            m_Sut.StartAzimuth);
        }

        [Test]
        public void StartPointTest()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(m_StartPoint,
                            m_Sut.StartPoint);
        }

        [Test]
        public void UnknownTest()
        {
            // Arrange
            // Act
            Settings settings = Settings.Unknown;

            // Assert
            Assert.AreEqual(Point.Unknown,
                            settings.StartPoint,
                            "StartPoint");
            Assert.AreEqual(Point.Unknown,
                            settings.FinishPoint,
                            "FinishPoint");
            Assert.AreEqual(Angle.Unknown,
                            settings.StartAzimuth,
                            "StartAzimuth");
            Assert.AreEqual(Angle.Unknown,
                            settings.FinishAzimuth,
                            "FinishAzimuth");
            Assert.AreEqual(Distance.Unknown,
                            settings.RadiusForPortTurn,
                            "Radius");
            Assert.AreEqual(Distance.Unknown,
                            settings.RadiusForStarboardTurn,
                            "Radius");
            Assert.True(settings.IsPortTurnAllowed,
                        "IsPortTurnAllowed");
            Assert.True(settings.IsStarboardTurnAllowed,
                        "IsStarboardTurnAllowed");
            Assert.AreEqual(Distance.Unknown,
                            settings.LargestRadiusForTurn,
                            "LargestRadiusForTurn");
        }
    }
}