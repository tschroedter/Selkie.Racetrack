using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.UTurn
{
    public interface IDetermineTurnCircleCalculator : ICalculator
    {
        [NotNull]
        ITurnCircle StartTurnCircle { get; }

        [NotNull]
        ITurnCircle FinishTurnCircle { get; }

        [NotNull]
        [UsedImplicitly]
        ISettings Settings { get; set; }

        [NotNull]
        [UsedImplicitly]
        IUTurnCircle UTurnCircle { get; set; }
    }
}