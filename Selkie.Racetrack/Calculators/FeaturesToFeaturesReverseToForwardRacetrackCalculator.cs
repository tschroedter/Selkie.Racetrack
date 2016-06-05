using JetBrains.Annotations;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Calculators
{
    public class FeaturesToFeaturesReverseToForwardRacetrackCalculator
        : BaseFeaturesToFeaturesRacetracksCalculator,
          IFeaturesToFeaturesReverseToForwardRacetrackCalculator
    {
        public FeaturesToFeaturesReverseToForwardRacetrackCalculator([NotNull] IReverseToForwardCalculator calculator)
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