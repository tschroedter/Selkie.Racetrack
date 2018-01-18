using System.Collections.Generic;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Surveying;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.Calculators.Surveying
{
    public interface IBaseFeaturesToFeaturesRacetracksCalculator : ICalculator
    {
        bool IsPortTurnAllowed { get; set; }
        bool IsStarboardTurnAllowed { get; set; }

        [NotNull]
        IEnumerable <ISurveyFeature> Features { get; set; }

        [NotNull]
        IPath[][] Paths { get; }

        [NotNull]
        Distance TurnRadiusForPort { get; set; }

        [NotNull]
        Distance TurnRadiusForStarboard { get; set; }

        [NotNull]
        ISurveyFeature[] ToFeatures { get; set; } // TODO currently work-around = ToLine

        [NotNull]
        ISurveyFeature FromFeature { get; set; } // TODO currently work-around = FromLine
    }
}