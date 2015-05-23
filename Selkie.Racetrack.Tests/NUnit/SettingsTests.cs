using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NUnit.Framework;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Tests.NUnit
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
            m_Radius = new Distance(100.0);

            m_Settings = new Settings(m_StartPoint,
                                      m_StartAzimuth,
                                      m_FinishPoint,
                                      m_FinishAzimuth,
                                      m_Radius,
                                      true,
                                      true);
        }

        private Angle m_FinishAzimuth;
        private Point m_FinishPoint;
        private Distance m_Radius;
        private Settings m_Settings;
        private Angle m_StartAzimuth;
        private Point m_StartPoint;

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
            var radius = new Distance(100.0);

            var settings = new Settings(startPoint,
                                        startAzimuth,
                                        finishPoint,
                                        finischAzimuth,
                                        radius,
                                        isPortTurnAllowed,
                                        isStarboardTurnAllowed);

            return settings;
        }

        [Test]
        public void DefaultTurnDirectionReturnsCounterclockwiseForPortOnlyTest()
        {
            Settings settings = CreateSettings(true,
                                               false);

            Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                            settings.DefaultTurnDirection);
        }

        [Test]
        public void DefaultTurnDirectionReturnsCounterclockwiseForStarboardOnlyTest()
        {
            Settings settings = CreateSettings(false,
                                               true);

            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            settings.DefaultTurnDirection);
        }

        [Test]
        public void DefaultTurnDirectionReturnsCounterclockwiseTest()
        {
            Settings settings = CreateSettings();

            Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                            settings.DefaultTurnDirection);
        }

        [Test]
        public void EqualsOperatorReturnsFalseForDifferentValueTest()
        {
            Settings settings1 = CreateSettings();
            var settings2 = new Settings(settings1.StartPoint.Move(10.0,
                                                                   10.0),
                                         settings1.StartAzimuth,
                                         settings1.FinishPoint,
                                         settings1.FinishAzimuth,
                                         settings1.Radius,
                                         settings1.IsPortTurnAllowed,
                                         settings1.IsStarboardTurnAllowed);

            Assert.False(settings1 == settings2);
        }

        [Test]
        public void EqualsOperatorReturnsTrueForSameValuesTest()
        {
            Settings settings1 = CreateSettings();
            var settings2 = new Settings(settings1.StartPoint,
                                         settings1.StartAzimuth,
                                         settings1.FinishPoint,
                                         settings1.FinishAzimuth,
                                         settings1.Radius,
                                         settings1.IsPortTurnAllowed,
                                         settings1.IsStarboardTurnAllowed);

            Assert.True(settings1 == settings2);
        }

        [Test]
        public void EqualsOperatorReturnsTrueForSameValueTest()
        {
            Settings settings1 = CreateSettings();
            Settings settings2 = CreateSettings();

            Assert.True(settings1 == settings2);
        }

        [Test]
        public void EqualsReturnsFalseForNullTest()
        {
            Settings settings = CreateSettings();

            Assert.False(settings.Equals(null));
        }

        [Test]
        public void EqualsReturnsFalseForOtherClassTest()
        {
            Settings settings = CreateSettings();

            Assert.False(settings.Equals(new object()));
        }

        [Test]
        public void EqualsReturnsTrueForSameTest()
        {
            Settings settings = CreateSettings();

            Assert.True(settings.Equals(settings));
        }

        [Test]
        public void EqualsReturnsTrueForSameValueTest()
        {
            Settings settings = CreateSettings();

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.DoesNotThrow(() => settings.GetHashCode());
        }

        [Test]
        public void FinishAzimuthTest()
        {
            Assert.AreEqual(m_FinishAzimuth,
                            m_Settings.FinishAzimuth);
        }

        [Test]
        public void FinishPointTest()
        {
            Assert.AreEqual(m_FinishPoint,
                            m_Settings.FinishPoint);
        }

        [Test]
        public void IsPortTurnAllowedTest()
        {
            Settings settings = CreateSettings();

            Assert.True(settings.IsPortTurnAllowed);
        }

        [Test]
        public void IsStarboardTurnAllowedTest()
        {
            Settings settings = CreateSettings();

            Assert.True(settings.IsStarboardTurnAllowed);
        }

        [Test]
        public void IsUnknownDefaultTest()
        {
            Assert.False(m_Settings.IsUnknown);
        }

        [Test]
        public void IsUnknownReturnsTrueForUnknownTest()
        {
            Assert.True(Settings.Unknown.IsUnknown);
        }

        [Test]
        public void NotEqualsOperatorReturnsFalseForDifferentValueTest()
        {
            Settings settings1 = CreateSettings();
            var settings2 = new Settings(settings1.StartPoint.Move(10.0,
                                                                   10.0),
                                         settings1.StartAzimuth,
                                         settings1.FinishPoint,
                                         settings1.FinishAzimuth,
                                         settings1.Radius,
                                         settings1.IsPortTurnAllowed,
                                         settings1.IsStarboardTurnAllowed);

            Assert.True(settings1 != settings2);
        }

        [Test]
        public void NotEqualsOperatorReturnsFalseForSameValuesTest()
        {
            Settings settings1 = CreateSettings();
            var settings2 = new Settings(settings1.StartPoint,
                                         settings1.StartAzimuth,
                                         settings1.FinishPoint,
                                         settings1.FinishAzimuth,
                                         settings1.Radius,
                                         settings1.IsPortTurnAllowed,
                                         settings1.IsStarboardTurnAllowed);

            Assert.False(settings1 != settings2);
        }

        [Test]
        public void NotEqualsOperatorReturnsTrueForSameValuesTest()
        {
            Settings settings1 = CreateSettings();
            Settings settings2 = CreateSettings();

            Assert.False(settings1 != settings2);
        }

        [Test]
        public void RadiusTest()
        {
            Assert.AreEqual(m_Radius,
                            m_Settings.Radius);
        }

        [Test]
        public void StartAzimuthTest()
        {
            Assert.AreEqual(m_StartAzimuth,
                            m_Settings.StartAzimuth);
        }

        [Test]
        public void StartPointTest()
        {
            Assert.AreEqual(m_StartPoint,
                            m_Settings.StartPoint);
        }

        [Test]
        public void UnknownTest()
        {
            Settings settings = Settings.Unknown;

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
                            settings.Radius,
                            "Radius");
            Assert.True(settings.IsPortTurnAllowed,
                        "IsPortTurnAllowed");
            Assert.True(settings.IsStarboardTurnAllowed,
                        "IsStarboardTurnAllowed");
        }
    }
}