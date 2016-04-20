using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Interfaces.Calculators
{
    public interface IBaseRacetrackCalculator : ICalculator
    {
        [NotNull]
        IPath[] Paths { get; }

        [NotNull]
        ILine FromLine { get; set; }

        [NotNull]
        ILine[] ToLines { get; set; }

        bool IsPortTurnAllowed { get; set; }

        bool IsStarboardTurnAllowed { get; set; }

        [NotNull]
        ILinePairToRacetrackCalculator Calculator { get; }

        [NotNull]
        Distance TurnRadiusForPort { get; set; }

        [NotNull]
        Distance TurnRadiusForStarboard { get; set; }
    }
}