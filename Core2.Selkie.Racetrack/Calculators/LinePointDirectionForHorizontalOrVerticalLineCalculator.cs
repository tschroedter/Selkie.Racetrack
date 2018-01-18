using System;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using JetBrains.Annotations;
using NLog;

namespace Core2.Selkie.Racetrack.Calculators
{
    public class LinePointDirectionForHorizontalOrVerticalLineCalculator : ILinePointDirectionCalculator
    {
        public LinePointDirectionForHorizontalOrVerticalLineCalculator([NotNull] ILine line,
                                                                       [NotNull] Point point,
                                                                       Constants.TurnDirection defaultTurnDirection)
        {
            m_DefaultTurnDirection = defaultTurnDirection;
            TurnDirection = DetermineDirection(line,
                                               point);
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Constants.TurnDirection m_DefaultTurnDirection;

        public Constants.TurnDirection TurnDirection { get; }

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

            string message = $"Could not determine TurnDirection line {line} and point{point}!";

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
                $"Could not determine TurnDirection for horizontal line, using default '{m_DefaultTurnDirection}'! - Parameters: ax:{ax} bx:{bx} by:{by} cy:{cy}]";

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
                $"Could not determine TurnDirection for vertical line, using default '{direction}'! - Parameters: ay:{ay} bx:{bx} by:{by} cx:{cx}]";

            Logger.Error(message);

            return direction;
        }
    }
}