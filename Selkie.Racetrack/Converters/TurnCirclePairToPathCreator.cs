using System;
using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Converters;
using Selkie.Racetrack.Interfaces.Turn;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Racetrack.Converters
{
    public class TurnCirclePairToPathCreator : ITurnCirclePairToPathCreator
    {
        private readonly ISelkieLogger m_Logger;
        private readonly IPath m_Path;

        public TurnCirclePairToPathCreator([NotNull] ISelkieLogger logger,
                                           [NotNull] ISettings settings,
                                           [NotNull] ITurnCirclePair turnCirclePair,
                                           [NotNull] ILine tangent)
        {
            m_Logger = logger;
            m_Path = CreatePath(settings,
                                turnCirclePair,
                                tangent);
        }

        #region ITurnCirclePairToPathCreator Members

        public IPath Path
        {
            get
            {
                return m_Path;
            }
        }

        #endregion

        // ReSharper disable once MethodTooLong
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
                string message =
                    "Don't know how to handle this! - TurnCirclePair:{0} Tangent: {1}".Inject(turnCirclePair,
                                                                                              tangent);

                m_Logger.Error(message,
                               ex);

                path = Racetrack.Path.Unknown;
            }

            return path;
        }
    }
}