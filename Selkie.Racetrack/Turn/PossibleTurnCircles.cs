using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Windsor;

namespace Selkie.Racetrack.Turn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class PossibleTurnCircles : IPossibleTurnCircles
    {
        public static readonly IPossibleTurnCircles Unknown = new PossibleTurnCircles(true);
        private readonly bool m_IsUnknown;
        private ITurnCircle m_FinishTurnCirclePort = TurnCircle.Unknown;
        private ITurnCircle m_FinishTurnCircleStarboard = TurnCircle.Unknown;
        private ISettings m_Settings = Racetrack.Settings.Unknown;
        private ITurnCircle m_StartTurnCirclePort = TurnCircle.Unknown;
        private ITurnCircle m_StartTurnCircleStarboard = TurnCircle.Unknown;

        public PossibleTurnCircles()
        {
        }

        private PossibleTurnCircles(bool isUnknown)
        {
            m_IsUnknown = isUnknown;
        }

        public void Calculate()
        {
            m_StartTurnCirclePort = CreateTurnCircleStartPointPort();
            m_StartTurnCircleStarboard = CreateTurnCircleStartPointStarboard();
            m_FinishTurnCirclePort = CreateTurnCircleFinishPointPort();
            m_FinishTurnCircleStarboard = CreateTurnCircleFinishPointStarboard();
        }

        [NotNull]
        internal ITurnCircle CreateTurnCircleStartPointPort()
        {
            Point centrePoint = CalculateCentrePointForStartPointPort();
            Distance radius = m_Settings.Radius;
            var circle = new Circle(centrePoint,
                                    radius.Length);

            var turnCircle = new TurnCircle(circle,
                                            Constants.CircleSide.Port,
                                            Constants.CircleOrigin.Start,
                                            Constants.TurnDirection.Counterclockwise);

            return turnCircle;
        }

        [NotNull]
        internal ITurnCircle CreateTurnCircleStartPointStarboard()
        {
            Point centrePoint = CalculateCentrePointForStartPointStarboard();
            Distance radius = m_Settings.Radius;
            var circle = new Circle(centrePoint,
                                    radius.Length);

            var turnCircle = new TurnCircle(circle,
                                            Constants.CircleSide.Starboard,
                                            Constants.CircleOrigin.Start,
                                            Constants.TurnDirection.Clockwise);

            return turnCircle;
        }

        [NotNull]
        internal Point CalculateCentrePointForStartPointPort()
        {
            Point centrePoint = m_Settings.StartPoint;
            Distance radius = m_Settings.Radius;
            Angle azimuth = m_Settings.StartAzimuth + Angle.For90Degrees;

            Point point = CalculateCentrePoint(centrePoint,
                                               radius,
                                               azimuth);

            return point;
        }

        [NotNull]
        internal Point CalculateCentrePointForStartPointStarboard()
        {
            Point centrePoint = m_Settings.StartPoint;
            Distance radius = m_Settings.Radius;
            Angle azimuth = m_Settings.StartAzimuth - Angle.FromDegrees(90.0);

            Point point = CalculateCentrePoint(centrePoint,
                                               radius,
                                               azimuth);

            return point;
        }

        [NotNull]
        internal ITurnCircle CreateTurnCircleFinishPointPort()
        {
            Point centrePoint = CalculateCentrePointForFinishPointPort();
            Distance radius = m_Settings.Radius;
            var circle = new Circle(centrePoint,
                                    radius.Length);

            var turnCircle = new TurnCircle(circle,
                                            Constants.CircleSide.Port,
                                            Constants.CircleOrigin.Finish,
                                            Constants.TurnDirection.Counterclockwise);

            return turnCircle;
        }

        [NotNull]
        internal ITurnCircle CreateTurnCircleFinishPointStarboard()
        {
            Point centrePoint = CalculateCentrePointForFinishPointStarboard();
            Distance radius = m_Settings.Radius;
            var circle = new Circle(centrePoint,
                                    radius.Length);

            var turnCircle = new TurnCircle(circle,
                                            Constants.CircleSide.Starboard,
                                            Constants.CircleOrigin.Finish,
                                            Constants.TurnDirection.Clockwise);

            return turnCircle;
        }

        [NotNull]
        internal Point CalculateCentrePointForFinishPointPort()
        {
            Point centrePoint = m_Settings.FinishPoint;
            Distance radius = m_Settings.Radius;
            Angle azimuth = PortAzimuth(m_Settings.FinishAzimuth);

            Point point = CalculateCentrePoint(centrePoint,
                                               radius,
                                               azimuth);

            return point;
        }

        [NotNull]
        internal Angle PortAzimuth([NotNull] Angle azimuth)
        {
            Angle portAzimuth = azimuth + Angle.FromDegrees(90.0);

            return portAzimuth;
        }

        [NotNull]
        internal Point CalculateCentrePointForFinishPointStarboard()
        {
            Point centrePoint = m_Settings.FinishPoint;
            Distance radius = m_Settings.Radius;
            Angle azimuth = StarboardAzimuth(m_Settings.FinishAzimuth);

            Point point = CalculateCentrePoint(centrePoint,
                                               radius,
                                               azimuth);

            return point;
        }

        [NotNull]
        internal Angle StarboardAzimuth([NotNull] Angle azimuth)
        {
            Angle portAzimuth = azimuth - Angle.FromDegrees(90.0);

            return portAzimuth;
        }

        [NotNull]
        private Point CalculateCentrePoint([NotNull] Point centrePoint,
                                           [NotNull] Distance radius,
                                           [NotNull] Angle azimuth)
        {
            ICircle circle = new Circle(centrePoint,
                                        radius.Length);
            Point point = circle.PointOnCircle(azimuth);

            return point;
        }

        #region IPossibleTurnCircles Members

        public bool IsUnknown
        {
            get
            {
                return m_IsUnknown;
            }
        }

        public ISettings Settings
        {
            get
            {
                return m_Settings;
            }
            set
            {
                m_Settings = value;
            }
        }

        public ITurnCircle StartTurnCirclePort
        {
            get
            {
                return m_StartTurnCirclePort;
            }
        }

        public ITurnCircle StartTurnCircleStarboard
        {
            get
            {
                return m_StartTurnCircleStarboard;
            }
        }

        public ITurnCircle FinishTurnCirclePort
        {
            get
            {
                return m_FinishTurnCirclePort;
            }
        }

        public ITurnCircle FinishTurnCircleStarboard
        {
            get
            {
                return m_FinishTurnCircleStarboard;
            }
        }

        #endregion
    }
}