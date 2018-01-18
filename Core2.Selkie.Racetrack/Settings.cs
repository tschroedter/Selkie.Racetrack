using System;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack
{
    [ProjectComponent(Lifestyle.Transient)]
    public class Settings
        : ISettings,
          IEquatable <Settings>
    {
        private Settings()
        {
            StartPoint = Point.Unknown;
            StartAzimuth = Angle.Unknown;
            FinishPoint = Point.Unknown;
            FinishAzimuth = Angle.Unknown;
            RadiusForPortTurn = Distance.Unknown;
            RadiusForStarboardTurn = Distance.Unknown;
            IsPortTurnAllowed = true;
            IsStarboardTurnAllowed = true;
            IsUnknown = true;
            LargestRadiusForTurn = Distance.Unknown;
        }

        public Settings([NotNull] Point startPoint,
                        [NotNull] Angle startAzimuth,
                        [NotNull] Point finishPoint,
                        [NotNull] Angle finishAzimuth,
                        [NotNull] Distance radiusForPortTurn,
                        [NotNull] Distance radiusForStarboardTurn,
                        bool isPortTurnAllowed,
                        bool isStarboardTurnAllowed)
        {
            StartPoint = startPoint;
            StartAzimuth = startAzimuth;
            FinishPoint = finishPoint;
            FinishAzimuth = finishAzimuth;
            RadiusForPortTurn = radiusForPortTurn;
            RadiusForStarboardTurn = radiusForStarboardTurn;
            IsPortTurnAllowed = isPortTurnAllowed;
            IsStarboardTurnAllowed = isStarboardTurnAllowed;
            LargestRadiusForTurn = RadiusForPortTurn >= RadiusForStarboardTurn
                                       ? RadiusForPortTurn
                                       : RadiusForStarboardTurn;
        }

        public static readonly Settings Unknown = new Settings();

        #region IEquatable<Settings> Members

        public bool Equals(Settings other)
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
            return Equals(other.StartPoint,
                          StartPoint) && Equals(other.StartAzimuth,
                                                StartAzimuth) && Equals(other.FinishPoint,
                                                                        FinishPoint) &&
                   Equals(other.FinishAzimuth,
                          FinishAzimuth) && Equals(other.FinishAzimuth,
                                                   FinishAzimuth) &&
                   Equals(other.RadiusForPortTurn,
                          RadiusForPortTurn) && Equals(other.RadiusForPortTurn,
                                                       RadiusForPortTurn) &&
                   Equals(other.RadiusForStarboardTurn,
                          RadiusForStarboardTurn) && Equals(other.RadiusForStarboardTurn,
                                                            RadiusForStarboardTurn);
        }

        #endregion

        public static bool operator ==(Settings left,
                                       Settings right)
        {
            return Equals(left,
                          right);
        }

        public static bool operator !=(Settings left,
                                       Settings right)
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
            return obj.GetType() == typeof( Settings ) && Equals(( Settings ) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = StartPoint.GetHashCode();

                result = ( result * 397 ) ^ StartAzimuth.GetHashCode();
                result = ( result * 397 ) ^ FinishPoint.GetHashCode();
                result = ( result * 397 ) ^ FinishAzimuth.GetHashCode();
                result = ( result * 397 ) ^ RadiusForPortTurn.GetHashCode();
                result = ( result * 397 ) ^ RadiusForStarboardTurn.GetHashCode();

                return result;
            }
        }

        #region ISettings Members

        public bool IsUnknown { get; }

        public bool IsPortTurnAllowed { get; }

        public bool IsStarboardTurnAllowed { get; }

        public Point StartPoint { get; }

        public Angle StartAzimuth { get; }

        public Point FinishPoint { get; }

        public Angle FinishAzimuth { get; }

        public Distance LargestRadiusForTurn { get; }

        public Distance RadiusForPortTurn { get; }

        public Distance RadiusForStarboardTurn { get; }

        public Constants.TurnDirection DefaultTurnDirection => IsPortTurnAllowed
                                                                   ? Constants.TurnDirection.Counterclockwise
                                                                   : Constants.TurnDirection.Clockwise;

        #endregion
    }
}