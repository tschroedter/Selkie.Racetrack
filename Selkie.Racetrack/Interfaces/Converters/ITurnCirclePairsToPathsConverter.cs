using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Racetrack.Interfaces.Turn;

namespace Selkie.Racetrack.Interfaces.Converters
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