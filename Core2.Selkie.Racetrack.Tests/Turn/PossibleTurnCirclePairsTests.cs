using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Racetrack.Turn;
using TurnCircle = Core2.Selkie.Racetrack.Turn.TurnCircle;

namespace Core2.Selkie.Racetrack.Tests.Turn
{
    
    [ExcludeFromCodeCoverage]
    internal sealed class PossibleTurnCirclePairsTests
    {
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

                Assert.NotNull(m_ToLine,
                               "m_ToLine is null!");

                m_RadiusForPortTurn = new Distance(30.0);
                m_RadiusForStarboardTurn = new Distance(30.0);

                
                m_Settings = new Settings(m_FromLine.EndPoint,
                                          m_FromLine.AngleToXAxis,
                                          m_ToLine.StartPoint,
                                          m_ToLine.AngleToXAxis,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                m_PossibleTurnCircles = new PossibleTurnCircles();

                m_Sut = new PossibleTurnCirclePairs(m_PossibleTurnCircles)
                        {
                            Settings = m_Settings
                        };

                m_Sut.Calculate();
            }

            private PossibleTurnCirclePairs m_Sut;
            private PossibleTurnCircles m_PossibleTurnCircles;
            private Settings m_Settings;
            private ILine m_ToLine;
            private Line m_FromLine;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void PossibleTurnCirclePairsCountTest()
            {
                // Arrange
                // Act
                // Assert
                Assert.AreEqual(4,
                                m_Sut.Pairs.Count());
            }

            [Test]
            public void PossibleTurnCirclePairsOneCircleOneTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 1 ].One;

