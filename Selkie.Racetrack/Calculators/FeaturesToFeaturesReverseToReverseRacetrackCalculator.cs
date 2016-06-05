using JetBrains.Annotations;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Calculators
{
    public class FeaturesToFeaturesReverseToReverseRacetrackCalculator
        : BaseFeaturesToFeaturesRacetracksCalculator,
          IFeaturesToFeaturesReverseToReverseRacetrackCalculator
    {
        public FeaturesToFeaturesReverseToReverseRacetrackCalculator([NotNull] IReverseToReverseCalculator calculator)
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