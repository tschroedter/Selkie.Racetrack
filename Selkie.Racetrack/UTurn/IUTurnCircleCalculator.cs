using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Calculators;
using Selkie.Racetrack.Turn;

namespace Selkie.Racetrack.UTurn
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