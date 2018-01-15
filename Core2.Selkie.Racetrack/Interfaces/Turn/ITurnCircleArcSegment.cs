using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.Turn
{
    public interface ITurnCircleArcSegment : IArcSegment
    {
        [NotNull]
        IArcSegment ArcSegment { get; }

        Constants.CircleOrigin CircleOrigin { get; }

        [NotNull]
        Angle Angle { get; }
    }
}