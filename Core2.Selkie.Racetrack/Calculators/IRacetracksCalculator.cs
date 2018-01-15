using System.Collections.Generic;
using JetBrains.Annotations;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;

namespace Core2.Selkie.Racetrack.Calculators
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