using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Turn;

namespace Selkie.Racetrack.Tests.Turn.NUnit
{
    // ReSharper disable ClassTooBig
    [ExcludeFromCodeCoverage]
    internal sealed class PossibleTurnCirclesTests
    {
        #region Nested type: PossibleTurnCirclesUnknownTests

        [TestFixture]
        internal class PossibleTurnCirclesUnknownTests
        {
            [SetUp]
            public void Setup()
            {
                m_Circles = PossibleTurnCircles.Unknown;
            }

            private IPossibleTurnCircles m_Circles;

            [Test]
            public void FinishTurnCirclePortDefaultTest()
            {
                Assert.True(m_Circles.FinishTurnCirclePort.IsUnknown);
            }

            [Test]
            public void FinishTurnCircleStarboardDefaultTest()
            {
                Assert.True(m_Circles.FinishTurnCircleStarboard.IsUnknown);
            }

            [Test]
            public void IsUnknownDefaultTest()
            {
                Assert.True(m_Circles.IsUnknown);
            }

            [Test]
            public void SettingsDefaultTest()
            {
                Assert.True(m_Circles.Settings.IsUnknown);
            }

            [Test]
            public void StartTurnCirclePortDefaultTest()
            {
                Assert.True(m_Circles.StartTurnCirclePort.IsUnknown);
            }

            [Test]
            public void StartTurnCircleStarboardDefaultTest()
            {
                Assert.True(m_Circles.StartTurnCircleStarboard.IsUnknown);
            }
        }

        #endregion

        #region Nested type: PossibleTurnCirclesCaseOneTests

        [TestFixture]
        internal class PossibleTurnCirclesCaseOneTests
        {
            [SetUp]
            public void Setup()
            {
                m_StartPoint = new Point(1.0,
                                         2.0);
                m_StartAzimuth = Angle.FromDegrees(0.0);

                m_FinishPoint = new Point(4.0,
                                          4.0);
                m_FinishAzimuth = Angle.FromDegrees(270.0);

                m_Radius = new Distance(10.0);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_Radius,
                                          true,
                                          true);

                m_Circles = new PossibleTurnCircles
                            {
                                Settings = m_Settings
                            };
                m_Circles.Calculate();
            }

            private PossibleTurnCircles m_Circles;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private Distance m_Radius;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;

            [Test]
            public void CalculateCentrePointForFinishPointPortTest()
            {
                var expected = new Point(14.0,
                                         4.0);
                Point actual = m_Circles.CalculateCentrePointForFinishPointPort();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointForFinishPointPort");
            }

            [Test]
            public void CalculateCentrePointForFinishPointStarboardTest()
            {
                var expected = new Point(-6.0,
                                         4.0);
                Point actual = m_Circles.CalculateCentrePointForFinishPointStarboard();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointForFinishPointStarboard");
            }

            [Test]
            public void CalculateCentrePointForStartPointPortTest()
            {
                var expected = new Point(1.0,
                                         12.0);
                Point actual = m_Circles.CalculateCentrePointForStartPointPort();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointPort");
            }

            [Test]
            public void CalculateCentrePointForStartPointStarboardTest()
            {
                var expected = new Point(1.0,
                                         -8.0);
                Point actual = m_Circles.CalculateCentrePointForStartPointStarboard();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointStarboard");
            }

