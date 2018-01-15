using JetBrains.Annotations;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Racetrack.Interfaces.Turn;

namespace Core2.Selkie.Racetrack.UTurn
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