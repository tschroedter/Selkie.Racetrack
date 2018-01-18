using System.Diagnostics.CodeAnalysis;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Racetrack.Interfaces.UTurn;
using Core2.Selkie.Racetrack.Turn;
using Core2.Selkie.Racetrack.UTurn;
using NSubstitute;
using NUnit.Framework;

namespace Core2.Selkie.Racetrack.Tests.UTurn
{
    [ExcludeFromCodeCoverage]
    internal sealed class UTurnCircleTests
    {
        #region Nested type: UTurnCircleGeneralTests

        [TestFixture]
        internal sealed class UTurnCircleGeneralTests
        {
            [SetUp]
            public void Setup()
            {
                m_StartPoint = new Point(200.0,
                                         100.0);
                m_StartAzimuth = Angle.FromDegrees(0.0);
                m_FinishPoint = new Point(180.0,
                                          80.0);
                m_FinishAzimuth = Angle.FromDegrees(180.0);

                m_RadiusForPortTurn = new Distance(30);
                m_RadiusForStarboardTurn = new Distance(30);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                var possibleTurnCircles = new PossibleTurnCircles
                                          {
                                              Settings = m_Settings
                                          };

                var determineCirclePair = new DetermineCirclePairCalculator(possibleTurnCircles)
                                          {
                                              Settings = m_Settings
                                          };
                determineCirclePair.Calculate();

                var uTurnCircleCalculator = new UTurnCircleCalculator(determineCirclePair,
                                                                      possibleTurnCircles,
                                                                      new AngleToCentrePointCalculator());

                m_Circle = new UTurnCircle(possibleTurnCircles,
                                           uTurnCircleCalculator)
                           {
                               Settings = m_Settings
                           };

                m_Circle.Calculate();
            }

            private UTurnCircle m_Circle;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private Distance m_RadiusForPortTurn;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void IsUnknownReturnsFalseForKnownTest()
            {
                Assert.False(m_Circle.IsUnknown);
            }

            [Test]
            public void IsUnknownReturnsTrueForUnknownTest()
            {
                Assert.True(UTurnCircle.Unknown.IsUnknown);
            }
        }

        #endregion

        #region Nested type: UTurnCircleCaseFourTests

        [TestFixture]
        internal sealed class UTurnCircleCaseFourTests
        {
            [SetUp]
            public void Setup()
            {
                m_StartPoint = new Point(200.0,
                                         100.0);
                m_StartAzimuth = Angle.FromDegrees(0.0);
                m_FinishPoint = new Point(180.0,
                                          80.0);
                m_FinishAzimuth = Angle.FromDegrees(180.0);

                m_RadiusForPortTurn = new Distance(30.0);
                m_RadiusForStarboardTurn = new Distance(30.0);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                var possibleTurnCircles = new PossibleTurnCircles
                                          {
                                              Settings = m_Settings
                                          };

                var determineCirclePair = new DetermineCirclePairCalculator(possibleTurnCircles)
                                          {
                                              Settings = m_Settings
                                          };
                determineCirclePair.Calculate();

                var uTurnCircleCalculator = new UTurnCircleCalculator(determineCirclePair,
                                                                      possibleTurnCircles,
                                                                      new AngleToCentrePointCalculator());

                m_Circle = new UTurnCircle(possibleTurnCircles,
                                           uTurnCircleCalculator)
                           {
                               Settings = m_Settings
                           };

                m_Circle.Calculate();
            }

