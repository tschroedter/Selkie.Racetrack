using JetBrains.Annotations;

namespace Selkie.Racetrack
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