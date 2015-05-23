using System;
using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Windsor;

namespace Selkie.Racetrack.Turn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class TurnCircle
        : ITurnCircle,
          IEquatable <TurnCircle>
    {
        public static TurnCircle Unknown = new TurnCircle();
        private readonly ICircle m_Circle;
        private readonly bool m_IsUnknown;
        private readonly Constants.CircleOrigin m_Origin;
        private readonly Distance m_Radius;
        private readonly Constants.CircleSide m_Side;
        private readonly Constants.TurnDirection m_TurnDirection;

        private TurnCircle()
        {
            m_Circle = Geometry.Shapes.Circle.Unknown;
            m_Side = Constants.CircleSide.Unknown;
            m_Origin = Constants.CircleOrigin.Unknown;
            m_TurnDirection = Constants.TurnDirection.Unknown;
            m_Radius = Distance.Unknown;
            m_IsUnknown = true;
        }

        public TurnCircle([NotNull] ICircle circle,
                          Constants.CircleSide side,
                          Constants.CircleOrigin origin,
                          Constants.TurnDirection turnDirection)
        {
            m_Circle = circle;
            m_Side = side;
            m_Origin = origin;
            m_TurnDirection = turnDirection;
            m_Radius = new Distance(m_Circle.Radius);
        }

        #region IEquatable<TurnCircle> Members

        // ReSharper disable once CodeAnnotationAnalyzer
        public bool Equals(TurnCircle other)
        {
            if ( ReferenceEquals(null,
                                 other) )
            {
                return false;
            }
            if ( ReferenceEquals(this,
                                 other) )
            {
                return true;
            }
            return Equals(other.m_Circle,
                          m_Circle) && Equals(other.m_Side,
                                              m_Side) && Equals(other.m_Origin,
                                                                m_Origin) && Equals(other.m_TurnDirection,
                                                                                    m_TurnDirection);
        }

        #endregion

        public bool IsUnknown
        {
            get
            {
                return m_IsUnknown;
            }
        }

        // ReSharper disable once CodeAnnotationAnalyzer
        public override bool Equals(object obj)
        {
            if ( ReferenceEquals(null,
                                 obj) )
            {
                return false;
            }
            if ( ReferenceEquals(this,
                                 obj) )
            {
                return true;
            }
            if ( obj.GetType() != typeof ( TurnCircle ) )
            {
                return false;
            }
            return Equals(( TurnCircle ) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = ( m_Circle != null
                                   ? m_Circle.GetHashCode()
                                   : 0 );
                result = ( result * 397 ) ^ m_Side.GetHashCode();
                result = ( result * 397 ) ^ m_Origin.GetHashCode();
                result = ( result * 397 ) ^ m_TurnDirection.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(TurnCircle left,
                                       TurnCircle right)
        {
            return Equals(left,
                          right);
        }

        public static bool operator !=(TurnCircle left,
                                       TurnCircle right)
        {
            return !Equals(left,
                           right);
        }

        #region ITurnCircle Members

        public ICircle Circle
        {
            get
            {
                return m_Circle;
            }
        }

        public Point CentrePoint
        {
            get
            {
                return m_Circle.CentrePoint;
            }
        }

        public Distance Radius
        {
            get
            {
                return m_Radius;
            }
        }

        public Constants.CircleSide Side
        {
            get
            {
                return m_Side;
            }
        }

        public Constants.CircleOrigin Origin
        {
            get
            {
                return m_Origin;
            }
        }

        public bool IsPointOnCircle(Point point)
        {
            return m_Circle.IsPointOnCircle(point);
        }

        public Constants.TurnDirection TurnDirection
        {
            get
            {
                return m_TurnDirection;
            }
        }

        #endregion
    }
}