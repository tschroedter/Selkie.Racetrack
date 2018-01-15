using JetBrains.Annotations;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Racetrack.Interfaces.Turn;

namespace Core2.Selkie.Racetrack.UTurn
{
    public interface IUTurnCircle : ICalculator
    {
        [NotNull]
        Point UTurnOneIntersectionPoint { get; }

        [NotNull]
        Point CentrePoint { get; }

        [NotNull]
        ITurnCirclePair TurnCirclePair { get; }

        [NotNull]
        ITurnCircle Zero { get; }

        [NotNull]
        ITurnCircle One { get; }

        [NotNull]
        ICircle Circle { get; }

        Constants.TurnDirection TurnDirection { get; }
        bool IsRequired { get; }
        bool IsPossible { get; }
        bool IsUnknown { get; }

        [NotNull]
        ISettings Settings { get; set; }

        [NotNull]
        Point UTurnZeroIntersectionPoint { get; }
    }
}