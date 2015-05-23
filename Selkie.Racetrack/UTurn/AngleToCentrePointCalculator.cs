using System;
using JetBrains.Annotations;
using NLog;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Turn;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Racetrack.UTurn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class AngleToCentrePointCalculator : IAngleToCentrePointCalculator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Angle m_AngleForLeftTurnCircle;
        private readonly Angle m_AngleForRightTurnCircle;
        private readonly Point m_CentrePoint;
        private readonly Point m_LeftIntersectionPoint;
        private readonly ITurnCircle m_LeftTurnCircle;
        private readonly Point m_RightIntersectionPoint;
        private readonly ITurnCircle m_RightTurnCircle;

        public AngleToCentrePointCalculator([NotNull] ITurnCirclePair pair)
        {
            if ( !IsValid(pair) )
            {
                string message = "TurnCirclePair is not valid! Pair.Zero: {0} Pair.One: {1}".Inject(pair.Zero,
                                                                                                    pair.One);

                Logger.Error(message);

                throw new ArgumentException(message);
            }

            m_LeftTurnCircle = DetermineLeftTurnCircle(pair);
            m_RightTurnCircle = m_LeftTurnCircle.Equals(pair.One)
                                    ? pair.Zero
                                    : pair.One;

            m_AngleForLeftTurnCircle = CalculateRadiansRelativeToXAxisForLeftTurnCircle(m_LeftTurnCircle.CentrePoint,
                                                                                        m_RightTurnCircle.CentrePoint,
                                                                                        m_LeftTurnCircle.Radius.Length *
                                                                                        2.0);

            m_AngleForRightTurnCircle = CalculateRadiansRelativeToXAxisForRightTurnCircle(m_RightTurnCircle.CentrePoint,
                                                                                          m_LeftTurnCircle.CentrePoint,
                                                                                          m_RightTurnCircle.Radius
                                                                                                           .Length * 2.0);

            m_CentrePoint = CalculateCentrePoint(m_LeftTurnCircle,
                                                 m_AngleForLeftTurnCircle);

            ICircle circleLeft = m_LeftTurnCircle.Circle;
            ICircle circleRight = m_RightTurnCircle.Circle;

            m_LeftIntersectionPoint = circleLeft.PointOnCircle(m_AngleForLeftTurnCircle);
            m_RightIntersectionPoint = circleRight.PointOnCircle(m_AngleForRightTurnCircle);
        }

        internal bool IsValid([NotNull] ITurnCirclePair pair)
        {
            return pair.Zero.Side == pair.One.Side;
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

            string message = "Can't determine left turn circle for pair {0}!".Inject(pair);

            Logger.Error(message);

            throw new ArgumentException(message);
        }

        private static bool IsOneStarboardAndStart([NotNull] ITurnCircle one)
        {
            return one.Side == Constants.CircleSide.Starboard && one.Origin == Constants.CircleOrigin.Start;
        }

        private static bool IsOnePortAndStart([NotNull] ITurnCircle one)
        {
            return one.Side == Constants.CircleSide.Port && one.Origin == Constants.CircleOrigin.Start;
        }

        private static bool IsZeroStarboardAndStart([NotNull] ITurnCircle zero)
        {
            return zero.Side == Constants.CircleSide.Starboard && zero.Origin == Constants.CircleOrigin.Start;
        }

        private static bool IsZeroPortAndStart([NotNull] ITurnCircle zero)
        {
            return zero.Side == Constants.CircleSide.Port && zero.Origin == Constants.CircleOrigin.Start;
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
        private static Angle CalculateCentreLineRadians([NotNull] Point zeroCentrePoint,
                                                        [NotNull] Point oneCentrePoint,
                                                        double radius)
        {
            ICircle circle = new Circle(zeroCentrePoint,
                                        radius);

            Angle centreLineRadians = circle.GetAngleRelativeToXAxis(oneCentrePoint);
            return centreLineRadians;
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

        #region IAngleToCentrePointCalculator Members

        public Point IntersectionPointForTurnCircle(ITurnCircle turnCircle)
        {
            if ( m_LeftTurnCircle.CentrePoint.Equals(turnCircle.CentrePoint) )
            {
                return m_LeftIntersectionPoint;
            }

            if ( m_RightTurnCircle.CentrePoint.Equals(turnCircle.CentrePoint) )
            {
                return m_RightIntersectionPoint;
            }

            Logger.Error("Couldn't find TurnCircle {0}!".Inject(turnCircle));

            return Point.Unknown;
        }

        public Point CentrePoint
        {
            get
            {
                return m_CentrePoint;
            }
        }

        public Point LeftIntersectionPoint
        {
            get
            {
                return m_LeftIntersectionPoint;
            }
        }

        public Point RightIntersectionPoint
        {
            get
            {
                return m_RightIntersectionPoint;
            }
        }

        public ITurnCircle LeftTurnCircle
        {
            get
            {
                return m_LeftTurnCircle;
            }
        }

        public ITurnCircle RightTurnCircle
        {
            get
            {
                return m_RightTurnCircle;
            }
        }

        public Angle AngleForLeftTurnCircle
        {
            get
            {
                return m_AngleForLeftTurnCircle;
            }
        }

        public Angle AngleForRightTurnCircle
        {
            get
            {
                return m_AngleForRightTurnCircle;
            }
        }

        #endregion
    }
}