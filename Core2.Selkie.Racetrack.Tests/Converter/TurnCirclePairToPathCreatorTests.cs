using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.NUnit.Extensions;
using Core2.Selkie.Racetrack.Converters;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Turn;
using Core2.Selkie.Windsor.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Constants = Core2.Selkie.Geometry.Constants;
using TurnCircle = Core2.Selkie.Racetrack.Turn.TurnCircle;

namespace Core2.Selkie.Racetrack.Tests.Converter
{
    [ExcludeFromCodeCoverage]
    internal sealed class TurnCirclePairToPathCreatorTests
    {
        [TestFixture]
        internal sealed class StartPoints180DegreesToFinishZeroTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

                m_StartPoint = new Point(18.5,
                                         2.5);
                m_FinishPoint = new Point(7.5,
                                          2.5);

                m_Settings = Substitute.For <ISettings>();
                m_Settings.StartPoint.Returns(m_StartPoint);
                m_Settings.FinishPoint.Returns(m_FinishPoint);

                m_TurnCircleZero = new TurnCircle(new Circle(16.0,
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

                m_Tangent = new Line(new Point(10.0,
                                               5.0),
                                     new Point(16.0,
                                               5.0));

                m_Creator = new TurnCirclePairToPathCreator(m_Logger,
                                                            m_Settings,
                                                            m_TurnCirclePair,
                                                            m_Tangent);
            }

            private TurnCirclePairToPathCreator m_Creator;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private ISettings m_Settings;
            private Point m_StartPoint;
            private ILine m_Tangent;
            private TurnCircle m_TurnCircleOne;
            private TurnCirclePair m_TurnCirclePair;
            private TurnCircle m_TurnCircleZero;

            [Test]
            public void EndArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 2 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(m_TurnCircleZero.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Tangent.EndPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_StartPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void LineTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 1 ] as ILine;

                Assert.NotNull(actual);

                Assert.AreEqual(m_Tangent.StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_Tangent.EndPoint,
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathDistanceTest()
            {
                Distance distance = m_Creator.Path.Distance;

                NUnitHelper.AssertIsEquivalent(13.85d,
                                               distance.Length,
                                               "Length");
            }

            [Test]
            public void SegementsCountTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;

                Assert.AreEqual(3,
                                polylineSegments.Count());
            }

            [Test]
            public void StartArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 0 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(m_TurnCircleOne.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_FinishPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_Tangent.StartPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }
        }

        [TestFixture]
        internal sealed class StartPointsBelowFinishPointTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

                m_StartPoint = new Point(8.5,
                                         2.5);
                m_FinishPoint = new Point(12.5,
                                          14.0);

                m_Settings = Substitute.For <ISettings>();
                m_Settings.StartPoint.Returns(m_StartPoint);
                m_Settings.FinishPoint.Returns(m_FinishPoint);

                m_TurnCircleZero = new TurnCircle(new Circle(11.0,
                                                             2.5,
                                                             2.5),
                                                  Constants.CircleSide.Starboard,
                                                  Constants.CircleOrigin.Start,
                                                  Constants.TurnDirection.Clockwise);
                m_TurnCircleOne = new TurnCircle(new Circle(10.0,
                                                            14.0,
                                                            2.5),
                                                 Constants.CircleSide.Port,
                                                 Constants.CircleOrigin.Finish,
                                                 Constants.TurnDirection.Counterclockwise);

                m_TurnCirclePair = new TurnCirclePair(m_Settings,
                                                      m_TurnCircleZero,
                                                      m_TurnCircleOne);

                m_Tangent = new Line(new Point(8.66,
                                               3.38),
                                     new Point(12.34,
                                               13.12));

                m_Creator = new TurnCirclePairToPathCreator(m_Logger,
                                                            m_Settings,
                                                            m_TurnCirclePair,
                                                            m_Tangent);
            }

            private TurnCirclePairToPathCreator m_Creator;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private ISettings m_Settings;
            private Point m_StartPoint;
            private ILine m_Tangent;
            private TurnCircle m_TurnCircleOne;
            private TurnCirclePair m_TurnCirclePair;
            private TurnCircle m_TurnCircleZero;

            [Test]
            public void EndArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 2 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(m_TurnCircleZero.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Tangent.StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_StartPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void LineTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 1 ] as ILine;

                Assert.NotNull(actual);

