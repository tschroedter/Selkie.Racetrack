using JetBrains.Annotations;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Calculators;

namespace Core2.Selkie.Racetrack.Interfaces.Turn
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