using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Calculators
{
    public class ReverseToReverseCalculator
        : BaseRacetrackCalculator,
          IReverseToReverseCalculator
    {
        public ReverseToReverseCalculator([NotNull] ILinePairToRacetrackCalculator calculator)
            : base(calculator)
        {
        }

        internal override ILinePairToRacetrackCalculator GetCalculator(ILine fromLine,
                                                                       ILine toLine,
                                                                       Distance radius)
        {
            var reverseToLine = toLine.Reverse() as ILine;

            Calculator.FromLine = fromLine;
            Calculator.ToLine = reverseToLine ?? toLine;
            Calculator.Radius = radius;
            Calculator.IsPortTurnAllowed = IsPortTurnAllowed;
            Calculator.IsStarboardTurnAllowed = IsStarboardTurnAllowed;
            Calculator.Calculate();

            return Calculator;
        }
    }
}