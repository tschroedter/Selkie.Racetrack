using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Tests.NUnit
{
    [ExcludeFromCodeCoverage]
    internal sealed class PathTests
    {
        #region Nested type: CommonTests

        [TestFixture]
        internal sealed class CommonTests
        {
            [SetUp]
            public void Setup()
            {
                m_Segment1 = Substitute.For <IPolylineSegment>();
                m_Segment1.Length.Returns(1.0);

                m_Segment2 = Substitute.For <IPolylineSegment>();
                m_Segment2.Length.Returns(2.0);

                m_StartArcSegment = Substitute.For <ITurnCircleArcSegment>();
                m_Line = Substitute.For <ILine>();
                m_EndArcSegment = Substitute.For <ITurnCircleArcSegment>();
            }

            private ITurnCircleArcSegment m_EndArcSegment;
            private ILine m_Line;
            private IPolylineSegment m_Segment1;
            private IPolylineSegment m_Segment2;
            private ITurnCircleArcSegment m_StartArcSegment;

            [Test]
            public void AddTest()
            {
                var path = new Path(Point.Unknown);

                path.AddSegment(m_Segment1);

                Assert.AreEqual(1,
                                path.Segments.Count(),
                                "Count");
                Assert.AreEqual(m_Segment1,
                                path.Segments.First(),
                                "First");
                Assert.AreEqual(new Distance(1.0),
                                path.Distance,
                                "Distance");
                Assert.False(path.IsUnknown,
                             "IsUnknown");
            }

            [Test]
            public void ConstructorOnePointTest()
            {
                var startPoint = new Point(10.0,
                                           10.0);

                var path = new Path(startPoint);

                Assert.AreEqual(startPoint,
                                path.StartPoint,
                                "StartPoint");
                Assert.AreEqual(startPoint,
                                path.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void ConstructorPolylineTest()
            {
                var line = new Line(-10.0,
                                    -10.0,
                                    10.0,
                                    10.0);
                var polyline = new Polyline();
                polyline.AddSegment(line);

                var path = new Path(polyline);

                Assert.AreEqual(line.StartPoint,
                                path.StartPoint,
                                "StartPoint");
                Assert.AreEqual(line.EndPoint,
                                path.EndPoint,
                                "EndPoint");
                Assert.AreEqual(polyline,
                                path.Polyline,
                                "Polyline");
            }

            [Test]
            public void DefaultIsUnknownTest()
            {
                Point path = Point.Unknown;

                Assert.True(path.IsUnknown);
            }

            [Test]
            public void DefaultSegmentsTest()
            {
                var path = new Path(m_Segment1.StartPoint);

                Assert.AreEqual(0,
                                path.Segments.Count(),
                                "Count");
                Assert.AreEqual(new Distance(0.0),
                                path.Distance,
                                "Distance");
            }

            [Test]
            public void EndArcSegmentTest()
            {
                var path = new Path(m_StartArcSegment,
                                    m_Line,
                                    m_EndArcSegment);

                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 2 ];

                Assert.AreEqual(m_EndArcSegment,
                                actual);
            }

            [Test]
            public void LengthTest()
            {
                var path = new Path(m_Segment1.StartPoint);

                path.AddSegment(m_Segment1);
                path.AddSegment(m_Segment2);

                Assert.AreEqual(3.0,
                                path.Distance.Length);
            }

            [Test]
            public void LineTest()
            {
                var path = new Path(m_StartArcSegment,
                                    m_Line,
                                    m_EndArcSegment);

                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 1 ];

                Assert.AreEqual(m_Line,
                                actual);
            }

            [Test]
            public void ReverseForEmptyTest()
            {
                var startPoint = new Point(-10.0,
                                           -10.0);

                var path = new Path(startPoint);

                IPath actual = path.Reverse();

                Assert.AreEqual(startPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(0,
                                actual.Segments.Count(),
                                "Count");
            }

            [Test]
            public void StartArcSegmentTest()
            {
                var path = new Path(m_StartArcSegment,
                                    m_Line,
                                    m_EndArcSegment);

                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 0 ];

                Assert.AreEqual(m_StartArcSegment,
                                actual);
            }

            [Test]
            public void ToStringTest()
            {
                var line = new Line(-10.0,
                                    -10.0,
                                    10.0,
                                    10.0);
                var polyline = new Polyline();

                polyline.AddSegment(line);

                var path = new Path(polyline);

                path.AddSegment(m_Segment1);

                const string expected = "Length: 29.28";
                string actual = path.ToString();

                Assert.AreEqual(expected,
                                actual);
            }
        }

        #endregion

        #region Nested type: ConstructorForPathTests

        [TestFixture]
        internal sealed class ConstructorForPathTests
        {
            [SetUp]
            public void Setup()
            {
                m_Segment1 = Substitute.For <IPolylineSegment>();
                m_Segment1.Length.Returns(1.0);

                m_Segment2 = Substitute.For <IPolylineSegment>();
                m_Segment2.Length.Returns(2.0);

                m_StartArcSegment = Substitute.For <ITurnCircleArcSegment>();
                m_Line = Substitute.For <ILine>();
                m_EndArcSegment = Substitute.For <ITurnCircleArcSegment>();
            }

            private ITurnCircleArcSegment m_EndArcSegment;
            private ILine m_Line;
            private IPolylineSegment m_Segment1;
            private IPolylineSegment m_Segment2;
            private ITurnCircleArcSegment m_StartArcSegment;

            [Test]
            public void EndArcSegmentTest()
            {
                var path = new Path(m_StartArcSegment,
                                    m_Line,
                                    m_EndArcSegment);

                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 2 ];

                Assert.AreEqual(m_EndArcSegment,
                                actual);
            }

            [Test]
            public void LineTest()
            {
                var path = new Path(m_StartArcSegment,
                                    m_Line,
                                    m_EndArcSegment);

                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 1 ];

                Assert.AreEqual(m_Line,
                                actual);
            }

            [Test]
            public void StartArcSegmentTest()
            {
                var path = new Path(m_StartArcSegment,
                                    m_Line,
                                    m_EndArcSegment);

                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 0 ];

                Assert.AreEqual(m_StartArcSegment,
                                actual);
            }
        }

        #endregion

        #region Nested type: ConstructorForUTurnTests

        [TestFixture]
        internal sealed class ConstructorForUTurnTests
        {
            [SetUp]
            public void Setup()
            {
                m_Segment1 = Substitute.For <IPolylineSegment>();
                m_Segment1.Length.Returns(1.0);

                m_Segment2 = Substitute.For <IPolylineSegment>();
                m_Segment2.Length.Returns(2.0);

                m_StartArcSegment = Substitute.For <ITurnCircleArcSegment>();
                m_EndArcSegment = Substitute.For <ITurnCircleArcSegment>();
                m_MiddleArcSegment = Substitute.For <ITurnCircleArcSegment>();
            }

            private ITurnCircleArcSegment m_EndArcSegment;
            private ITurnCircleArcSegment m_MiddleArcSegment;
            private IPolylineSegment m_Segment1;
            private IPolylineSegment m_Segment2;
            private ITurnCircleArcSegment m_StartArcSegment;

            [Test]
            public void EndArcSegmentTest()
            {
                var path = new Path(m_StartArcSegment,
                                    m_MiddleArcSegment,
                                    m_EndArcSegment);

                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 2 ];

                Assert.AreEqual(m_EndArcSegment,
                                actual);
            }

            [Test]
            public void MiddleArcSegmentTest()
            {
                var path = new Path(m_StartArcSegment,
                                    m_MiddleArcSegment,
                                    m_EndArcSegment);

                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 1 ];

                Assert.AreEqual(m_MiddleArcSegment,
                                actual);
            }

            [Test]
            public void StartArcSegmentTest()
            {
                var path = new Path(m_StartArcSegment,
                                    m_MiddleArcSegment,
                                    m_EndArcSegment);

                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 0 ];

                Assert.AreEqual(m_StartArcSegment,
                                actual);
            }
        }

        #endregion
    }
}