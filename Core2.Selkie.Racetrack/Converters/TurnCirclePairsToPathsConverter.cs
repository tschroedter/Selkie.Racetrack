using System.Collections.Generic;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Converters;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Converters
{
    public class TurnCirclePairsToPathsConverter : ITurnCirclePairsToPathsConverter
    {
        public TurnCirclePairsToPathsConverter([NotNull] ITurnCirclePairToPathConverter turnCirclePairToPathConverter,
                                               [NotNull] IPossibleTurnCirclePairs possibleTurnCirclePairs)
        {
            m_TurnCirclePairToPathConverter = turnCirclePairToPathConverter;
            PossibleTurnCirclePairs = possibleTurnCirclePairs;
        }

        private readonly ITurnCirclePairToPathConverter m_TurnCirclePairToPathConverter;

        public void Convert()
        {
            PossibleTurnCirclePairs.Settings = Settings;
            PossibleTurnCirclePairs.Calculate();

            Paths = CreatePaths(Settings,
                                PossibleTurnCirclePairs);
        }

        public ISettings Settings { get; set; } = Racetrack.Settings.Unknown;

        public IPossibleTurnCirclePairs PossibleTurnCirclePairs { get; }

        public IEnumerable <IPath> Paths { get; private set; } = new IPath[0];

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