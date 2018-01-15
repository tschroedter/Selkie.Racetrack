using JetBrains.Annotations;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;

namespace Core2.Selkie.Racetrack.UTurn
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