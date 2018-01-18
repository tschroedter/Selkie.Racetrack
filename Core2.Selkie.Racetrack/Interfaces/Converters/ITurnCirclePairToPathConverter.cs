using System.Collections.Generic;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.Converters
{
    public interface ITurnCirclePairToPathConverter : IConverter
    {
        [NotNull]
        IEnumerable <IPath> Paths { get; }

        [NotNull]
        IEnumerable <IPath> PossiblePaths { get; }

        [NotNull]
        ISettings Settings { get; set; }

        [NotNull]
        ITurnCirclePair TurnCirclePair { get; set; }

        void Convert();
    }
}