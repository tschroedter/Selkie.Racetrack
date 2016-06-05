using JetBrains.Annotations;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Surveying;

namespace Selkie.Racetrack.Interfaces.Calculators
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