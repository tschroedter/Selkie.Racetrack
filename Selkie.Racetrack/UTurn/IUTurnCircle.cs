using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Calculators;
using Selkie.Racetrack.Turn;

namespace Selkie.Racetrack.UTurn
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