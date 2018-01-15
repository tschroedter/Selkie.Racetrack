using JetBrains.Annotations;
using Core2.Selkie.Geometry.Surveying;
using Core2.Selkie.Racetrack.Interfaces.Calculators;

namespace Core2.Selkie.Racetrack.Calculators
{
    public class FeaturesToFeaturesReverseToForwardRacetrackCalculator
        : BaseFeaturesToFeaturesRacetracksCalculator,
          IFeaturesToFeaturesReverseToForwardRacetrackCalculator
    {
        public FeaturesToFeaturesReverseToForwardRacetrackCalculator([NotNull] IReverseToForwardFeatureCalculator calculator)
            : base(calculator)
        {
        }

        protected override ISurveyFeature GetFromFeature(ISurveyFeature toFeature)
        {
            ISurveyFeature lineReversed = toFeature.Reverse();

            return lineReversed ?? toFeature;
        }
    }
}