using System.Collections.Generic;
using JetBrains.Annotations;
using Core2.Selkie.Racetrack.Interfaces.Turn;

namespace Core2.Selkie.Racetrack.Interfaces.Converters
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