using JetBrains.Annotations;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Calculators
{
    public class LinesToLinesReverseToReverseRacetrackCalculator
        : BaseLinesToLinesRacetracksCalculator,
          ILinesToLinesReverseToReverseRacetrackCalculator
    {
        public LinesToLinesReverseToReverseRacetrackCalculator([NotNull] IReverseToReverseCalculator calculator)
            : base(calculator)
        {
        }

        protected override ILine GetFromLine(ILine toLine)
        {
            var lineReversed = toLine.Reverse() as ILine;

            return lineReversed ?? toLine;
        }
    }
}