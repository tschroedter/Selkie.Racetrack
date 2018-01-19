using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.UTurn
{
    public interface IDetermineCirclePairCalculator : ICalculator
    {
        [NotNull]
        ITurnCirclePair Pair { get; }

        [NotNull]
        ITurnCircle Zero { get; }

        [NotNull]
        ITurnCircle One { get; }

        [NotNull]
        [UsedImplicitly]
        ISettings Settings { get; set; }
    }
}