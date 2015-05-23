using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Windsor;

namespace Selkie.Racetrack.Calculators
{
    [ProjectComponent(Lifestyle.Transient)]
    public class RacetracksCalculator : IRacetracksCalculator
    {
        private readonly ILinesToLinesForwardToForwardRacetrackCalculator m_LinesToLinesForwardToForwardCalculator;
        private readonly ILinesToLinesForwardToReverseRacetrackCalculator m_LinesToLinesForwardToReverseCalculator;
        private readonly ILinesToLinesReverseToForwardRacetrackCalculator m_LinesToLinesReverseToForwardCalculator;
        private readonly ILinesToLinesReverseToReverseRacetrackCalculator m_LinesToLinesReverseToReverseCalculator;
        private IPath[][] m_ForwardToForward = new IPath[0][];
        private IPath[][] m_ForwardToReverse = new IPath[0][];
        private bool m_IsPortTurnAllowed = true;
        private bool m_IsStarboardTurnAllowed = true;
        private IEnumerable <ILine> m_Lines = new List <ILine>();
        private Distance m_Radius = new Distance(30.0);
        private IPath[][] m_ReverseToForward = new IPath[0][];
        private IPath[][] m_ReverseToReverse = new IPath[0][];

        public RacetracksCalculator(
            [NotNull] ILinesToLinesForwardToForwardRacetrackCalculator linesToLinesForwardToForwardCalculator,
            [NotNull] ILinesToLinesForwardToReverseRacetrackCalculator linesToLinesForwardToReverseCalculator,
            [NotNull] ILinesToLinesReverseToForwardRacetrackCalculator linesToLinesReverseToForwardCalculator,
            [NotNull] ILinesToLinesReverseToReverseRacetrackCalculator linesToLinesReverseToReverseCalculator)
        {
            m_LinesToLinesForwardToForwardCalculator = linesToLinesForwardToForwardCalculator;
            m_LinesToLinesForwardToReverseCalculator = linesToLinesForwardToReverseCalculator;
            m_LinesToLinesReverseToForwardCalculator = linesToLinesReverseToForwardCalculator;
            m_LinesToLinesReverseToReverseCalculator = linesToLinesReverseToReverseCalculator;
        }

        public void Calculate()
        {
            m_ForwardToForward = CalculateGeneral(m_LinesToLinesForwardToForwardCalculator,
                                                  m_Lines,
                                                  m_Radius);

            m_ForwardToReverse = CalculateGeneral(m_LinesToLinesForwardToReverseCalculator,
                                                  m_Lines,
                                                  m_Radius);

            m_ReverseToForward = CalculateGeneral(m_LinesToLinesReverseToForwardCalculator,
                                                  m_Lines,
                                                  m_Radius);

            m_ReverseToReverse = CalculateGeneral(m_LinesToLinesReverseToReverseCalculator,
                                                  m_Lines,
                                                  m_Radius);
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

        public IEnumerable <ILine> Lines
        {
            get
            {
                return m_Lines;
            }
            set
            {
                m_Lines = value.ToArray();
            }
        }

        public bool IsUnknown
        {
            get
            {
                return false;
            }
        }

        public IPath[][] ForwardToForward
        {
            get
            {
                return m_ForwardToForward;
            }
        }

        public IPath[][] ForwardToReverse
        {
            get
            {
                return m_ForwardToReverse;
            }
        }

        public IPath[][] ReverseToForward
        {
            get
            {
                return m_ReverseToForward;
            }
        }

        public IPath[][] ReverseToReverse
        {
            get
            {
                return m_ReverseToReverse;
            }
        }

        [NotNull]
        internal IPath[][] CalculateGeneral([NotNull] IBaseLinesToLinesRacetracksCalculator calculator,
                                            [NotNull] IEnumerable <ILine> lines,
                                            [NotNull] Distance radius)
        {
            calculator.Lines = lines;
            calculator.Radius = radius;
            calculator.IsPortTurnAllowed = m_IsPortTurnAllowed;
            calculator.IsStarboardTurnAllowed = m_IsStarboardTurnAllowed;
            calculator.Calculate();

            return calculator.Paths;
        }
    }
}