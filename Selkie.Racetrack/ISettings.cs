using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack
{
    public interface ISettings
    {
        [NotNull]
        Point StartPoint { get; }

        [NotNull]
        Angle StartAzimuth { get; }

        [NotNull]
        Point FinishPoint { get; }

        [NotNull]
        Angle FinishAzimuth { get; }

        [NotNull]
        Distance Radius { get; }

        bool IsPortTurnAllowed { get; }
        bool IsStarboardTurnAllowed { get; }
        Constants.TurnDirection DefaultTurnDirection { get; }
        bool IsUnknown { get; }
    }
}