            [Test]
            public void CreateTurnCircleFinishPointPortTest()
            {
                var expectedCentrePoint = new Point(14.0,
                                                    4.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleFinishPointPort();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void CreateTurnCircleFinishPointStarboardTest()
            {
                var expectedCentrePoint = new Point(-6.0,
                                                    4.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleFinishPointStarboard();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void CreateTurnCircleStartPointPortTest()
            {
                var expectedCentrePoint = new Point(1.0,
                                                    12.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleStartPointPort();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void CreateTurnCircleStartPointStarboardTest()
            {
                var expectedCentrePoint = new Point(1.0,
                                                    -8.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleStartPointStarboard();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void FinishPointPortTest()
            {
                var expectedCentrePoint = new Point(14.0,
                                                    4.0);
                ITurnCircle actual = m_Circles.FinishTurnCirclePort;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void FinishPointStarboardTest()
            {
                var expectedCentrePoint = new Point(-6.0,
                                                    4.0);
                ITurnCircle actual = m_Circles.FinishTurnCircleStarboard;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void PortAzimuthTest()
            {
                Angle actual = m_Circles.PortAzimuth(Angle.For45Degrees);

                Assert.AreEqual(Angle.For135Degrees,
                                actual);
            }

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_Circles.Settings);
            }

            [Test]
            public void StarboardAzimuthTest()
            {
                Angle actual = m_Circles.StarboardAzimuth(Angle.For45Degrees);

                Assert.AreEqual(Angle.For315Degrees,
                                actual);
            }

            [Test]
            public void StartPointPortTest()
            {
                var expectedCentrePoint = new Point(1.0,
                                                    12.0);
                ITurnCircle actual = m_Circles.StartTurnCirclePort;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void StartPointStarboardTest()
            {
                var expectedCentrePoint = new Point(1.0,
                                                    -8.0);
                ITurnCircle actual = m_Circles.StartTurnCircleStarboard;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }
        }

        #endregion

        #region Nested type: PossibleTurnCirclesCaseThreeTests

        [TestFixture]
        internal class PossibleTurnCirclesCaseThreeTests
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

                m_Radius = new Distance(30);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_Radius,
                                          true,
                                          true);

                m_Circles = new PossibleTurnCircles
                            {
                                Settings = m_Settings
                            };
                m_Circles.Calculate();
            }

            private PossibleTurnCircles m_Circles;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private Distance m_Radius;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;

            [Test]
            public void CalculateCentrePointForFinishPointPortTest()
            {
                var expected = new Point(200.0,
                                         90.0);
                Point actual = m_Circles.CalculateCentrePointForFinishPointPort();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointForFinishPointPort");
            }

            [Test]
            public void CalculateCentrePointForFinishPointStarboardTest()
            {
                var expected = new Point(200.0,
                                         150.0);
                Point actual = m_Circles.CalculateCentrePointForFinishPointStarboard();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointForFinishPointStarboard");
            }

            [Test]
            public void CalculateCentrePointForStartPointPortTest()
            {
                var expected = new Point(200.0,
                                         30.0);
                Point actual = m_Circles.CalculateCentrePointForStartPointPort();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointPort");
            }

            [Test]
            public void CalculateCentrePointForStartPointStarboardTest()
            {
                var expected = new Point(200.0,
                                         -30.0);
                Point actual = m_Circles.CalculateCentrePointForStartPointStarboard();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointStarboard");
            }

            [Test]
            public void CreateTurnCircleFinishPointPortTest()
            {
                var expectedCentrePoint = new Point(200.0,
                                                    90.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleFinishPointPort();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void CreateTurnCircleFinishPointStarboardTest()
            {
                var expectedCentrePoint = new Point(200.0,
                                                    150.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleFinishPointStarboard();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void CreateTurnCircleStartPointPortTest()
            {
                var expectedCentrePoint = new Point(200.0,
                                                    30.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleStartPointPort();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void CreateTurnCircleStartPointStarboardTest()
            {
                var expectedCentrePoint = new Point(200.0,
                                                    -30.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleStartPointStarboard();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void FinishPointPortTest()
            {
                var expectedCentrePoint = new Point(200.0,
                                                    90.0);
                ITurnCircle actual = m_Circles.FinishTurnCirclePort;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void FinishPointStarboardTest()
            {
                var expectedCentrePoint = new Point(200.0,
                                                    150.0);
                ITurnCircle actual = m_Circles.FinishTurnCircleStarboard;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_Circles.Settings);
            }

            [Test]
            public void StartPointPortTest()
            {
                var expectedCentrePoint = new Point(200.0,
                                                    30.0);
                ITurnCircle actual = m_Circles.StartTurnCirclePort;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void StartPointStarboardTest()
            {
                var expectedCentrePoint = new Point(200.0,
                                                    -30.0);
                ITurnCircle actual = m_Circles.StartTurnCircleStarboard;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }
        }

        #endregion

        #region Nested type: PossibleTurnCirclesCaseTwoTests

        [TestFixture]
        internal class PossibleTurnCirclesCaseTwoTests
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

                m_Radius = new Distance(2.5);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_Radius,
                                          true,
                                          true);

                m_Circles = new PossibleTurnCircles
                            {
                                Settings = m_Settings
                            };
                m_Circles.Calculate();
            }

            private PossibleTurnCircles m_Circles;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private Distance m_Radius;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;

            [Test]
            public void CalculateCentrePointForFinishPointPortTest()
            {
                var expected = new Point(5.0,
                                         0.0);
                Point actual = m_Circles.CalculateCentrePointForFinishPointPort();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointForFinishPointPort");
            }

            [Test]
            public void CalculateCentrePointForFinishPointStarboardTest()
            {
                var expected = new Point(10.0,
                                         0.0);
                Point actual = m_Circles.CalculateCentrePointForFinishPointStarboard();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointForFinishPointStarboard");
            }

            [Test]
            public void CalculateCentrePointForStartPointPortTest()
            {
                var expected = new Point(11.0,
                                         0.0);
                Point actual = m_Circles.CalculateCentrePointForStartPointPort();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointPort");
            }

            [Test]
            public void CalculateCentrePointForStartPointStarboardTest()
            {
                var expected = new Point(6.0,
                                         0.0);
                Point actual = m_Circles.CalculateCentrePointForStartPointStarboard();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointStarboard");
            }

            [Test]
            public void CreateTurnCircleFinishPointPortTest()
            {
                var expectedCentrePoint = new Point(5.0,
                                                    0.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleFinishPointPort();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void CreateTurnCircleFinishPointStarboardTest()
            {
                var expectedCentrePoint = new Point(10.0,
                                                    0.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleFinishPointStarboard();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void CreateTurnCircleStartPointPortTest()
            {
                var expectedCentrePoint = new Point(11.0,
                                                    0.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleStartPointPort();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void CreateTurnCircleStartPointStarboardTest()
            {
                var expectedCentrePoint = new Point(6.0,
                                                    0.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleStartPointStarboard();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void FinishPointPortTest()
            {
                var expectedCentrePoint = new Point(5.0,
                                                    0.0);
                ITurnCircle actual = m_Circles.FinishTurnCirclePort;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void FinishPointStarboardTest()
            {
                var expectedCentrePoint = new Point(10.0,
                                                    0.0);
                ITurnCircle actual = m_Circles.FinishTurnCircleStarboard;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_Circles.Settings);
            }

            [Test]
            public void StartPointPortTest()
            {
                var expectedCentrePoint = new Point(11.0,
                                                    0.0);
                ITurnCircle actual = m_Circles.StartTurnCirclePort;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void StartPointStarboardTest()
            {
                var expectedCentrePoint = new Point(6.0,
                                                    0.0);
                ITurnCircle actual = m_Circles.StartTurnCircleStarboard;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }
        }

        #endregion

        #region Nested type: PossibleTurnCirclesCaseFiveTests

        [TestFixture]
        internal class PossibleTurnCirclesCaseFiveTests
        {
            [SetUp]
            public void Setup()
            {
                //            var line1 = new Line(1, new Point(150, 610), new Point(350, 610));
                //            var line2 = new Line(0, new Point(350, 520), new Point(150, 520));

                // Line 31 [350,520 - 150,520] reverse
                // Line 28 [150,610 - 350,610] forward

                m_StartPoint = new Point(350,
                                         610);
                m_StartAzimuth = Angle.ForZeroDegrees;

                m_FinishPoint = new Point(350,
                                          520);
                m_FinishAzimuth = Angle.For180Degrees;

                m_Radius = new Distance(30.0);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_Radius,
                                          true,
                                          true);

                m_Circles = new PossibleTurnCircles
                            {
                                Settings = m_Settings
                            };
                m_Circles.Calculate();
            }

            private PossibleTurnCircles m_Circles;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private Distance m_Radius;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;

            [Test]
            public void CalculateCentrePointForFinishPointPortTest()
            {
                var expected = new Point(350.0,
                                         490.0);
                Point actual = m_Circles.CalculateCentrePointForFinishPointPort();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointForFinishPointPort");
            }

            [Test]
            public void CalculateCentrePointForFinishPointStarboardTest()
            {
                var expected = new Point(350.0,
                                         550.0);
                Point actual = m_Circles.CalculateCentrePointForFinishPointStarboard();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointForFinishPointStarboard");
            }

            [Test]
            public void CalculateCentrePointForStartPointPortTest()
            {
                var expected = new Point(350.0,
                                         640.0);
                Point actual = m_Circles.CalculateCentrePointForStartPointPort();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointPort");
            }

            [Test]
            public void CalculateCentrePointForStartPointStarboardTest()
            {
                var expected = new Point(350.0,
                                         580.0);
                Point actual = m_Circles.CalculateCentrePointForStartPointStarboard();

                Assert.AreEqual(expected,
                                actual,
                                "CalculateCentrePointStarboard");
            }

            [Test]
            public void CreateTurnCircleFinishPointPortTest()
            {
                var expectedCentrePoint = new Point(350.0,
                                                    490.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleFinishPointPort();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void CreateTurnCircleFinishPointStarboardTest()
            {
                var expectedCentrePoint = new Point(350.0,
                                                    550.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleFinishPointStarboard();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void CreateTurnCircleStartPointPortTest()
            {
                var expectedCentrePoint = new Point(350.0,
                                                    640.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleStartPointPort();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void CreateTurnCircleStartPointStarboardTest()
            {
                var expectedCentrePoint = new Point(350.0,
                                                    580.0);
                ITurnCircle actual = m_Circles.CreateTurnCircleStartPointStarboard();

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void FinishPointPortTest()
            {
                var expectedCentrePoint = new Point(350.0,
                                                    490.0);
                ITurnCircle actual = m_Circles.FinishTurnCirclePort;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void FinishPointStarboardTest()
            {
                var expectedCentrePoint = new Point(350.0,
                                                    550.0);
                ITurnCircle actual = m_Circles.FinishTurnCircleStarboard;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_Circles.Settings);
            }

            [Test]
            public void StartPointPortTest()
            {
                var expectedCentrePoint = new Point(350.0,
                                                    640.0);
                ITurnCircle actual = m_Circles.StartTurnCirclePort;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Port,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void StartPointStarboardTest()
            {
                var expectedCentrePoint = new Point(350.0,
                                                    580.0);
                ITurnCircle actual = m_Circles.StartTurnCircleStarboard;

                Assert.AreEqual(expectedCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Settings.Radius,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.CircleSide.Starboard,
                                actual.Side,
                                "Side");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }
        }

        #endregion
    }
}