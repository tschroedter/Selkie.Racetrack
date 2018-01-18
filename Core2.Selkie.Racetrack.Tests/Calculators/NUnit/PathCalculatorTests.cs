using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.NUnit.Extensions;
using Core2.Selkie.Racetrack.Calculators;
using Core2.Selkie.Racetrack.Converters;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Racetrack.Interfaces.Converters;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Racetrack.Interfaces.UTurn;
using Core2.Selkie.Racetrack.Turn;
using Core2.Selkie.Racetrack.UTurn;
using Core2.Selkie.Windsor.Interfaces;
using Constants = Core2.Selkie.Geometry.Constants;
using ITurnCircleArcSegment = Core2.Selkie.Geometry.Shapes.ITurnCircleArcSegment;

namespace Core2.Selkie.Racetrack.Tests.Calculators.NUnit
{
    [ExcludeFromCodeCoverage]
    internal sealed class PathCalculatorTests
    {
        #region Nested type: CaseOneTest

        [TestFixture]
        internal sealed class CaseOneTest
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

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
                m_PossibleTurnCirclePairs = new PossibleTurnCirclePairs(m_PossibleTurnCircles);
                m_PathValidator = new PathValidator(m_Logger);
                m_Converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                m_PathValidator);

                m_TurnCirclePairsToPathsConverter = new TurnCirclePairsToPathsConverter(m_Converter,
                                                                                        m_PossibleTurnCirclePairs);

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

                m_DetermineTurnCircleCalculator = new DetermineTurnCircleCalculator();

                m_UTurnPath = new UTurnPath(m_UTurnCircle,
                                            m_DetermineTurnCircleCalculator);

                m_PathShortestFinder = new PathShortestFinder();

                m_Selector = new PathSelectorCalculator(m_TurnCirclePairsToPathsConverter,
                                                        m_UTurnPath);

                m_Calculator = new PathCalculator(m_Selector,
                                                  m_PathShortestFinder)
                               {
                                   Settings = m_Settings
                               };

