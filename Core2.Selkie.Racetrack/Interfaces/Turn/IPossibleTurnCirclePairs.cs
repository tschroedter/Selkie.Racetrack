using System.Collections.Generic;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.Turn
{
    public interface IPossibleTurnCirclePairs : ICalculator
    {
        [NotNull]
        IEnumerable <ITurnCirclePair> Pairs { get; }

        [NotNull]
        ISettings Settings { get; set; }
    }
}