                Assert.AreEqual(m_Tangent.EndPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_Tangent.StartPoint,
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathDistanceTest()
            {
                Distance distance = m_Creator.Path.Distance;

                NUnitHelper.AssertIsEquivalent(40.03d,
                                               distance.Length);
            }

            [Test]
            public void PathsCountTest()
            {
                IEnumerable <IPolylineSegment> segments = m_Creator.Path.Segments;

                Assert.AreEqual(3,
                                segments.Count());
            }

            [Test]
            public void SegementsCountTest()
            {
                IEnumerable <IPolylineSegment> segments = m_Creator.Path.Segments;

                Assert.AreEqual(3,
                                segments.Count());
            }

            [Test]
            public void StartArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 0 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(m_TurnCircleOne.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_FinishPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_Tangent.EndPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Counterclockwise,
                                actual.TurnDirection,
                                "Direction");
            }
        }

        [TestFixture]
        internal sealed class StartPointsSameYAsFinishPointButPointsSwappedTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

                m_StartPoint = new Point(8.5,
                                         2.5);
                m_FinishPoint = new Point(12.5,
                                          14.0);

                m_Settings = Substitute.For <ISettings>();
                m_Settings.StartPoint.Returns(m_StartPoint);
                m_Settings.FinishPoint.Returns(m_FinishPoint);

                m_TurnCircleZero = new TurnCircle(new Circle(11.0,
                                                             2.5,
                                                             2.5),
                                                  Constants.CircleSide.Starboard,
                                                  Constants.CircleOrigin.Start,
                                                  Constants.TurnDirection.Clockwise);

                m_TurnCircleOne = new TurnCircle(new Circle(10.0,
                                                            14.0,
                                                            2.5),
                                                 Constants.CircleSide.Port,
                                                 Constants.CircleOrigin.Finish,
                                                 Constants.TurnDirection.Clockwise);

                m_TurnCirclePair = new TurnCirclePair(m_Settings,
                                                      m_TurnCircleZero,
                                                      m_TurnCircleOne);

                m_Tangent = new Line(new Point(12.34,
                                               13.12),
                                     new Point(8.66,
                                               3.38));

                m_Creator = new TurnCirclePairToPathCreator(m_Logger,
                                                            m_Settings,
                                                            m_TurnCirclePair,
                                                            m_Tangent);
            }

            private TurnCirclePairToPathCreator m_Creator;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private ISettings m_Settings;
            private Point m_StartPoint;
            private ILine m_Tangent;
            private TurnCircle m_TurnCircleOne;
            private TurnCirclePair m_TurnCirclePair;
            private TurnCircle m_TurnCircleZero;

            [Test]
            public void EndArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 2 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(m_TurnCircleZero.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Tangent.EndPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_StartPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void LineTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 1 ] as ILine;

                Assert.NotNull(actual);

                Assert.AreEqual(m_Tangent.StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_Tangent.EndPoint,
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathDistanceTest()
            {
                Distance distance = m_Creator.Path.Distance;

                NUnitHelper.AssertIsEquivalent(26.12d,
                                               distance.Length,
                                               "Length");
            }

            [Test]
            public void SegementsCountTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;

                Assert.AreEqual(3,
                                polylineSegments.Count());
            }

            [Test]
            public void StartArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 0 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(m_TurnCircleOne.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_FinishPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_Tangent.StartPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }
        }

        [TestFixture]
        internal sealed class StartPointsSameYAsFinishPointSwappedTangentPointsTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

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
                                                  Constants.TurnDirection.Clockwise);

                m_TurnCircleOne = new TurnCircle(new Circle(10.0,
                                                            14.0,
                                                            2.5),
                                                 Constants.CircleSide.Port,
                                                 Constants.CircleOrigin.Finish,
                                                 Constants.TurnDirection.Clockwise);

                m_TurnCirclePair = new TurnCirclePair(m_Settings,
                                                      m_TurnCircleZero,
                                                      m_TurnCircleOne);

                m_Tangent = new Line(new Point(8.66,
                                               3.38),
                                     new Point(12.34,
                                               13.12));

                m_Creator = new TurnCirclePairToPathCreator(m_Logger,
                                                            m_Settings,
                                                            m_TurnCirclePair,
                                                            m_Tangent);
            }

            private TurnCirclePairToPathCreator m_Creator;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private ISettings m_Settings;
            private Point m_StartPoint;
            private ILine m_Tangent;
            private TurnCircle m_TurnCircleOne;
            private TurnCirclePair m_TurnCirclePair;
            private TurnCircle m_TurnCircleZero;

            [Test]
            public void EndArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 2 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(m_TurnCircleZero.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Tangent.StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_FinishPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void LineTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 1 ] as ILine;

                Assert.NotNull(actual);

                Assert.AreEqual(m_Tangent.EndPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_Tangent.StartPoint,
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathDistanceTest()
            {
                Distance distance = m_Creator.Path.Distance;

                NUnitHelper.AssertIsEquivalent(26.12d,
                                               distance.Length,
                                               "Length");
            }

            [Test]
            public void SegementsCountTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;

                Assert.AreEqual(3,
                                polylineSegments.Count());
            }

            [Test]
            public void StartArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 0 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(m_TurnCircleOne.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_Tangent.EndPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }
        }

        [TestFixture]
        internal sealed class StartPointsSameYAsFinishPointTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

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
                                                  Constants.TurnDirection.Clockwise);

                m_TurnCircleOne = new TurnCircle(new Circle(10.0,
                                                            14.0,
                                                            2.5),
                                                 Constants.CircleSide.Port,
                                                 Constants.CircleOrigin.Finish,
                                                 Constants.TurnDirection.Clockwise);

                m_TurnCirclePair = new TurnCirclePair(m_Settings,
                                                      m_TurnCircleZero,
                                                      m_TurnCircleOne);

                m_Tangent = new Line(new Point(12.34,
                                               13.12),
                                     new Point(8.66,
                                               3.38));

                m_Creator = new TurnCirclePairToPathCreator(m_Logger,
                                                            m_Settings,
                                                            m_TurnCirclePair,
                                                            m_Tangent);
            }

            private TurnCirclePairToPathCreator m_Creator;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private ISettings m_Settings;
            private Point m_StartPoint;
            private ILine m_Tangent;
            private TurnCircle m_TurnCircleOne;
            private TurnCirclePair m_TurnCirclePair;
            private TurnCircle m_TurnCircleZero;

            [Test]
            public void EndArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 2 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(m_TurnCircleZero.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_Tangent.EndPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_FinishPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }

            [Test]
            public void LineTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 1 ] as ILine;

                Assert.NotNull(actual);

                Assert.AreEqual(m_Tangent.StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_Tangent.EndPoint,
                                actual.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void PathDistanceTest()
            {
                Distance distance = m_Creator.Path.Distance;

                NUnitHelper.AssertIsEquivalent(26.12d,
                                               distance.Length,
                                               "Length");
            }

            [Test]
            public void SegementsCountTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;

                Assert.AreEqual(3,
                                polylineSegments.Count());
            }

            [Test]
            public void StartArcSegmentTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;
                IPolylineSegment[] segments = polylineSegments.ToArray();
                var actual = segments [ 0 ] as ITurnCircleArcSegment;

                Assert.NotNull(actual);

                Assert.AreEqual(m_TurnCircleOne.CentrePoint,
                                actual.CentrePoint,
                                "CentrePoint");
                Assert.AreEqual(m_StartPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(m_Tangent.StartPoint,
                                actual.EndPoint,
                                "EndPoint");
                Assert.AreEqual(Constants.TurnDirection.Clockwise,
                                actual.TurnDirection,
                                "Direction");
            }
        }

        [TestFixture]
        internal sealed class ThrowsExceptionForTangentStartPointNotOnCircleTests
        {
            [SetUp]
            public void Setup()
            {
                m_Logger = Substitute.For <ISelkieLogger>();

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
                                                  Constants.TurnDirection.Clockwise);

                m_TurnCircleOne = new TurnCircle(new Circle(10.0,
                                                            14.0,
                                                            2.5),
                                                 Constants.CircleSide.Port,
                                                 Constants.CircleOrigin.Finish,
                                                 Constants.TurnDirection.Clockwise);

                m_TurnCirclePair = new TurnCirclePair(m_Settings,
                                                      m_TurnCircleZero,
                                                      m_TurnCircleOne);

                m_Tangent = new Line(new Point(0.0,
                                               0.0),
                                     new Point(8.66,
                                               3.38));

                m_Creator = new TurnCirclePairToPathCreator(m_Logger,
                                                            m_Settings,
                                                            m_TurnCirclePair,
                                                            m_Tangent);
            }

            private TurnCirclePairToPathCreator m_Creator;
            private Point m_FinishPoint;
            private ISelkieLogger m_Logger;
            private ISettings m_Settings;
            private Point m_StartPoint;
            private ILine m_Tangent;
            private TurnCircle m_TurnCircleOne;
            private TurnCirclePair m_TurnCirclePair;
            private TurnCircle m_TurnCircleZero;

            [Test]
            public void PathDistanceTest()
            {
                Distance distance = m_Creator.Path.Distance;

                NUnitHelper.AssertIsEquivalent(0.0d,
                                               distance.Length);
            }

            [Test]
            public void PathIsUnknownTest()
            {
                Assert.True(m_Creator.Path.IsUnknown);
            }

            [Test]
            public void SegementsCountTest()
            {
                IEnumerable <IPolylineSegment> polylineSegments = m_Creator.Path.Segments;

                Assert.AreEqual(0,
                                polylineSegments.Count());
            }
        }
    }
}