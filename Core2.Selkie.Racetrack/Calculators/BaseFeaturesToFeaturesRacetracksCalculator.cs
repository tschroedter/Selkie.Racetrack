using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Surveying;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;

namespace Core2.Selkie.Racetrack.Calculators
{
    public abstract class BaseFeaturesToFeaturesRacetracksCalculator : IBaseFeaturesToFeaturesRacetracksCalculator
    {
        protected BaseFeaturesToFeaturesRacetracksCalculator([NotNull] IBaseRacetrackFeatureCalculator calculator)
        {
            TurnRadiusForStarboard = Distance.Unknown;
            TurnRadiusForPort = Distance.Unknown;
            Paths = new IPath[0][];
            Features = new ISurveyFeature[0];
            IsStarboardTurnAllowed = true;
            IsPortTurnAllowed = true;
            m_Calculator = calculator;
        }

        private readonly IBaseRacetrackFeatureCalculator m_Calculator;

        public bool IsPortTurnAllowed { get; set; }

        public bool IsStarboardTurnAllowed { get; set; }

        public IEnumerable <ISurveyFeature> Features { get; set; }

        public IPath[][] Paths { get; private set; }

        public Distance TurnRadiusForPort { get; set; }

        public Distance TurnRadiusForStarboard { get; set; }

        public ISurveyFeature[] ToFeatures { get; set; }   // TODO FIXS THIS not tested, currently work-around = ToLine
        public ISurveyFeature FromFeature { get; set; }    // TODO FIXS THIS not tested, currently work-around = FromLine

        public void Calculate()
        {
            ISurveyFeature[] toFeatures = Features.ToArray();

            int size = toFeatures.Length;

            var racetracks = new IPath[size][];

            for ( var i = 0 ; i < size ; i++ )
            {
                ISurveyFeature fromFeature = GetFromFeature(toFeatures [ i ]);

                IPath[] racetracksForLine = CallCalculator(fromFeature,
                                                           toFeatures);

                racetracks [ i ] = racetracksForLine;
            }

            Paths = racetracks;
        }

        [NotNull]
        protected abstract ISurveyFeature GetFromFeature([NotNull] ISurveyFeature toFeature);

        [NotNull]
        internal IPath[] CallCalculator([NotNull] ISurveyFeature fromLine,
                                        [NotNull] ISurveyFeature[] toFeatures)
        {
            m_Calculator.ToFeatures = toFeatures;
            m_Calculator.FromFeature = fromLine;
            m_Calculator.TurnRadiusForPort = TurnRadiusForPort;
            m_Calculator.TurnRadiusForStarboard = TurnRadiusForStarboard;
            m_Calculator.IsPortTurnAllowed = IsPortTurnAllowed;
            m_Calculator.IsStarboardTurnAllowed = IsStarboardTurnAllowed;
            m_Calculator.Calculate();

            return m_Calculator.Paths;
        }
    }
}