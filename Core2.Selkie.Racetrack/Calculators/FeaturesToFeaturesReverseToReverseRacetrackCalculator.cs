using JetBrains.Annotations;
using Core2.Selkie.Geometry.Surveying;
using Core2.Selkie.Racetrack.Interfaces.Calculators;

namespace Core2.Selkie.Racetrack.Calculators
{
    public class FeaturesToFeaturesReverseToReverseRacetrackCalculator
        : BaseFeaturesToFeaturesRacetracksCalculator,
          IFeaturesToFeaturesReverseToReverseRacetrackCalculator
    {
        public FeaturesToFeaturesReverseToReverseRacetrackCalculator([NotNull] IReverseToReverseFeatureCalculator calculator)
            : base(calculator)
        {
        }

        protected override ISurveyFeature GetFromFeature(ISurveyFeature toFeature)
        {
            ISurveyFeature featureReversed = toFeature.Reverse();

            return featureReversed ?? toFeature;
        }
    }
}