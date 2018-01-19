using System.Linq;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Calculators;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Windsor;
using Core2.Selkie.Windsor.Interfaces;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack
{
    [UsedImplicitly]
    [ProjectComponent(Lifestyle.Transient)]
    public class PathValidator : IPathValidator
    {
        public PathValidator([NotNull] ISelkieLogger logger)
        {
            m_Logger = logger;
        }

        private readonly ISelkieLogger m_Logger;

        #region IPathValidator Members

        public bool IsValid(IPath path,
                            Constants.TurnDirection defaultTurnDirection)
        {
            if ( path.IsUnknown )
            {
                m_Logger.Warn("Given IPath is null or unknown!");

                return false;
            }

            IPolylineSegment[] segments = path.Segments.ToArray();

            if ( !( segments [ 1 ] is ILine middleSegment ) ||
                 middleSegment.StartPoint == middleSegment.EndPoint )
            {
                return true;
            }

            if ( !( segments [ 0 ] is ITurnCircleArcSegment startSegment ) )
            {
                return false;
            }

            if ( !( segments [ 2 ] is ITurnCircleArcSegment endSegment ) )
            {
                return false;
            }

            if ( !IsValidArcSegment(middleSegment,
                                    startSegment,
                                    defaultTurnDirection) )
            {
                return false;
            }

            if ( !IsValidArcSegment(middleSegment,
                                    endSegment,
                                    defaultTurnDirection) )
            {
                return false;
            }

            return true;
        }

        #endregion

        internal bool IsValidArcSegment([NotNull] ILine line,
                                        [NotNull] ITurnCircleArcSegment arcSegment,
                                        Constants.TurnDirection defaultTurnDirection)
        {
            var calculator = new RacetrackLineDirectionCalculator(line,
                                                                  arcSegment.CentrePoint,
                                                                  defaultTurnDirection);

            return arcSegment.TurnDirection == calculator.TurnDirection;
        }
    }
}