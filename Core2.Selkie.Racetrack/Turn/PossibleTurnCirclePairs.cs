using System.Collections.Generic;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Turn;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Turn
{
    [ProjectComponent(Lifestyle.Transient)]
    public class PossibleTurnCirclePairs : IPossibleTurnCirclePairs
    {
        public PossibleTurnCirclePairs([NotNull] IPossibleTurnCircles possibleTurnCircles)
        {
            m_PossibleTurnCircles = possibleTurnCircles;
        }

        private readonly IPossibleTurnCircles m_PossibleTurnCircles;

        public void Calculate()
        {
            m_PossibleTurnCircles.Settings = Settings;
            m_PossibleTurnCircles.Calculate();

            Pairs = CreatePairs(Settings,
                                m_PossibleTurnCircles);
        }

        [NotNull]
        internal IEnumerable <ITurnCirclePair> CreateAllPairs([NotNull] ISettings settings,
                                                              [NotNull] IPossibleTurnCircles turnCircles)
        {
            TurnCirclePair one = CreateTurnCirclePairPortToPort(settings,
                                                                turnCircles);

            TurnCirclePair two = CreateTurnCirclePairStarboardToStarboard(settings,
                                                                          turnCircles);

            TurnCirclePair three = CreateTurnCirclePairPortToStarboard(settings,
                                                                       turnCircles);

            TurnCirclePair four = CreateTurnCirclePairStarboardToPort(settings,
                                                                      turnCircles);

            var pairs = new List <ITurnCirclePair>
                        {
                            one,
                            two,
                            three,
                            four
                        };

            return pairs;
        }

        [NotNull]
        internal List <ITurnCirclePair> CreateOnlyPortTurnsPairs([NotNull] ISettings settings,
                                                                 [NotNull] IPossibleTurnCircles turnCircles)
        {
            TurnCirclePair one = CreateTurnCirclePairPortToPort(settings,
                                                                turnCircles);

            var pairs = new List <ITurnCirclePair>
                        {
                            one
                        };

            return pairs;
        }

        [NotNull]
        internal IEnumerable <ITurnCirclePair> CreateOnlyStarboardTurnsPairs([NotNull] ISettings settings,
                                                                             [NotNull] IPossibleTurnCircles turnCircles)
        {
            TurnCirclePair two = CreateTurnCirclePairStarboardToStarboard(settings,
                                                                          turnCircles);

            var pairs = new List <ITurnCirclePair>
                        {
                            two
                        };

            return pairs;
        }

        [NotNull]
        internal IEnumerable <ITurnCirclePair> CreatePairs([NotNull] ISettings settings,
                                                           [NotNull] IPossibleTurnCircles turnCircles)
        {
            if ( settings.IsPortTurnAllowed &&
                 settings.IsStarboardTurnAllowed )
            {
                return CreateAllPairs(settings,
                                      turnCircles);
            }

            if ( settings.IsPortTurnAllowed )
            {
                return CreateOnlyPortTurnsPairs(settings,
                                                turnCircles);
            }

            return CreateOnlyStarboardTurnsPairs(settings,
                                                 turnCircles);
        }

        [NotNull]
        internal TurnCirclePair CreateTurnCirclePairPortToPort([NotNull] ISettings settings,
                                                               [NotNull] IPossibleTurnCircles turnCircles)
        {
            var one = new TurnCirclePair(settings,
                                         turnCircles.StartTurnCirclePort,
                                         turnCircles.FinishTurnCirclePort);
            return one;
        }

        [NotNull]
        internal TurnCirclePair CreateTurnCirclePairPortToStarboard([NotNull] ISettings settings,
                                                                    [NotNull] IPossibleTurnCircles turnCircles)
        {
            var three = new TurnCirclePair(settings,
                                           turnCircles.StartTurnCirclePort,
                                           turnCircles.FinishTurnCircleStarboard);
            return three;
        }

        [NotNull]
        internal TurnCirclePair CreateTurnCirclePairStarboardToPort([NotNull] ISettings settings,
                                                                    [NotNull] IPossibleTurnCircles turnCircles)
        {
            var four = new TurnCirclePair(settings,
                                          turnCircles.StartTurnCircleStarboard,
                                          turnCircles.FinishTurnCirclePort);
            return four;
        }

        [NotNull]
        internal TurnCirclePair CreateTurnCirclePairStarboardToStarboard([NotNull] ISettings settings,
                                                                         [NotNull] IPossibleTurnCircles turnCircles)
        {
            var two = new TurnCirclePair(settings,
                                         turnCircles.StartTurnCircleStarboard,
                                         turnCircles.FinishTurnCircleStarboard);
            return two;
        }

        #region IPossibleTurnCirclePairs Members

        public ISettings Settings { get; set; } = Racetrack.Settings.Unknown;

        public IEnumerable <ITurnCirclePair> Pairs { get; private set; } = new ITurnCirclePair[0];

        #endregion
    }
}