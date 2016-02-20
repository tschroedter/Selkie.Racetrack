using System;
using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Windsor;

namespace Selkie.Racetrack
{
    [ProjectComponent(Lifestyle.Transient)]
    public class Settings
        : ISettings,
          IEquatable <Settings>
    {
        public static readonly Settings Unknown = new Settings();
        private readonly Angle m_FinishAzimuth;
        private readonly Point m_FinishPoint;
        private readonly bool m_IsPortTurnAllowed;
        private readonly bool m_IsStarboardTurnAllowed;

        private readonly bool m_IsUnknown;
        private readonly Distance m_LargestRadiusForTurn;
        private readonly Distance m_RadiusForPortTurn;
        private readonly Distance m_RadiusForStarboardTurn;
        private readonly Angle m_StartAzimuth;
        private readonly Point m_StartPoint;

        private Settings()
        {
            m_StartPoint = Point.Unknown;
            m_StartAzimuth = Angle.Unknown;
            m_FinishPoint = Point.Unknown;
            m_FinishAzimuth = Angle.Unknown;
            m_RadiusForPortTurn = Distance.Unknown;
            m_RadiusForStarboardTurn = Distance.Unknown;
            m_IsPortTurnAllowed = true;
            m_IsStarboardTurnAllowed = true;
            m_IsUnknown = true;
            m_LargestRadiusForTurn = Distance.Unknown;
        }

        // ReSharper disable once TooManyDependencies
        public Settings([NotNull] Point startPoint,
                        [NotNull] Angle startAzimuth,
                        [NotNull] Point finishPoint,
                        [NotNull] Angle finishAzimuth,
                        [NotNull] Distance radiusForPortTurn,
                        [NotNull] Distance radiusForStarboardTurn,
                        bool isPortTurnAllowed,
                        bool isStarboardTurnAllowed)
        {
            m_StartPoint = startPoint;
            m_StartAzimuth = startAzimuth;
            m_FinishPoint = finishPoint;
            m_FinishAzimuth = finishAzimuth;
            m_RadiusForPortTurn = radiusForPortTurn;
            m_RadiusForStarboardTurn = radiusForStarboardTurn;
            m_IsPortTurnAllowed = isPortTurnAllowed;
            m_IsStarboardTurnAllowed = isStarboardTurnAllowed;
            m_LargestRadiusForTurn = RadiusForPortTurn >= RadiusForStarboardTurn
                                         ? RadiusForPortTurn
                                         : RadiusForStarboardTurn;
        }

        #region IEquatable<Settings> Members

        // ReSharper disable once CodeAnnotationAnalyzer
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
            if ( obj.GetType() != typeof ( Settings ) )
            {
                return false;
            }
            return Equals(( Settings ) obj);
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

        #region ISettings Members

        public bool IsUnknown
        {
            get
            {
                return m_IsUnknown;
            }
        }

        public bool IsPortTurnAllowed
        {
            get
            {
                return m_IsPortTurnAllowed;
            }
        }

        public bool IsStarboardTurnAllowed
        {
            get
            {
                return m_IsStarboardTurnAllowed;
            }
        }

        public Point StartPoint
        {
            get
            {
                return m_StartPoint;
            }
        }

        public Angle StartAzimuth
        {
            get
            {
                return m_StartAzimuth;
            }
        }

        public Point FinishPoint
        {
            get
            {
                return m_FinishPoint;
            }
        }

        public Angle FinishAzimuth
        {
            get
            {
                return m_FinishAzimuth;
            }
        }

        public Distance LargestRadiusForTurn
        {
            get
            {
                return m_LargestRadiusForTurn;
            }
        }

        public Distance RadiusForPortTurn
        {
            get
            {
                return m_RadiusForPortTurn;
            }
        }

        public Distance RadiusForStarboardTurn
        {
            get
            {
                return m_RadiusForStarboardTurn;
            }
        }

        public Constants.TurnDirection DefaultTurnDirection
        {
            get
            {
                return IsPortTurnAllowed
                           ? Constants.TurnDirection.Counterclockwise
                           : Constants.TurnDirection.Clockwise;
            }
        }

        #endregion
    }
}