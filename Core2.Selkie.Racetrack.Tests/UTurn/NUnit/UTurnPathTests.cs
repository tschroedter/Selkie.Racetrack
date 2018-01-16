using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.NUnit.Extensions;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.UTurn;
using Core2.Selkie.Racetrack.Turn;
using Core2.Selkie.Racetrack.UTurn;
using Constants = Core2.Selkie.Geometry.Constants;
using TurnCircle = Core2.Selkie.Racetrack.Turn.TurnCircle;

namespace Core2.Selkie.Racetrack.Tests.UTurn.NUnit
{
    // ReSharper disable MethodTooLong
    // ReSharper disable ClassTooBig
    [ExcludeFromCodeCoverage]
    internal sealed class UTurnPathTests
    {
        #region Nested type: UTurnPathGeneralPropertiesTests

        [TestFixture]
        internal sealed class UTurnPathGeneralPropertiesTests
        {
            [SetUp]
            public void Setup()
            {
                m_Settings = new Settings(new Point(-50.0,
                                                    0.0),
                                          Angle.For90Degrees,
                                          new Point(0.0,
                                                    0.0),
                                          Angle.For270Degrees,
                                          new Distance(100.0),
                                          new Distance(100.0),
                                          true,
                                          true);

                m_UTurnCircle = Substitute.For <IUTurnCircle>();
                m_Calculator = Substitute.For <IDetermineTurnCircleCalculator>();

                m_Sut = new UTurnPath(m_UTurnCircle,
                                      m_Calculator)
                        {
                            Settings = m_Settings
                        };
            }

            private IDetermineTurnCircleCalculator m_Calculator;
            private UTurnPath m_Sut;
            private Settings m_Settings;
            private IUTurnCircle m_UTurnCircle;

            [Test]
            public void CalculateCallsUTurnCircleCalculateTest()
            {
                m_Sut.Calculate();

                m_UTurnCircle.Received().Calculate();
            }

            [Test]
            public void CalculateSetsPathToUnknownForIsRequiredFalseTest()
            {
                m_UTurnCircle.IsRequired.Returns(false);

                m_Sut.Calculate();

                Assert.True(m_Sut.Path.IsUnknown);
            }

            [Test]
            public void CalculateSetsUTurnCircleSettingsTest()
            {
                m_Sut.Calculate();

                Assert.AreEqual(m_Settings,
                                m_UTurnCircle.Settings);
            }

            [Test]
            public void DetermineTurnCirclesCallsCalculateTest()
            {
                m_Sut.DetermineTurnCircles();

                m_Calculator.Received().Calculate();
            }

            [Test]
            public void DetermineTurnCirclesSetsSettingsTest()
            {
                m_Sut.DetermineTurnCircles();

                Assert.AreEqual(m_Settings,
                                m_Sut.Settings);
            }

            [Test]
            public void DetermineTurnCirclesSetsUTurnCircleTest()
            {
                m_Sut.DetermineTurnCircles();

                Assert.AreEqual(m_UTurnCircle,
                                m_Sut.UTurnCircle);
            }

            [Test]
            public void PathDefaultTest()
            {
                var sut = new UTurnPath(m_UTurnCircle,
                                        m_Calculator);

                Assert.True(sut.Path.IsUnknown);
            }

            [Test]
            public void SettingsRoundtripTest()
            {
                var expected = new Settings(new Point(1.0,
                                                      2.0),
                                            Angle.ForZeroDegrees,
                                            new Point(3.0,
                                                      4.0),
                                            Angle.For180Degrees,
                                            new Distance(100.0),
                                            new Distance(10.0),
                                            true,
                                            true);

                m_Sut.Settings = expected;

                ISettings actual = m_Sut.Settings;

                Assert.AreEqual(expected.StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(expected.StartAzimuth,
                                actual.StartAzimuth,
                                "StartAzimuth");
                Assert.AreEqual(expected.FinishPoint,
                                actual.FinishPoint,
                                "FinishPoint");
                Assert.AreEqual(expected.FinishAzimuth,
                                actual.FinishAzimuth,
                                "FinishAzimuth");
                Assert.AreEqual(new Distance(100.0),
                                actual.RadiusForPortTurn,
                                "RadiusForPortTurn");
                Assert.AreEqual(new Distance(10.0),
                                actual.RadiusForStarboardTurn,
                                "RadiusForStarboardTurn");
                Assert.AreEqual(expected.IsPortTurnAllowed,
                                actual.IsPortTurnAllowed,
                                "IsPortTurnAllowed");
                Assert.AreEqual(expected.IsStarboardTurnAllowed,
                                actual.IsStarboardTurnAllowed,
                                "IsStarboardTurnAllowed");
            }

            [Test]
            public void UTurnCircleDefaultTest()
            {
                Assert.AreEqual(m_UTurnCircle,
                                m_Sut.UTurnCircle);
            }
        }

