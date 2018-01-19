using Core2.Selkie.Geometry.Surveying;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Racetrack.Interfaces.Calculators.Surveying;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Calculators.Surveying
{
    [UsedImplicitly]
    public class FeaturesToFeaturesForwardToForwardRacetrackCalculator
        : BaseFeaturesToFeaturesRacetracksCalculator,
          IFeaturesToFeaturesForwardToForwardRacetrackCalculator
    {
        public FeaturesToFeaturesForwardToForwardRacetrackCalculator(
            [NotNull] IForwardToForwardFeatureCalculator calculator)
            : base(calculator)
        {
        }

        protected override ISurveyFeature GetFromFeature(ISurveyFeature toFeature)
        {
            return toFeature;
        }
    }
}