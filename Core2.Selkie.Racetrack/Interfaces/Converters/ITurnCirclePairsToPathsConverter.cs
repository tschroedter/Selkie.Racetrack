using System.Collections.Generic;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.Converters
{
    public interface ITurnCirclePairsToPathsConverter : IConverter
    {
        [NotNull]
        IEnumerable <IPath> Paths { get; }

        [NotNull]
        [UsedImplicitly]
        ISettings Settings { get; set; }

        [NotNull]
        [UsedImplicitly]
        IPossibleTurnCirclePairs PossibleTurnCirclePairs { get; }

        void Convert();
    }
}