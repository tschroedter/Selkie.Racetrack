using JetBrains.Annotations;
using Selkie.Racetrack.Calculators;
using Selkie.Racetrack.Turn;

namespace Selkie.Racetrack.UTurn
{
    public interface IDetermineTurnCircleCalculator : ICalculator
    {
        [NotNull]
        ITurnCircle StartTurnCircle { get; }

        [NotNull]
        ITurnCircle FinishTurnCircle { get; }

        [NotNull]
        ISettings Settings { get; set; }

        [NotNull]
        IUTurnCircle UTurnCircle { get; set; }
    }
}