using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces.Calculators;
using Selkie.Racetrack.Interfaces.Turn;

namespace Selkie.Racetrack.Interfaces.UTurn
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