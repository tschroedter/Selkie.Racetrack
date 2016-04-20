using System.Collections.Generic;
using JetBrains.Annotations;

namespace Selkie.Racetrack.Interfaces.Calculators
{
    public interface IPathSelectorCalculator : ICalculator
    {
        [NotNull]
        IEnumerable <IPath> Paths { get; }

        [NotNull]
        ISettings Settings { get; set; }
    }
}