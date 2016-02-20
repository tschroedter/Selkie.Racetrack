using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Calculators
{
    public abstract class BaseLinesToLinesRacetracksCalculator : IBaseLinesToLinesRacetracksCalculator
    {
        private readonly IBaseRacetrackCalculator m_Calculator;

        protected BaseLinesToLinesRacetracksCalculator([NotNull] IBaseRacetrackCalculator calculator)
        {
            TurnRadiusForStarboard = Distance.Unknown;
            TurnRadiusForPort = Distance.Unknown;
            Paths = new IPath[0][];
            Lines = new ILine[0];
            IsStarboardTurnAllowed = true;
            IsPortTurnAllowed = true;
            m_Calculator = calculator;
        }

        public bool IsPortTurnAllowed { get; set; }

        public bool IsStarboardTurnAllowed { get; set; }

        public IEnumerable <ILine> Lines { get; set; }

        public IPath[][] Paths { get; private set; }

        public Distance TurnRadiusForPort { get; set; }

        public Distance TurnRadiusForStarboard { get; set; }

        public void Calculate()
        {
            ILine[] toLines = Lines.ToArray();

            int size = toLines.Length;

            var racetracks = new IPath[size][];

            for ( var i = 0 ; i < size ; i++ )
            {
                ILine fromLine = GetFromLine(toLines [ i ]);

                IPath[] racetracksForLine = CallCalculator(fromLine,
                                                           toLines);

                racetracks [ i ] = racetracksForLine;
            }

            Paths = racetracks;
        }

        [NotNull]
        protected abstract ILine GetFromLine([NotNull] ILine toLine);

        [NotNull]
        internal IPath[] CallCalculator([NotNull] ILine fromLine,
                                        [NotNull] ILine[] toLines)
        {
            m_Calculator.ToLines = toLines;
            m_Calculator.FromLine = fromLine;
            m_Calculator.TurnRadiusForPort = TurnRadiusForPort;
            m_Calculator.TurnRadiusForStarboard = TurnRadiusForStarboard;
            m_Calculator.IsPortTurnAllowed = IsPortTurnAllowed;
            m_Calculator.IsStarboardTurnAllowed = IsStarboardTurnAllowed;
            m_Calculator.Calculate();

            return m_Calculator.Paths;
        }
    }
}