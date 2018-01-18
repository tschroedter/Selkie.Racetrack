using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;
using ITurnCircleArcSegment = Core2.Selkie.Racetrack.Interfaces.Turn.ITurnCircleArcSegment;

namespace Core2.Selkie.Racetrack.Turn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class TurnCircleArcSegment : ITurnCircleArcSegment
    {
        private TurnCircleArcSegment()
        {
            IsUnknown = true;
            ArcSegment = Geometry.Shapes.ArcSegment.Unknown;
        }

        private TurnCircleArcSegment([NotNull] IArcSegment arcSegment,
                                     Constants.TurnDirection direction,
                                     Constants.CircleOrigin circleOrigin)
        {
            ArcSegment = arcSegment;
            TurnDirection = direction;
            CircleOrigin = circleOrigin;
        }

        public TurnCircleArcSegment([NotNull] ICircle circle,
                                    Constants.TurnDirection direction,
                                    Constants.CircleOrigin circleOrigin,
                                    [NotNull] Point startPoint,
                                    [NotNull] Point endPoint)
        {
            ArcSegment = new ArcSegment(circle,
                                        startPoint,
                                        endPoint,
                                        direction);

            TurnDirection = direction;
            CircleOrigin = circleOrigin;
        }

        public static readonly ITurnCircleArcSegment Unknown = new TurnCircleArcSegment();

        public Angle Angle => ArcSegment.TurnDirection == Constants.TurnDirection.Clockwise
                                  ? ArcSegment.AngleClockwise
                                  : ArcSegment.AngleCounterClockwise;

        public override string ToString()
        {
            return
                $"CentrePoint: {ArcSegment.CentrePoint} StartPoint: {ArcSegment.StartPoint} EndPoint: {ArcSegment.EndPoint} Direction: {ArcSegment.TurnDirection}";
        }

        #region ITurnCircleArcSegment Members

        public bool IsUnknown { get; }

        public IArcSegment ArcSegment { get; }

        public Point CentrePoint => ArcSegment.CentrePoint;

        public Angle AngleClockwise => ArcSegment.AngleClockwise;

        public Angle AngleCounterClockwise => ArcSegment.AngleCounterClockwise;

        public Point StartPoint => ArcSegment.StartPoint;

        public Point EndPoint => ArcSegment.EndPoint;

        public Angle AngleToXAxisAtEndPoint { get; }
        public Angle AngleToXAxisAtStartPoint { get; }

        public Constants.TurnDirection TurnDirectionToPoint(Point point)
        {
            return ArcSegment.TurnDirectionToPoint(point); // todo testing
        }

        public double Length => ArcSegment.TurnDirection == Constants.TurnDirection.Clockwise
                                    ? ArcSegment.LengthClockwise
                                    : ArcSegment.LengthCounterClockwise;

        public bool IsOnLine(Point point)
        {
            return ArcSegment.IsOnLine(point); // todo testing
        }

        public IPolylineSegment Reverse()
        {
            var arcSegment = ArcSegment.Reverse() as IArcSegment;

            if ( arcSegment == null )
            {
                // todo maybe log this???
                return Unknown;
            }

            Constants.CircleOrigin origin = CircleOrigin == Constants.CircleOrigin.Start
                                                ? Constants.CircleOrigin.Finish
                                                : Constants.CircleOrigin.Start;

            var reverse = new TurnCircleArcSegment(arcSegment,
                                                   TurnDirection,
                                                   origin);

            return reverse;
        }

        public double Radius => ArcSegment.Radius;

        public double LengthClockwise => ArcSegment.LengthClockwise;

        public double LengthCounterClockwise => ArcSegment.LengthCounterClockwise;

        public Constants.TurnDirection TurnDirection { get; }

        public Constants.CircleOrigin CircleOrigin { get; }

        #endregion
    }
}