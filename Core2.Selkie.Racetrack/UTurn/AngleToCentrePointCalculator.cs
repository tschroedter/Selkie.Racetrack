using System;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Racetrack.Interfaces.UTurn;
using Core2.Selkie.Racetrack.Turn;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;
using NLog;
using TurnCircle = Core2.Selkie.Racetrack.Turn.TurnCircle;

namespace Core2.Selkie.Racetrack.UTurn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class AngleToCentrePointCalculator : IAngleToCentrePointCalculator
    {
        public AngleToCentrePointCalculator()
        {
            CentrePoint = Point.Unknown;
            LeftIntersectionPoint = Point.Unknown;
            RightIntersectionPoint = Point.Unknown;
            LeftTurnCircle = TurnCircle.Unknown;
            RightTurnCircle = TurnCircle.Unknown;
            AngleForLeftTurnCircle = Angle.Unknown;
            AngleForRightTurnCircle = Angle.Unknown;
            Pair = TurnCirclePair.Unknown;
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ITurnCirclePair Pair { get; set; }

        public void Calculate()
        {
            ValidatePair();

            LeftTurnCircle = DetermineLeftTurnCircle(Pair);
            RightTurnCircle = LeftTurnCircle.Equals(Pair.One)
                                  ? Pair.Zero
                                  : Pair.One;

            AngleForLeftTurnCircle = CalculateRadiansRelativeToXAxisForLeftTurnCircle(LeftTurnCircle.CentrePoint,
                                                                                      RightTurnCircle.CentrePoint,
                                                                                      LeftTurnCircle.Radius.Length *
                                                                                      2.0);

            AngleForRightTurnCircle = CalculateRadiansRelativeToXAxisForRightTurnCircle(RightTurnCircle.CentrePoint,
                                                                                        LeftTurnCircle.CentrePoint,
                                                                                        RightTurnCircle.Radius
                                                                                                       .Length * 2.0);

            CentrePoint = CalculateCentrePoint(LeftTurnCircle,
                                               AngleForLeftTurnCircle);

            ICircle circleLeft = LeftTurnCircle.Circle;
            ICircle circleRight = RightTurnCircle.Circle;

            LeftIntersectionPoint = circleLeft.PointOnCircle(AngleForLeftTurnCircle);
            RightIntersectionPoint = circleRight.PointOnCircle(AngleForRightTurnCircle);
        }

        [NotNull]
        internal Angle CalculateAngleRelativeToCentreLine([NotNull] Point zeroCentrePoint,
                                                          [NotNull] Point oneCentrePoint,
                                                          double radius)
        {
            double distance = zeroCentrePoint.DistanceTo(oneCentrePoint);

            double a = distance / 2.0;
            double c = radius;
            double radians = Math.Acos(a / c);

            return Angle.FromRadians(radians);
        }

        [NotNull]
        internal Angle CalculateRadiansRelativeToXAxisForLeftTurnCircle([NotNull] Point zeroCentrePoint,
                                                                        [NotNull] Point oneCentrePoint,
                                                                        double radius)
        {
            Angle relativeToCentreLine = CalculateAngleRelativeToCentreLine(zeroCentrePoint,
                                                                            oneCentrePoint,
                                                                            radius);

            Angle centreLineRadians = CalculateCentreLineRadians(zeroCentrePoint,
                                                                 oneCentrePoint,
                                                                 radius);

            Angle relativeToXAxis = centreLineRadians + relativeToCentreLine;

            return relativeToXAxis;
        }

        [NotNull]
        internal Angle CalculateRadiansRelativeToXAxisForRightTurnCircle([NotNull] Point zeroCentrePoint,
                                                                         [NotNull] Point oneCentrePoint,
                                                                         double radius)
        {
            Angle relativeToCentreLine = CalculateAngleRelativeToCentreLine(zeroCentrePoint,
                                                                            oneCentrePoint,
                                                                            radius);

            Angle centreLineRadians = CalculateCentreLineRadians(zeroCentrePoint,
                                                                 oneCentrePoint,
                                                                 radius);

            Angle relativeToXAxis = centreLineRadians - relativeToCentreLine;

            return relativeToXAxis;
        }

        [NotNull]
        internal ITurnCircle DetermineLeftTurnCircle([NotNull] ITurnCirclePair pair)
        {
            ITurnCircle zero = pair.Zero;
            ITurnCircle one = pair.One;

            if ( IsZeroPortAndStart(zero) ||
                 IsOneStarboardAndStart(one) )
            {
                return zero;
            }

            if ( IsZeroStarboardAndStart(zero) ||
                 IsOnePortAndStart(one) )
            {
                return one;
            }

            string message = $"Can't determine left turn circle for pair {pair}!";

            Logger.Error(message);

            throw new ArgumentException(message);
        }

        internal bool IsValid([NotNull] ITurnCirclePair pair)
        {
            return pair.Zero.Side == pair.One.Side;
        }

        [NotNull]
        private static Angle CalculateCentreLineRadians([NotNull] Point zeroCentrePoint,
                                                        [NotNull] Point oneCentrePoint,
                                                        double radius)
        {
            ICircle circle = new Circle(zeroCentrePoint,
                                        radius);

            Angle centreLineRadians = circle.GetAngleRelativeToXAxis(oneCentrePoint);
            return centreLineRadians;
        }

        private static bool IsOnePortAndStart([NotNull] ITurnCircle one)
        {
            return one.Side == Constants.CircleSide.Port && one.Origin == Constants.CircleOrigin.Start;
        }

        private static bool IsOneStarboardAndStart([NotNull] ITurnCircle one)
        {
            return one.Side == Constants.CircleSide.Starboard && one.Origin == Constants.CircleOrigin.Start;
        }

        private static bool IsZeroPortAndStart([NotNull] ITurnCircle zero)
        {
            return zero.Side == Constants.CircleSide.Port && zero.Origin == Constants.CircleOrigin.Start;
        }

        private static bool IsZeroStarboardAndStart([NotNull] ITurnCircle zero)
        {
            return zero.Side == Constants.CircleSide.Starboard && zero.Origin == Constants.CircleOrigin.Start;
        }

        [NotNull]
        private Point CalculateCentrePoint([NotNull] ITurnCircle turnCircle,
                                           [NotNull] Angle angle)
        {
            Point basePoint = CalculateCentrePointFromUnitCircle(turnCircle.Radius.Length,
                                                                 angle);

            Point centrePoint = turnCircle.CentrePoint;

            double x = centrePoint.X + basePoint.X;
            double y = centrePoint.Y + basePoint.Y;

            var point = new Point(x,
                                  y);

            return point;
        }

        [NotNull]
        private Point CalculateCentrePointFromUnitCircle(double radius,
                                                         [NotNull] Angle angle)
        {
            ICircle unitCircle = new Circle(0.0,
                                            0.0,
                                            radius * 2.0);
            Point point = unitCircle.PointOnCircle(angle);

            return point;
        }

        private void ValidatePair()
        {
            if ( IsValid(Pair) )
            {
                return;
            }

            string message = $"TurnCirclePair is not valid! Pair.Zero: {Pair.Zero} Pair.One: {Pair.One}";

            Logger.Error(message);

            throw new ArgumentException(message);
        }

        #region IAngleToCentrePointCalculator Members

        public Point IntersectionPointForTurnCircle(ITurnCircle turnCircle)
        {
            if ( LeftTurnCircle.CentrePoint.Equals(turnCircle.CentrePoint) )
            {
                return LeftIntersectionPoint;
            }

            if ( RightTurnCircle.CentrePoint.Equals(turnCircle.CentrePoint) )
            {
                return RightIntersectionPoint;
            }

            Logger.Error($"Couldn't find TurnCircle {turnCircle}!");

            return Point.Unknown;
        }

        public Point CentrePoint { get; private set; }

        public Point LeftIntersectionPoint { get; private set; }

        public Point RightIntersectionPoint { get; private set; }

        public ITurnCircle LeftTurnCircle { get; private set; }

        public ITurnCircle RightTurnCircle { get; private set; }

        public Angle AngleForLeftTurnCircle { get; private set; }

        public Angle AngleForRightTurnCircle { get; private set; }

        #endregion
    }
}