using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Calculators
{
    public class ForwardToForwardCalculator
        : BaseRacetrackCalculator,
          IForwardToForwardCalculator
    {
        public ForwardToForwardCalculator([NotNull] IFeaturePairToRacetrackCalculator calculator)
            : base(calculator)
        {
        }

        internal override IFeaturePairToRacetrackCalculator GetCalculator(ISurveyFeature fromFeature,
                                                                          ISurveyFeature toFeature,
                                                                          Distance radiusForPortTurn,
                                                                          Distance radiusForStarboardTurn)
        {
            Calculator.FromFeature = fromFeature;
            Calculator.ToFeature = toFeature;
            Calculator.RadiusForPortTurn = radiusForPortTurn;
            Calculator.RadiusForStarboardTurn = radiusForStarboardTurn;
            Calculator.IsPortTurnAllowed = IsPortTurnAllowed;
            Calculator.IsStarboardTurnAllowed = IsStarboardTurnAllowed;
            Calculator.Calculate();

            return Calculator;
        }
    }
}