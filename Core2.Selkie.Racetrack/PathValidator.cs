using System.Linq;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Windsor;
using Core2.Selkie.Windsor.Interfaces;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack
{
    [ProjectComponent(Lifestyle.Transient)]
    public class PathValidator : IPathValidator
    {
        public PathValidator([NotNull] ISelkieLogger logger)
        {
            m_Logger = logger;
        }

        private readonly ISelkieLogger m_Logger;

        #region IPathValidator Members

        // ReSharper disable once MethodTooLong
        public bool IsValid(IPath path,
                            Constants.TurnDirection defaultTurnDirection)
        {
            if ( path.IsUnknown )
            {
                m_Logger.Warn("Given IPath is null or unknown!");

                return false;
            }

            IPolylineSegment[] segments = path.Segments.ToArray();

            var middleSegment = segments [ 1 ] as ILine;

            if ( middleSegment == null ||
                 middleSegment.StartPoint == middleSegment.EndPoint )
            {
                return true;
            }

            var startSegment = segments [ 0 ] as ITurnCircleArcSegment;
            if ( startSegment == null )
            {
                return false;
            }

            var endSegment = segments [ 2 ] as ITurnCircleArcSegment;
            if ( endSegment == null )
            {
                return false;
            }

            if ( !IsValidArcSegment(middleSegment,
                                    startSegment,
                                    defaultTurnDirection) )
            {
                return false;
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
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