using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces
{
    public interface IRacetracks
    {
        [UsedImplicitly]
        bool IsUnknown { get; }

        [NotNull]
        [UsedImplicitly]
        IPath[][] ForwardToForward { get; }

        [NotNull]
        [UsedImplicitly]
        IPath[][] ForwardToReverse { get; }

        [NotNull]
        [UsedImplicitly]
        IPath[][] ReverseToForward { get; }

        [NotNull]
        [UsedImplicitly]
        IPath[][] ReverseToReverse { get; }
    }
}