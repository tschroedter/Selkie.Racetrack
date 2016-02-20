using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Turn;

namespace Selkie.Racetrack.UTurn
{
    public interface IAngleToCentrePointCalculator
    {
        [NotNull]
        Point CentrePoint { get; }

        [NotNull]
        Point LeftIntersectionPoint { get; }

        [NotNull]
        Point RightIntersectionPoint { get; }

        [NotNull]
        ITurnCircle LeftTurnCircle { get; }

        [NotNull]
        ITurnCircle RightTurnCircle { get; }

        [NotNull]
        Angle AngleForLeftTurnCircle { get; }

        [NotNull]
        Angle AngleForRightTurnCircle { get; }

        [NotNull]
        ITurnCirclePair Pair { get; set; }

        [NotNull]
        Point IntersectionPointForTurnCircle([NotNull] ITurnCircle turnCircle);

        void Calculate();
    }
}