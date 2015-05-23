using JetBrains.Annotations;
using Selkie.Racetrack.Calculators;
using Selkie.Racetrack.Turn;

namespace Selkie.Racetrack.UTurn
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
        ISettings Settings { get; set; }
    }
}