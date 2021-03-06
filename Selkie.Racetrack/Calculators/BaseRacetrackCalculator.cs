﻿using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Calculators
{
    public abstract class BaseRacetrackCalculator : IBaseRacetrackCalculator
    {
        protected BaseRacetrackCalculator([NotNull] IFeaturePairToRacetrackCalculator calculator)
        {
            IsPortTurnAllowed = true;
            IsStarboardTurnAllowed = true;
            Paths = new IPath[0];
            FromFeature = SurveyFeature.Unknown;
            ToFeatures = new ISurveyFeature[0];
            TurnRadiusForPort = Distance.Unknown;
            TurnRadiusForStarboard = Distance.Unknown;
            Calculator = calculator;
        }

        public IFeaturePairToRacetrackCalculator Calculator { get; private set; }

        public bool IsPortTurnAllowed { get; set; }

        public bool IsStarboardTurnAllowed { get; set; }

        public IPath[] Paths { get; private set; }

        public ISurveyFeature FromFeature { get; set; }

        public ISurveyFeature[] ToFeatures { get; set; }

        public Distance TurnRadiusForPort { get; set; }

        public Distance TurnRadiusForStarboard { get; set; }

        public void Calculate()
        {
            Paths = CalculateRacetracks();
        }

        [NotNull]
        internal abstract IFeaturePairToRacetrackCalculator GetCalculator([NotNull] ISurveyFeature fromFeature,
                                                                          [NotNull] ISurveyFeature toFeature,
                                                                          [NotNull] Distance radiusForPortTurn,
                                                                          [NotNull] Distance radiusForStarboardTurn);

        [NotNull]
        private IPath[] CalculateRacetracks()
        {
            var racetracks = new IPath[ToFeatures.Length];

            for ( var i = 0 ; i < ToFeatures.Length ; i++ )
            {
                ISurveyFeature toFeature = ToFeatures [ i ];
                IPath racetrack;

                if ( FromFeature.Equals(toFeature) )
                {
                    racetrack = Path.Unknown;
                }
                else
                {
                    IFeaturePairToRacetrackCalculator featurePairToRacetrackCalculator = GetCalculator(FromFeature,
                                                                                                       toFeature,
                                                                                                       TurnRadiusForPort,
                                                                                                       TurnRadiusForStarboard);

                    racetrack = featurePairToRacetrackCalculator.Racetrack;
                }

                racetracks [ i ] = racetrack;
            }

            return racetracks;
        }
    }
}