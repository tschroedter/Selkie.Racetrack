using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Turn
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