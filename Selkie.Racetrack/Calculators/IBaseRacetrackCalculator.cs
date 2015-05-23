using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Calculators
{
    public interface IBaseRacetrackCalculator : ICalculator
    {
        [NotNull]
        IPath[] Paths { get; }

        [NotNull]
        ILine FromLine { get; set; }

        [NotNull]
        ILine[] ToLines { get; set; }

        [NotNull]
        Distance Radius { get; set; }

        bool IsPortTurnAllowed { get; set; }
        bool IsStarboardTurnAllowed { get; set; }

        [NotNull]
        ILinePairToRacetrackCalculator Calculator { get; }
    }
}