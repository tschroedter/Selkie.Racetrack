using JetBrains.Annotations;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Interfaces
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

        bool IsPortTurnAllowed { get; }
        bool IsStarboardTurnAllowed { get; }
        Constants.TurnDirection DefaultTurnDirection { get; }
        bool IsUnknown { get; }

        [NotNull]
        Distance RadiusForPortTurn { get; }

        [NotNull]
        Distance RadiusForStarboardTurn { get; }

        [NotNull]
        Distance LargestRadiusForTurn { get; }
    }
}