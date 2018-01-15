using JetBrains.Annotations;
using Core2.Selkie.Geometry.Surveying;
using Core2.Selkie.Racetrack.Interfaces.Calculators;

namespace Core2.Selkie.Racetrack.Calculators
{
    public class FeaturesToFeaturesForwardToReverseRacetrackCalculator
        : BaseFeaturesToFeaturesRacetracksCalculator,
          IFeaturesToFeaturesForwardToReverseRacetrackCalculator
    {
        public FeaturesToFeaturesForwardToReverseRacetrackCalculator([NotNull] IForwardToReverseFeatureCalculator calculator)
            : base(calculator)
        {
        }

        protected override ISurveyFeature GetFromFeature(ISurveyFeature toFeature)
        {
            return toFeature;
        }
    }
}