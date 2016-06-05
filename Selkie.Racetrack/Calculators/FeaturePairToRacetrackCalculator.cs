using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;
using Selkie.Windsor;
using SelkieRacetrack = Selkie.Racetrack;

namespace Selkie.Racetrack.Calculators
{
    [ProjectComponent(Lifestyle.Transient)]
    public sealed class FeaturePairToRacetrackCalculator
        : IFeaturePairToRacetrackCalculator
    {
        public FeaturePairToRacetrackCalculator([NotNull] IPathCalculator calculator)
        {
            Settings = SelkieRacetrack.Settings.Unknown;
            IsPortTurnAllowed = true;
            IsStarboardTurnAllowed = true;
            RadiusForPortTurn = Distance.Unknown;
            RadiusForStarboardTurn = Distance.Unknown;
            m_Calculator = calculator;
            FromFeature = SurveyFeature.Unknown;
            ToFeature = SurveyFeature.Unknown;
        }

        private readonly IPathCalculator m_Calculator;

        public bool IsPortTurnAllowed { get; set; }

        public bool IsStarboardTurnAllowed { get; set; }

        public Distance RadiusForPortTurn { get; set; }

        public Distance RadiusForStarboardTurn { get; set; }

        public ISurveyFeature FromFeature { get; set; }

        public ISurveyFeature ToFeature { get; set; }

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
            Settings = new Settings(FromFeature.EndPoint,
                                    FromFeature.AngleToXAxisAtEndPoint,
                                    ToFeature.StartPoint,
                                    ToFeature.AngleToXAxisAtStartPoint,
                                    RadiusForPortTurn,
                                    RadiusForStarboardTurn,
                                    IsPortTurnAllowed,
                                    IsStarboardTurnAllowed);

            m_Calculator.Settings = Settings;
            m_Calculator.Calculate();
        }
    }
}