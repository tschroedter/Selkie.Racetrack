using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Surveying;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Racetrack.Interfaces.Calculators.Surveying;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Calculators.Surveying
{
    public abstract class BaseRacetrackFeatureCalculator : IBaseRacetrackFeatureCalculator
    {
        protected BaseRacetrackFeatureCalculator([NotNull] IFeaturePairToRacetrackCalculator calculator)
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

        public IFeaturePairToRacetrackCalculator Calculator { get; }

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