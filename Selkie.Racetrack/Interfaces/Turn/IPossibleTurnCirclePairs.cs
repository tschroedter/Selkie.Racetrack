using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Interfaces.Turn
{
    public interface IPossibleTurnCirclePairs : ICalculator
    {
        [NotNull]
        IEnumerable <ITurnCirclePair> Pairs { get; }

        [NotNull]
        ISettings Settings { get; set; }
    }
}