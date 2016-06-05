using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;
using Selkie.Windsor;

namespace Selkie.Racetrack.Calculators
{
    [ProjectComponent(Lifestyle.Transient)]
    public class PathCalculator : IPathCalculator
    {
        public PathCalculator([NotNull] IPathSelectorCalculator selector,
                              [NotNull] IPathShortestFinder pathShortestFinder)
        {
            Settings = Racetrack.Settings.Unknown;
            m_Selector = selector;
            PathShortestFinder = pathShortestFinder;
        }

        private readonly IPathSelectorCalculator m_Selector;

        public void Calculate()
        {
            m_Selector.Settings = Settings;
            m_Selector.Calculate();

            PathShortestFinder.Paths = m_Selector.Paths;
            PathShortestFinder.Find();
        }

        #region ICalculator Members

        [NotNull]
        internal IPathShortestFinder PathShortestFinder { get; private set; }

        public IPath Path
        {
            get
            {
                return PathShortestFinder.ShortestPath;
            }
        }

        public IEnumerable <IPath> Paths
        {
            get
            {
                return m_Selector.Paths;
            }
        }

        public ISettings Settings { get; set; }

        #endregion
    }
}