        #endregion

        #region Nested type: UTurnPathGeneralTests

        [TestFixture]
        internal sealed class UTurnPathGeneralTests
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

                m_UTurnCircle = new UTurnCircle(possibleTurnCircles,
                                                uTurnCircleCalculator)
                                {
                                    Settings = m_Settings
                                };
                m_UTurnCircle.Calculate();

                m_Calculator = new DetermineTurnCircleCalculator
                               {
                                   Settings = m_Settings,
                                   UTurnCircle = m_UTurnCircle
                               };
                m_Calculator.Calculate();

                m_Path = new UTurnPath(m_UTurnCircle,
                                       m_Calculator)
                         {
                             Settings = m_Settings
                         };

                m_Path.Calculate();
            }

            private DetermineTurnCircleCalculator m_Calculator;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private UTurnPath m_Path;
            private Distance m_RadiusForPortTurn;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private IUTurnCircle m_UTurnCircle;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void CalculateFinishArcSegmentTest()
            {
                m_Path.Calculate();

                IEnumerable <IPolylineSegment> polylineSegments = m_Path.Path.Segments;
                var actual = polylineSegments.ElementAt(2) as ITurnCircleArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(100.0,
                                          0.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(37.5,
                                          78.06247497998d),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(0.0,
                                          0.0),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.CircleOrigin,
                                "CircleOrigin");
            }

            [Test]
            public void CalculateSegmentsCountTest()
            {
                m_Path.Calculate();

                IEnumerable <IPolylineSegment> actual = m_Path.Path.Segments;

                Assert.AreEqual(3,
                                actual.Count());
            }

            [Test]
            public void CalculateStartArcSegmentTest()
            {
                m_Path.Calculate();

                IEnumerable <IPolylineSegment> polylineSegments = m_Path.Path.Segments;
                var actual = polylineSegments.ElementAt(0) as ITurnCircleArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(-150.0,
                                          0.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(-50.0,
                                          0.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(-87.5,
                                          78.06247497998d),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.CircleOrigin,
                                "CircleOrigin");
            }

            [Test]
            public void CalculateUTurnArcSegmentTest()
            {
                m_Path.Calculate();

                IEnumerable <IPolylineSegment> polylineSegments = m_Path.Path.Segments;
                var actual = polylineSegments.ElementAt(1) as ITurnCircleArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(-25,
                                          156.12494995996),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(-87.5,
                                          78.06247497998),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(37.5,
                                          78.06247497998),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Unknown,
                                actual.CircleOrigin,
                                "CircleOrigin");
            }

