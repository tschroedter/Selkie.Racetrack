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
        private bool m_IsPortTurnAllowed = true;
        private bool m_IsStarboardTurnAllowed = true;
        private IEnumerable <ILine> m_Lines = new ILine[0];
        private IPath[][] m_Paths = new IPath[0][];
        private Distance m_Radius = Distance.Unknown;

        protected BaseLinesToLinesRacetracksCalculator([NotNull] IBaseRacetrackCalculator calculator)
        {
            m_Calculator = calculator;
        }

        public bool IsPortTurnAllowed
        {
            get
            {
                return m_IsPortTurnAllowed;
            }
            set
            {
                m_IsPortTurnAllowed = value;
            }
        }

        public bool IsStarboardTurnAllowed
        {
            get
            {
                return m_IsStarboardTurnAllowed;
            }
            set
            {
                m_IsStarboardTurnAllowed = value;
            }
        }

        public IEnumerable <ILine> Lines
        {
            get
            {
                return m_Lines;
            }
            set
            {
                m_Lines = value;
            }
        }

        public Distance Radius
        {
            get
            {
                return m_Radius;
            }
            set
            {
                m_Radius = value;
            }
        }

        public IPath[][] Paths
        {
            get
            {
                return m_Paths;
            }
        }

        public void Calculate()
        {
            ILine[] toLines = m_Lines.ToArray();

            int size = toLines.Length;

            var racetracks = new IPath[size][];

            for ( var i = 0 ; i < size ; i++ )
            {
                ILine fromLine = GetFromLine(toLines [ i ]);

                IPath[] racetracksForLine = CallCalculator(fromLine,
                                                           toLines);

                racetracks [ i ] = racetracksForLine;
            }

            m_Paths = racetracks;
        }

        [NotNull]
        protected abstract ILine GetFromLine([NotNull] ILine toLine);

        [NotNull]
        internal IPath[] CallCalculator([NotNull] ILine fromLine,
                                        [NotNull] ILine[] toLines)
        {
            m_Calculator.ToLines = toLines;
            m_Calculator.FromLine = fromLine;
            m_Calculator.Radius = m_Radius;
            m_Calculator.IsPortTurnAllowed = m_IsPortTurnAllowed;
            m_Calculator.IsStarboardTurnAllowed = m_IsStarboardTurnAllowed;
            m_Calculator.Calculate();

            return m_Calculator.Paths;
        }
    }
}