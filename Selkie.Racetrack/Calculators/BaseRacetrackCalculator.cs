using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;

namespace Selkie.Racetrack.Calculators
{
    public abstract class BaseRacetrackCalculator : IBaseRacetrackCalculator
    {
        private readonly ILinePairToRacetrackCalculator m_Calculator;
        private ILine m_FromLine = Line.Unknown;
        private bool m_IsPortTurnAllowed = true;
        private bool m_IsStarboardTurnAllowed = true;

        private IPath[] m_Paths =
        {
        };

        private Distance m_Radius = Distance.Unknown;

        private ILine[] m_ToLines =
        {
        };

        protected BaseRacetrackCalculator([NotNull] ILinePairToRacetrackCalculator calculator)
        {
            m_Calculator = calculator;
        }

        public ILinePairToRacetrackCalculator Calculator
        {
            get
            {
                return m_Calculator;
            }
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

        public IPath[] Paths
        {
            get
            {
                return m_Paths;
            }
        }

        public ILine FromLine
        {
            get
            {
                return m_FromLine;
            }
            set
            {
                m_FromLine = value;
            }
        }

        public ILine[] ToLines
        {
            get
            {
                return m_ToLines;
            }
            set
            {
                m_ToLines = value;
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

        public void Calculate()
        {
            m_Paths = CalculateRacetracks();
        }

        [NotNull]
        internal abstract ILinePairToRacetrackCalculator GetCalculator([NotNull] ILine fromLine,
                                                                       [NotNull] ILine toLine,
                                                                       [NotNull] Distance radius);

        [NotNull]
        private IPath[] CalculateRacetracks()
        {
            var racetracks = new IPath[ToLines.Length];

            for ( var i = 0 ; i < ToLines.Length ; i++ )
            {
                ILine toLine = ToLines [ i ];
                IPath racetrack;

                if ( FromLine.Equals(toLine) )
                {
                    racetrack = Path.Unknown;
                }
                else
                {
                    ILinePairToRacetrackCalculator linePairToRacetrackCalculator = GetCalculator(FromLine,
                                                                                                 toLine,
                                                                                                 Radius);

                    racetrack = linePairToRacetrackCalculator.Racetrack;
                }

                racetracks [ i ] = racetrack;
            }

            return racetracks;
        }
    }
}