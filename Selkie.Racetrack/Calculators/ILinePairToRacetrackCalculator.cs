using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Calculators
{
    public interface ILinePairToRacetrackCalculator : ICalculator
    {
        [NotNull]
        Distance Radius { get; set; }

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
    }
}