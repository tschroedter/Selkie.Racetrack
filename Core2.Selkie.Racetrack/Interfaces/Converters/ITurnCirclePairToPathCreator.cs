using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.Converters
{
    public interface ITurnCirclePairToPathCreator : IConverter
    {
        [NotNull]
        IPath Path { get; }
    }
}