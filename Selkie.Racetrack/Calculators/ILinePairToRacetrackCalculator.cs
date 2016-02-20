using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Calculators
{
    public interface ILinePairToRacetrackCalculator : ICalculator
    {
        [NotNull]
        ILine FromLine { get; set; }

        [NotNull]
        ILine ToLine { get; set; }

        [NotNull]
        ISettings Settings { get; }

        [NotNull]
        IPath Racetrack { get; }

        bool IsPortTurnAllowed { get; set; }
        bool IsStarboardTurnAllowed { get; set; }

        [NotNull]
        Distance RadiusForPortTurn { get; set; }

        [NotNull]
        Distance RadiusForStarboardTurn { get; set; }
    }
}