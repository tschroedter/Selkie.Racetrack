using System.Collections.Generic;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Calculators
{
    [UsedImplicitly]
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
        [UsedImplicitly]
        internal IPathShortestFinder PathShortestFinder { get; }

        public IPath Path => PathShortestFinder.ShortestPath;

        public IEnumerable <IPath> Paths => m_Selector.Paths;

        public ISettings Settings { get; set; }

        #endregion
    }
}