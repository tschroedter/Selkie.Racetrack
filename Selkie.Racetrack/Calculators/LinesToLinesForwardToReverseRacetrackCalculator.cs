using JetBrains.Annotations;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Calculators
{
    public class LinesToLinesForwardToReverseRacetrackCalculator
        : BaseLinesToLinesRacetracksCalculator,
          ILinesToLinesForwardToReverseRacetrackCalculator
    {
        public LinesToLinesForwardToReverseRacetrackCalculator([NotNull] IForwardToReverseCalculator calculator)
            : base(calculator)
        {
        }

        protected override ILine GetFromLine(ILine toLine)
        {
            return toLine;
        }
    }
}