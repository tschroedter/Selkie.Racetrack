using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using JetBrains.Annotations;

// ReSharper disable UnusedMemberInSuper.Global

namespace Core2.Selkie.Racetrack.Interfaces.UTurn
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

        void Calculate();

        [NotNull]
        Point IntersectionPointForTurnCircle([NotNull] ITurnCircle turnCircle);
    }
}