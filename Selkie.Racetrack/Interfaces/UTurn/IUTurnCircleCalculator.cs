using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces.Calculators;
using Selkie.Racetrack.Interfaces.Turn;

namespace Selkie.Racetrack.Interfaces.UTurn
{
    public interface IUTurnCircleCalculator : ICalculator
    {
        [NotNull]
        ISettings Settings { get; set; }

        [NotNull]
        ICircle Circle { get; }

        Constants.TurnDirection TurnDirection { get; }

        [NotNull]
        Point UTurnOneIntersectionPoint { get; }

        [NotNull]
        Point UTurnZeroIntersectionPoint { get; }

        [NotNull]
        ITurnCircle Zero { get; }

        [NotNull]
        ITurnCircle One { get; }

        [NotNull]
        ITurnCirclePair Pair { get; }
    }
}