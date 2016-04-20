using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Interfaces.UTurn
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