            private UTurnCircle m_Circle;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void CentrePointTest()
            {
                var expected = new Point(232.29,
                                         79.43);
                Point actual = m_Circle.CentrePoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void IsRequiredTest()
            {
                Assert.True(m_Circle.IsRequired);
            }

            [Test]
            public void TurnCirclePairTest()
            {
                Assert.NotNull(m_Circle.TurnCirclePair);
            }

            [Test]
            public void UTurnOneIntersectionPointTest()
            {
                var expected = new Point(216.14,
                                         104.71);
                Point actual = m_Circle.UTurnOneIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void UTurnZeroIntersectionPointTest()
            {
                var expected = new Point(206.14,
                                         64.71);
                Point actual = m_Circle.UTurnZeroIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }
        }

        #endregion

        #region Nested type: UTurnCircleCaseOneTests

        [TestFixture]
        internal sealed class UTurnCircleCaseOneTests
        {
            [SetUp]
            public void Setup()
            {
                m_StartPoint = new Point(8.5,
                                         0.0);
                m_StartAzimuth = Angle.FromDegrees(180.0 + 90.0);
                m_FinishPoint = new Point(7.5,
                                          0.0);
                m_FinishAzimuth = Angle.FromDegrees(0.0 + 90.0);

                m_RadiusForPortTurn = new Distance(2.5);
                m_RadiusForStarboardTurn = new Distance(2.5);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                var possibleTurnCircles = new PossibleTurnCircles
                                          {
                                              Settings = m_Settings
                                          };

                var determineCirclePair = new DetermineCirclePairCalculator(possibleTurnCircles)
                                          {
                                              Settings = m_Settings
                                          };
                determineCirclePair.Calculate();

                var uTurnCircleCalculator = new UTurnCircleCalculator(determineCirclePair,
                                                                      possibleTurnCircles,
                                                                      new AngleToCentrePointCalculator());

                m_Circle = new UTurnCircle(possibleTurnCircles,
                                           uTurnCircleCalculator)
                           {
                               Settings = m_Settings
                           };

                m_Circle.Calculate();
            }

            private UTurnCircle m_Circle;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void CentrePointTest()
            {
                var expected = new Point(8.0,
                                         -4.0);
                Point actual = m_Circle.CentrePoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void IsRequiredTest()
            {
                Assert.True(m_Circle.IsRequired);
            }

            [Test]
            public void TurnCirclePairTest()
            {
                Assert.NotNull(m_Circle.TurnCirclePair);
            }

            [Test]
            public void UTurnOneIntersectionPointTest()
            {
                var expected = new Point(9.5,
                                         -2.0);
                Point actual = m_Circle.UTurnOneIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void UTurnZeroIntersectionPointTest()
            {
                var expected = new Point(6.5,
                                         -2.0);
                Point actual = m_Circle.UTurnZeroIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }
        }

        #endregion

        #region Nested type: UTurnCircleCaseQ145DegreesTests

        [TestFixture]
        internal sealed class UTurnCircleCaseQ145DegreesTests
        {
            [SetUp]
            public void Setup()
            {
                m_StartPoint = new Point(4.0,
                                         2.5);
                m_StartAzimuth = Angle.FromDegrees(45.0);
                m_FinishPoint = new Point(5.0,
                                          2.0);
                m_FinishAzimuth = Angle.FromDegrees(225.0);

                var line = new Line(m_StartPoint,
                                    new Point(2.0,
                                              4.5));

                m_RadiusForPortTurn = new Distance(line.Length);
                m_RadiusForStarboardTurn = new Distance(line.Length);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                var possibleTurnCircles = new PossibleTurnCircles
                                          {
                                              Settings = m_Settings
                                          };

                var determineCirclePair = new DetermineCirclePairCalculator(possibleTurnCircles)
                                          {
                                              Settings = m_Settings
                                          };
                determineCirclePair.Calculate();

                var uTurnCircleCalculator = new UTurnCircleCalculator(determineCirclePair,
                                                                      possibleTurnCircles,
                                                                      new AngleToCentrePointCalculator());

                m_Circle = new UTurnCircle(possibleTurnCircles,
                                           uTurnCircleCalculator)
                           {
                               Settings = m_Settings
                           };

                m_Circle.Calculate();
            }

            private UTurnCircle m_Circle;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void CentrePointTest()
            {
                var expected = new Point(7.54,
                                         5.63);
                Point actual = m_Circle.CentrePoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void IsRequiredTest()
            {
                Assert.True(m_Circle.IsRequired);
            }

            [Test]
            public void TurnCirclePairTest()
            {
                Assert.NotNull(m_Circle.TurnCirclePair);
            }

            [Test]
            public void UTurnOneIntersectionPointTest()
            {
                var expected = new Point(4.77,
                                         5.07);
                Point actual = m_Circle.UTurnOneIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void UTurnZeroIntersectionPointTest()
            {
                var expected = new Point(7.27,
                                         2.82);
                Point actual = m_Circle.UTurnZeroIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }
        }

        #endregion

        #region Nested type: UTurnCircleCaseThreeTests

        [TestFixture]
        internal sealed class UTurnCircleCaseThreeTests
        {
            [SetUp]
            public void Setup()
            {
                m_StartPoint = new Point(200.0,
                                         0.0);
                m_StartAzimuth = Angle.FromDegrees(0.0);
                m_FinishPoint = new Point(200.0,
                                          120.0);
                m_FinishAzimuth = Angle.FromDegrees(180.0);

                m_RadiusForPortTurn = new Distance(30.0);
                m_RadiusForStarboardTurn = new Distance(30.0);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                var possibleTurnCircles = new PossibleTurnCircles
                                          {
                                              Settings = m_Settings
                                          };

                var determineCirclePair = new DetermineCirclePairCalculator(possibleTurnCircles)
                                          {
                                              Settings = m_Settings
                                          };
                determineCirclePair.Calculate();

                var uTurnCircleCalculator = new UTurnCircleCalculator(determineCirclePair,
                                                                      possibleTurnCircles,
                                                                      new AngleToCentrePointCalculator());

                m_Circle = new UTurnCircle(possibleTurnCircles,
                                           uTurnCircleCalculator)
                           {
                               Settings = m_Settings
                           };

                m_Circle.Calculate();
            }

            private UTurnCircle m_Circle;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void IsRequiredTest()
            {
                Assert.False(m_Circle.IsRequired);
            }
        }

        #endregion

        #region Nested type: UTurnCircleCaseTwoTests

        [TestFixture]
        internal sealed class UTurnCircleCaseTwoTests
        {
            [SetUp]
            public void Setup()
            {
                m_StartPoint = new Point(200.0,
                                         0.0);
                m_StartAzimuth = Angle.FromDegrees(0.0);
                m_FinishPoint = new Point(200.0,
                                          30.0);
                m_FinishAzimuth = Angle.FromDegrees(180.0);

                m_RadiusForPortTurn = new Distance(30.0);
                m_RadiusForStarboardTurn = new Distance(30.0);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                var possibleTurnCircles = new PossibleTurnCircles
                                          {
                                              Settings = m_Settings
                                          };

                var determineCirclePair = new DetermineCirclePairCalculator(possibleTurnCircles)
                                          {
                                              Settings = m_Settings
                                          };
                determineCirclePair.Calculate();

                var uTurnCircleCalculator = new UTurnCircleCalculator(determineCirclePair,
                                                                      possibleTurnCircles,
                                                                      new AngleToCentrePointCalculator());

                m_Circle = new UTurnCircle(possibleTurnCircles,
                                           uTurnCircleCalculator)
                           {
                               Settings = m_Settings
                           };

                m_Circle.Calculate();
            }

            private UTurnCircle m_Circle;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void CentrePointTest()
            {
                var expected = new Point(239.69,
                                         15.0);
                Point actual = m_Circle.CentrePoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void IsRequiredTest()
            {
                Assert.True(m_Circle.IsRequired);
            }

            [Test]
            public void TurnCirclePairTest()
            {
                Assert.NotNull(m_Circle.TurnCirclePair);
            }

            [Test]
            public void UTurnOneIntersectionPointTest()
            {
                var expected = new Point(219.84,
                                         -7.5);
                Point actual = m_Circle.UTurnOneIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void UTurnZeroIntersectionPointTest()
            {
                var expected = new Point(219.84,
                                         37.5);
                Point actual = m_Circle.UTurnZeroIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }
        }

        #endregion

        #region Nested type: UTurnCircleForStartAzimuth0AndFinishAzimuth180Tests

        [TestFixture]
        internal sealed class UTurnCircleForStartAzimuth0AndFinishAzimuth180Tests
        {
            [SetUp]
            public void Setup()
            {
                m_StartPoint = new Point(-50.0,
                                         0.0);
                m_StartAzimuth = Angle.FromDegrees(90.0);
                m_FinishPoint = new Point(0.0,
                                          0.0);
                m_FinishAzimuth = Angle.FromDegrees(270.0);

                m_RadiusForPortTurn = new Distance(100.0);
                m_RadiusForStarboardTurn = new Distance(100.0);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                var possibleTurnCircles = new PossibleTurnCircles
                                          {
                                              Settings = m_Settings
                                          };

                var determineCirclePair = new DetermineCirclePairCalculator(possibleTurnCircles)
                                          {
                                              Settings = m_Settings
                                          };
                determineCirclePair.Calculate();

                var uTurnCircleCalculator = new UTurnCircleCalculator(determineCirclePair,
                                                                      possibleTurnCircles,
                                                                      new AngleToCentrePointCalculator());

                m_Circle = new UTurnCircle(possibleTurnCircles,
                                           uTurnCircleCalculator)
                           {
                               Settings = m_Settings
                           };

                m_Circle.Calculate();
            }

            private UTurnCircle m_Circle;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void CentrePointTest()
            {
                var expected = new Point(-25.0,
                                         156.13);
                Point actual = m_Circle.CentrePoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void IsPossibleReturnsFalseTest()
            {
                var settings = Substitute.For <ISettings>();
                settings.StartAzimuth.Returns(Angle.FromDegrees(90.0));
                settings.FinishAzimuth.Returns(Angle.FromDegrees(90.0));

                var possibleTurnCircles = Substitute.For <IPossibleTurnCircles>();
                var uTurnCircleCalculator = Substitute.For <IUTurnCircleCalculator>();

                var circle = new UTurnCircle(possibleTurnCircles,
                                             uTurnCircleCalculator)
                             {
                                 Settings = settings
                             };

                circle.Calculate();

                Assert.False(circle.IsPossible,
                             "IsPossible");
                Assert.False(circle.IsRequired,
                             "IsRequired");
            }

            [Test]
            public void IsPossibleReturnsTrueTest()
            {
                Assert.True(m_Circle.IsPossible);
            }

            [Test]
            public void TurnCirclePairTest()
            {
                Assert.NotNull(m_Circle.TurnCirclePair);
            }

            [Test]
            public void TurnDirectionClockwiseTest()
            {
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                m_Circle.TurnDirection);
            }

            [Test]
            public void TurnDirectionCounterclockwiseTest()
            {
                var settings = new Settings(new Point(0.0,
                                                      0.0),
                                            Angle.FromDegrees(90.0),
                                            new Point(-50.0,
                                                      0.0),
                                            Angle.FromDegrees(270.0),
                                            new Distance(100.0),
                                            new Distance(100.0),
                                            true,
                                            true);

                var possibleTurnCircles = new PossibleTurnCircles
                                          {
                                              Settings = m_Settings
                                          };

                var determineCirclePair = new DetermineCirclePairCalculator(possibleTurnCircles)
                                          {
                                              Settings = m_Settings
                                          };
                determineCirclePair.Calculate();

                var uTurnCircleCalculator = new UTurnCircleCalculator(determineCirclePair,
                                                                      possibleTurnCircles,
                                                                      new AngleToCentrePointCalculator());

                var circle = new UTurnCircle(possibleTurnCircles,
                                             uTurnCircleCalculator)
                             {
                                 Settings = settings
                             };

                circle.Calculate();

                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                circle.TurnDirection);
            }

            [Test]
            public void UTurnOneIntersectionPointTest()
            {
                var expected = new Point(-87.5,
                                         78.06);
                Point actual = m_Circle.UTurnOneIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void UTurnZeroIntersectionPointTest()
            {
                var expected = new Point(37.5,
                                         78.06);
                Point actual = m_Circle.UTurnZeroIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }
        }

        #endregion

        #region Nested type: UTurnCircleForStartAzimuth180AndFinishAzimuth0Tests

        [TestFixture]
        internal sealed class UTurnCircleForStartAzimuth180AndFinishAzimuth0Tests
        {
            [SetUp]
            public void Setup()
            {
                m_StartPoint = new Point(-50.0,
                                         0.0);
                m_StartAzimuth = Angle.FromDegrees(270.0);
                m_FinishPoint = new Point(0.0,
                                          0.0);
                m_FinishAzimuth = Angle.FromDegrees(90.0);

                m_RadiusForPortTurn = new Distance(100.0);
                m_RadiusForStarboardTurn = new Distance(100.0);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                var possibleTurnCircles = new PossibleTurnCircles
                                          {
                                              Settings = m_Settings
                                          };

                var determineCirclePair = new DetermineCirclePairCalculator(possibleTurnCircles)
                                          {
                                              Settings = m_Settings
                                          };
                determineCirclePair.Calculate();

                var uTurnCircleCalculator = new UTurnCircleCalculator(determineCirclePair,
                                                                      possibleTurnCircles,
                                                                      new AngleToCentrePointCalculator());

                m_Circle = new UTurnCircle(possibleTurnCircles,
                                           uTurnCircleCalculator)
                           {
                               Settings = m_Settings
                           };

                m_Circle.Calculate();
            }

            private UTurnCircle m_Circle;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void CentrePointTest()
            {
                var expected = new Point(-25.0,
                                         -156.13);
                Point actual = m_Circle.CentrePoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void TurnCirclePairTest()
            {
                Assert.NotNull(m_Circle.TurnCirclePair);
            }

            [Test]
            public void UTurnOneIntersectionPointTest()
            {
                var expected = new Point(-87.5,
                                         -78.06);
                Point actual = m_Circle.UTurnOneIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void UTurnZeroIntersectionPointTest()
            {
                var expected = new Point(37.5,
                                         -78.06);
                Point actual = m_Circle.UTurnZeroIntersectionPoint;

                Assert.AreEqual(expected,
                                actual);
            }
        }

        #endregion
    }
}