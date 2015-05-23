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
        private readonly Distance m_Radius;
        private readonly Angle m_StartAzimuth;
        private readonly Point m_StartPoint;

        private Settings()
        {
            m_StartPoint = Point.Unknown;
            m_StartAzimuth = Angle.Unknown;
            m_FinishPoint = Point.Unknown;
            m_FinishAzimuth = Angle.Unknown;
            m_Radius = Distance.Unknown;
            m_IsPortTurnAllowed = true;
            m_IsStarboardTurnAllowed = true;
            m_IsUnknown = true;
        }

        // ReSharper disable once TooManyDependencies
        public Settings([NotNull] Point startPoint,
                        [NotNull] Angle startAzimuth,
                        [NotNull] Point finishPoint,
                        [NotNull] Angle finishAzimuth,
                        [NotNull] Distance radius,
                        bool isPortTurnAllowed,
                        bool isStarboardTurnAllowed)
        {
            m_StartPoint = startPoint;
            m_StartAzimuth = startAzimuth;
            m_FinishPoint = finishPoint;
            m_FinishAzimuth = finishAzimuth;
            m_Radius = radius;
            m_IsPortTurnAllowed = isPortTurnAllowed;
            m_IsStarboardTurnAllowed = isStarboardTurnAllowed;
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
            return Equals(other.m_StartPoint,
                          m_StartPoint) && Equals(other.m_StartAzimuth,
                                                  m_StartAzimuth) && Equals(other.m_FinishPoint,
                                                                            m_FinishPoint) &&
                   Equals(other.m_FinishAzimuth,
                          m_FinishAzimuth) && Equals(other.m_Radius,
                                                     m_Radius);
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
                int result = ( m_StartPoint != null
                                   ? m_StartPoint.GetHashCode()
                                   : 0 );
                result = ( result * 397 ) ^ ( m_StartAzimuth != null
                                                  ? m_StartAzimuth.GetHashCode()
                                                  : 0 );
                result = ( result * 397 ) ^ ( m_FinishPoint != null
                                                  ? m_FinishPoint.GetHashCode()
                                                  : 0 );
                result = ( result * 397 ) ^ ( m_FinishAzimuth != null
                                                  ? m_FinishAzimuth.GetHashCode()
                                                  : 0 );
                result = ( result * 397 ) ^ ( m_Radius != null
                                                  ? m_Radius.GetHashCode()
                                                  : 0 );
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

        public Distance Radius
        {
            get
            {
                return m_Radius;
            }
        }

        public Constants.TurnDirection DefaultTurnDirection
        {
            get
            {
                return m_IsPortTurnAllowed
                           ? Constants.TurnDirection.Counterclockwise
                           : Constants.TurnDirection.Clockwise;
            }
        }

        #endregion
    }
}