using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces.Calculators;
using Selkie.Racetrack.Interfaces.Turn;

namespace Selkie.Racetrack.Interfaces.UTurn
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