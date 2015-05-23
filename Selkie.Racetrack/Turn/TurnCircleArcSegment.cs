using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Racetrack.Turn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class TurnCircleArcSegment : ITurnCircleArcSegment
    {
        public static readonly ITurnCircleArcSegment Unknown = new TurnCircleArcSegment();
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
            return "CentrePoint: {0} StartPoint: {1} EndPoint: {2} Direction: {3}".Inject(m_ArcSegment.CentrePoint,
                                                                                          m_ArcSegment.StartPoint,
                                                                                          m_ArcSegment.EndPoint,
                                                                                          m_ArcSegment.TurnDirection);
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

        public double Length
        {
            get
            {
                return m_ArcSegment.TurnDirection == Constants.TurnDirection.Clockwise
                           ? m_ArcSegment.LengthClockwise
                           : m_ArcSegment.LengthCounterClockwise;
            }
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