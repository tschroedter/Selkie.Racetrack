using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Calculators
{
    public interface IRacetracksCalculator
        : IRacetracks,
          ICalculator
    {
        [NotNull]
        IEnumerable <ILine> Lines { get; set; }

        [NotNull]
        Distance Radius { get; set; }

        bool IsPortTurnAllowed { get; set; }
        bool IsStarboardTurnAllowed { get; set; }
    }
}