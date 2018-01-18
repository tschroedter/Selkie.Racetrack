using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Calculators
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