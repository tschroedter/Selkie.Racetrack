using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Windsor.Interfaces;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;

namespace Core2.Selkie.Racetrack.Tests
{
    // todo cleanup all tests, make sure to use arrange, act, assert and method name


    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class PathValidatorTests
    {
        [SetUp]
        public void Setup()
        {
            m_DefaultTurnDirection = Constants.TurnDirection.Clockwise;

            m_Logger = Substitute.For <ISelkieLogger>();
            m_DefaultTurnDirection = Constants.TurnDirection.Clockwise;
        }

        private Constants.TurnDirection m_DefaultTurnDirection;
        private ISelkieLogger m_Logger;

        [NotNull]
        private PathValidator CreatePathValidator()
        {
            return new PathValidator(m_Logger);
        }

        [Test]
        public void IsValidArcSegmentReturnsFalseForLineClockwiseArcSegmentCounterclockwiseTest()
        {
            var line = new Line(0.0,
                                0.0,
                                10.0,
                                10.0);
            var centrePoint = new Point(5.0,
                                        0.0);
            var arcSegment = Substitute.For <ITurnCircleArcSegment>();
            arcSegment.CentrePoint.Returns(centrePoint);
            arcSegment.TurnDirection.Returns(Constants.TurnDirection.Counterclockwise);

            PathValidator validator = CreatePathValidator();

            Assert.False(validator.IsValidArcSegment(line,
                                                     arcSegment,
                                                     m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidArcSegmentReturnsFalseForLineCounterclockwiseArcSegmentClockwiseTest()
        {
            var line = new Line(10.0,
                                10.0,
                                0.0,
                                0.0);
            var centrePoint = new Point(0.0,
                                        5.0);
            var arcSegment = Substitute.For <ITurnCircleArcSegment>();
            arcSegment.CentrePoint.Returns(centrePoint);
            arcSegment.TurnDirection.Returns(Constants.TurnDirection.Counterclockwise);

            PathValidator validator = CreatePathValidator();

            Assert.False(validator.IsValidArcSegment(line,
                                                     arcSegment,
                                                     m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidArcSegmentReturnsTrueForLineClockwiseArcSegmentClockwiseTest()
        {
            var line = new Line(0.0,
                                0.0,
                                10.0,
                                10.0);
            var centrePoint = new Point(5.0,
                                        0.0);
            var arcSegment = Substitute.For <ITurnCircleArcSegment>();
            arcSegment.CentrePoint.Returns(centrePoint);
            arcSegment.TurnDirection.Returns(Constants.TurnDirection.Clockwise);

            PathValidator validator = CreatePathValidator();

            Assert.True(validator.IsValidArcSegment(line,
                                                    arcSegment,
                                                    m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidArcSegmentReturnsTrueForLineCounterclockwiseArcSegmentCounterclockwiseTest()
        {
            var line = new Line(0.0,
                                0.0,
                                10.0,
                                10.0);
            var centrePoint = new Point(0.0,
                                        5.0);
            var arcSegment = Substitute.For <ITurnCircleArcSegment>();
            arcSegment.CentrePoint.Returns(centrePoint);
            arcSegment.TurnDirection.Returns(Constants.TurnDirection.Counterclockwise);

            PathValidator validator = CreatePathValidator();

            Assert.True(validator.IsValidArcSegment(line,
                                                    arcSegment,
                                                    m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsFalseForCaseFourTest()
        {
            var startPoint = new Point(8.5,
                                       0.0);
            var finishPoint = new Point(13.5,
                                        0.0);
            var middleStart = new Point(11.0,
                                        -2.50);
            var middleEnd = new Point(16.0,
                                      -2.50);

            var startCircle = new Circle(11.0,
                                         0.0,
                                         2.5);
            var startArcSegment = new TurnCircleArcSegment(startCircle,
                                                           Constants.TurnDirection.Counterclockwise,
                                                           Constants.CircleOrigin.Start,
                                                           startPoint,
                                                           middleStart);

            var middleSegment = new Line(middleStart,
                                         middleEnd);

            var endCircle = new Circle(16.0,
                                       0.0,
                                       2.5);
            var endArcSegment = new TurnCircleArcSegment(endCircle,
                                                         Constants.TurnDirection.Clockwise,
                                                         Constants.CircleOrigin.Start,
                                                         middleEnd,
                                                         finishPoint);

            var path = new Path(startPoint);
            path.AddSegment(startArcSegment);
            path.AddSegment(middleSegment);
            path.AddSegment(endArcSegment);

            PathValidator validator = CreatePathValidator();

            Assert.False(validator.IsValid(path,
                                           m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsFalseForCaseOneTest()
        {
            var startPoint = new Point(13.5,
                                       2.5);
            var finishPoint = new Point(7.5,
                                        2.5);
            var middleStart = new Point(16.0,
                                        5.0);
            var middleEnd = new Point(10.0,
                                      5.0);

            var startCircle = new Circle(16.0,
                                         2.5,
                                         2.5);
            var startArcSegment = new TurnCircleArcSegment(startCircle,
                                                           Constants.TurnDirection.Clockwise,
                                                           Constants.CircleOrigin.Start,
                                                           startPoint,
                                                           middleStart);

            var middleSegment = new Line(middleStart,
                                         middleEnd);

            var endCircle = new Circle(10.0,
                                       2.5,
                                       2.5);
            var endArcSegment = new TurnCircleArcSegment(endCircle,
                                                         Constants.TurnDirection.Counterclockwise,
                                                         Constants.CircleOrigin.Start,
                                                         middleEnd,
                                                         finishPoint);

            var path = new Path(startPoint);
            path.AddSegment(startArcSegment);
            path.AddSegment(middleSegment);
            path.AddSegment(endArcSegment);

            PathValidator validator = CreatePathValidator();

            Assert.False(validator.IsValid(path,
                                           m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsFalseForCaseThreeTest()
        {
            var startPoint = new Point(8.5,
                                       0.0);
            var finishPoint = new Point(13.5,
                                        0.0);
            var middleStart = new Point(11.0,
                                        -2.50);
            var middleEnd = new Point(16.0,
                                      -2.50);

            var startCircle = new Circle(11.0,
                                         0.0,
                                         2.5);
            var startArcSegment = new TurnCircleArcSegment(startCircle,
                                                           Constants.TurnDirection.Clockwise,
                                                           Constants.CircleOrigin.Start,
                                                           startPoint,
                                                           middleStart);

            var middleSegment = new Line(middleStart,
                                         middleEnd);

            var endCircle = new Circle(16.0,
                                       0.0,
                                       2.5);
            var endArcSegment = new TurnCircleArcSegment(endCircle,
                                                         Constants.TurnDirection.Counterclockwise,
                                                         Constants.CircleOrigin.Start,
                                                         middleEnd,
                                                         finishPoint);

            var path = new Path(startPoint);
            path.AddSegment(startArcSegment);
            path.AddSegment(middleSegment);
            path.AddSegment(endArcSegment);

            PathValidator validator = CreatePathValidator();

            Assert.False(validator.IsValid(path,
                                           m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsFalseForCaseTwoTest()
        {
            var startPoint = new Point(18.5,
                                       2.5);
            var finishPoint = new Point(12.5,
                                        2.5);
            var middleStart = new Point(16.0,
                                        5.0);
            var middleEnd = new Point(10.0,
                                      5.0);

            var startCircle = new Circle(16.0,
                                         2.5,
                                         2.5);
            var startArcSegment = new TurnCircleArcSegment(startCircle,
                                                           Constants.TurnDirection.Counterclockwise,
                                                           Constants.CircleOrigin.Start,
                                                           startPoint,
                                                           middleStart);

            var middleSegment = new Line(middleStart,
                                         middleEnd);

            var endCircle = new Circle(10.0,
                                       2.5,
                                       2.5);
            var endArcSegment = new TurnCircleArcSegment(endCircle,
                                                         Constants.TurnDirection.Clockwise,
                                                         Constants.CircleOrigin.Start,
                                                         middleEnd,
                                                         finishPoint);

            var path = new Path(startPoint);
            path.AddSegment(startArcSegment);
            path.AddSegment(middleSegment);
            path.AddSegment(endArcSegment);

            PathValidator validator = CreatePathValidator();

            Assert.False(validator.IsValid(path,
                                           m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsFalseForFirstSegmentIsNotArcSegmentTest()
        {
            var line = new Line(-10.0,
                                -10.0,
                                10.0,
                                10.0);

            var segments = new List <IPolylineSegment>
                           {
                               Substitute.For <ILine>(),
                               line,
                               Substitute.For <ITurnCircleArcSegment>()
                           };

            var path = Substitute.For <IPath>();
            path.Segments.Returns(segments);

            PathValidator validator = CreatePathValidator();

            Assert.False(validator.IsValid(path,
                                           m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsFalseForFirstSegmentIsNotValidTest()
        {
            var line = new Line(0.0,
                                0.0,
                                10.0,
                                10.0);
            var centrePoint = new Point(5.0,
                                        0.0);
            var first = Substitute.For <ITurnCircleArcSegment>();
            first.CentrePoint.Returns(centrePoint);
            first.TurnDirection.Returns(Constants.TurnDirection.Counterclockwise);

            var segments = new List <IPolylineSegment>
                           {
                               first,
                               line,
                               Substitute.For <ITurnCircleArcSegment>()
                           };

            var path = Substitute.For <IPath>();
            path.Segments.Returns(segments);

            PathValidator validator = CreatePathValidator();

            Assert.False(validator.IsValid(path,
                                           m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsFalseForPathIsUnknownTest()
        {
            PathValidator validator = CreatePathValidator();

            Assert.False(validator.IsValid(Path.Unknown,
                                           m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsFalseForThirdSegmentIsNotArcSegmentTest()
        {
            var line = new Line(-10.0,
                                -10.0,
                                10.0,
                                10.0);

            var segments = new List <IPolylineSegment>
                           {
                               Substitute.For <ITurnCircleArcSegment>(),
                               line,
                               Substitute.For <ILine>()
                           };

            var path = Substitute.For <IPath>();
            path.Segments.Returns(segments);

            PathValidator validator = CreatePathValidator();

            Assert.False(validator.IsValid(path,
                                           m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsFalseForThirdSegmentIsNotValidTest()
        {
            var line = new Line(0.0,
                                0.0,
                                10.0,
                                10.0);
            var first = Substitute.For <ITurnCircleArcSegment>();
            first.CentrePoint.Returns(new Point(0.0,
                                                5.0));
            first.TurnDirection.Returns(Constants.TurnDirection.Counterclockwise);

            var third = Substitute.For <ITurnCircleArcSegment>();
            third.CentrePoint.Returns(new Point(5.0,
                                                0.0));
            third.TurnDirection.Returns(Constants.TurnDirection.Counterclockwise);

            var segments = new List <IPolylineSegment>
                           {
                               first,
                               line,
                               third
                           };

            var path = Substitute.For <IPath>();
            path.Segments.Returns(segments);

            PathValidator validator = CreatePathValidator();

            Assert.False(validator.IsValid(path,
                                           m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsTrueForAllSegmentsValidTest()
        {
            var line = new Line(0.0,
                                0.0,
                                10.0,
                                10.0);
            var first = Substitute.For <ITurnCircleArcSegment>();
            first.CentrePoint.Returns(new Point(0.0,
                                                5.0));
            first.TurnDirection.Returns(Constants.TurnDirection.Counterclockwise);

            var third = Substitute.For <ITurnCircleArcSegment>();
            third.CentrePoint.Returns(new Point(5.0,
                                                0.0));
            third.TurnDirection.Returns(Constants.TurnDirection.Clockwise);

            var segments = new List <IPolylineSegment>
                           {
                               first,
                               line,
                               third
                           };

            var path = Substitute.For <IPath>();
            path.Segments.Returns(segments);

            PathValidator validator = CreatePathValidator();

            Assert.True(validator.IsValid(path,
                                          m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsTrueForCaseFourTest()
        {
            var startPoint = new Point(8.5,
                                       0.0);
            var finishPoint = new Point(13.5,
                                        0.0);
            var middleStart = new Point(11.0,
                                        -2.50);
            var middleEnd = new Point(16.0,
                                      -2.50);

            var startCircle = new Circle(11.0,
                                         0.0,
                                         2.5);
            var startArcSegment = new TurnCircleArcSegment(startCircle,
                                                           Constants.TurnDirection.Counterclockwise,
                                                           Constants.CircleOrigin.Start,
                                                           startPoint,
                                                           middleStart);

            var middleSegment = new Line(middleStart,
                                         middleEnd);

            var endCircle = new Circle(16.0,
                                       0.0,
                                       2.5);
            var endArcSegment = new TurnCircleArcSegment(endCircle,
                                                         Constants.TurnDirection.Counterclockwise,
                                                         Constants.CircleOrigin.Start,
                                                         middleEnd,
                                                         finishPoint);

            var path = new Path(startPoint);
            path.AddSegment(startArcSegment);
            path.AddSegment(middleSegment);
            path.AddSegment(endArcSegment);

            PathValidator validator = CreatePathValidator();

            Assert.True(validator.IsValid(path,
                                          m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsTrueForCaseOneTest()
        {
            var startPoint = new Point(18.5,
                                       2.5);
            var finishPoint = new Point(7.5,
                                        2.5);
            var middleStart = new Point(16.0,
                                        5.0);
            var middleEnd = new Point(10.0,
                                      5.0);

            var startCircle = new Circle(16.0,
                                         2.5,
                                         2.5);
            var startArcSegment = new TurnCircleArcSegment(startCircle,
                                                           Constants.TurnDirection.Counterclockwise,
                                                           Constants.CircleOrigin.Start,
                                                           startPoint,
                                                           middleStart);

            var middleSegment = new Line(middleStart,
                                         middleEnd);

            var endCircle = new Circle(10.0,
                                       2.5,
                                       2.5);
            var endArcSegment = new TurnCircleArcSegment(endCircle,
                                                         Constants.TurnDirection.Counterclockwise,
                                                         Constants.CircleOrigin.Finish,
                                                         middleEnd,
                                                         finishPoint);

            var path = new Path(startPoint);
            path.AddSegment(startArcSegment);
            path.AddSegment(middleSegment);
            path.AddSegment(endArcSegment);

            PathValidator validator = CreatePathValidator();

            Assert.True(validator.IsValid(path,
                                          m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsTrueForCaseThreeTest()
        {
            var startPoint = new Point(8.5,
                                       0.0);
            var finishPoint = new Point(18.5,
                                        0.0);
            var middleStart = new Point(11.0,
                                        -2.50);
            var middleEnd = new Point(16.0,
                                      -2.50);

            var startCircle = new Circle(11.0,
                                         0.0,
                                         2.5);
            var startArcSegment = new TurnCircleArcSegment(startCircle,
                                                           Constants.TurnDirection.Counterclockwise,
                                                           Constants.CircleOrigin.Start,
                                                           startPoint,
                                                           middleStart);

            var middleSegment = new Line(middleStart,
                                         middleEnd);

            var endCircle = new Circle(16.0,
                                       0.0,
                                       2.5);
            var endArcSegment = new TurnCircleArcSegment(endCircle,
                                                         Constants.TurnDirection.Counterclockwise,
                                                         Constants.CircleOrigin.Start,
                                                         middleEnd,
                                                         finishPoint);

            var path = new Path(startPoint);
            path.AddSegment(startArcSegment);
            path.AddSegment(middleSegment);
            path.AddSegment(endArcSegment);

            PathValidator validator = CreatePathValidator();

            Assert.True(validator.IsValid(path,
                                          m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsTrueForCaseTwoTest()
        {
            var startPoint = new Point(18.5,
                                       2.5);
            var finishPoint = new Point(12.5,
                                        2.5);
            var middleStart = new Point(16.0,
                                        5.0);
            var middleEnd = new Point(10.0,
                                      5.0);

            var startCircle = new Circle(16.0,
                                         2.5,
                                         2.5);
            var startArcSegment = new TurnCircleArcSegment(startCircle,
                                                           Constants.TurnDirection.Counterclockwise,
                                                           Constants.CircleOrigin.Start,
                                                           startPoint,
                                                           middleStart);

            var middleSegment = new Line(middleStart,
                                         middleEnd);

            var endCircle = new Circle(10.0,
                                       2.5,
                                       2.5);
            var endArcSegment = new TurnCircleArcSegment(endCircle,
                                                         Constants.TurnDirection.Counterclockwise,
                                                         Constants.CircleOrigin.Start,
                                                         middleEnd,
                                                         finishPoint);

            var path = new Path(startPoint);
            path.AddSegment(startArcSegment);
            path.AddSegment(middleSegment);
            path.AddSegment(endArcSegment);

            PathValidator validator = CreatePathValidator();

            Assert.True(validator.IsValid(path,
                                          m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsTrueForMiddleSegmentIsLineTest()
        {
            var segments = new List <IPolylineSegment>
                           {
                               Substitute.For <ITurnCircleArcSegment>(),
                               Substitute.For <ILine>(),
                               Substitute.For <ITurnCircleArcSegment>()
                           };

            var path = Substitute.For <IPath>();
            path.Segments.Returns(segments);

            PathValidator validator = CreatePathValidator();

            Assert.True(validator.IsValid(path,
                                          m_DefaultTurnDirection));
        }

        [Test]
        public void IsValidReturnsTrueForMiddleSegmentIsLineWithSameStartEndPointTest()
        {
            var line = new Line(10.0,
                                10.0,
                                10.0,
                                10.0);

            var segments = new List <IPolylineSegment>
                           {
                               Substitute.For <ITurnCircleArcSegment>(),
                               line,
                               Substitute.For <ITurnCircleArcSegment>()
                           };

            var path = Substitute.For <IPath>();
            path.Segments.Returns(segments);

            PathValidator validator = CreatePathValidator();

            Assert.True(validator.IsValid(path,
                                          m_DefaultTurnDirection));
        }
    }
}