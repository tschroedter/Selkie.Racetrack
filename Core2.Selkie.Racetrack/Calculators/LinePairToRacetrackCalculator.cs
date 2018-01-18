using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;
using Core2.Selkie.Racetrack.Interfaces.Calculators;
using Core2.Selkie.Windsor;
using JetBrains.Annotations;
using SelkieRacetrack = Core2.Selkie.Racetrack;

namespace Core2.Selkie.Racetrack.Calculators
{
    [ProjectComponent(Lifestyle.Transient)]
    public sealed class LinePairToRacetrackCalculator
        : ILinePairToRacetrackCalculator
    {
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

        private readonly IPathCalculator m_Calculator;

        public bool IsPortTurnAllowed { get; set; }

        public bool IsStarboardTurnAllowed { get; set; }

        public Distance RadiusForPortTurn { get; set; }

        public Distance RadiusForStarboardTurn { get; set; }

        public ILine FromLine { get; set; }

        public ILine ToLine { get; set; }

        public ISettings Settings { get; private set; }

        public IPath Racetrack => m_Calculator.Path;

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