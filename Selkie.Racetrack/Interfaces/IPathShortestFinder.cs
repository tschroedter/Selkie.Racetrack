using System.Collections.Generic;
using JetBrains.Annotations;

namespace Selkie.Racetrack.Interfaces
{
    public interface IPathShortestFinder
    {
        [NotNull]
        IPath ShortestPath { get; }

        [NotNull]
        IEnumerable <IPath> Paths { get; set; }

        void Find();
    }
}