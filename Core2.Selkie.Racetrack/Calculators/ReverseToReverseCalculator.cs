using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Calculators
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
                                                                       Distance radiusForPortTurn,
                                                                       Distance radiusForStarboardTurn)
        {
            IPolylineSegment reverseToSegment = toLine.Reverse();
            ILine reversedLine = null;

            if ( reverseToSegment != null )
            {
                reversedLine = new Line(toLine.Id,
                                        toLine.EndPoint,
                                        toLine.StartPoint);
            }


            Calculator.FromLine = fromLine;
            Calculator.ToLine = reversedLine ?? toLine;
            Calculator.RadiusForPortTurn = radiusForPortTurn;
            Calculator.RadiusForStarboardTurn = radiusForStarboardTurn;
            Calculator.IsPortTurnAllowed = IsPortTurnAllowed;
            Calculator.IsStarboardTurnAllowed = IsStarboardTurnAllowed;
            Calculator.Calculate();

            return Calculator;
        }
    }
}