                m_Calculator.Calculate();
            }

            private IPathCalculator m_Calculator;
            private TurnCirclePairToPathConverter m_Converter;
            private DetermineTurnCircleCalculator m_DetermineTurnCircleCalculator;
            private ILine m_FromLine;
            private ISelkieLogger m_Logger;
            private PathShortestFinder m_PathShortestFinder;
            private PathValidator m_PathValidator;
            private IPossibleTurnCirclePairs m_PossibleTurnCirclePairs;
            private IPossibleTurnCircles m_PossibleTurnCircles;
            private Distance m_RadiusForPortTurn;
            private IPathSelectorCalculator m_Selector;
            private ISettings m_Settings;
            private ILine m_ToLine;
            private TurnCirclePairsToPathsConverter m_TurnCirclePairsToPathsConverter;
            private UTurnCircle m_UTurnCircle;
            private UTurnPath m_UTurnPath;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void PathFirstSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 0 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(new Point(200.0,
                                          30.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(200.0,
                                          0.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(230.0,
                                          30.0),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(30.0,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.CircleOrigin,
                                "Origin");
            }

            [Test]
            public void PathLengthTest()
            {
                Assert.AreEqual(new Distance(214.25),
                                m_Calculator.Path.Distance);
            }

            [Test]
            public void PathSecondSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 1 ] as ILine;

                Assert.NotNull(actual);

                Assert.AreEqual(new Point(230.0,
                                          30.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(230.0,
                                          150.0),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathSegmentCountTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;

                Assert.AreEqual(3,
                                polylineSegments.Count());
            }

            [Test]
            public void PathThirdSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 2 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(new Point(200.0,
                                          150.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(230.0,
                                          150.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(200.0,
                                          180.0),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(30.0,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.CircleOrigin,
                                "Origin");
            }

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_Calculator.Settings);
            }
        }

        #endregion

        #region Nested type: CaseTwoTimesRadiusDistanceTest

        [TestFixture]
        internal sealed class CaseTwoTimesRadiusDistanceTest
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

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
                m_PossibleTurnCirclePairs = new PossibleTurnCirclePairs(m_PossibleTurnCircles);
                m_PathValidator = new PathValidator(m_Logger);
                m_Converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                m_PathValidator);

                m_TurnCirclePairsToPathsConverter = new TurnCirclePairsToPathsConverter(m_Converter,
                                                                                        m_PossibleTurnCirclePairs);

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

                m_DetermineTurnCircleCalculator = new DetermineTurnCircleCalculator();

                m_UTurnPath = new UTurnPath(m_UTurnCircle,
                                            m_DetermineTurnCircleCalculator);

                m_PathShortestFinder = new PathShortestFinder();

                m_Selector = new PathSelectorCalculator(m_TurnCirclePairsToPathsConverter,
                                                        m_UTurnPath);

                m_Calculator = new PathCalculator(m_Selector,
                                                  m_PathShortestFinder)
                               {
                                   Settings = m_Settings
                               };


                m_Calculator.Calculate();
            }

            private IPathCalculator m_Calculator;
            private TurnCirclePairToPathConverter m_Converter;
            private DetermineTurnCircleCalculator m_DetermineTurnCircleCalculator;
            private ILine m_FromLine;
            private ISelkieLogger m_Logger;
            private PathShortestFinder m_PathShortestFinder;
            private PathValidator m_PathValidator;
            private IPossibleTurnCirclePairs m_PossibleTurnCirclePairs;
            private IPossibleTurnCircles m_PossibleTurnCircles;
            private PathSelectorCalculator m_Selector;
            private ISettings m_Settings;
            private ILine m_ToLine;
            private TurnCirclePairsToPathsConverter m_TurnCirclePairsToPathsConverter;
            private UTurnCircle m_UTurnCircle;
            private UTurnPath m_UTurnPath;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void PathFirstSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 0 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(new Point(200.0,
                                          30.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(200.0,
                                          0.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(230.0,
                                          30.0),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(30.0,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.CircleOrigin,
                                "Origin");
            }

            [Test]
            public void PathLengthTest()
            {
                IPath actual = m_Calculator.Path;

                NUnitHelper.AssertIsEquivalent(94.247779607693801d,
                                               actual.Distance.Length,
                                               0.01,
                                               "Length");
            }

            [Test]
            public void PathSecondSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 1 ] as ILine;

                Assert.NotNull(actual);

                Assert.AreEqual(new Point(230.0,
                                          30.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(230.0,
                                          30.0),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathSegmentCountTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;

                Assert.AreEqual(3,
                                polylineSegments.Count());
            }

            [Test]
            public void PathThirdSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 2 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(new Point(200.0,
                                          30.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(230.0,
                                          30.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(200.0,
                                          60.0),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(30.0,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.CircleOrigin,
                                "Origin");
            }

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_Calculator.Settings);
            }
        }

        #endregion

        #region Nested type: ConstructorForUTurnIsRequiredIsFalseCaseTwoTest

        [TestFixture]
        internal sealed class ConstructorForUTurnIsRequiredIsFalseCaseTwoTest
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

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

                m_PossibleTurnCircles = new PossibleTurnCircles();
                m_PossibleTurnCirclePairs = new PossibleTurnCirclePairs(m_PossibleTurnCircles);
                m_PathValidator = new PathValidator(m_Logger);
                m_Converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                m_PathValidator);

                m_TurnCirclePairsToPathsConverter = new TurnCirclePairsToPathsConverter(m_Converter,
                                                                                        m_PossibleTurnCirclePairs);

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

                m_DetermineTurnCircleCalculator = new DetermineTurnCircleCalculator();

                m_UTurnPath = new UTurnPath(m_UTurnCircle,
                                            m_DetermineTurnCircleCalculator);

                m_PathShortestFinder = new PathShortestFinder();

                m_Selector = new PathSelectorCalculator(m_TurnCirclePairsToPathsConverter,
                                                        m_UTurnPath);

                m_Calculator = new PathCalculator(m_Selector,
                                                  m_PathShortestFinder)
                               {
                                   Settings = m_Settings
                               };


                m_Calculator.Calculate();
            }

            private PathCalculator m_Calculator;
            private TurnCirclePairToPathConverter m_Converter;
            private DetermineTurnCircleCalculator m_DetermineTurnCircleCalculator;
            private Line m_FromLine;
            private ISelkieLogger m_Logger;
            private PathShortestFinder m_PathShortestFinder;
            private PathValidator m_PathValidator;
            private IPossibleTurnCirclePairs m_PossibleTurnCirclePairs;
            private IPossibleTurnCircles m_PossibleTurnCircles;
            private PathSelectorCalculator m_Selector;
            private Settings m_Settings;
            private Line m_ToLine;
            private TurnCirclePairsToPathsConverter m_TurnCirclePairsToPathsConverter;
            private UTurnCircle m_UTurnCircle;
            private UTurnPath m_UTurnPath;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void PathCountTest()
            {
                IPath actual = m_Calculator.Path;

                Assert.AreEqual(3,
                                actual.Segments.Count(),
                                "Count");
            }

            [Test]
            public void PathEndSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                IPolylineSegment actual = segments [ 2 ];

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(2.67880827278685,
                                          1.57152330911474),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_ToLine.StartPoint,
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathFirstSegementTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                IPolylineSegment actual = segments [ 0 ];

                Assert.NotNull(actual);
                Assert.AreEqual(m_FromLine.EndPoint,
                                actual.StartPoint);
                Assert.AreEqual(new Point(3.67880827278685,
                                          -0.928476690885259),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathLengthTest()
            {
                const double expected = 18.400545671516213d;
                Distance distance = m_Calculator.Path.Distance;
                double actual = distance.Length;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void PathUTurnSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                IPolylineSegment actual = segments [ 1 ];

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(3.67880827278685,
                                          -0.928476690885259),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(2.67880827278685,
                                          1.57152330911474),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_Calculator.Settings);
            }
        }

        #endregion

        #region Nested type: ConstructorForUTurnIsRequiredIsFalseTest

        [TestFixture]
        internal sealed class ConstructorForUTurnIsRequiredIsFalseTest
        {
            [SetUp]
            public void Setup()
            {
                Substitute.For <ISelkieLogger>();

                m_StartPoint = new Point(1.0,
                                         2.0);
                m_StartAzimuth = Angle.FromDegrees(90.0);

                m_FinishPoint = new Point(4.0,
                                          4.0);
                m_FinishAzimuth = Angle.FromDegrees(180.0);

                m_RadiusForPortTurn = new Distance(10.0);
                m_RadiusForStarboardTurn = new Distance(10.0);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                m_TurnCirclePairsToPathsConverter = Substitute.For <ITurnCirclePairsToPathsConverter>();

                var segments = new List <IPolylineSegment>();
                var startSegment = Substitute.For <IPolylineSegment>();
                startSegment.StartPoint.Returns(new Point(1.0,
                                                          2.0));
                startSegment.EndPoint.Returns(new Point(3.0,
                                                        4.0));

                var endSegment = Substitute.For <IPolylineSegment>();
                segments.Add(startSegment);
                segments.Add(Substitute.For <ILine>());
                segments.Add(endSegment);

                var path = Substitute.For <IPath>();
                path.Segments.Returns(segments);
                path.Distance.Returns(new Distance(10.0));
                m_Paths = new List <IPath>
                          {
                              path
                          };
                m_TurnCirclePairsToPathsConverter.Paths.Returns(m_Paths);

                m_UTurnPath = Substitute.For <IUTurnPath>();
                m_UTurnPath.IsRequired.Returns(false);

                m_TurnCirclePairsToPathsConverter = Substitute.For <ITurnCirclePairsToPathsConverter>();
                m_TurnCirclePairsToPathsConverter.Paths.Returns(m_Paths);

                m_PathShortestFinder = new PathShortestFinder();

                m_Selector = new PathSelectorCalculator(m_TurnCirclePairsToPathsConverter,
                                                        m_UTurnPath);

                m_Calculator = new PathCalculator(m_Selector,
                                                  m_PathShortestFinder)
                               {
                                   Settings = m_Settings
                               };
            }

            private PathCalculator m_Calculator;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private IEnumerable <IPath> m_Paths;
            private PathShortestFinder m_PathShortestFinder;
            private PathSelectorCalculator m_Selector;
            private Settings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private ITurnCirclePairsToPathsConverter m_TurnCirclePairsToPathsConverter;
            private IUTurnPath m_UTurnPath;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void PathShortestFinderTest()
            {
                Assert.NotNull(m_Calculator.PathShortestFinder);
            }

            [Test]
            public void PathsTest()
            {
                m_Calculator.Calculate();

                Assert.AreEqual(m_Paths,
                                m_Calculator.Paths);
            }

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_Calculator.Settings);
            }
        }

        #endregion

        #region Nested type: ConstructorForUTurnIsRequiredIsTrueTest

        [TestFixture]
        internal sealed class ConstructorForUTurnIsRequiredIsTrueTest
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

                m_StartPoint = new Point(1.0,
                                         2.0);
                m_StartAzimuth = Angle.FromDegrees(90.0);

                m_FinishPoint = new Point(4.0,
                                          4.0);
                m_FinishAzimuth = Angle.FromDegrees(180.0);

                m_RadiusForPortTurn = new Distance(10.0);
                m_RadiusForStarboardTurn = new Distance(10.0);

                m_Settings = new Settings(m_StartPoint,
                                          m_StartAzimuth,
                                          m_FinishPoint,
                                          m_FinishAzimuth,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                m_PossibleTurnCirclePairs = Substitute.For <IPossibleTurnCirclePairs>();
                m_TurnCirclePairsToPathsConverter = Substitute.For <ITurnCirclePairsToPathsConverter>();
                m_Path = Substitute.For <IPath>();
                m_Paths = new List <IPath>
                          {
                              m_Path
                          };
                m_TurnCirclePairsToPathsConverter.Paths.Returns(m_Paths);

                m_UTurnPath = Substitute.For <IUTurnPath>();
                m_UTurnPath.IsRequired.Returns(true);
                m_PathForUTurnPath = Substitute.For <IPath>();
                m_PathForUTurnPath.Distance.Returns(new Distance(100.0));
                m_UTurnPath.Path.Returns(m_PathForUTurnPath);

                m_PathValidator = new PathValidator(m_Logger);
                m_Converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                m_PathValidator);

                m_TurnCirclePairsToPathsConverter = new TurnCirclePairsToPathsConverter(m_Converter,
                                                                                        m_PossibleTurnCirclePairs);

                m_PathShortestFinder = new PathShortestFinder();

                m_Selector = new PathSelectorCalculator(m_TurnCirclePairsToPathsConverter,
                                                        m_UTurnPath);

                m_Calculator = new PathCalculator(m_Selector,
                                                  m_PathShortestFinder)
                               {
                                   Settings = m_Settings
                               };

                m_Calculator.Calculate();
            }

            private PathCalculator m_Calculator;
            private TurnCirclePairToPathConverter m_Converter;
            private Angle m_FinishAzimuth;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private IPath m_Path;
            private IPath m_PathForUTurnPath;
            private IEnumerable <IPath> m_Paths;
            private PathShortestFinder m_PathShortestFinder;
            private PathValidator m_PathValidator;
            private IPossibleTurnCirclePairs m_PossibleTurnCirclePairs;
            private PathSelectorCalculator m_Selector;
            private Settings m_Settings;
            private Angle m_StartAzimuth;
            private Point m_StartPoint;
            private ITurnCirclePairsToPathsConverter m_TurnCirclePairsToPathsConverter;
            private IUTurnPath m_UTurnPath;
            private Distance m_RadiusForPortTurn;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void PathsContainsNormalPathTest()
            {
                Assert.False(m_Calculator.Paths.Contains(m_Path));
            }

            [Test]
            public void PathsContainsUTurnPathTest()
            {
                Assert.True(m_Calculator.Paths.Contains(m_PathForUTurnPath));
            }

            [Test]
            public void PathsCountTest()
            {
                Assert.AreEqual(1,
                                m_Calculator.Paths.Count());
            }
        }

        #endregion

        #region Nested type: LinePairToRacetrackConverterUTurnTests

        [TestFixture]
        internal sealed class LinePairToRacetrackConverterUTurnTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

                m_FromLine = new Line(new Point(8.5,
                                                0.0),
                                      new Point(8.5,
                                                2.5));
                m_ToLine = new Line(new Point(7.5,
                                              2.5),
                                    new Point(7.5,
                                              0.0));

                m_Settings = new Settings(m_FromLine.EndPoint,
                                          m_FromLine.AngleToXAxis,
                                          m_ToLine.StartPoint,
                                          m_ToLine.AngleToXAxis,
                                          new Distance(2.5),
                                          new Distance(2.5),
                                          true,
                                          true);

                m_PossibleTurnCircles = new PossibleTurnCircles();
                m_PossibleTurnCirclePairs = new PossibleTurnCirclePairs(m_PossibleTurnCircles);
                m_PathValidator = new PathValidator(m_Logger);
                m_Converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                m_PathValidator);

                m_TurnCirclePairsToPathsConverter = new TurnCirclePairsToPathsConverter(m_Converter,
                                                                                        m_PossibleTurnCirclePairs);

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

                m_DetermineTurnCircleCalculator = new DetermineTurnCircleCalculator();

                m_UTurnPath = new UTurnPath(m_UTurnCircle,
                                            m_DetermineTurnCircleCalculator);

                m_PathShortestFinder = new PathShortestFinder();

                m_Selector = new PathSelectorCalculator(m_TurnCirclePairsToPathsConverter,
                                                        m_UTurnPath);

                m_Calculator = new PathCalculator(m_Selector,
                                                  m_PathShortestFinder)
                               {
                                   Settings = m_Settings
                               };

                m_Calculator.Calculate();
            }

            private PathCalculator m_Calculator;
            private TurnCirclePairToPathConverter m_Converter;
            private DetermineTurnCircleCalculator m_DetermineTurnCircleCalculator;
            private Line m_FromLine;
            private ISelkieLogger m_Logger;
            private PathShortestFinder m_PathShortestFinder;
            private PathValidator m_PathValidator;
            private IPossibleTurnCirclePairs m_PossibleTurnCirclePairs;
            private IPossibleTurnCircles m_PossibleTurnCircles;
            private PathSelectorCalculator m_Selector;
            private Settings m_Settings;
            private Line m_ToLine;
            private TurnCirclePairsToPathsConverter m_TurnCirclePairsToPathsConverter;
            private UTurnCircle m_UTurnCircle;
            private UTurnPath m_UTurnPath;

            [Test]
            public void PathCountTest()
            {
                IPath actual = m_Calculator.Path;

                Assert.AreEqual(3,
                                actual.Segments.Count(),
                                "Count");
            }

            [Test]
            public void PathEndSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                IPolylineSegment actual = segments [ 2 ];

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(6.5,
                                          4.5),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_ToLine.StartPoint,
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathFirstSegementTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                IPolylineSegment actual = segments [ 0 ];

                Assert.NotNull(actual);
                Assert.AreEqual(m_FromLine.EndPoint,
                                actual.StartPoint);
                Assert.AreEqual(new Point(9.5,
                                          4.5),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathLengthTest()
            {
                const double expected = 17.126933813990604d;
                Distance distance = m_Calculator.Path.Distance;
                double actual = distance.Length;

                Assert.AreEqual(expected,
                                actual);
            }

            [Test]
            public void PathUTurnSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                IPolylineSegment actual = segments [ 1 ];

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(9.5,
                                          4.5),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(6.5,
                                          4.5),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_Calculator.Settings);
            }
        }

        #endregion

        #region Nested type: CaseOneTest

        [TestFixture]
        internal sealed class CaseForPortAndStarboardWithDifferentRadiusTest
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

                m_FromLine = new Line(new Point(100.0,
                                                0.0),
                                      new Point(100.0,
                                                100.0));
                m_ToLine = new Line(new Point(1000.0,
                                              100.0),
                                    new Point(1000.0,
                                              0.0));

                Assert.NotNull(m_ToLine,
                               "m_ToLine is null!");

                m_RadiusForPortTurn = new Distance(100.0);
                m_RadiusForStarboardTurn = new Distance(10.0);

                m_Settings = new Settings(m_FromLine.EndPoint,
                                          m_FromLine.AngleToXAxis,
                                          m_ToLine.StartPoint,
                                          m_ToLine.AngleToXAxis,
                                          m_RadiusForPortTurn,
                                          m_RadiusForStarboardTurn,
                                          true,
                                          true);

                m_PossibleTurnCircles = new PossibleTurnCircles();
                m_PossibleTurnCirclePairs = new PossibleTurnCirclePairs(m_PossibleTurnCircles);
                m_PathValidator = new PathValidator(m_Logger);
                m_Converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                m_PathValidator);

                m_TurnCirclePairsToPathsConverter = new TurnCirclePairsToPathsConverter(m_Converter,
                                                                                        m_PossibleTurnCirclePairs);

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

                m_DetermineTurnCircleCalculator = new DetermineTurnCircleCalculator();

                m_UTurnPath = new UTurnPath(m_UTurnCircle,
                                            m_DetermineTurnCircleCalculator);

                m_PathShortestFinder = new PathShortestFinder();

                m_Selector = new PathSelectorCalculator(m_TurnCirclePairsToPathsConverter,
                                                        m_UTurnPath);

                m_Calculator = new PathCalculator(m_Selector,
                                                  m_PathShortestFinder)
                               {
                                   Settings = m_Settings
                               };

                m_Calculator.Calculate();
            }

            private IPathCalculator m_Calculator;
            private TurnCirclePairToPathConverter m_Converter;
            private DetermineTurnCircleCalculator m_DetermineTurnCircleCalculator;
            private ILine m_FromLine;
            private ISelkieLogger m_Logger;
            private PathShortestFinder m_PathShortestFinder;
            private PathValidator m_PathValidator;
            private IPossibleTurnCirclePairs m_PossibleTurnCirclePairs;
            private IPossibleTurnCircles m_PossibleTurnCircles;
            private Distance m_RadiusForPortTurn;
            private IPathSelectorCalculator m_Selector;
            private ISettings m_Settings;
            private ILine m_ToLine;
            private TurnCirclePairsToPathsConverter m_TurnCirclePairsToPathsConverter;
            private UTurnCircle m_UTurnCircle;
            private UTurnPath m_UTurnPath;
            private Distance m_RadiusForStarboardTurn;

            [Test]
            public void PathFirstSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 0 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(new Point(110.0,
                                          100.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(100.0,
                                          100.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(110.0,
                                          110.0),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(10.0,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Start,
                                actual.CircleOrigin,
                                "Origin");
            }

            [Test]
            public void PathLengthTest()
            {
                Assert.AreEqual(new Distance(911.42),
                                m_Calculator.Path.Distance);
            }

            [Test]
            public void PathSecondSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 1 ] as ILine;

                Assert.NotNull(actual);

                Assert.AreEqual(new Point(110.0,
                                          110.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(990.0,
                                          110.0),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathSegmentCountTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;

                Assert.AreEqual(3,
                                polylineSegments.Count());
            }

            [Test]
            public void PathThirdSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Calculator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 2 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(new Point(990.0,
                                          100.0),
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(990.0,
                                          110.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(1000.0,
                                          100.0),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(10.0,
                                actual.Radius,
                                "Radius");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                Assert.AreEqual(Constants.CircleOrigin.Finish,
                                actual.CircleOrigin,
                                "Origin");
            }

            [Test]
            public void SettingsTest()
            {
                Assert.AreEqual(m_Settings,
                                m_Calculator.Settings);
            }
        }

        #endregion
    }
}