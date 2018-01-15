using JetBrains.Annotations;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Racetrack.Interfaces.Turn;

namespace Core2.Selkie.Racetrack.UTurn
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