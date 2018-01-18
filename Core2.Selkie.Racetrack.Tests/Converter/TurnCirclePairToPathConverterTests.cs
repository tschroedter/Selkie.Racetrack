using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.NUnit.Extensions;
using Core2.Selkie.Racetrack.Converters;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Racetrack.Turn;
using Core2.Selkie.Windsor.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Constants = Core2.Selkie.Geometry.Constants;
using ITurnCircleArcSegment = Core2.Selkie.Geometry.Shapes.ITurnCircleArcSegment;
using TurnCircle = Core2.Selkie.Racetrack.Turn.TurnCircle;
// using ITurnCircleArcSegment = Core2.Selkie.Racetrack.Interfaces.Turn.ITurnCircleArcSegment; // Todo check if there is a general TurnCircle,ITurnCircleArcSegment in Geometry

namespace Core2.Selkie.Racetrack.Tests.Converter
{
    [ExcludeFromCodeCoverage]
    internal sealed class TurnCirclePairToPathConverterTests
    {
        #region Nested type: CreatePathForInvalidTurnCirclePairTests

        [TestFixture]
        internal sealed class CreatePathForInvalidTurnCirclePairTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

                m_PathValidator = Substitute.For <IPathValidator>();

                m_StartPoint = new Point(9.5,
                                         2.5);
                m_FinishPoint = new Point(8.5,
                                          2.5);

                m_Settings = Substitute.For <ISettings>();
                m_Settings.StartPoint.Returns(m_StartPoint);
                m_Settings.FinishPoint.Returns(m_FinishPoint);

                m_TurnCircleZero = new TurnCircle(new Circle(12.0,
                                                             2.5,
                                                             2.5),
                                                  Constants.CircleSide.Starboard,
                                                  Constants.CircleOrigin.Start,
                                                  Constants.TurnDirection.Clockwise);

                m_TurnCircleOne = new TurnCircle(new Circle(10.0,
                                                            2.5,
                                                            2.5),
                                                 Constants.CircleSide.Starboard,
                                                 Constants.CircleOrigin.Finish,
                                                 Constants.TurnDirection.Clockwise);

                m_TurnCirclePair = new TurnCirclePair(m_Settings,
                                                      m_TurnCircleZero,
                                                      m_TurnCircleOne);

                m_Converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                m_PathValidator)
                              {
                                  Settings = m_Settings,
                                  TurnCirclePair = m_TurnCirclePair
                              };

