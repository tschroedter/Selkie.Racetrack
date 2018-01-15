using JetBrains.Annotations;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Windsor;

namespace Core2.Selkie.Racetrack.Turn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class TurnCircleArcSegment : Interfaces.Turn.ITurnCircleArcSegment
    {
        public static readonly Interfaces.Turn.ITurnCircleArcSegment Unknown = new TurnCircleArcSegment();
        private readonly IArcSegment m_ArcSegment;
        private readonly Constants.CircleOrigin m_CircleOrigin;
        private readonly Constants.TurnDirection m_Direction;
        private readonly bool m_IsUnknown;

        private TurnCircleArcSegment()
        {
            m_IsUnknown = true;
            m_ArcSegment = Geometry.Shapes.ArcSegment.Unknown;
        }

        private TurnCircleArcSegment([NotNull] IArcSegment arcSegment,
                                     Constants.TurnDirection direction,
                                     Constants.CircleOrigin circleOrigin)
        {
            m_ArcSegment = arcSegment;
            m_Direction = direction;
            m_CircleOrigin = circleOrigin;
        }

        public TurnCircleArcSegment([NotNull] ICircle circle,
                                    Constants.TurnDirection direction,
                                    Constants.CircleOrigin circleOrigin,
                                    [NotNull] Point startPoint,
                                    [NotNull] Point endPoint)
        {
            m_ArcSegment = new ArcSegment(circle,
                                          startPoint,
                                          endPoint,
                                          direction);

            m_Direction = direction;
            m_CircleOrigin = circleOrigin;
        }

        public Angle Angle
        {
            get
            {
                return m_ArcSegment.TurnDirection == Constants.TurnDirection.Clockwise
                           ? m_ArcSegment.AngleClockwise
                           : m_ArcSegment.AngleCounterClockwise;
            }
        }

        public override string ToString()
        {
            return $"CentrePoint: {m_ArcSegment.CentrePoint} StartPoint: {m_ArcSegment.StartPoint} EndPoint: {m_ArcSegment.EndPoint} Direction: {m_ArcSegment.TurnDirection}";
        }

        #region ITurnCircleArcSegment Members

        public bool IsUnknown
        {
            get
            {
                return m_IsUnknown;
            }
        }

        public IArcSegment ArcSegment
        {
            get
            {
                return m_ArcSegment;
            }
        }

        public Point CentrePoint
        {
            get
            {
                return m_ArcSegment.CentrePoint;
            }
        }

        public Angle AngleClockwise
        {
            get
            {
                return m_ArcSegment.AngleClockwise;
            }
        }

        public Angle AngleCounterClockwise
        {
            get
            {
                return m_ArcSegment.AngleCounterClockwise;
            }
        }

        public Point StartPoint
        {
            get
            {
                return m_ArcSegment.StartPoint;
            }
        }

        public Point EndPoint
        {
            get
            {
                return m_ArcSegment.EndPoint;
            }
        }

        public Angle AngleToXAxisAtEndPoint { get; }
        public Angle AngleToXAxisAtStartPoint { get; }

        public Constants.TurnDirection TurnDirectionToPoint(Point point)
        {
            return m_ArcSegment.TurnDirectionToPoint(point); // todo testing
        }

        public double Length
        {
            get
            {
                return m_ArcSegment.TurnDirection == Constants.TurnDirection.Clockwise
                           ? m_ArcSegment.LengthClockwise
                           : m_ArcSegment.LengthCounterClockwise;
            }
        }

        public bool IsOnLine(Point point)
        {
            return m_ArcSegment.IsOnLine(point); // todo testing
        }

        public IPolylineSegment Reverse()
        {
            var arcSegment = m_ArcSegment.Reverse() as IArcSegment;

            if ( arcSegment == null )
            {
                // todo maybe log this???
                return Unknown;
            }

            Constants.CircleOrigin origin = m_CircleOrigin == Constants.CircleOrigin.Start
                                                ? Constants.CircleOrigin.Finish
                                                : Constants.CircleOrigin.Start;

            var reverse = new TurnCircleArcSegment(arcSegment,
                                                   m_Direction,
                                                   origin);

            return reverse;
        }

        public double Radius
        {
            get
            {
                return m_ArcSegment.Radius;
            }
        }

        public double LengthClockwise
        {
            get
            {
                return m_ArcSegment.LengthClockwise;
            }
        }

        public double LengthCounterClockwise
        {
            get
            {
                return m_ArcSegment.LengthCounterClockwise;
            }
        }

        public Constants.TurnDirection TurnDirection
        {
            get
            {
                return m_Direction;
            }
        }

        public Constants.CircleOrigin CircleOrigin
        {
            get
            {
                return m_CircleOrigin;
            }
        }

        #endregion
    }
}