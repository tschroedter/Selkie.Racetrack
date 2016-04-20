using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Racetrack.Calculators
{
    public class ForwardToReverseCalculator
        : BaseRacetrackCalculator,
          IForwardToReverseCalculator
    {
        public ForwardToReverseCalculator([NotNull] ILinePairToRacetrackCalculator calculator)
            : base(calculator)
        {
        }

        internal override ILinePairToRacetrackCalculator GetCalculator(ILine fromLine,
                                                                       ILine toLine,
                                                                       Distance radiusForPortTurn,
                                                                       Distance radiusForStarboardTurn)
        {
            var reverseToLine = toLine.Reverse() as ILine;

            Calculator.FromLine = fromLine;
            Calculator.ToLine = reverseToLine ?? toLine;
            Calculator.RadiusForPortTurn = radiusForPortTurn;
            Calculator.RadiusForStarboardTurn = radiusForStarboardTurn;
            Calculator.IsPortTurnAllowed = IsPortTurnAllowed;
            Calculator.IsStarboardTurnAllowed = IsStarboardTurnAllowed;
            Calculator.Calculate();

            return Calculator;
        }
    }
}