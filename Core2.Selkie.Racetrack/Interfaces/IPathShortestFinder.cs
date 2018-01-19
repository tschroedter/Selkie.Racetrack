using System.Collections.Generic;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces
{
    public interface IPathShortestFinder
    {
        [NotNull]
        IPath ShortestPath { get; }

        [NotNull]
        [UsedImplicitly]
        IEnumerable <IPath> Paths { get; set; }

        void Find();
    }
}