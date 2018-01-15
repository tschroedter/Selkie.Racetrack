using JetBrains.Annotations;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Windsor;

namespace Core2.Selkie.Racetrack.Turn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class PossibleTurnCircles : IPossibleTurnCircles
    {
        public PossibleTurnCircles()
        {
            FinishTurnCircleStarboard = TurnCircle.Unknown;
            FinishTurnCirclePort = TurnCircle.Unknown;
            StartTurnCircleStarboard = TurnCircle.Unknown;
            StartTurnCirclePort = TurnCircle.Unknown;
            Settings = Racetrack.Settings.Unknown;
        }

        private PossibleTurnCircles(bool isUnknown)
        {
            FinishTurnCircleStarboard = TurnCircle.Unknown;
            FinishTurnCirclePort = TurnCircle.Unknown;
            StartTurnCircleStarboard = TurnCircle.Unknown;
            StartTurnCirclePort = TurnCircle.Unknown;
            Settings = Racetrack.Settings.Unknown;
            IsUnknown = isUnknown;
        }

        public static readonly IPossibleTurnCircles Unknown = new PossibleTurnCircles(true);

        public void Calculate()
        {
            StartTurnCirclePort = CreateTurnCircleStartPointPort();
            StartTurnCircleStarboard = CreateTurnCircleStartPointStarboard();
            FinishTurnCirclePort = CreateTurnCircleFinishPointPort();
            FinishTurnCircleStarboard = CreateTurnCircleFinishPointStarboard();
        }

        [NotNull]
        internal Point CalculateCentrePointForFinishPointPort()
        {
            Point centrePoint = Settings.FinishPoint;
            Distance radius = Settings.RadiusForPortTurn;
            Angle azimuth = PortAzimuth(Settings.FinishAzimuth);

            Point point = CalculateCentrePoint(centrePoint,
                                               radius,
                                               azimuth);

            return point;
        }

        [NotNull]
        internal Point CalculateCentrePointForFinishPointStarboard()
        {
            Point centrePoint = Settings.FinishPoint;
            Distance radius = Settings.RadiusForStarboardTurn;
            Angle azimuth = StarboardAzimuth(Settings.FinishAzimuth);

            Point point = CalculateCentrePoint(centrePoint,
                                               radius,
                                               azimuth);

            return point;
        }

        [NotNull]
        internal Point CalculateCentrePointForStartPointPort()
        {
            Point centrePoint = Settings.StartPoint;
            Distance radius = Settings.RadiusForPortTurn;
            Angle azimuth = Settings.StartAzimuth + Angle.For90Degrees;

            Point point = CalculateCentrePoint(centrePoint,
                                               radius,
                                               azimuth);

            return point;
        }

        [NotNull]
        internal Point CalculateCentrePointForStartPointStarboard()
        {
            Point centrePoint = Settings.StartPoint;
            Distance radius = Settings.RadiusForStarboardTurn;
            Angle azimuth = Settings.StartAzimuth - Angle.FromDegrees(90.0);

            Point point = CalculateCentrePoint(centrePoint,
                                               radius,
                                               azimuth);

            return point;
        }

        [NotNull]
        internal ITurnCircle CreateTurnCircleFinishPointPort()
        {
            Point centrePoint = CalculateCentrePointForFinishPointPort();
            Distance radius = Settings.RadiusForPortTurn;
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
            Distance radius = Settings.RadiusForStarboardTurn;
            var circle = new Circle(centrePoint,
                                    radius.Length);

            var turnCircle = new TurnCircle(circle,
                                            Constants.CircleSide.Starboard,
                                            Constants.CircleOrigin.Finish,
                                            Constants.TurnDirection.Clockwise);

            return turnCircle;
        }

        [NotNull]
        internal ITurnCircle CreateTurnCircleStartPointPort()
        {
            Point centrePoint = CalculateCentrePointForStartPointPort();
            Distance radius = Settings.RadiusForPortTurn;
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
            Distance radius = Settings.RadiusForStarboardTurn;
            var circle = new Circle(centrePoint,
                                    radius.Length);

            var turnCircle = new TurnCircle(circle,
                                            Constants.CircleSide.Starboard,
                                            Constants.CircleOrigin.Start,
                                            Constants.TurnDirection.Clockwise);

            return turnCircle;
        }

        [NotNull]
        internal Angle PortAzimuth([NotNull] Angle azimuth)
        {
            Angle portAzimuth = azimuth + Angle.FromDegrees(90.0);

            return portAzimuth;
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

        public bool IsUnknown { get; private set; }

        public ISettings Settings { get; set; }

        public ITurnCircle StartTurnCirclePort { get; private set; }

        public ITurnCircle StartTurnCircleStarboard { get; private set; }

        public ITurnCircle FinishTurnCirclePort { get; private set; }

        public ITurnCircle FinishTurnCircleStarboard { get; private set; }

        #endregion
    }
}