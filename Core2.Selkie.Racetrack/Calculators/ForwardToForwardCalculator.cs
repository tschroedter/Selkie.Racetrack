using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Calculators
{
    public class ForwardToForwardCalculator
        : BaseRacetrackCalculator,
          IForwardToForwardCalculator
    {
        public ForwardToForwardCalculator([NotNull] ILinePairToRacetrackCalculator calculator)
            : base(calculator)
        {
        }

        internal override ILinePairToRacetrackCalculator GetCalculator(ILine fromLine,
                                                                       ILine toLine,
                                                                       Distance radiusForPortTurn,
                                                                       Distance radiusForStarboardTurn)
        {
            Calculator.FromLine = fromLine;
            Calculator.ToLine = toLine;
            Calculator.RadiusForPortTurn = radiusForPortTurn;
            Calculator.RadiusForStarboardTurn = radiusForStarboardTurn;
            Calculator.IsPortTurnAllowed = IsPortTurnAllowed;
            Calculator.IsStarboardTurnAllowed = IsStarboardTurnAllowed;
            Calculator.Calculate();

            return Calculator;
        }
    }
}