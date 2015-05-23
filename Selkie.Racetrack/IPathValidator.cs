using JetBrains.Annotations;
using Selkie.Geometry;

namespace Selkie.Racetrack
{
    public interface IPathValidator
    {
        bool IsValid([NotNull] IPath path,
                     Constants.TurnDirection defaultTurnDirection);
    }
}