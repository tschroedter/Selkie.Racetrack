using JetBrains.Annotations;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Geometry.Surveying;

namespace Core2.Selkie.Racetrack.Calculators
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