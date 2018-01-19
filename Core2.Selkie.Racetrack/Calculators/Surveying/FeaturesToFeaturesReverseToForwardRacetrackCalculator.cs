using Core2.Selkie.Geometry.Surveying;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Racetrack.Interfaces.Calculators.Surveying;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Calculators.Surveying
{
    [UsedImplicitly]
    public class FeaturesToFeaturesReverseToForwardRacetrackCalculator
        : BaseFeaturesToFeaturesRacetracksCalculator,
          IFeaturesToFeaturesReverseToForwardRacetrackCalculator
    {
        public FeaturesToFeaturesReverseToForwardRacetrackCalculator(
            [NotNull] IReverseToForwardFeatureCalculator calculator)
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