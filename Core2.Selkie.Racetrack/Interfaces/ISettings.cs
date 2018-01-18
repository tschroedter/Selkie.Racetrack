using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces
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

        Constants.TurnDirection
            DefaultTurnDirection { get; } // TODO: Remove TurnDirectiom from Selkie.Geometry and use Selkie.Common one

        bool IsUnknown { get; }

        [NotNull]
        Distance RadiusForPortTurn { get; }

        [NotNull]
        Distance RadiusForStarboardTurn { get; }

        [NotNull]
        Distance LargestRadiusForTurn { get; }
    }
}