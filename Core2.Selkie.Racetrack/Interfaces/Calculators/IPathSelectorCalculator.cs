using System.Collections.Generic;
using JetBrains.Annotations;
// ReSharper disable UnusedMemberInSuper.Global

namespace Core2.Selkie.Racetrack.Interfaces.Calculators
{
    public interface IPathSelectorCalculator : ICalculator
    {
        [NotNull]
        IEnumerable <IPath> Paths { get; }

        [NotNull]
        ISettings Settings { get; set; }
    }
}