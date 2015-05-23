using System.Collections.Generic;
using Selkie.Windsor;

namespace Selkie.Racetrack
{
    [ProjectComponent(Lifestyle.Transient)]
    public class PathShortestFinder : IPathShortestFinder
    {
        private IEnumerable <IPath> m_Paths = new IPath[0];
        private IPath m_ShortestPath = Path.Unknown;

        public IEnumerable <IPath> Paths
        {
            get
            {
                return m_Paths;
            }
            set
            {
                m_Paths = value;
            }
        }

        public IPath ShortestPath
        {
            get
            {
                return m_ShortestPath;
            }
        }

        public void Find()
        {
            IPath shortest = Path.Unknown;
            double minLength = double.MaxValue;

            foreach ( IPath path in m_Paths )
            {
                if ( minLength > path.Distance.Length )
                {
                    minLength = path.Distance.Length;
                    shortest = path;
                }
            }

            m_ShortestPath = shortest;
        }
    }
}