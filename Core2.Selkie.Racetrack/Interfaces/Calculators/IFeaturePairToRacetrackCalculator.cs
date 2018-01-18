using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Surveying;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.Calculators
{
    public interface IFeaturePairToRacetrackCalculator : ICalculator
    {
        [NotNull]
        ISettings Settings { get; }

        [NotNull]
        IPath Racetrack { get; }

        bool IsPortTurnAllowed { get; set; }
        bool IsStarboardTurnAllowed { get; set; }

        [NotNull]
        Distance RadiusForPortTurn { get; set; }

        [NotNull]
        Distance RadiusForStarboardTurn { get; set; }

        [NotNull]
        ISurveyFeature FromFeature { get; set; }

        [NotNull]
        ISurveyFeature ToFeature { get; set; }
    }
}