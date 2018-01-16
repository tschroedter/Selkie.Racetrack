using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;

namespace Core2.Selkie.Racetrack.Tests
{
    [ExcludeFromCodeCoverage]
    internal sealed class PathTests
    {
        private const int DoNotCareId = -1;

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
            public void Add_AddsSegmentToSegments_ForSegment()
            {
                // Arrange
                var path = new Path(Point.Unknown);

                // Act
                path.AddSegment(m_Segment1);

                // Assert
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
            public void Constructor_AddsPointToSegments_ForSinglePoint()
            {
                // Arrange
                var startPoint = new Point(10.0,
                                           10.0);

                // Act
                var path = new Path(startPoint);

                // Assert
                Assert.AreEqual(startPoint,
                                path.StartPoint,
                                "StartPoint");
                Assert.AreEqual(startPoint,
                                path.EndPoint,
                                "EndPoint");
            }

            [Test]
            public void Constructor_AddsPolylineToSegements_ForPolyline()
            {
                // Arrange
                var line = new Line(-10.0,
                                    -10.0,
                                    10.0,
                                    10.0);
                var polyline = new Polyline(DoNotCareId,
                                            Constants.LineDirection.Forward);
                polyline.AddSegment(line);

                // Act
                var path = new Path(polyline);

                // Assert
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
            public void Constructor_AddsSegments_ForEndArcSegment()
            {
                // Arrange
                var path = new Path(m_StartArcSegment,
                                    m_Line,
                                    m_EndArcSegment);

                // Act
                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 2 ];

                // Assert
                Assert.AreEqual(m_EndArcSegment,
                                actual);
            }

            [Test]
            public void Constructor_AddsSegments_ForMiddleSegment()
            {
                // Arrange
                var path = new Path(m_StartArcSegment,
                                    m_Line,
                                    m_EndArcSegment);

                // Act
                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 1 ];

                // Assert
                Assert.AreEqual(m_Line,
                                actual);
            }

            [Test]
            public void Constructor_AddsSegments_ForStartArcSegment()
            {
                // Arrange
                var path = new Path(m_StartArcSegment,
                                    m_Line,
                                    m_EndArcSegment);

                // Act
                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 0 ];

                // Assert
                Assert.AreEqual(m_StartArcSegment,
                                actual);
            }

            [Test]
            public void IsUnknown_ReturnsTrue_ByDefault()
            {
                // Arrange
                // Act
                Point path = Point.Unknown;

                // Assert
                Assert.True(path.IsUnknown);
            }

            [Test]
            public void Length_ReturnsValue_ForSegments()
            {
                // Arrange
                var path = new Path(m_Segment1.StartPoint);

                // Act
                path.AddSegment(m_Segment1);
                path.AddSegment(m_Segment2);

                // Assert
                Assert.AreEqual(3.0,
                                path.Distance.Length);
            }

            [Test]
            public void Reverse_ReturnsPath_ForPoint()
            {
                // Arrange
                var startPoint = new Point(-10.0,
                                           -10.0);

                var path = new Path(startPoint);

                // Act
                IPath actual = path.Reverse();

                // Assert
                Assert.AreEqual(startPoint,
                                actual.StartPoint,
                                "StartPoint");
                Assert.AreEqual(0,
                                actual.Segments.Count(),
                                "Count");
            }

            [Test]
            public void Segments_ReturnsEmpty_ByDefault()
            {
                // Arrange
                // Act
                var path = new Path(m_Segment1.StartPoint);

                // Assert
                Assert.AreEqual(0,
                                path.Segments.Count(),
                                "Count");
                Assert.AreEqual(new Distance(0.0),
                                path.Distance,
                                "Distance");
            }

            [Test]
            public void ToString_ReturnsString_ForInstance()
            {
                // Arrange
                const string expected = "Length: 29.28";

                var line = new Line(-10.0,
                                    -10.0,
                                    10.0,
                                    10.0);
                var polyline = new Polyline(DoNotCareId,
                                            Constants.LineDirection.Forward);

                polyline.AddSegment(line);

                var path = new Path(polyline);

                // Act
                path.AddSegment(m_Segment1);
                string actual = path.ToString();

                // Assert
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
            public void Constructor_AddsSegments_ForEndArcSegment()
            {
                // Arrange
                var path = new Path(m_StartArcSegment,
                                    m_Line,
                                    m_EndArcSegment);

                // Act
                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 2 ];

                // Assert
                Assert.AreEqual(m_EndArcSegment,
                                actual);
            }

            [Test]
            public void Constructor_AddsSegments_ForMiddleSegment()
            {
                // Arrange
                var path = new Path(m_StartArcSegment,
                                    m_Line,
                                    m_EndArcSegment);

                // Act
                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 1 ];

                // Assert
                Assert.AreEqual(m_Line,
                                actual);
            }

            [Test]
            public void Constructor_AddsSegments_ForStartArcSegment()
            {
                // Arrange
                var path = new Path(m_StartArcSegment,
                                    m_Line,
                                    m_EndArcSegment);

                // Act
                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 0 ];

                // Assert
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
            public void Constructor_AddsSegments_ForEndArcSegment()
            {
                // Arrange
                var path = new Path(m_StartArcSegment,
                                    m_MiddleArcSegment,
                                    m_EndArcSegment);

                // Act
                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 2 ];

                // Assert
                Assert.AreEqual(m_EndArcSegment,
                                actual);
            }

            [Test]
            public void Constructor_AddsSegments_ForMiddleSegmentt()
            {
                // Arrange
                var path = new Path(m_StartArcSegment,
                                    m_MiddleArcSegment,
                                    m_EndArcSegment);

                // Act
                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 1 ];

                // Assert
                Assert.AreEqual(m_MiddleArcSegment,
                                actual);
            }

            [Test]
            public void Constructor_AddsSegments_ForStartArcSegment()
            {
                // Arrange
                var path = new Path(m_StartArcSegment,
                                    m_MiddleArcSegment,
                                    m_EndArcSegment);

                // Act
                IPolylineSegment[] segments = path.Segments.ToArray();
                IPolylineSegment actual = segments [ 0 ];

                // Assert
                Assert.AreEqual(m_StartArcSegment,
                                actual);
            }
        }

        #endregion
    }
}