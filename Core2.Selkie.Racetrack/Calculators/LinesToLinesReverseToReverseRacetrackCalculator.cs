using JetBrains.Annotations;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Calculators;

namespace Core2.Selkie.Racetrack.Calculators
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