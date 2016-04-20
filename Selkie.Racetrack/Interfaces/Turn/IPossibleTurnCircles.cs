using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Interfaces.Turn
{
    public interface IPossibleTurnCircles : ICalculator
    {
        [NotNull]
        ISettings Settings { get; set; }

        [NotNull]
        ITurnCircle StartTurnCirclePort { get; }

        [NotNull]
        ITurnCircle StartTurnCircleStarboard { get; }

        [NotNull]
        ITurnCircle FinishTurnCirclePort { get; }

        [NotNull]
        ITurnCircle FinishTurnCircleStarboard { get; }

        bool IsUnknown { get; }
    }
}