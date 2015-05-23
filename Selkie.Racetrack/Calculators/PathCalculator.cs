using System.Collections.Generic;
using JetBrains.Annotations;

namespace Selkie.Racetrack.Calculators
{
    public class PathCalculator : IPathCalculator
    {
        private readonly IPathShortestFinder m_PathShortestFinder;
        private readonly IPathSelectorCalculator m_Selector;
        private ISettings m_Settings = Racetrack.Settings.Unknown;

        public PathCalculator([NotNull] IPathSelectorCalculator selector,
                              [NotNull] IPathShortestFinder pathShortestFinder)
        {
            m_Selector = selector;
            m_PathShortestFinder = pathShortestFinder;
        }

        public void Calculate()
        {
            m_Selector.Settings = Settings;
            m_Selector.Calculate();

            m_PathShortestFinder.Paths = m_Selector.Paths;
            m_PathShortestFinder.Find();
        }

        #region ICalculator Members

        [NotNull]
        internal IPathShortestFinder PathShortestFinder
        {
            get
            {
                return m_PathShortestFinder;
            }
        }

        public IPath Path
        {
            get
            {
                return m_PathShortestFinder.ShortestPath;
            }
        }

        public IEnumerable <IPath> Paths
        {
            get
            {
                return m_Selector.Paths;
            }
        }

        public ISettings Settings
        {
            get
            {
                return m_Settings;
            }
            set
            {
                m_Settings = value;
            }
        }

        #endregion
    }
}