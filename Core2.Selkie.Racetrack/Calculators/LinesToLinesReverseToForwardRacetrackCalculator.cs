using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Calculators
{
    public class LinesToLinesReverseToForwardRacetrackCalculator
        : BaseLinesToLinesRacetracksCalculator,
          ILinesToLinesReverseToForwardRacetrackCalculator
    {
        public LinesToLinesReverseToForwardRacetrackCalculator([NotNull] IReverseToForwardCalculator calculator)
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