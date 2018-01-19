using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.Turn
{
    public interface IPossibleTurnCircles : ICalculator
    {
        [NotNull]
        [UsedImplicitly]
        ISettings Settings { get; set; }

        [NotNull]
        ITurnCircle StartTurnCirclePort { get; }

        [NotNull]
        ITurnCircle StartTurnCircleStarboard { get; }

        [NotNull]
        ITurnCircle FinishTurnCirclePort { get; }

        [NotNull]
        ITurnCircle FinishTurnCircleStarboard { get; }

        [UsedImplicitly]
        bool IsUnknown { get; }
    }
}