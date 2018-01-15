using System.Collections.Generic;
using JetBrains.Annotations;
using Core2.Selkie.Racetrack.Interfaces;

namespace Core2.Selkie.Racetrack.Calculators
{
    public interface IPathSelectorCalculator : ICalculator
    {
        [NotNull]
        IEnumerable <IPath> Paths { get; }

        [NotNull]
        ISettings Settings { get; set; }
    }
}