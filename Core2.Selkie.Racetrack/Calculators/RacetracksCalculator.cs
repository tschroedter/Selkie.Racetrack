using System.Collections.Generic;
using System.Linq;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Calculators
{
    [UsedImplicitly]
    [ProjectComponent(Lifestyle.Transient)]
    public class RacetracksCalculator : IRacetracksCalculator
    {
        public RacetracksCalculator(
            [NotNull] ILinesToLinesForwardToForwardRacetrackCalculator
                linesToLinesForwardToForwardCalculator,
            [NotNull] ILinesToLinesForwardToReverseRacetrackCalculator
                linesToLinesForwardToReverseCalculator,
            [NotNull] ILinesToLinesReverseToForwardRacetrackCalculator
                linesToLinesReverseToForwardCalculator,
            [NotNull] ILinesToLinesReverseToReverseRacetrackCalculator
                linesToLinesReverseToReverseCalculator)
        {
            ReverseToReverse = new IPath[0][];
            ReverseToForward = new IPath[0][];
            ForwardToReverse = new IPath[0][];
            ForwardToForward = new IPath[0][];
            IsUnknown = false;
            IsStarboardTurnAllowed = true;
            IsPortTurnAllowed = true;
            TurnRadiusForPort = DefaultTurnRadius;
            TurnRadiusForStarboard = DefaultTurnRadius;
            m_LinesToLinesForwardToForwardCalculator = linesToLinesForwardToForwardCalculator;
            m_LinesToLinesForwardToReverseCalculator = linesToLinesForwardToReverseCalculator;
            m_LinesToLinesReverseToForwardCalculator = linesToLinesReverseToForwardCalculator;
            m_LinesToLinesReverseToReverseCalculator = linesToLinesReverseToReverseCalculator;
        }

        [UsedImplicitly]
        public static readonly Distance DefaultTurnRadius = new Distance(60.0);

        private readonly ILinesToLinesForwardToForwardRacetrackCalculator
            m_LinesToLinesForwardToForwardCalculator;

        private readonly ILinesToLinesForwardToReverseRacetrackCalculator
            m_LinesToLinesForwardToReverseCalculator;

        private readonly ILinesToLinesReverseToForwardRacetrackCalculator
            m_LinesToLinesReverseToForwardCalculator;

        private readonly ILinesToLinesReverseToReverseRacetrackCalculator
            m_LinesToLinesReverseToReverseCalculator;

        private IEnumerable <ILine> m_Lines = new List <ILine>();

        public void Calculate()
        {
            ForwardToForward = CalculateGeneral(m_LinesToLinesForwardToForwardCalculator,
                                                m_Lines,
                                                TurnRadiusForPort,
                                                TurnRadiusForStarboard);

            ForwardToReverse = CalculateGeneral(m_LinesToLinesForwardToReverseCalculator,
                                                m_Lines,
                                                TurnRadiusForPort,
                                                TurnRadiusForStarboard);

            ReverseToForward = CalculateGeneral(m_LinesToLinesReverseToForwardCalculator,
                                                m_Lines,
                                                TurnRadiusForPort,
                                                TurnRadiusForStarboard);

            ReverseToReverse = CalculateGeneral(m_LinesToLinesReverseToReverseCalculator,
                                                m_Lines,
                                                TurnRadiusForPort,
                                                TurnRadiusForStarboard);
        }

        public Distance TurnRadiusForStarboard { get; set; }

        public bool IsPortTurnAllowed { get; set; }

        public bool IsStarboardTurnAllowed { get; set; }

        public IEnumerable <ILine> Lines
        {
            get => m_Lines;
            set => m_Lines = value.ToArray();
        }

        public Distance TurnRadiusForPort { get; set; }

        public bool IsUnknown { get; }

        public IPath[][] ForwardToForward { get; private set; }

        public IPath[][] ForwardToReverse { get; private set; }

        public IPath[][] ReverseToForward { get; private set; }

        public IPath[][] ReverseToReverse { get; private set; }

        [NotNull]
        [UsedImplicitly]
        internal IPath[][] CalculateGeneral([NotNull] IBaseLinesToLinesRacetracksCalculator calculator,
                                            [NotNull] IEnumerable <ILine> lines,
                                            [NotNull] Distance turnRadiusForPort,
                                            [NotNull] Distance turnRadiusForStarboard)
        {
            calculator.Lines = lines;
            calculator.TurnRadiusForPort = turnRadiusForPort;
            calculator.TurnRadiusForStarboard = turnRadiusForStarboard;
            calculator.IsPortTurnAllowed = IsPortTurnAllowed;
            calculator.IsStarboardTurnAllowed = IsStarboardTurnAllowed;
            calculator.Calculate();

            return calculator.Paths;
        }
    }
}