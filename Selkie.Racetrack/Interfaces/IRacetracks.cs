using JetBrains.Annotations;

namespace Selkie.Racetrack.Interfaces
{
    public interface IRacetracks
    {
        bool IsUnknown { get; }

        [NotNull]
        IPath[][] ForwardToForward { get; }

        [NotNull]
        IPath[][] ForwardToReverse { get; }

        [NotNull]
        IPath[][] ReverseToForward { get; }

        [NotNull]
        IPath[][] ReverseToReverse { get; }
    }
}