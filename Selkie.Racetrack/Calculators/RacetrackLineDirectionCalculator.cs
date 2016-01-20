using System;
using JetBrains.Annotations;
using NLog;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Windsor.Extensions;

namespace Selkie.Racetrack.Calculators
{
    internal class RacetrackLineDirectionCalculator : IRacetrackLineDirectionCalculator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Constants.TurnDirection m_TurnDirection;

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
                    m_TurnDirection = direction;
                    break;

                default:
                    var calculator = new LinePointDirectionForHorizontalOrVerticalLineCalculator(line,
                                                                                                 point,
                                                                                                 defaultTurnDirection);
                    m_TurnDirection = calculator.TurnDirection;
                    break;
            }
        }

        #region IRacetrackLineDirectionCalculator Members

        public Constants.TurnDirection TurnDirection
        {
            get
            {
                return m_TurnDirection;
            }
        }

        #endregion

        // ReSharper disable once MethodTooLong
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

                default:
                    return Constants.TurnDirection.Unknown;
            }
        }

        // ReSharper disable once TooManyArguments
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
                                            @by,
                                            cx,
                                            cy);
        }

        // ReSharper disable once TooManyArguments
        internal Side FindSideDependingOnSlope(double ax,
                                               double ay,
                                               double bx,
                                               double @by,
                                               double cx,
                                               double cy)
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

            string message = "Could not determine Side! Parameters: ax:{0} ay:{1} bx:{2} by:{3} cx:{4} cy:{5}".Inject(
                                                                                                                      ax,
                                                                                                                      ay,
                                                                                                                      bx,
                                                                                                                      by,
                                                                                                                      cx,
                                                                                                                      cy);
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