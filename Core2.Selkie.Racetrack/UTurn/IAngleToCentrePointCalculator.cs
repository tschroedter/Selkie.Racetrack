using JetBrains.Annotations;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Racetrack.Turn;

namespace Core2.Selkie.Racetrack.UTurn
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