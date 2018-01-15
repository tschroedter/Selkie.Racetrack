using System.Collections.Generic;
using JetBrains.Annotations;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Converters;
using Core2.Selkie.Racetrack.Interfaces.UTurn;

namespace Core2.Selkie.Racetrack.Calculators
{
    public class PathSelectorCalculator : IPathSelectorCalculator
    {
        public PathSelectorCalculator([NotNull] ITurnCirclePairsToPathsConverter converter,
                                      [NotNull] IUTurnPath uTurnPath)
        {
            m_Converter = converter;
            m_UTurnPath = uTurnPath;
        }

        internal bool IsUTurnAllowed
        {
            get
            {
                return m_Settings.IsPortTurnAllowed && m_Settings.IsStarboardTurnAllowed;
            }
        }

        private readonly ITurnCirclePairsToPathsConverter m_Converter;
        private readonly IUTurnPath m_UTurnPath;
        private List <IPath> m_Paths = new List <IPath>();
        private ISettings m_Settings = Racetrack.Settings.Unknown;

        public void Calculate()
        {
            m_Converter.Settings = m_Settings;
            m_Converter.Convert();

            m_UTurnPath.Settings = m_Settings;
            m_UTurnPath.Calculate();

            var paths = new List <IPath>();

            IEnumerable <IPath> selectPaths = SelectPaths(m_UTurnPath,
                                                          m_Converter);

            paths.AddRange(selectPaths);

            m_Paths = paths;
        }

        public IEnumerable <IPath> Paths
        {
            get
            {
                return m_Paths;
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

        [NotNull]
        internal IEnumerable <IPath> SelectPaths([NotNull] IUTurnPath uTurnPath,
                                                 [NotNull] ITurnCirclePairsToPathsConverter converters)
        {
            var paths = new List <IPath>();

            if ( uTurnPath.IsRequired )
            {
                if ( IsUTurnAllowed )
                {
                    paths.Add(uTurnPath.Path);
                }
                else
                {
                    paths.AddRange(converters.Paths);
                }
            }
            else
            {
                paths.AddRange(converters.Paths);
            }

            return paths;
        }
    }
}