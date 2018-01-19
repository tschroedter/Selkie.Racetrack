using System.Collections.Generic;
using JetBrains.Annotations;
// ReSharper disable UnusedMemberInSuper.Global

namespace Core2.Selkie.Racetrack.Interfaces.Calculators
{
    public interface IPathCalculator : ICalculator
    {
        [NotNull]
        IPath Path { get; }

        [NotNull]
        ISettings Settings { get; set; }

        [NotNull]
        [UsedImplicitly]
        IEnumerable <IPath> Paths { get; }
    }
}