using System.Collections.Generic;
using JetBrains.Annotations;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Converters;
using Core2.Selkie.Racetrack.Interfaces.Turn;

namespace Core2.Selkie.Racetrack.Converters
{
    public class TurnCirclePairsToPathsConverter : ITurnCirclePairsToPathsConverter
    {
        public TurnCirclePairsToPathsConverter([NotNull] ITurnCirclePairToPathConverter turnCirclePairToPathConverter,
                                               [NotNull] IPossibleTurnCirclePairs possibleTurnCirclePairs)
        {
            m_TurnCirclePairToPathConverter = turnCirclePairToPathConverter;
            m_PossibleTurnCirclePairs = possibleTurnCirclePairs;
        }

        private readonly IPossibleTurnCirclePairs m_PossibleTurnCirclePairs;
        private readonly ITurnCirclePairToPathConverter m_TurnCirclePairToPathConverter;
        private IEnumerable <IPath> m_Paths = new IPath[0];
        private ISettings m_Settings = Racetrack.Settings.Unknown;

        public void Convert()
        {
            m_PossibleTurnCirclePairs.Settings = m_Settings;
            m_PossibleTurnCirclePairs.Calculate();

            m_Paths = CreatePaths(m_Settings,
                                  m_PossibleTurnCirclePairs);
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

        public IPossibleTurnCirclePairs PossibleTurnCirclePairs
        {
            get
            {
                return m_PossibleTurnCirclePairs;
            }
        }

        public IEnumerable <IPath> Paths
        {
            get
            {
                return m_Paths;
            }
        }

        [NotNull]
        private IEnumerable <IPath> CreatePaths([NotNull] ISettings settings,
                                                [NotNull] IPossibleTurnCirclePairs possibleTurnCirclePairs)
        {
            var paths = new List <IPath>();

            m_TurnCirclePairToPathConverter.Settings = settings;

            foreach ( ITurnCirclePair pair in possibleTurnCirclePairs.Pairs )
            {
                if ( !pair.IsValid )
                {
                    continue;
                }

                m_TurnCirclePairToPathConverter.TurnCirclePair = pair;
                m_TurnCirclePairToPathConverter.Convert();

                paths.AddRange(m_TurnCirclePairToPathConverter.Paths);
            }

            return paths;
        }
    }
}