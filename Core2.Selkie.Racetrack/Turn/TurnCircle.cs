using System;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Turn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class TurnCircle
        : ITurnCircle,
          IEquatable <TurnCircle>
    {
        private TurnCircle()
        {
            Circle = Geometry.Shapes.Circle.Unknown;
            Side = Constants.CircleSide.Unknown;
            Origin = Constants.CircleOrigin.Unknown;
            TurnDirection = Constants.TurnDirection.Unknown;
            Radius = Distance.Unknown;
            IsUnknown = true;
        }

        public TurnCircle([NotNull] ICircle circle,
                          Constants.CircleSide side,
                          Constants.CircleOrigin origin,
                          Constants.TurnDirection turnDirection)
        {
            Circle = circle;
            Side = side;
            Origin = origin;
            TurnDirection = turnDirection;
            Radius = new Distance(Circle.Radius);
        }

        public static TurnCircle Unknown = new TurnCircle();

        #region IEquatable<TurnCircle> Members

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
            return Equals(other.Circle,
                          Circle) && Equals(other.Side,
                                            Side) && Equals(other.Origin,
                                                            Origin) && Equals(other.TurnDirection,
                                                                              TurnDirection);
        }

        #endregion

        public bool IsUnknown { get; }

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
            if ( obj.GetType() != typeof( TurnCircle ) )
            {
                return false;
            }
            return Equals(( TurnCircle ) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Circle != null
                                 ? Circle.GetHashCode()
                                 : 0;
                result = ( result * 397 ) ^ Side.GetHashCode();
                result = ( result * 397 ) ^ Origin.GetHashCode();
                result = ( result * 397 ) ^ TurnDirection.GetHashCode();
                return result;
            }
        }

        #region ITurnCircle Members

        public ICircle Circle { get; }

        public Point CentrePoint => Circle.CentrePoint;

        public Distance Radius { get; }

        public Constants.CircleSide Side { get; }

        public Constants.CircleOrigin Origin { get; }

        public bool IsPointOnCircle(Point point)
        {
            return Circle.IsPointOnCircle(point);
        }

        public Constants.TurnDirection TurnDirection { get; }

        #endregion
    }
}