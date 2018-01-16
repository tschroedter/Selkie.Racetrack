using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.NUnit.Extensions;
using Constants = Core2.Selkie.Geometry.Constants;

namespace Core2.Selkie.Racetrack.Tests.Turn
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class TurnCircleArcSegmentTests
    {
        [SetUp]
        public void Setup()
        {
            m_Circle = new Circle(0.0,
                                  0.0,
                                  100);
            m_StartPoint = new Point(100.0,
                                     0.0);
            m_EndPoint = new Point(0.0,
                                   100.0);

            m_Segment = new TurnCircleArcSegment(m_Circle,
                                                 Constants.TurnDirection.Clockwise,
                                                 Constants.CircleOrigin.Start,
                                                 m_StartPoint,
                                                 m_EndPoint);
        }

        private ICircle m_Circle;
        private Point m_EndPoint;
        private TurnCircleArcSegment m_Segment;
        private Point m_StartPoint;

        [Test]
        public void AngleTest()
        {
            Assert.AreEqual(Angle.For270Degrees,
                            m_Segment.Angle);
        }

        [Test]
        public void ArcSegmentTest()
        {
            Angle expectedAngle = Angle.For270Degrees;
            IArcSegment actual = m_Segment.ArcSegment;

            Assert.AreEqual(m_Circle.CentrePoint,
                            actual.CentrePoint,
                            "CentrePoint");
            Assert.AreEqual(m_StartPoint,
                            actual.StartPoint,
                            "StartPoint");
            Assert.AreEqual(m_EndPoint,
                            actual.EndPoint,
                            "EndPoint");
            Assert.AreEqual(expectedAngle,
                            actual.AngleClockwise,
                            "AngleClockwise");
        }

        [Test]
        public void CentrePointTest()
        {
            Assert.AreEqual(m_Circle.CentrePoint,
                            m_Segment.CentrePoint);
        }

        [Test]
        public void DirectionTest()
        {
            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            m_Segment.TurnDirection);
        }

        [Test]
        public void IsUnknownTest()
        {
            Assert.False(m_Segment.IsUnknown);
        }

        [Test]
        public void LengthClockwiseTest()
        {
            NUnitHelper.AssertIsEquivalent(471.23889803846896,
                                           m_Segment.LengthClockwise,
                                           Constants.EpsilonDistance,
                                           "LengthClockwise");
        }

        [Test]
        public void LengthCounterClockwiseTest()
        {
            NUnitHelper.AssertIsEquivalent(157.07963267948966,
                                           m_Segment.LengthCounterClockwise,
                                           Constants.EpsilonDistance,
                                           "LengthCounterClockwise");
        }

        [Test]
        public void LengthTest()
        {
            NUnitHelper.AssertIsEquivalent(471.23889803846896,
                                           m_Segment.Length,
                                           Constants.EpsilonDistance,
                                           "Length");
        }

        [Test]
        public void OriginTest()
        {
            Assert.AreEqual(Constants.CircleOrigin.Start,
                            m_Segment.CircleOrigin);
        }

        [Test]
        public void RadiusTest()
        {
            Assert.AreEqual(100.0,
                            m_Segment.Radius);
        }

        [Test]
        public void ReverseTest()
        {
            var actual = m_Segment.Reverse() as ITurnCircleArcSegment;

            Assert.NotNull(actual);
            Assert.AreEqual(m_Segment.LengthCounterClockwise,
                            actual.Length,
                            "Length");
            Assert.AreEqual(m_Segment.EndPoint,
                            actual.StartPoint,
                            "StartPoint");
            Assert.AreEqual(m_Segment.StartPoint,
                            actual.EndPoint,
                            "EndPoint");
            Assert.AreEqual(m_Segment.AngleCounterClockwise,
                            actual.AngleClockwise,
                            "AngleClockwise");
            Assert.AreEqual(m_Segment.TurnDirection,
                            actual.TurnDirection,
                            "Direction");
        }

        [Test]
        public void StartPointTest()
        {
            Assert.AreEqual(m_StartPoint,
                            m_Segment.StartPoint);
        }

        [Test]
        public void ToStringTest()
        {
            const string expected = "CentrePoint: [0,0] StartPoint: [100,0] EndPoint: [0,100] Direction: Clockwise";
            string actual = m_Segment.ToString();

            Assert.AreEqual(expected,
                            actual);
        }

        [Test]
        public void UnknownTest()
        {
            ITurnCircleArcSegment actual = TurnCircleArcSegment.Unknown;

            Assert.True(actual.IsUnknown,
                        "IsUnknown");
            Assert.True(actual.ArcSegment.IsUnknown,
                        "ArcSegment.IsUnknown");
        }
    }
}