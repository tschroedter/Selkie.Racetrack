using JetBrains.Annotations;

namespace Selkie.Racetrack.Interfaces.Converters
{
    public interface ITurnCirclePairToPathCreator : IConverter
    {
        [NotNull]
        IPath Path { get; }
    }
}