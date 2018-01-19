using System;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using JetBrains.Annotations;
using NLog;

namespace Core2.Selkie.Racetrack.Calculators
{
    public class RacetrackLineDirectionCalculator : IRacetrackLineDirectionCalculator
    {
        public RacetrackLineDirectionCalculator([NotNull] ILine line,
                                                [NotNull] Point point,
                                                Constants.TurnDirection defaultTurnDirection)
        {
            Constants.TurnDirection direction = Calculate(line,
                                                          point);

            switch ( direction )
            {
                case Constants.TurnDirection.Clockwise:
                case Constants.TurnDirection.Counterclockwise:
                    TurnDirection = direction;
                    break;

                case Constants.TurnDirection.Unknown:
                    var calculator = new LinePointDirectionForHorizontalOrVerticalLineCalculator(line,
                                                                                                 point,
                                                                                                 defaultTurnDirection);
                    TurnDirection = calculator.TurnDirection;
                    break;

                default:
                    string message = $"Calculated turn direction '{direction}' is not known!";
                    throw new ArgumentException(message);
            }
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region IRacetrackLineDirectionCalculator Members

        public Constants.TurnDirection TurnDirection { get; }

        #endregion

        [UsedImplicitly]
        internal Constants.TurnDirection Calculate([NotNull] ILine line,
                                                   [NotNull] Point point)
        {
            double ax = line.StartPoint.X;
            double ay = line.StartPoint.Y;
            double bx = line.EndPoint.X;
            double by = line.EndPoint.Y;
            double cx = point.X;
            double cy = point.Y;

            Side side = FindSide(ax,
                                 ay,
                                 bx,
                                 by,
                                 cx,
                                 cy);

            switch ( side )
            {
                case Side.Right:
                    return Constants.TurnDirection.Clockwise;

                case Side.Left:
                    return Constants.TurnDirection.Counterclockwise;

                case Side.Unknown:
                    return Constants.TurnDirection.Unknown;

                default:
                    string message = $"Calculated side '{side}' is not known!";
                    throw new ArgumentException(message);
            }
        }

        [UsedImplicitly]
        internal Side FindSide(double ax,
                               double ay,
                               double bx,
                               double by,
                               double cx,
                               double cy)
        {
            ILine line = new Line(ax,
                                  ay,
                                  bx,
                                  by);
            bool isOnLine = line.IsOnLine(new Point(cx,
                                                    cy));

            if ( isOnLine )
            {
                return Side.Unknown;
            }

            if ( Math.Abs(bx - ax) < Constants.EpsilonDistance )
            {
                return Side.Unknown;
            }

            if ( Math.Abs(by - ay) < Constants.EpsilonDistance )
            {
                return Side.Unknown;
            }

            return FindSideDependingOnSlope(ax,
                                            ay,
                                            bx,
                                            by,
                                            cx,
                                            cy);
        }

        [UsedImplicitly]
        internal Side FindSideDependingOnSlope(double ax,
                                               double ay,
                                               double bx,
                                               double by,
                                               double cx,
                                               double cy)
        {
            if ( Math.Abs(bx - ax) > 0.0 )
            {
                double slope = ( by - ay ) / ( bx - ax );
                double yIntercept = ay - ax * slope;
                double cSolution = slope * cx + yIntercept;

                if ( Math.Abs(slope - 0.0) > Constants.EpsilonDistance )
                {
                    if ( cy > cSolution )
                    {
                        return bx > ax
                                   ? Side.Left
                                   : Side.Right;
                    }

                    return bx > ax
                               ? Side.Right
                               : Side.Left;
                }
            }

            string message = $"Could not determine Side! Parameters: ax:{ax} ay:{ay} bx:{bx} by:{by} cx:{cx} cy:{cy}";
            Logger.Error(message);

            return Side.Unknown;
        }

        #region Nested type: Side

        internal enum Side
        {
            Right = -1,
            Left = 1,
            Unknown = 0
        }

        #endregion
    }
}