using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Windsor;
using SelkieRacetrack = Selkie.Racetrack;

namespace Selkie.Racetrack.Calculators
{
    [ProjectComponent(Lifestyle.Transient)]
    public sealed class LinePairToRacetrackCalculator
        : ILinePairToRacetrackCalculator
    {
        private readonly IPathCalculator m_Calculator;

        public LinePairToRacetrackCalculator([NotNull] IPathCalculator calculator)
        {
            Settings = SelkieRacetrack.Settings.Unknown;
            ToLine = Line.Unknown;
            IsPortTurnAllowed = true;
            IsStarboardTurnAllowed = true;
            RadiusForPortTurn = Distance.Unknown;
            RadiusForStarboardTurn = Distance.Unknown;
            FromLine = Line.Unknown;
            m_Calculator = calculator;
        }

        public bool IsPortTurnAllowed { get; set; }

        public bool IsStarboardTurnAllowed { get; set; }

        public Distance RadiusForPortTurn { get; set; }

        public Distance RadiusForStarboardTurn { get; set; }

        public ILine FromLine { get; set; }

        public ILine ToLine { get; set; }

        public ISettings Settings { get; private set; }

        public IPath Racetrack
        {
            get
            {
                return m_Calculator.Path;
            }
        }

        public void Calculate()
        {
            Settings = new Settings(FromLine.EndPoint,
                                    FromLine.AngleToXAxis,
                                    ToLine.StartPoint,
                                    ToLine.AngleToXAxis,
                                    RadiusForPortTurn,
                                    RadiusForStarboardTurn,
                                    IsPortTurnAllowed,
                                    IsStarboardTurnAllowed);

            m_Calculator.Settings = Settings;
            m_Calculator.Calculate();
        }
    }
}