using JetBrains.Annotations;

namespace Selkie.Racetrack.Converter
{
    public interface ITurnCirclePairToPathCreator : IConverter
    {
        [NotNull]
        IPath Path { get; }
    }
}