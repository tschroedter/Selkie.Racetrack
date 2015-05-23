using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Racetrack.Turn;

namespace Selkie.Racetrack.Converter
{
    public interface ITurnCirclePairsToPathsConverter : IConverter
    {
        [NotNull]
        IEnumerable <IPath> Paths { get; }

        [NotNull]
        ISettings Settings { get; set; }

        [NotNull]
        IPossibleTurnCirclePairs PossibleTurnCirclePairs { get; }

        void Convert();
    }
}