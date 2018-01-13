using Core2.Selkie.Geometry;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces
{
    public interface IPathValidator
    {
        bool IsValid([NotNull] IPath path,
                     Constants.TurnDirection defaultTurnDirection);
    }
}