            [Test]
            public void CreateFinishArcSegmentTest()
            {
                var circle = new Circle(2.0,
                                        3.0,
                                        4.0);

                var turnCircle = new TurnCircle(circle,
                                                Constants.CircleSide.Starboard,
                                                Constants.CircleOrigin.Start,
                                                Constants.TurnDirection.Clockwise);

                var setting = Substitute.For <ISettings>();
                var finishPoint = new Point(2.0,
                                            7.0);
                setting.FinishPoint.Returns(finishPoint);

                var intersectionPoint = new Point(6.0,
                                                  3.0);

                ITurnCircleArcSegment actual = m_Path.CreateFinishArcSegment(setting,
                                                                             intersectionPoint,
                                                                             turnCircle);

                Assert.NotNull(actual);
                Assert.AreEqual(circle.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(intersectionPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(finishPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(turnCircle.TurnDirection,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.CircleOrigin,
                                "CircleOrigin");
            }

            [Test]
            public void CreateFinishTurnCircleArcSegmentTest()
            {
                var calculator = new DetermineTurnCircleCalculator
                                 {
                                     Settings = m_Settings,
                                     UTurnCircle = m_UTurnCircle
                                 };
                calculator.Calculate();

                ITurnCircleArcSegment actual = m_Path.CreateFinishTurnCircleArcSegment(m_Settings,
                                                                                       m_UTurnCircle,
                                                                                       calculator);

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(100.0,
                                          0.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(37.5,
                                          78.06247497998d),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(0.0,
                                          0.0),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.CircleOrigin,
                                "CircleOrigin");
            }

            [Test]
            public void CreateStartArcSegmentTest()
            {
                var circle = new Circle(2.0,
                                        3.0,
                                        4.0);

                var turnCircle = new TurnCircle(circle,
                                                Constants.CircleSide.Starboard,
                                                Constants.CircleOrigin.Start,
                                                Constants.TurnDirection.Clockwise);

                var setting = Substitute.For <ISettings>();
                var startPoint = new Point(2.0,
                                           7.0);
                setting.StartPoint.Returns(startPoint);

                var intersectionPoint = new Point(6.0,
                                                  3.0);

                ITurnCircleArcSegment actual = m_Path.CreateStartArcSegment(setting,
                                                                            intersectionPoint,
                                                                            turnCircle);

                Assert.NotNull(actual);
                Assert.AreEqual(circle.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(startPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(intersectionPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(turnCircle.TurnDirection,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.CircleOrigin,
                                "CircleOrigin");
            }

            [Test]
            public void CreateStartTurnCircleArcSegmentTest()
            {
                var calculator = new DetermineTurnCircleCalculator
                                 {
                                     Settings = m_Settings,
                                     UTurnCircle = m_UTurnCircle
                                 };
                calculator.Calculate();

                ITurnCircleArcSegment actual = m_Path.CreateStartTurnCircleArcSegment(m_Settings,
                                                                                      m_UTurnCircle,
                                                                                      calculator);

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(-150.0,
                                          0.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(-50.0,
                                          0.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(-87.5,
                                          78.06247497998d),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.CircleOrigin,
                                "CircleOrigin");
            }

            [Test]
            public void CreateUTurnArcSegmentTest()
            {
                var circle = new Circle(2.0,
                                        3.0,
                                        4.0);

                var uTurnCircle = Substitute.For <IUTurnCircle>();
                uTurnCircle.Circle.Returns(circle);
                uTurnCircle.TurnDirection.Returns(Constants.TurnDirection.Counterclockwise);

                var startPoint = new Point(2.0,
                                           7.0);
                var finishPoint = new Point(6.0,
                                            3.0);

                ITurnCircleArcSegment actual = m_Path.CreateUTurnArcSegment(uTurnCircle,
                                                                            Constants.CircleOrigin.Start,
                                                                            startPoint,
                                                                            finishPoint);

                Assert.NotNull(actual);
                Assert.AreEqual(circle.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(startPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(finishPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(uTurnCircle.TurnDirection,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.CircleOrigin,
                                "CircleOrigin");
            }
        }

        #endregion

        #region Nested type: UTurnPathWithPortStarboardRadiusDifferentTests

        [TestFixture]
        internal sealed class UTurnPathWithPortStarboardRadiusDifferentTests
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
                m_RadiusForStarboardTurn = new Distance(10.0);

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

                m_UTurnCircle = new UTurnCircle(possibleTurnCircles,
                                                uTurnCircleCalculator)
                                {
                                    Settings = m_Settings
                                };
                m_UTurnCircle.Calculate();

                m_Calculator = new DetermineTurnCircleCalculator
                               {
                                   Settings = m_Settings,
                                   UTurnCircle = m_UTurnCircle
                               };
                m_Calculator.Calculate();

                m_Path = new UTurnPath(m_UTurnCircle,
                                       m_Calculator)
                         {
                             Settings = m_Settings
                         };

                m_Path.Calculate();
            }

            private DetermineTurnCircleCalculator m_Calculator;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private UTurnPath m_Path;
            private Distance m_RadiusForPortTurn;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private IUTurnCircle m_UTurnCircle;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void CalculateFinishArcSegmentTest()
            {
                m_Path.Calculate();

                IEnumerable <IPolylineSegment> polylineSegments = m_Path.Path.Segments;
                var actual = polylineSegments.ElementAt(2) as ITurnCircleArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(100.0,
                                          0.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(37.5,
                                          78.06247497998d),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(0.0,
                                          0.0),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.CircleOrigin,
                                "CircleOrigin");
            }

            [Test]
            public void CalculateSegmentsCountTest()
            {
                m_Path.Calculate();

                IEnumerable <IPolylineSegment> actual = m_Path.Path.Segments;

                Assert.AreEqual(3,
                                actual.Count());
            }

            [Test]
            public void CalculateStartArcSegmentTest()
            {
                m_Path.Calculate();

                IEnumerable <IPolylineSegment> polylineSegments = m_Path.Path.Segments;
                var actual = polylineSegments.ElementAt(0) as ITurnCircleArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(-150.0,
                                          0.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(-50.0,
                                          0.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(-87.5,
                                          78.06247497998d),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.CircleOrigin,
                                "CircleOrigin");
                NUnitHelper.AssertIsEquivalent(m_RadiusForPortTurn.Length,
                                               actual.Radius,
                                               "Radius");
            }

            [Test]
            public void CalculateUTurnMiddleArcSegmentTest()
            {
                m_Path.Calculate();

                IEnumerable <IPolylineSegment> polylineSegments = m_Path.Path.Segments;
                var actual = polylineSegments.ElementAt(1) as ITurnCircleArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(-25,
                                          156.12494995996),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(-87.5,
                                          78.06247497998),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(37.5,
                                          78.06247497998),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Unknown,
                                actual.CircleOrigin,
                                "CircleOrigin");
                NUnitHelper.AssertIsEquivalent(m_RadiusForPortTurn.Length,
                                               actual.Radius,
                                               "Radius");
            }

            [Test]
            public void CreateFinishArcSegmentTest()
            {
                var circle = new Circle(2.0,
                                        3.0,
                                        4.0);

                var turnCircle = new TurnCircle(circle,
                                                Constants.CircleSide.Starboard,
                                                Constants.CircleOrigin.Start,
                                                Constants.TurnDirection.Clockwise);

                var setting = Substitute.For <ISettings>();
                var finishPoint = new Point(2.0,
                                            7.0);
                setting.FinishPoint.Returns(finishPoint);

                var intersectionPoint = new Point(6.0,
                                                  3.0);

                ITurnCircleArcSegment actual = m_Path.CreateFinishArcSegment(setting,
                                                                             intersectionPoint,
                                                                             turnCircle);

                Assert.NotNull(actual);
                Assert.AreEqual(circle.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(intersectionPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(finishPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(turnCircle.TurnDirection,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.CircleOrigin,
                                "CircleOrigin");
                NUnitHelper.AssertIsEquivalent(circle.Radius,
                                               actual.Radius,
                                               "Radius");
            }

            [Test]
            public void CreateFinishTurnCircleArcSegmentTest()
            {
                var calculator = new DetermineTurnCircleCalculator
                                 {
                                     Settings = m_Settings,
                                     UTurnCircle = m_UTurnCircle
                                 };
                calculator.Calculate();

                ITurnCircleArcSegment actual = m_Path.CreateFinishTurnCircleArcSegment(m_Settings,
                                                                                       m_UTurnCircle,
                                                                                       calculator);

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(100.0,
                                          0.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(37.5,
                                          78.06247497998d),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(0.0,
                                          0.0),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.CircleOrigin,
                                "CircleOrigin");
                NUnitHelper.AssertIsEquivalent(m_RadiusForPortTurn.Length,
                                               actual.Radius,
                                               "Radius");
            }

            [Test]
            public void CreateStartArcSegmentTest()
            {
                var circle = new Circle(2.0,
                                        3.0,
                                        4.0);

                var turnCircle = new TurnCircle(circle,
                                                Constants.CircleSide.Starboard,
                                                Constants.CircleOrigin.Start,
                                                Constants.TurnDirection.Clockwise);

                var setting = Substitute.For <ISettings>();
                var startPoint = new Point(2.0,
                                           7.0);
                setting.StartPoint.Returns(startPoint);

                var intersectionPoint = new Point(6.0,
                                                  3.0);

                ITurnCircleArcSegment actual = m_Path.CreateStartArcSegment(setting,
                                                                            intersectionPoint,
                                                                            turnCircle);

                Assert.NotNull(actual);
                Assert.AreEqual(circle.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(startPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(intersectionPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(turnCircle.TurnDirection,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.CircleOrigin,
                                "CircleOrigin");
                NUnitHelper.AssertIsEquivalent(circle.Radius,
                                               actual.Radius,
                                               "Radius");
            }

            [Test]
            public void CreateStartTurnCircleArcSegmentTest()
            {
                var calculator = new DetermineTurnCircleCalculator
                                 {
                                     Settings = m_Settings,
                                     UTurnCircle = m_UTurnCircle
                                 };
                calculator.Calculate();

                ITurnCircleArcSegment actual = m_Path.CreateStartTurnCircleArcSegment(m_Settings,
                                                                                      m_UTurnCircle,
                                                                                      calculator);

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(-150.0,
                                          0.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(-50.0,
                                          0.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(-87.5,
                                          78.06247497998d),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.CircleOrigin,
                                "CircleOrigin");
                NUnitHelper.AssertIsEquivalent(m_RadiusForPortTurn.Length,
                                               actual.Radius,
                                               "Radius");
            }

            [Test]
            public void CreateUTurnArcSegmentTest()
            {
                var circle = new Circle(2.0,
                                        3.0,
                                        4.0);

                var uTurnCircle = Substitute.For <IUTurnCircle>();
                uTurnCircle.Circle.Returns(circle);
                uTurnCircle.TurnDirection.Returns(Constants.TurnDirection.Counterclockwise);

                var startPoint = new Point(2.0,
                                           7.0);
                var finishPoint = new Point(6.0,
                                            3.0);

                ITurnCircleArcSegment actual = m_Path.CreateUTurnArcSegment(uTurnCircle,
                                                                            Constants.CircleOrigin.Start,
                                                                            startPoint,
                                                                            finishPoint);

                Assert.NotNull(actual);
                Assert.AreEqual(circle.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(startPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(finishPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(uTurnCircle.TurnDirection,
                                actual.TurnDirection,
                                "TurnDirection");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.CircleOrigin,
                                "CircleOrigin");
                NUnitHelper.AssertIsEquivalent(circle.Radius,
                                               actual.Radius,
                                               "Radius");
            }
        }

        #endregion

        #region Nested type: UTurnPathPortToPortStartAzimuth0FinishAzimuth180Tests

        [TestFixture]
        internal sealed class UTurnPathPortToPortStartAzimuth0FinishAzimuth180Tests
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
                m_RadiusForStarboardTurn = new Distance(50.0);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                m_CentrePoint = new Point(-25.0,
                                          156.12);
                m_UTurnZeroIntersectionPoint = new Point(-87.5,
                                                         78.06);
                m_UTurnOneIntersectionPoint = new Point(37.5,
                                                        78.06);

                m_ZeroCentrePoint = new Point(-150.0,
                                              0.0);
                m_OneCentrePoint = new Point(100.0,
                                             0.0);

                m_Circle = new Circle(m_CentrePoint,
                                      100.0);

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

                m_UTurnCircle = new UTurnCircle(possibleTurnCircles,
                                                uTurnCircleCalculator)
                                {
                                    Settings = m_Settings
                                };
                m_UTurnCircle.Calculate();

                m_Calculator = new DetermineTurnCircleCalculator();

                m_Path = new UTurnPath(m_UTurnCircle,
                                       m_Calculator)
                         {
                             Settings = m_Settings
                         };

                m_Path.Calculate();
            }

            private DetermineTurnCircleCalculator m_Calculator;
            private Point m_CentrePoint;
            private ICircle m_Circle;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private Point m_OneCentrePoint;
            private UTurnPath m_Path;
            private ISettings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private IUTurnCircle m_UTurnCircle;
            private Point m_UTurnOneIntersectionPoint;
            private Point m_UTurnZeroIntersectionPoint;
            private Point m_ZeroCentrePoint;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void PathCountTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Path.Path.Segments;

                Assert.AreEqual(3,
                                polylineSegments.Count());
            }

            [Test]
            public void PathFinishArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Path.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 2 ] as IArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(m_OneCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_UTurnOneIntersectionPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_FinishPoint,
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathLengthTest()
            {
                const double expected = 672.43;
                Distance distance = m_Path.Path.Distance;
                double actual = distance.Length;

                NUnitHelper.AssertIsEquivalent(expected,
                                               actual,
                                               0.01,
                                               "Length");
            }

            [Test]
            public void PathMiddleArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Path.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 1 ] as IArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(m_Circle.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_UTurnZeroIntersectionPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_UTurnOneIntersectionPoint,
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathStartArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Path.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 0 ] as IArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(m_ZeroCentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_UTurnZeroIntersectionPoint,
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathUnknownForIsRequiredIsFalseTest()
            {
                var uTurnCircle = Substitute.For <IUTurnCircle>();
                uTurnCircle.IsRequired.Returns(false);

                var calculator = Substitute.For <IDetermineTurnCircleCalculator>();

                var uTurnPath = new UTurnPath(uTurnCircle,
                                              calculator)
                                {
                                    Settings = m_Settings
                                };

                m_Path.Calculate();

                Assert.AreEqual(Path.Unknown,
                                uTurnPath.Path);
            }

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_Path.Settings);
            }

            [Test]
            public void UTrunCircleSettingsDefaultTest()
            {
                var sut = new UTurnPath(m_UTurnCircle,
                                        m_Calculator);

                Assert.AreEqual(Settings.Unknown,
                                sut.UTrunCircleSettings);
            }

            [Test]
            public void UTrunCircleSettingsTest()
            {
                Distance expectedRadiusForBothTurns = m_RadiusForPortTurn;

                ISettings actual = m_Path.UTrunCircleSettings;

                Assert.AreEqual(m_StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_FinishPoint,
                                actual.FinishPoint,
                                "FinishPoint");
                Assert.AreEqual(m_StartAzimuth,
                                actual.StartAzimuth,
                                "StartAzimuth");
                Assert.AreEqual(m_FinishAzimuth,
                                actual.FinishAzimuth,
                                "FinishAzimuth");
                Assert.AreEqual(expectedRadiusForBothTurns,
                                actual.RadiusForPortTurn,
                                "Radius");
                Assert.AreEqual(expectedRadiusForBothTurns,
                                actual.RadiusForStarboardTurn,
                                "Radius");
                Assert.True(actual.IsPortTurnAllowed,
                            "IsPortTurnAllowed");
                Assert.True(actual.IsStarboardTurnAllowed,
                            "IsStarboardTurnAllowed");
                Assert.AreEqual(m_RadiusForPortTurn,
                                actual.LargestRadiusForTurn,
                                "LargestRadiusForTurn");
            }

            [Test]
            public void UTurnCircleTest()
            {
                Assert.AreEqual(m_UTurnCircle,
                                m_Path.UTurnCircle);
            }
        }

        #endregion

        [TestFixture]
        internal sealed class CaseOneTest
        {
            [SetUp]
            public void Setup()
            {
                m_FromLine = new Line(new Point(100.0,
                                                0.0),
                                      new Point(200.0,
                                                0.0));
                m_ToLine = new Line(new Point(100.0,
                                              180.0),
                                    new Point(200.0,
                                              180.0));
                m_ToLine = m_ToLine.Reverse() as ILine;
                m_RadiusForPortTurn = new Distance(100.0);
                m_RadiusForStarboardTurn = new Distance(100.0);

                // ReSharper disable once PossibleNullReferenceException
                m_Settings = new Settings(m_FromLine.EndPoint,
                                          m_FromLine.AngleToXAxis,
                                          m_ToLine.StartPoint,
                                          m_ToLine.AngleToXAxis,
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

                m_UTurnCircle = new UTurnCircle(possibleTurnCircles,
                                                uTurnCircleCalculator)
                                {
                                    Settings = m_Settings
                                };
                m_UTurnCircle.Calculate();

                m_Calculator = new DetermineTurnCircleCalculator();

                m_UTurnPath = new UTurnPath(m_UTurnCircle,
                                            m_Calculator)
                              {
                                  Settings = m_Settings
                              };
                m_UTurnPath.Calculate();
            }

            private ISettings m_Settings;
            private ILine m_FromLine;
            private ILine m_ToLine;
            private UTurnPath m_UTurnPath;
            private UTurnCircle m_UTurnCircle;
            private DetermineTurnCircleCalculator m_Calculator;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_UTurnPath.Settings);
            }

            [Test]
            public void UTurnPathTest()
            {
                Assert.False(m_UTurnPath.IsRequired);
            }
        }

        [TestFixture]
        internal sealed class CaseTwoTimesRadiusDistanceTest
        {
            [SetUp]
            public void Setup()
            {
                m_FromLine = new Line(new Point(100.0,
                                                0.0),
                                      new Point(200.0,
                                                0.0));
                m_ToLine = new Line(new Point(100.0,
                                              60.0),
                                    new Point(200.0,
                                              60.0));
                m_ToLine = m_ToLine.Reverse() as ILine;

                m_RadiusForPortTurn = new Distance(30.0);
                m_RadiusForStarboardTurn = new Distance(30.0);

                // ReSharper disable once PossibleNullReferenceException
                m_Settings = new Settings(m_FromLine.EndPoint,
                                          m_FromLine.AngleToXAxis,
                                          m_ToLine.StartPoint,
                                          m_ToLine.AngleToXAxis,
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

                m_UTurnCircle = new UTurnCircle(possibleTurnCircles,
                                                uTurnCircleCalculator)
                                {
                                    Settings = m_Settings
                                };
                m_UTurnCircle.Calculate();

                m_Calculator = new DetermineTurnCircleCalculator();

                m_UTurnPath = new UTurnPath(m_UTurnCircle,
                                            m_Calculator)
                              {
                                  Settings = m_Settings
                              };
                m_UTurnPath.Calculate();
            }

            private ISettings m_Settings;
            private ILine m_FromLine;
            private ILine m_ToLine;
            private UTurnPath m_UTurnPath;
            private UTurnCircle m_UTurnCircle;
            private DetermineTurnCircleCalculator m_Calculator;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_UTurnPath.Settings);
            }

            [Test]
            public void UTurnPathTest()
            {
                Assert.False(m_UTurnPath.IsRequired);
            }
        }

        [TestFixture]
        internal sealed class ConstructorForUTurnIsRequiredIsFalseCaseTwoTest
        {
            [SetUp]
            public void Setup()
            {
                m_FromLine = new Line(new Point(8.5,
                                                2.5),
                                      new Point(8.5,
                                                0.0));
                m_ToLine = new Line(new Point(7.5,
                                              2.5),
                                    new Point(7.5,
                                              0.0));

                m_RadiusForPortTurn = new Distance(2.5);
                m_RadiusForStarboardTurn = new Distance(2.5);

                m_Settings = new Settings(m_FromLine.EndPoint,
                                          m_FromLine.AngleToXAxis,
                                          m_ToLine.StartPoint,
                                          m_ToLine.AngleToXAxis,
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

                m_UTurnCircle = new UTurnCircle(possibleTurnCircles,
                                                uTurnCircleCalculator)
                                {
                                    Settings = m_Settings
                                };

                m_UTurnCircle.Calculate();

                m_Calculator = new DetermineTurnCircleCalculator();

                m_UTurnPath = new UTurnPath(m_UTurnCircle,
                                            m_Calculator)
                              {
                                  Settings = m_Settings
                              };

                m_UTurnPath.Calculate();
            }

            private DetermineTurnCircleCalculator m_Calculator;
            private Line m_FromLine;
            private Settings m_Settings;
            private Line m_ToLine;
            private UTurnCircle m_UTurnCircle;
            private UTurnPath m_UTurnPath;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_UTurnPath.Settings);
            }

            [Test]
            public void UTurnPathIsRequiredTest()
            {
                Assert.False(m_UTurnPath.IsRequired);
            }
        }
    }
}