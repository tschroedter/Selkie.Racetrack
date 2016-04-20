using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.NUnit.Extensions;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Turn;
using Selkie.Racetrack.Interfaces.UTurn;
using Selkie.Racetrack.Turn;
using Selkie.Racetrack.UTurn;
using Constants = Selkie.Geometry.Constants;

namespace Selkie.Racetrack.Tests.UTurn.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class UTurnCircleCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_PossibleTurnCircles = Substitute.For <IPossibleTurnCircles>();
            m_PairCalculator = Substitute.For <IDetermineCirclePairCalculator>();
            m_PointCalculator = Substitute.For <IAngleToCentrePointCalculator>();

            m_Sut = new UTurnCircleCalculator(m_PairCalculator,
                                              m_PossibleTurnCircles,
                                              m_PointCalculator);
        }

        private UTurnCircleCalculator m_Sut;
        private IDetermineCirclePairCalculator m_PairCalculator;
        private IPossibleTurnCircles m_PossibleTurnCircles;
        private IAngleToCentrePointCalculator m_PointCalculator;

        private ISettings CreateSettingsWithDifferentRadius(Distance expected)
        {
            var settings = new Settings(new Point(0.0,
                                                  0.0),
                                        Angle.ForZeroDegrees,
                                        new Point(10.0,
                                                  10.0),
                                        Angle.For180Degrees,
                                        new Distance(expected.Length - 1.0), // just to be smaller
                                        expected,
                                        true,
                                        true);
            return settings;
        }

        private static TurnCircle CreateTurnCircleDoesNotMatter()
        {
            var doesNotMatter = new TurnCircle(new Circle(0.0,
                                                          0.0,
                                                          10.0),
                                               Constants.CircleSide.Port,
                                               Constants.CircleOrigin.Start,
                                               Constants.TurnDirection.Counterclockwise);
            return doesNotMatter;
        }

        private static TurnCirclePair CreateDoesNotMatterPair(ISettings settings)
        {
            TurnCircle doesNotMatterTurnCircle = CreateTurnCircleDoesNotMatter();
            var doesNotMatterPair = new TurnCirclePair(settings,
                                                       doesNotMatterTurnCircle,
                                                       doesNotMatterTurnCircle);
            return doesNotMatterPair;
        }

        [Test]
        public void Calculate_CallsCalculateInCalculator_WhenCalled()
        {
            // Arrange
            ISettings doesNotMattersettings = CreateSettingsWithDifferentRadius(new Distance(100.0));
            TurnCirclePair doesNotMatterPair = CreateDoesNotMatterPair(doesNotMattersettings);
            m_PairCalculator.Pair.Returns(doesNotMatterPair);

            // Act
            m_Sut.Calculate();

            // Assert
            m_PointCalculator.Received().Calculate();
        }

        [Test]
        public void Calculate_SetsPairInCalculator_WhenCalled()
        {
            // Arrange
            ISettings doesNotMattersettings = CreateSettingsWithDifferentRadius(new Distance(100.0));
            TurnCirclePair doesNotMatterPair = CreateDoesNotMatterPair(doesNotMattersettings);
            m_PairCalculator.Pair.Returns(doesNotMatterPair);

            // Act
            m_Sut.Calculate();

            // Assert
            Assert.AreEqual(m_PairCalculator.Pair,
                            m_PointCalculator.Pair);
        }

        [Test]
        public void Calculate_UsesTheBiggerRadius_WhenPortAndStarboardRadiusAreDifferent()
        {
            // Arrange
            var expected = new Distance(1000.0);
            ISettings settings = CreateSettingsWithDifferentRadius(expected);
            m_Sut.Settings = settings;

            TurnCirclePair doesNotMatterPair = CreateDoesNotMatterPair(settings);
            m_PairCalculator.Pair.Returns(doesNotMatterPair);

            // Act
            m_Sut.Calculate();

            // Assert
            NUnitHelper.AssertIsEquivalent(expected.Length,
                                           m_Sut.Circle.Radius,
                                           0.01);
        }

        [Test]
        public void Circle_IsUnknown_ByDefault()
        {
            // Arrange
            // Act
            // Assert
            Assert.True(m_Sut.Circle.IsUnknown);
        }

        [Test]
        public void One_ReturnsPairCalculatorsOne_ByDefault()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(m_PairCalculator.One,
                            m_Sut.One);
        }

        [Test]
        public void Settings_IsUnknown_ByDefault()
        {
            // Arrange
            // Act
            // Assert
            Assert.True(m_Sut.Settings.IsUnknown);
        }

        [Test]
        public void Settings_Roundtrip_Test()
        {
            // Arrange
            var settings = Substitute.For <ISettings>();

            // Act
            m_Sut.Settings = settings;

            // Assert
            Assert.AreEqual(settings,
                            m_Sut.Settings);
        }

        [Test]
        public void TurnDirection_IsUnknown_ByDefault()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(Constants.TurnDirection.Unknown,
                            m_Sut.TurnDirection);
        }

        [Test]
        public void Zero_ReturnsPairCalculatorsZero_ByDefault()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(m_PairCalculator.Zero,
                            m_Sut.Zero);
        }
    }
}