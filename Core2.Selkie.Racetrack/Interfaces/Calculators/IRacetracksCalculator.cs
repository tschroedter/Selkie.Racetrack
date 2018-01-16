using System.Collections.Generic;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.Calculators
{
    public interface IRacetracksCalculator
        : IRacetracks,
          ICalculator
    {
        [NotNull]
        IEnumerable <ILine> Lines { get; set; }

        [NotNull]
        Distance TurnRadiusForPort { get; set; }

        [NotNull]
        Distance TurnRadiusForStarboard { get; set; }

        bool IsPortTurnAllowed { get; set; }
        bool IsStarboardTurnAllowed { get; set; }
    }
}