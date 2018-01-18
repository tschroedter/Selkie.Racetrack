using System;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Converters;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Windsor.Interfaces;
using JetBrains.Annotations;
using ITurnCircleArcSegment = Core2.Selkie.Geometry.Shapes.ITurnCircleArcSegment;

namespace Core2.Selkie.Racetrack.Converters
{
    public class TurnCirclePairToPathCreator : ITurnCirclePairToPathCreator
    {
        public TurnCirclePairToPathCreator([NotNull] ISelkieLogger logger,
                                           [NotNull] ISettings settings,
                                           [NotNull] ITurnCirclePair turnCirclePair,
                                           [NotNull] ILine tangent)
        {
            m_Logger = logger;
            Path = CreatePath(settings,
                              turnCirclePair,
                              tangent);
        }

        private readonly ISelkieLogger m_Logger;

        #region ITurnCirclePairToPathCreator Members

        public IPath Path { get; }

        #endregion

        [NotNull]
        private IPath CreatePath([NotNull] ISettings settings,
                                 [NotNull] ITurnCirclePair turnCirclePair,
                                 [NotNull] ILine tangent)
        {
            ITurnCircle circleZero = turnCirclePair.Zero;
            ITurnCircle circleOne = turnCirclePair.One;

            Constants.TurnDirection circleZeroDirection = turnCirclePair.Zero.TurnDirection;
            Constants.TurnDirection circleOneDirection = turnCirclePair.One.TurnDirection;

            Point circleZeroStartPoint;
            Point circleOneEndPoint;

            if ( circleZero.IsPointOnCircle(settings.StartPoint) &&
                 circleOne.IsPointOnCircle(settings.FinishPoint) )
            {
                circleZeroStartPoint = settings.StartPoint;
                circleOneEndPoint = settings.FinishPoint;
            }
            else
            {
                circleZeroStartPoint = settings.FinishPoint;
                circleOneEndPoint = settings.StartPoint;
            }

            Point startPoint = tangent.StartPoint;
            Point endPoint = tangent.EndPoint;

            if ( !circleZero.IsPointOnCircle(startPoint) )
            {
                startPoint = tangent.EndPoint;
                endPoint = tangent.StartPoint;
            }

            ICircle arcCircleZero = new Circle(circleZero.CentrePoint,
                                               circleZero.Radius.Length);
            ICircle arcCircleOne = new Circle(circleOne.CentrePoint,
                                              circleOne.Radius.Length);
            ILine line = new Line(startPoint,
                                  endPoint);

            IPath path;

            try
            {
                ITurnCircleArcSegment startArcSegment = new TurnCircleArcSegment(arcCircleZero,
                                                                                 circleZeroDirection,
                                                                                 Constants.CircleOrigin.Start,
                                                                                 circleZeroStartPoint,
                                                                                 startPoint);

                ITurnCircleArcSegment endArcSegment = new TurnCircleArcSegment(arcCircleOne,
                                                                               circleOneDirection,
                                                                               Constants.CircleOrigin.Finish,
                                                                               endPoint,
                                                                               circleOneEndPoint);

                path = new Path(startArcSegment,
                                line,
                                endArcSegment);
            }
            catch ( ArgumentException ex )
            {
                string message = $"Don't know how to handle this! - TurnCirclePair:{turnCirclePair} Tangent: {tangent}";

                m_Logger.Error(message,
                               ex);

                path = Racetrack.Path.Unknown;
            }

            return path;
        }
    }
}