using System;
using JetBrains.Annotations;
using NLog;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces.Calculators;
using Selkie.Windsor.Extensions;

namespace Selkie.Racetrack.Calculators
{
    public class LinePointDirectionForHorizontalOrVerticalLineCalculator : ILinePointDirectionCalculator
    {
        public LinePointDirectionForHorizontalOrVerticalLineCalculator([NotNull] ILine line,
                                                                       [NotNull] Point point,
                                                                       Constants.TurnDirection defaultTurnDirection)
        {
            m_DefaultTurnDirection = defaultTurnDirection;
            m_TurnDirection = DetermineDirection(line,
                                                 point);
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Constants.TurnDirection m_DefaultTurnDirection;
        private readonly Constants.TurnDirection m_TurnDirection;

        public Constants.TurnDirection TurnDirection
        {
            get
            {
                return m_TurnDirection;
            }
        }

        internal Constants.TurnDirection DetermineDirection([NotNull] ILine line,
                                                            [NotNull] Point point)
        {
            double ax = line.StartPoint.X;
            double ay = line.StartPoint.Y;
            double bx = line.EndPoint.X;
            double by = line.EndPoint.Y;
            double cx = point.X;
            double cy = point.Y;

            if ( Math.Abs(bx - ax) < 0.001 )
            {
                return FindSideForVerticalLine(ay,
                                               bx,
                                               by,
                                               cx);
            }

            if ( Math.Abs(by - ay) < 0.001 )
            {
                return FindSideForHorizontalLine(ax,
                                                 bx,
                                                 by,
                                                 cy);
            }

            string message = "Could not determine TurnDirection line {0} and point{1}!".Inject(line,
                                                                                               point);

            Logger.Error(message);

            return Constants.TurnDirection.Unknown;
        }

        internal Constants.TurnDirection FindSideForHorizontalLine(double ax,
                                                                   double bx,
                                                                   double by,
                                                                   double cy)
        {
            if ( cy < by )
            {
                return bx > ax
                           ? Constants.TurnDirection.Clockwise
                           : Constants.TurnDirection.Counterclockwise;
            }

            if ( cy > by )
            {
                return bx > ax
                           ? Constants.TurnDirection.Counterclockwise
                           : Constants.TurnDirection.Clockwise;
            }

            string message =
                "Could not determine TurnDirection for horizontal line, using default '{0}'! - Parameters: ay:{1} bx:{2} by:{3} cx:{4}]"
                    .Inject(m_DefaultTurnDirection,
                            ax,
                            bx,
                            by,
                            cy);

            Logger.Error(message);

            return m_DefaultTurnDirection;
        }

        internal Constants.TurnDirection FindSideForVerticalLine(double ay,
                                                                 double bx,
                                                                 double by,
                                                                 double cx)
        {
            if ( cx < bx )
            {
                return by > ay
                           ? Constants.TurnDirection.Counterclockwise
                           : Constants.TurnDirection.Clockwise;
            }
            if ( cx > bx )
            {
                return by > ay
                           ? Constants.TurnDirection.Clockwise
                           : Constants.TurnDirection.Counterclockwise;
            }

            const Constants.TurnDirection direction = Constants.TurnDirection.Clockwise;

            string message =
                "Could not determine TurnDirection for vertical line, using default '{0}'! - Parameters: ay:{1} bx:{2} by:{3} cx:{4}]"
                    .Inject(direction,
                            ay,
                            bx,
                            by,
                            cx);

            Logger.Error(message);

            return direction;
        }
    }
}