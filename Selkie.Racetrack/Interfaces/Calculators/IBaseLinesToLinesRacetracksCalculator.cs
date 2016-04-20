using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Interfaces.Calculators
{
    public interface IBaseLinesToLinesRacetracksCalculator : ICalculator
    {
        bool IsPortTurnAllowed { get; set; }
        bool IsStarboardTurnAllowed { get; set; }

        [NotNull]
        IEnumerable <ILine> Lines { get; set; }

        [NotNull]
        IPath[][] Paths { get; }

        [NotNull]
        Distance TurnRadiusForPort { get; set; }

        [NotNull]
        Distance TurnRadiusForStarboard { get; set; }
    }
}