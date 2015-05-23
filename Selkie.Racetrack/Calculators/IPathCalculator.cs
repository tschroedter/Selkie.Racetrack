using System.Collections.Generic;
using JetBrains.Annotations;

namespace Selkie.Racetrack.Calculators
{
    public interface IPathCalculator : ICalculator
    {
        [NotNull]
        IPath Path { get; }

        [NotNull]
        ISettings Settings { get; set; }

        [NotNull]
        IEnumerable <IPath> Paths { get; }
    }
}