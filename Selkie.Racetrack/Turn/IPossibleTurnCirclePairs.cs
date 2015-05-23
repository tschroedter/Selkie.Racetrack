using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Racetrack.Calculators;

namespace Selkie.Racetrack.Turn
{
    public interface IPossibleTurnCirclePairs : ICalculator
    {
        [NotNull]
        IEnumerable <ITurnCirclePair> Pairs { get; }

        [NotNull]
        ISettings Settings { get; set; }
    }
}