using System.Collections.Generic;
using JetBrains.Annotations;
using Core2.Selkie.Racetrack.Interfaces;

namespace Core2.Selkie.Racetrack.Calculators
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