                m_Converter.Convert();
            }

            private TurnCirclePairToPathConverter m_Converter;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private IPathValidator m_PathValidator;
            private ISettings m_Settings;
            private Point m_StartPoint;
            private TurnCircle m_TurnCircleOne;
            private TurnCirclePair m_TurnCirclePair;
            private TurnCircle m_TurnCircleZero;

            [Test]
            public void PossiblePathsCountTest()
            {
                Assert.AreEqual(0,
                                m_Converter.PossiblePaths.Count());
            }
        }

        #endregion

        #region Nested type: CreatePathsForUnknownPathTests

        [TestFixture]
        internal sealed class CreatePathsForUnknownPathTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

                m_PathValidator = Substitute.For <IPathValidator>();

                m_StartPoint = new Point(12.5,
                                         14.0);
                m_FinishPoint = new Point(8.5,
                                          2.5);

                m_Settings = Substitute.For <ISettings>();
                m_Settings.StartPoint.Returns(m_StartPoint);
                m_Settings.FinishPoint.Returns(m_FinishPoint);

                m_TurnCircleZero = new TurnCircle(new Circle(11.0,
                                                             2.5,
                                                             2.5),
                                                  Constants.CircleSide.Starboard,
                                                  Constants.CircleOrigin.Start,
                                                  Constants.TurnDirection.Counterclockwise);
                m_TurnCircleOne = new TurnCircle(new Circle(10.0,
                                                            14.0,
                                                            2.5),
                                                 Constants.CircleSide.Port,
                                                 Constants.CircleOrigin.Finish,
                                                 Constants.TurnDirection.Clockwise);

                m_TurnCirclePair = new TurnCirclePair(m_Settings,
                                                      m_TurnCircleZero,
                                                      m_TurnCircleOne);

                m_Converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                m_PathValidator)
                              {
                                  Settings = m_Settings,
                                  TurnCirclePair = m_TurnCirclePair
                              };

                m_Converter.Convert();
            }

            private TurnCirclePairToPathConverter m_Converter;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private IPathValidator m_PathValidator;
            private ISettings m_Settings;
            private Point m_StartPoint;
            private TurnCircle m_TurnCircleOne;
            private TurnCirclePair m_TurnCirclePair;
            private TurnCircle m_TurnCircleZero;

            [Test]
            public void ReversePathIfNeccessaryReturnsPathForPathLengthIsEpsilonTest()
            {
                var path = Substitute.For <IPath>();
                path.IsUnknown.Returns(false);
                path.Distance.Returns(new Distance(Constants.EpsilonDistance));

                IPath actual = m_Converter.ReversePathIfNeccessary(m_Settings,
                                                                   path);

                Assert.AreEqual(path,
                                actual);
            }

            [Test]
            public void ReversePathIfNeccessaryReturnsPathForPathLengthIsNegativeTest()
            {
                var path = Substitute.For <IPath>();
                path.IsUnknown.Returns(false);
                path.Distance.Returns(new Distance(-100.0));

                IPath actual = m_Converter.ReversePathIfNeccessary(m_Settings,
                                                                   path);

                Assert.AreEqual(path,
                                actual);
            }

            [Test]
            public void ReversePathIfNeccessaryReturnsPathForPathLengthIsZeroTest()
            {
                var path = Substitute.For <IPath>();
                path.IsUnknown.Returns(false);
                path.Distance.Returns(Distance.Zero);

                IPath actual = m_Converter.ReversePathIfNeccessary(m_Settings,
                                                                   path);

                Assert.AreEqual(path,
                                actual);
            }

            [Test]
            public void ReversePathIfNeccessaryReturnsPathForPathStartPointIsSettingsStartPointTest()
            {
                Point startPoint = m_Settings.StartPoint;

                var path = Substitute.For <IPath>();
                path.IsUnknown.Returns(false);
                path.Distance.Returns(new Distance(100.0));
                path.StartPoint.Returns(startPoint);

                IPath actual = m_Converter.ReversePathIfNeccessary(m_Settings,
                                                                   path);

                Assert.AreEqual(path,
                                actual);
            }

            [Test]
            public void ReversePathIfNeccessaryReturnsPathForReversedPathStartPointIsSettingsFinishPointTest()
            {
                Point startPoint = m_Settings.FinishPoint;

                var path = Substitute.For <IPath>();
                path.IsUnknown.Returns(false);
                path.Distance.Returns(new Distance(100.0));
                path.StartPoint.Returns(startPoint);

                m_Converter.ReversePathIfNeccessary(m_Settings,
                                                    path);

                path.Received().Reverse();
            }

            [Test]
            public void ReversePathIfNeccessaryReturnsUnknownPathForUnknownPathTest()
            {
                IPath actual = m_Converter.ReversePathIfNeccessary(m_Settings,
                                                                   Path.Unknown);

                Assert.AreEqual(Path.Unknown,
                                actual);
            }
        }

        #endregion

        #region Nested type: CreatePossiblePathsForStartPointSameAsFinishPointTests

        [TestFixture]
        internal sealed class CreatePossiblePathsForStartPointSameAsFinishPointTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

                m_PathValidator = Substitute.For <IPathValidator>();

                m_StartPoint = new Point(12.5,
                                         14.0);
                m_FinishPoint = new Point(8.5,
                                          2.5);

                m_Settings = Substitute.For <ISettings>();
                m_Settings.StartPoint.Returns(m_StartPoint);
                m_Settings.FinishPoint.Returns(m_FinishPoint);

                m_TurnCircleZero = new TurnCircle(new Circle(11.0,
                                                             2.5,
                                                             2.5),
                                                  Constants.CircleSide.Starboard,
                                                  Constants.CircleOrigin.Start,
                                                  Constants.TurnDirection.Counterclockwise);
                m_TurnCircleOne = new TurnCircle(new Circle(10.0,
                                                            14.0,
                                                            2.5),
                                                 Constants.CircleSide.Port,
                                                 Constants.CircleOrigin.Finish,
                                                 Constants.TurnDirection.Clockwise);

                m_TurnCirclePair = new TurnCirclePair(m_Settings,
                                                      m_TurnCircleZero,
                                                      m_TurnCircleOne);

                m_Converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                m_PathValidator)
                              {
                                  Settings = m_Settings,
                                  TurnCirclePair = m_TurnCirclePair
                              };

                m_Converter.Convert();
            }

            private TurnCirclePairToPathConverter m_Converter;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private IPathValidator m_PathValidator;
            private ISettings m_Settings;
            private Point m_StartPoint;
            private TurnCircle m_TurnCircleOne;
            private TurnCirclePair m_TurnCirclePair;
            private TurnCircle m_TurnCircleZero;

            [Test]
            public void FirstPathEndArcSegmentTest()
            {
                IPath path = m_Converter.PossiblePaths.FirstOrDefault();
                Assert.NotNull(path);

                IPolylineSegment[] segments = path.Segments.ToArray();
                var actual = segments [ 2 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(m_TurnCircleZero.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(8.51,
                                          2.28),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_FinishPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                NUnitHelper.AssertIsEquivalent(15.49,
                                               actual.Length,
                                               "Length");
            }

            [Test]
            public void FirstPathLineTest()
            {
                IPath path = m_Converter.PossiblePaths.FirstOrDefault();
                Assert.NotNull(path);

                IPolylineSegment[] segments = path.Segments.ToArray();
                var actual = segments [ 1 ] as ILine;

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(7.51,
                                          13.78),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(8.51,
                                          2.28),
                                actual.EndPoint,
                                "EndPoint");
                NUnitHelper.AssertIsEquivalent(11.54,
                                               actual.Length,
                                               "Length");
            }

            [Test]
            public void FirstPathSegmentsCountTest()
            {
                IPath path = m_Converter.PossiblePaths.FirstOrDefault();

                Assert.NotNull(path);
                Assert.AreEqual(3,
                                path.Segments.Count());
            }

            [Test]
            public void FirstPathSegmentsDistanceTest()
            {
                IPath path = m_Converter.PossiblePaths.FirstOrDefault();
                Assert.NotNull(path);

                NUnitHelper.AssertIsEquivalent(34.67,
                                               path.Distance.Length,
                                               "Length");
            }

            [Test]
            public void FirstPathStartArcSegmentTest()
            {
                IPath path = m_Converter.PossiblePaths.FirstOrDefault();
                Assert.NotNull(path);

                IPolylineSegment[] segments = path.Segments.ToArray();
                var actual = segments [ 0 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(m_TurnCircleOne.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(7.51,
                                          13.78),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                NUnitHelper.AssertIsEquivalent(7.64,
                                               actual.Length,
                                               "Length");
            }

            [Test]
            public void PathsCountTest()
            {
                Assert.AreEqual(4,
                                m_Converter.PossiblePaths.Count());
            }

            [Test]
            public void SecondPathEndArcSegmentTest()
            {
                IPath path = m_Converter.PossiblePaths.LastOrDefault();
                Assert.NotNull(path);

                IPolylineSegment[] segments = path.Segments.ToArray();
                var actual = segments [ 2 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(m_TurnCircleZero.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(8.66,
                                          3.38),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_FinishPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
                NUnitHelper.AssertIsEquivalent(0.90,
                                               actual.Length,
                                               "Length");
            }

            [Test]
            public void SecondPathLineTest()
            {
                IPath path = m_Converter.PossiblePaths.LastOrDefault();
                Assert.NotNull(path);

                IPolylineSegment[] segments = path.Segments.ToArray();
                var actual = segments [ 1 ] as ILine;

                Assert.NotNull(actual);
                Assert.AreEqual(new Point(12.34,
                                          13.12),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(8.66,
                                          3.38),
                                actual.EndPoint,
                                "EndPoint");
                NUnitHelper.AssertIsEquivalent(10.40d,
                                               actual.Length,
                                               "Length");
            }

            [Test]
            public void SecondPathSegmentsCountTest()
            {
                IPath path = m_Converter.PossiblePaths.LastOrDefault();

                Assert.NotNull(path);
                Assert.AreEqual(3,
                                path.Segments.Count());
            }

            [Test]
            public void SecondPathSegmentsDistanceTest()
            {
                IPath path = m_Converter.PossiblePaths.LastOrDefault();
                Assert.NotNull(path);

                NUnitHelper.AssertIsEquivalent(12.21,
                                               path.Distance.Length,
                                               "Length");
            }

            [Test]
            public void SecondPathStartArcSegmentTest()
            {
                IPath path = m_Converter.PossiblePaths.LastOrDefault();
                Assert.NotNull(path);

                IPolylineSegment[] segments = path.Segments.ToArray();
                var actual = segments [ 0 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);
                Assert.AreEqual(m_TurnCircleOne.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(12.34,
                                          13.12),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
                NUnitHelper.AssertIsEquivalent(0.90,
                                               actual.Length,
                                               "Length");
            }
        }

        #endregion

        #region Nested type: CreatePossiblePathsReversesPathWhenRequiredTests

        [TestFixture]
        internal sealed class CreatePossiblePathsReversesPathWhenRequiredTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

                m_PathValidator = new PathValidator(m_Logger);

                m_StartPoint = new Point(18.5,
                                         2.5);
                m_FinishPoint = new Point(7.5,
                                          2.5);

                m_Settings = Substitute.For <ISettings>();
                m_Settings.IsPortTurnAllowed.Returns(true);
                m_Settings.IsStarboardTurnAllowed.Returns(true);

                m_Settings.StartPoint.Returns(m_StartPoint);
                m_Settings.FinishPoint.Returns(m_FinishPoint);

                m_TurnCircleZero = new TurnCircle(new Circle(16.0,
                                                             2.5,
                                                             2.5),
                                                  Constants.CircleSide.Port,
                                                  Constants.CircleOrigin.Start,
                                                  Constants.TurnDirection.Counterclockwise);

                m_TurnCircleOne = new TurnCircle(new Circle(10.0,
                                                            2.5,
                                                            2.5),
                                                 Constants.CircleSide.Port,
                                                 Constants.CircleOrigin.Finish,
                                                 Constants.TurnDirection.Counterclockwise);

                m_TurnCirclePair = new TurnCirclePair(m_Settings,
                                                      m_TurnCircleZero,
                                                      m_TurnCircleOne);

                m_Converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                m_PathValidator)
                              {
                                  Settings = m_Settings,
                                  TurnCirclePair = m_TurnCirclePair
                              };

                m_Converter.Convert();
            }

            private TurnCirclePairToPathConverter m_Converter;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private IPathValidator m_PathValidator;
            private ISettings m_Settings;
            private Point m_StartPoint;
            private TurnCircle m_TurnCircleOne;
            private TurnCirclePair m_TurnCirclePair;
            private TurnCircle m_TurnCircleZero;

            [Test]
            public void EndArcSegmentTest()
            {
                IPath path = m_Converter.Paths.FirstOrDefault();
                Assert.NotNull(path);

                IPolylineSegment[] segments = path.Segments.ToArray();
                var actual = segments [ 2 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(m_TurnCircleOne.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(new Point(10.0,
                                          5.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_FinishPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void LineTest()
            {
                IPath path = m_Converter.Paths.FirstOrDefault();
                Assert.NotNull(path);

                IPolylineSegment[] segments = path.Segments.ToArray();
                var actual = segments [ 1 ] as ILine;

                Assert.NotNull(actual);

                Assert.AreEqual(new Point(16.0,
                                          5.0),
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(10.0,
                                          5.0),
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathDistanceTest()
            {
                IPath path = m_Converter.Paths.FirstOrDefault();
                Assert.NotNull(path);

                NUnitHelper.AssertIsEquivalent(13.85d,
                                               path.Distance.Length,
                                               "Length");
            }

            [Test]
            public void PathsCountTest()
            {
                Assert.AreEqual(1,
                                m_Converter.Paths.Count());
            }

            [Test]
            public void PossiblePathsCountTest()
            {
                Assert.AreEqual(4,
                                m_Converter.PossiblePaths.Count());
            }

            [Test]
            public void SegementsCountTest()
            {
                IPath path = m_Converter.Paths.FirstOrDefault();
                Assert.NotNull(path);

                Assert.AreEqual(3,
                                path.Segments.Count());
            }

            [Test]
            public void StartArcSegmentTest()
            {
                IPath path = m_Converter.Paths.FirstOrDefault();
                Assert.NotNull(path);

                IPolylineSegment[] segments = path.Segments.ToArray();
                var actual = segments [ 0 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(m_TurnCircleZero.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(new Point(16.0,
                                          5.0),
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }
        }

        #endregion

        internal sealed class GeneralTests
        {
            private ISelkieLogger m_Logger;
            private IPathValidator m_PathValidator;

            [Test]
            public void SettingsDefaultTest()
            {
                var converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                  m_PathValidator);

                Assert.True(converter.Settings.IsUnknown);
            }

            [Test]
            public void SettingsRoundtripTest()
            {
                var settings = Substitute.For <ISettings>();

                var converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                  m_PathValidator)
                                {
                                    Settings = settings
                                };

                Assert.AreEqual(settings,
                                converter.Settings);
            }

            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();
                m_PathValidator = Substitute.For <IPathValidator>();
            }

            [Test]
            public void TurnCirclePairDefaultTest()
            {
                var converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                  m_PathValidator);

                Assert.True(converter.TurnCirclePair.IsUnknown);
            }

            [Test]
            public void TurnCirclePairRoundtripTest()
            {
                var pair = Substitute.For <ITurnCirclePair>();

                var converter = new TurnCirclePairToPathConverter(m_Logger,
                                                                  m_PathValidator)
                                {
                                    TurnCirclePair = pair
                                };

                Assert.AreEqual(pair,
                                converter.TurnCirclePair);
            }
        }
    }
}