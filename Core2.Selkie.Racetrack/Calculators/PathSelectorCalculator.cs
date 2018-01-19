using System.Collections.Generic;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Racetrack.Interfaces.Converters;
using Core2.Selkie.Racetrack.Interfaces.UTurn;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Calculators
{
    [UsedImplicitly]
    public class PathSelectorCalculator : IPathSelectorCalculator
    {
        public PathSelectorCalculator([NotNull] ITurnCirclePairsToPathsConverter converter,
                                      [NotNull] IUTurnPath uTurnPath)
        {
            m_Converter = converter;
            m_UTurnPath = uTurnPath;
        }

        internal bool IsUTurnAllowed => Settings.IsPortTurnAllowed && Settings.IsStarboardTurnAllowed;

        private readonly ITurnCirclePairsToPathsConverter m_Converter;
        private readonly IUTurnPath m_UTurnPath;
        private List <IPath> m_Paths = new List <IPath>();

        public void Calculate()
        {
            m_Converter.Settings = Settings;
            m_Converter.Convert();

            m_UTurnPath.Settings = Settings;
            m_UTurnPath.Calculate();

            var paths = new List <IPath>();

            IEnumerable <IPath> selectPaths = SelectPaths(m_UTurnPath,
                                                          m_Converter);

            paths.AddRange(selectPaths);

            m_Paths = paths;
        }

        public IEnumerable <IPath> Paths => m_Paths;

        public ISettings Settings { get; set; } = Racetrack.Settings.Unknown;

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