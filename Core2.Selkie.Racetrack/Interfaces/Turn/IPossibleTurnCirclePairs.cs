using System.Collections.Generic;
using JetBrains.Annotations;
using Core2.Selkie.Racetrack.Interfaces.Calculators;

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