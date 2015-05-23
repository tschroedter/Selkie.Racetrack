using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Racetrack.Turn;

namespace Selkie.Racetrack.Converter
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