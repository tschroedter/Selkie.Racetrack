using JetBrains.Annotations;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Calculators
{
    public class LinesToLinesForwardToForwardRacetrackCalculator
        : BaseLinesToLinesRacetracksCalculator,
          ILinesToLinesForwardToForwardRacetrackCalculator
    {
        public LinesToLinesForwardToForwardRacetrackCalculator([NotNull] IForwardToForwardCalculator calculator)
            : base(calculator)
        {
        }

        protected override ILine GetFromLine(ILine toLine)
        {
            return toLine;
        }
    }
}