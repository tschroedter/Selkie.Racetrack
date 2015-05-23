using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Calculators
{
    public class ReverseToForwardCalculator
        : BaseRacetrackCalculator,
          IReverseToForwardCalculator
    {
        public ReverseToForwardCalculator([NotNull] ILinePairToRacetrackCalculator calculator)
            : base(calculator)
        {
        }

        internal override ILinePairToRacetrackCalculator GetCalculator(ILine fromLine,
                                                                       ILine toLine,
                                                                       Distance radius)
        {
            Calculator.FromLine = fromLine;
            Calculator.ToLine = toLine;
            Calculator.Radius = radius;
            Calculator.IsPortTurnAllowed = IsPortTurnAllowed;
            Calculator.IsStarboardTurnAllowed = IsStarboardTurnAllowed;
            Calculator.Calculate();

            return Calculator;
        }
    }
}