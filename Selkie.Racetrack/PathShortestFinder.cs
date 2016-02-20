using System.Collections.Generic;
using Selkie.Windsor;

namespace Selkie.Racetrack
{
    [ProjectComponent(Lifestyle.Transient)]
    public class PathShortestFinder : IPathShortestFinder
    {
        public PathShortestFinder()
        {
            ShortestPath = Path.Unknown;
            Paths = new IPath[0];
        }

        public IEnumerable <IPath> Paths { get; set; }

        public IPath ShortestPath { get; private set; }

        public void Find()
        {
            IPath shortest = Path.Unknown;
            double minLength = double.MaxValue;

            foreach ( IPath path in Paths )
            {
                if ( minLength > path.Distance.Length )
                {
                    minLength = path.Distance.Length;
                    shortest = path;
                }
            }

            ShortestPath = shortest;
        }
    }
}