using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Turn;

namespace Selkie.Racetrack.Tests.Turn.NUnit
{
    // ReSharper disable ClassTooBig
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

                m_Radius = new Distance(30.0);

                // ReSharper disable once PossibleNullReferenceException
                m_Settings = new Settings(m_FromLine.EndPoint,
                                          m_FromLine.AngleToXAxis,
                                          m_ToLine.StartPoint,
                                          m_ToLine.AngleToXAxis,
                                          m_Radius,
                                          true,
                                          true);

                m_PossibleTurnCircles = new PossibleTurnCircles();

                m_Pairs = new PossibleTurnCirclePairs(m_PossibleTurnCircles)
                          {
                              Settings = m_Settings
                          };
                m_Pairs.Calculate();
            }

            private PossibleTurnCirclePairs m_Pairs;
            private PossibleTurnCircles m_PossibleTurnCircles;
            private Settings m_Settings;
            private Distance m_Radius;
            private ILine m_ToLine;
            private Line m_FromLine;

            [Test]
            public void PossibleTurnCirclePairsCountTest()
            {
                Assert.AreEqual(4,
                                m_Pairs.Pairs.Count());
            }

            [Test]
            public void PossibleTurnCirclePairsOneCircleOneTest()
            {
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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

                m_Radius = new Distance(30.0);

                // ReSharper disable once PossibleNullReferenceException
                m_Settings = new Settings(m_FromLine.EndPoint,
                                          m_FromLine.AngleToXAxis,
                                          m_ToLine.StartPoint,
                                          m_ToLine.AngleToXAxis,
                                          m_Radius,
                                          true,
                                          true);

                m_PossibleTurnCircles = new PossibleTurnCircles();

                m_Pairs = new PossibleTurnCirclePairs(m_PossibleTurnCircles)
                          {
                              Settings = m_Settings
                          };
                m_Pairs.Calculate();
            }

            private PossibleTurnCirclePairs m_Pairs;
            private PossibleTurnCircles m_PossibleTurnCircles;
            private Settings m_Settings;
            private Distance m_Radius;
            private ILine m_ToLine;
            private Line m_FromLine;

            [Test]
            public void PossibleTurnCirclePairsCountTest()
            {
                Assert.AreEqual(4,
                                m_Pairs.Pairs.Count());
            }

            [Test]
            public void PossibleTurnCirclePairsOneCircleOneTest()
            {
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
                ITurnCirclePair[] pairs = m_Pairs.Pairs.ToArray();
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
            // ReSharper disable once MethodTooLong
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

                m_Pairs = new PossibleTurnCirclePairs(m_Circles)
                          {
                              Settings = m_Settings
                          };

                m_Pairs.Calculate();
            }

            private IPossibleTurnCircles m_Circles;
            private ITurnCircle m_FinishPointPort;
            private ITurnCircle m_FinishPointStarboard;
            private PossibleTurnCirclePairs m_Pairs;
            private ISettings m_Settings;
            private ITurnCircle m_StartPointPort;
            private ITurnCircle m_StartPointStarboard;

            [Test]
            public void CalculateCallsCalculateOfPossibleTurnCirclesTest()
            {
                m_Circles.Received().Calculate();
            }

            [Test]
            public void CalculateSetsSettingForPossibleTurnCirclesTest()
            {
                Assert.AreEqual(m_Settings,
                                m_Circles.Settings);
            }

            [Test]
            public void CreateOnlyPortTurnsPairsCountTest()
            {
                var settings = Substitute.For <ISettings>();
                settings.IsPortTurnAllowed.Returns(true);
                settings.IsStarboardTurnAllowed.Returns(false);

                List <ITurnCirclePair> pairs = m_Pairs.CreateOnlyPortTurnsPairs(m_Settings,
                                                                                m_Circles);

                Assert.AreEqual(1,
                                pairs.Count,
                                "Count");
            }

            [Test]
            public void CreateOnlyPortTurnsPairsTest()
            {
                List <ITurnCirclePair> onlyPortTurnsPairs = m_Pairs.CreateOnlyPortTurnsPairs(m_Settings,
                                                                                             m_Circles);
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
                var settings = Substitute.For <ISettings>();
                settings.IsPortTurnAllowed.Returns(true);
                settings.IsStarboardTurnAllowed.Returns(false);

                IEnumerable <ITurnCirclePair> pairs = m_Pairs.CreateOnlyStarboardTurnsPairs(m_Settings,
                                                                                            m_Circles);

                Assert.AreEqual(1,
                                pairs.Count(),
                                "Count");
            }

            [Test]
            public void CreateOnlyStarboardTurnsPairsTest()
            {
                IEnumerable <ITurnCirclePair> onlyStarboardTurnsPairs = m_Pairs.CreateOnlyStarboardTurnsPairs(
                                                                                                              m_Settings,
                                                                                                              m_Circles);
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
                IEnumerable <ITurnCirclePair> turnCirclePairs = m_Pairs.CreateAllPairs(m_Settings,
                                                                                       m_Circles);
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
                IEnumerable <ITurnCirclePair> pairs = m_Pairs.CreatePairs(m_Settings,
                                                                          m_Circles);

                Assert.AreEqual(4,
                                pairs.Count(),
                                "Count");
            }

            [Test]
            public void CreatePairsForPortOnlyCirclePairTest()
            {
                var settings = Substitute.For <ISettings>();
                settings.IsPortTurnAllowed.Returns(true);
                settings.IsStarboardTurnAllowed.Returns(false);

                IEnumerable <ITurnCirclePair> turnCirclePairs = m_Pairs.CreatePairs(settings,
                                                                                    m_Circles);
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
                var settings = Substitute.For <ISettings>();
                settings.IsPortTurnAllowed.Returns(true);
                settings.IsStarboardTurnAllowed.Returns(false);

                IEnumerable <ITurnCirclePair> pairs = m_Pairs.CreatePairs(settings,
                                                                          m_Circles);

                Assert.AreEqual(1,
                                pairs.Count(),
                                "Count");
            }

            [Test]
            public void CreatePairsForStarboardOnlyCirclePairTest()
            {
                var settings = Substitute.For <ISettings>();
                settings.IsPortTurnAllowed.Returns(false);
                settings.IsStarboardTurnAllowed.Returns(true);

                IEnumerable <ITurnCirclePair> turnCirclePairs = m_Pairs.CreatePairs(settings,
                                                                                    m_Circles);
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
                var settings = Substitute.For <ISettings>();
                settings.IsPortTurnAllowed.Returns(false);
                settings.IsStarboardTurnAllowed.Returns(true);

                IEnumerable <ITurnCirclePair> pairs = m_Pairs.CreatePairs(settings,
                                                                          m_Circles);

                Assert.AreEqual(1,
                                pairs.Count(),
                                "Count");
            }

            [Test]
            public void CreatePairsFourthPairTest()
            {
                IEnumerable <ITurnCirclePair> turnCirclePairs = m_Pairs.CreateAllPairs(m_Settings,
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
                IEnumerable <ITurnCirclePair> turnCirclePairs = m_Pairs.CreateAllPairs(m_Settings,
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
                IEnumerable <ITurnCirclePair> turnCirclePairs = m_Pairs.CreateAllPairs(m_Settings,
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
                TurnCirclePair pair = m_Pairs.CreateTurnCirclePairPortToPort(m_Settings,
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
                TurnCirclePair pair = m_Pairs.CreateTurnCirclePairPortToStarboard(m_Settings,
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
                TurnCirclePair pair = m_Pairs.CreateTurnCirclePairStarboardToPort(m_Settings,
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
                TurnCirclePair pair = m_Pairs.CreateTurnCirclePairStarboardToStarboard(m_Settings,
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
                Assert.AreEqual(4,
                                m_Pairs.Pairs.Count());
            }

            [Test]
            public void PairsDefaultTest()
            {
                var pairs = new PossibleTurnCirclePairs(m_Circles);

                Assert.AreEqual(0,
                                pairs.Pairs.Count());
            }

            [Test]
            public void SettingsDefaultTest()
            {
                var pairs = new PossibleTurnCirclePairs(m_Circles);

                Assert.True(pairs.Settings.IsUnknown);
            }
        }
    }
}