                Assert.AreEqual(new Point(200.0,
                                          -30.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsOneCircleZeroTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 1 ].Zero;

                Assert.AreEqual(new Point(200,
                                          210),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsThreeCircleOneTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 3 ].One;

                Assert.AreEqual(new Point(200.0,
                                          -30.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsThreeCircleZeroTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 2 ].Zero;

                Assert.AreEqual(new Point(200,
                                          210),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsTwoCircleOneTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 2 ].One;

                Assert.AreEqual(new Point(200.0,
                                          30.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsTwoCircleZeroTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 2 ].Zero;

                Assert.AreEqual(new Point(200,
                                          210),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsZeroCircleOneTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 0 ].One;

                Assert.AreEqual(new Point(200.0,
                                          30.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsZeroCircleZeroTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 0 ].Zero;

                Assert.AreEqual(new Point(200.0,
                                          150.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
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

                Assert.NotNull(m_ToLine,
                               "m_ToLine is null!");

                m_RadiusForPortTurn = new Distance(30.0);
                m_RadiusForStarboardTurn = new Distance(30.0);

                
                m_Settings = new Settings(m_FromLine.EndPoint,
                                          m_FromLine.AngleToXAxis,
                                          m_ToLine.StartPoint,
                                          m_ToLine.AngleToXAxis,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                m_PossibleTurnCircles = new PossibleTurnCircles();

                m_Sut = new PossibleTurnCirclePairs(m_PossibleTurnCircles)
                        {
                            Settings = m_Settings
                        };

                m_Sut.Calculate();
            }

            private PossibleTurnCirclePairs m_Sut;
            private PossibleTurnCircles m_PossibleTurnCircles;
            private Settings m_Settings;
            private ILine m_ToLine;
            private Line m_FromLine;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void PossibleTurnCirclePairsCountTest()
            {
                // Arrange
                // Act
                // Assert
                Assert.AreEqual(4,
                                m_Sut.Pairs.Count());
            }

            [Test]
            public void PossibleTurnCirclePairsOneCircleOneTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 1 ].One;

                Assert.AreEqual(new Point(200.0,
                                          -30.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsOneCircleZeroTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 1 ].Zero;

                Assert.AreEqual(new Point(200,
                                          90),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsThreeCircleOneTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 3 ].One;

                Assert.AreEqual(new Point(200.0,
                                          -30.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsThreeCircleZeroTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 2 ].Zero;

                Assert.AreEqual(new Point(200,
                                          90),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsTwoCircleOneTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 2 ].One;

                Assert.AreEqual(new Point(200.0,
                                          30.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsTwoCircleZeroTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 2 ].Zero;

                Assert.AreEqual(new Point(200,
                                          90),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsZeroCircleOneTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 0 ].One;

                Assert.AreEqual(new Point(200.0,
                                          30.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsZeroCircleZeroTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 0 ].Zero;

                Assert.AreEqual(new Point(200.0,
                                          30.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(30.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
            }
        }

        [TestFixture]
        internal sealed class PossibleTurnCirclePairsGeneralTests
        {
            [SetUp]
            public void Setup()
            {
                m_Settings = Substitute.For <ISettings>();
                m_Settings.IsPortTurnAllowed.Returns(true);
                m_Settings.IsStarboardTurnAllowed.Returns(true);

                var circleStartPointPort = new Circle(-10.0,
                                                      0.0,
                                                      10);
                var circleStartPointStarboard = new Circle(10.0,
                                                           0.0,
                                                           10);
                var circleFinishPointPort = new Circle(-10.0,
                                                       30.0,
                                                       10);
                var circleFinishPointStarboard = new Circle(10.0,
                                                            30.0,
                                                            10);

                m_StartPointPort = new TurnCircle(circleStartPointPort,
                                                  Constants.CircleSide.Port,
                                                  Constants.CircleOrigin.Start,
                                                  Constants.TurnDirection.Counterclockwise);

                m_StartPointStarboard = new TurnCircle(circleStartPointStarboard,
                                                       Constants.CircleSide.Starboard,
                                                       Constants.CircleOrigin.Start,
                                                       Constants.TurnDirection.Clockwise);

                m_FinishPointPort = new TurnCircle(circleFinishPointPort,
                                                   Constants.CircleSide.Port,
                                                   Constants.CircleOrigin.Finish,
                                                   Constants.TurnDirection.Counterclockwise);

                m_FinishPointStarboard = new TurnCircle(circleFinishPointStarboard,
                                                        Constants.CircleSide.Starboard,
                                                        Constants.CircleOrigin.Finish,
                                                        Constants.TurnDirection.Clockwise);

                m_Circles = Substitute.For <IPossibleTurnCircles>();
                m_Circles.StartTurnCirclePort.Returns(m_StartPointPort);
                m_Circles.StartTurnCircleStarboard.Returns(m_StartPointStarboard);
                m_Circles.FinishTurnCirclePort.Returns(m_FinishPointPort);
                m_Circles.FinishTurnCircleStarboard.Returns(m_FinishPointStarboard);

                m_Sut = new PossibleTurnCirclePairs(m_Circles)
                        {
                            Settings = m_Settings
                        };

                m_Sut.Calculate();
            }

            private IPossibleTurnCircles m_Circles;
            private ITurnCircle m_FinishPointPort;
            private ITurnCircle m_FinishPointStarboard;
            private PossibleTurnCirclePairs m_Sut;
            private ISettings m_Settings;
            private ITurnCircle m_StartPointPort;
            private ITurnCircle m_StartPointStarboard;

            [Test]
            public void CalculateCallsCalculateOfPossibleTurnCirclesTest()
            {
                // Arrange
                // Act
                // Assert
                m_Circles.Received().Calculate();
            }

            [Test]
            public void CalculateSetsSettingForPossibleTurnCirclesTest()
            {
                // Arrange
                // Act
                // Assert
                Assert.AreEqual(m_Settings,
                                m_Circles.Settings);
            }

            [Test]
            public void CreateOnlyPortTurnsPairsCountTest()
            {
                // Arrange
                var settings = Substitute.For <ISettings>();
                settings.IsPortTurnAllowed.Returns(true);
                settings.IsStarboardTurnAllowed.Returns(false);

                // Act
                List <ITurnCirclePair> pairs = m_Sut.CreateOnlyPortTurnsPairs(m_Settings,
                                                                              m_Circles);

                // Assert
                Assert.AreEqual(1,
                                pairs.Count,
                                "Count");
            }

            [Test]
            public void CreateOnlyPortTurnsPairsTest()
            {
                // Arrange
                // Act
                List <ITurnCirclePair> onlyPortTurnsPairs = m_Sut.CreateOnlyPortTurnsPairs(m_Settings,
                                                                                           m_Circles);

                // Assert
                ITurnCirclePair pair = onlyPortTurnsPairs.First();
                ICirclePair circlePair = pair.CirclePair;

                Assert.AreEqual(m_FinishPointPort.CentrePoint,
                                circlePair.Zero.CentrePoint,
                                "One: CentrePoint");
                Assert.AreEqual(m_FinishPointPort.Radius.Length,
                                circlePair.Zero.Radius,
                                "One: Radius");

                Assert.AreEqual(m_StartPointPort.CentrePoint,
                                circlePair.One.CentrePoint,
                                "Zero: CentrePoint");
                Assert.AreEqual(m_StartPointPort.Radius.Length,
                                circlePair.One.Radius,
                                "Zero: Radius");
            }

            [Test]
            public void CreateOnlyStarboardTurnsPairsCountTest()
            {
                // Arrange
                var settings = Substitute.For <ISettings>();
                settings.IsPortTurnAllowed.Returns(true);
                settings.IsStarboardTurnAllowed.Returns(false);

                // Act
                IEnumerable <ITurnCirclePair> pairs = m_Sut.CreateOnlyStarboardTurnsPairs(m_Settings,
                                                                                          m_Circles);

                // Assert
                Assert.AreEqual(1,
                                pairs.Count(),
                                "Count");
            }

            [Test]
            public void CreateOnlyStarboardTurnsPairsTest()
            {
                // Arrange
                // Act
                IEnumerable <ITurnCirclePair> onlyStarboardTurnsPairs = m_Sut.CreateOnlyStarboardTurnsPairs(m_Settings,
                                                                                                            m_Circles);

                // Assert
                ITurnCirclePair pair = onlyStarboardTurnsPairs.First();
                ICirclePair circlePair = pair.CirclePair;

                Assert.AreEqual(m_FinishPointStarboard.CentrePoint,
                                circlePair.Zero.CentrePoint,
                                "One: CentrePoint");
                Assert.AreEqual(m_FinishPointStarboard.Radius.Length,
                                circlePair.Zero.Radius,
                                "One: Radius");

                Assert.AreEqual(m_StartPointStarboard.CentrePoint,
                                circlePair.One.CentrePoint,
                                "Zero: CentrePoint");
                Assert.AreEqual(m_StartPointStarboard.Radius.Length,
                                circlePair.One.Radius,
                                "Zero: Radius");
            }

            [Test]
            public void CreatePairsFirstPairTest()
            {
                // Arrange
                // Act
                IEnumerable <ITurnCirclePair> turnCirclePairs = m_Sut.CreateAllPairs(m_Settings,
                                                                                     m_Circles);

                // Assert
                ITurnCirclePair[] pairs = turnCirclePairs.ToArray();
                ITurnCirclePair first = pairs [ 0 ];

                ICircle circleZero = first.CirclePair.Zero;
                Assert.AreEqual(m_FinishPointPort.CentrePoint,
                                circleZero.CentrePoint,
                                "One: CentrePoint");
                Assert.AreEqual(m_FinishPointPort.Radius.Length,
                                circleZero.Radius,
                                "One: Radius");

                ICircle circleOne = first.CirclePair.One;
                Assert.AreEqual(m_StartPointPort.CentrePoint,
                                circleOne.CentrePoint,
                                "Zero: CentrePoint");
                Assert.AreEqual(m_StartPointPort.Radius.Length,
                                circleOne.Radius,
                                "Zero: Radius");
            }

            [Test]
            public void CreatePairsForAllCountTest()
            {
                // Arrange
                // Act
                IEnumerable <ITurnCirclePair> pairs = m_Sut.CreatePairs(m_Settings,
                                                                        m_Circles);

                // Assert
                Assert.AreEqual(4,
                                pairs.Count(),
                                "Count");
            }

            [Test]
            public void CreatePairsForPortOnlyCirclePairTest()
            {
                // Arrange
                var settings = Substitute.For <ISettings>();
                settings.IsPortTurnAllowed.Returns(true);
                settings.IsStarboardTurnAllowed.Returns(false);

                // Act
                IEnumerable <ITurnCirclePair> turnCirclePairs = m_Sut.CreatePairs(settings,
                                                                                  m_Circles);

                // Assert
                ITurnCirclePair pairs = turnCirclePairs.First();
                ICirclePair circlePair = pairs.CirclePair;

                Assert.AreEqual(m_FinishPointPort.CentrePoint,
                                circlePair.Zero.CentrePoint,
                                "One: CentrePoint");
                Assert.AreEqual(m_FinishPointPort.Radius.Length,
                                circlePair.Zero.Radius,
                                "One: Radius");

                Assert.AreEqual(m_StartPointPort.CentrePoint,
                                circlePair.One.CentrePoint,
                                "Zero: CentrePoint");
                Assert.AreEqual(m_StartPointPort.Radius.Length,
                                circlePair.One.Radius,
                                "Zero: Radius");
            }

            [Test]
            public void CreatePairsForPortOnlyCountTest()
            {
                // Arrange
                var settings = Substitute.For <ISettings>();
                settings.IsPortTurnAllowed.Returns(true);
                settings.IsStarboardTurnAllowed.Returns(false);

                // Act
                IEnumerable <ITurnCirclePair> pairs = m_Sut.CreatePairs(settings,
                                                                        m_Circles);

                // Assert
                Assert.AreEqual(1,
                                pairs.Count(),
                                "Count");
            }

            [Test]
            public void CreatePairsForStarboardOnlyCirclePairTest()
            {
                // Arrange
                var settings = Substitute.For <ISettings>();
                settings.IsPortTurnAllowed.Returns(false);
                settings.IsStarboardTurnAllowed.Returns(true);

                // Act
                IEnumerable <ITurnCirclePair> turnCirclePairs = m_Sut.CreatePairs(settings,
                                                                                  m_Circles);

                // Assert
                ITurnCirclePair pairs = turnCirclePairs.First();
                ICirclePair circlePair = pairs.CirclePair;

                Assert.AreEqual(m_FinishPointStarboard.CentrePoint,
                                circlePair.Zero.CentrePoint,
                                "One: CentrePoint");
                Assert.AreEqual(m_FinishPointStarboard.Radius.Length,
                                circlePair.Zero.Radius,
                                "One: Radius");

                Assert.AreEqual(m_StartPointStarboard.CentrePoint,
                                circlePair.One.CentrePoint,
                                "Zero: CentrePoint");
                Assert.AreEqual(m_StartPointStarboard.Radius.Length,
                                circlePair.One.Radius,
                                "Zero: Radius");
            }

            [Test]
            public void CreatePairsForStarboardOnlyCountTest()
            {
                // Arrange
                var settings = Substitute.For <ISettings>();
                settings.IsPortTurnAllowed.Returns(false);
                settings.IsStarboardTurnAllowed.Returns(true);

                // Act
                IEnumerable <ITurnCirclePair> pairs = m_Sut.CreatePairs(settings,
                                                                        m_Circles);

                // Assert
                Assert.AreEqual(1,
                                pairs.Count(),
                                "Count");
            }

            [Test]
            public void CreatePairsFourthPairTest()
            {
                // Arrange
                // Act
                // Assert
                IEnumerable <ITurnCirclePair> turnCirclePairs = m_Sut.CreateAllPairs(m_Settings,
                                                                                     m_Circles);
                ITurnCirclePair[] pairs = turnCirclePairs.ToArray();
                ITurnCirclePair first = pairs [ 3 ];

                ICircle circleZero = first.CirclePair.Zero;
                Assert.AreEqual(m_FinishPointPort.CentrePoint,
                                circleZero.CentrePoint,
                                "One: CentrePoint");
                Assert.AreEqual(m_FinishPointPort.Radius.Length,
                                circleZero.Radius,
                                "One: Radius");

                ICircle circleOne = first.CirclePair.One;
                Assert.AreEqual(m_StartPointStarboard.CentrePoint,
                                circleOne.CentrePoint,
                                "Zero: CentrePoint");
                Assert.AreEqual(m_StartPointStarboard.Radius.Length,
                                circleOne.Radius,
                                "Zero: Radius");
            }

            [Test]
            public void CreatePairsSecondPairTest()
            {
                // Arrange
                // Act
                // Assert
                IEnumerable <ITurnCirclePair> turnCirclePairs = m_Sut.CreateAllPairs(m_Settings,
                                                                                     m_Circles);
                ITurnCirclePair[] pairs = turnCirclePairs.ToArray();
                ITurnCirclePair first = pairs [ 1 ];

                ICircle circleZero = first.CirclePair.Zero;
                Assert.AreEqual(m_FinishPointStarboard.CentrePoint,
                                circleZero.CentrePoint,
                                "One: CentrePoint");
                Assert.AreEqual(m_FinishPointStarboard.Radius.Length,
                                circleZero.Radius,
                                "One: Radius");

                ICircle circleOne = first.CirclePair.One;
                Assert.AreEqual(m_StartPointStarboard.CentrePoint,
                                circleOne.CentrePoint,
                                "Zero: CentrePoint");
                Assert.AreEqual(m_StartPointStarboard.Radius.Length,
                                circleOne.Radius,
                                "Zero: Radius");
            }

            [Test]
            public void CreatePairsThirdPairTest()
            {
                // Arrange
                // Act
                // Assert
                IEnumerable <ITurnCirclePair> turnCirclePairs = m_Sut.CreateAllPairs(m_Settings,
                                                                                     m_Circles);
                ITurnCirclePair[] pairs = turnCirclePairs.ToArray();
                ITurnCirclePair first = pairs [ 2 ];

                ICircle circleZero = first.CirclePair.Zero;
                Assert.AreEqual(m_FinishPointStarboard.CentrePoint,
                                circleZero.CentrePoint,
                                "One: CentrePoint");
                Assert.AreEqual(m_FinishPointStarboard.Radius.Length,
                                circleZero.Radius,
                                "One: Radius");

                ICircle circleOne = first.CirclePair.One;
                Assert.AreEqual(m_StartPointPort.CentrePoint,
                                circleOne.CentrePoint,
                                "Zero: CentrePoint");
                Assert.AreEqual(m_StartPointPort.Radius.Length,
                                circleOne.Radius,
                                "Zero: Radius");
            }

            [Test]
            public void CreateTurnCirclePairPortToPortTest()
            {
                // Arrange
                // Act
                // Assert
                TurnCirclePair pair = m_Sut.CreateTurnCirclePairPortToPort(m_Settings,
                                                                           m_Circles);
                ICirclePair circlePair = pair.CirclePair;

                Assert.AreEqual(m_FinishPointPort.CentrePoint,
                                circlePair.Zero.CentrePoint,
                                "One: CentrePoint");
                Assert.AreEqual(m_FinishPointPort.Radius.Length,
                                circlePair.Zero.Radius,
                                "One: Radius");

                Assert.AreEqual(m_StartPointPort.CentrePoint,
                                circlePair.One.CentrePoint,
                                "Zero: CentrePoint");
                Assert.AreEqual(m_StartPointPort.Radius.Length,
                                circlePair.One.Radius,
                                "Zero: Radius");
            }

            [Test]
            public void CreateTurnCirclePairPortToStarboardTest()
            {
                // Arrange
                // Act
                // Assert
                TurnCirclePair pair = m_Sut.CreateTurnCirclePairPortToStarboard(m_Settings,
                                                                                m_Circles);
                ICirclePair circlePair = pair.CirclePair;

                Assert.AreEqual(m_FinishPointStarboard.CentrePoint,
                                circlePair.Zero.CentrePoint,
                                "One: CentrePoint");
                Assert.AreEqual(m_FinishPointStarboard.Radius.Length,
                                circlePair.Zero.Radius,
                                "One: Radius");

                Assert.AreEqual(m_StartPointPort.CentrePoint,
                                circlePair.One.CentrePoint,
                                "Zero: CentrePoint");
                Assert.AreEqual(m_StartPointPort.Radius.Length,
                                circlePair.One.Radius,
                                "Zero: Radius");
            }

            [Test]
            public void CreateTurnCirclePairStarboardToPortTest()
            {
                // Arrange
                // Act
                // Assert
                TurnCirclePair pair = m_Sut.CreateTurnCirclePairStarboardToPort(m_Settings,
                                                                                m_Circles);
                ICirclePair circlePair = pair.CirclePair;

                Assert.AreEqual(m_FinishPointPort.CentrePoint,
                                circlePair.Zero.CentrePoint,
                                "One: CentrePoint");
                Assert.AreEqual(m_FinishPointPort.Radius.Length,
                                circlePair.Zero.Radius,
                                "One: Radius");

                Assert.AreEqual(m_StartPointStarboard.CentrePoint,
                                circlePair.One.CentrePoint,
                                "Zero: CentrePoint");
                Assert.AreEqual(m_StartPointStarboard.Radius.Length,
                                circlePair.One.Radius,
                                "Zero: Radius");
            }

            [Test]
            public void CreateTurnCirclePairStarboardToStarboardTest()
            {
                // Arrange
                // Act
                // Assert
                TurnCirclePair pair = m_Sut.CreateTurnCirclePairStarboardToStarboard(m_Settings,
                                                                                     m_Circles);
                ICirclePair circlePair = pair.CirclePair;

                Assert.AreEqual(m_FinishPointStarboard.CentrePoint,
                                circlePair.Zero.CentrePoint,
                                "One: CentrePoint");
                Assert.AreEqual(m_FinishPointStarboard.Radius.Length,
                                circlePair.Zero.Radius,
                                "One: Radius");

                Assert.AreEqual(m_StartPointStarboard.CentrePoint,
                                circlePair.One.CentrePoint,
                                "Zero: CentrePoint");
                Assert.AreEqual(m_StartPointStarboard.Radius.Length,
                                circlePair.One.Radius,
                                "Zero: Radius");
            }

            [Test]
            public void PairsCountTest()
            {
                // Arrange
                // Act
                // Assert
                Assert.AreEqual(4,
                                m_Sut.Pairs.Count());
            }

            [Test]
            public void PairsDefaultTest()
            {
                // Arrange
                // Act
                // Assert
                var actual = new PossibleTurnCirclePairs(m_Circles);

                Assert.AreEqual(0,
                                actual.Pairs.Count());
            }

            [Test]
            public void SettingsDefaultTest()
            {
                // Arrange
                // Act
                // Assert
                var actual = new PossibleTurnCirclePairs(m_Circles);

                Assert.True(actual.Settings.IsUnknown);
            }
        }

        [TestFixture]
        internal sealed class PossibleTurnCirclePairsForPortAndStarboardWithDifferentRadiusTest
        {
            [SetUp]
            public void Setup()
            {
                m_FromLine = new Line(new Point(0.0,
                                                0.0),
                                      new Point(0.0,
                                                100.0));
                m_ToLine = new Line(new Point(1000.0,
                                              100.0),
                                    new Point(1000.0,
                                              0.0));

                Assert.NotNull(m_ToLine,
                               "m_ToLine is null!");

                m_RadiusForPortTurn = new Distance(10.0);
                m_RadiusForStarboardTurn = new Distance(100.0);

                
                m_Settings = new Settings(m_FromLine.EndPoint,
                                          m_FromLine.AngleToXAxis,
                                          m_ToLine.StartPoint,
                                          m_ToLine.AngleToXAxis,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                m_PossibleTurnCircles = new PossibleTurnCircles();

                m_Sut = new PossibleTurnCirclePairs(m_PossibleTurnCircles)
                        {
                            Settings = m_Settings
                        };

                m_Sut.Calculate();
            }

            private PossibleTurnCirclePairs m_Sut;
            private PossibleTurnCircles m_PossibleTurnCircles;
            private Settings m_Settings;
            private ILine m_ToLine;
            private Line m_FromLine;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void PossibleTurnCirclePairsCountTest()
            {
                // Arrange
                // Act
                // Assert
                Assert.AreEqual(4,
                                m_Sut.Pairs.Count());
            }

            [Test]
            public void PossibleTurnCirclePairsOneCircleOneTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 1 ].One;

                Assert.AreEqual(new Point(100.0,
                                          100.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(100.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsOneCircleZeroTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 1 ].Zero;

                Assert.AreEqual(new Point(900.0,
                                          100.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(100.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsThreeCircleOneTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 3 ].One;

                Assert.AreEqual(new Point(1010.0,
                                          100.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(10.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsThreeCircleZeroTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 2 ].Zero;

                Assert.AreEqual(new Point(900.0,
                                          100.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(100.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsTwoCircleOneTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 2 ].One;

                Assert.AreEqual(new Point(-10.0,
                                          100.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(10.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsTwoCircleZeroTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 2 ].Zero;

                Assert.AreEqual(new Point(900.0,
                                          100.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(100.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsZeroCircleOneTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 0 ].One;

                Assert.AreEqual(new Point(-10.0,
                                          100.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(10.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.Origin,
                                "Origin");
            }

            [Test]
            public void PossibleTurnCirclePairsZeroCircleZeroTest()
            {
                // Arrange
                // Act
                // Assert
                ITurnCirclePair[] pairs = m_Sut.Pairs.ToArray();
                ITurnCircle actual = pairs [ 0 ].Zero;

                Assert.AreEqual(new Point(1010.0,
                                          100.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(10.0,
                                actual.Radius.Length,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.Origin,
                                "Origin");
            }
        }
    }
}