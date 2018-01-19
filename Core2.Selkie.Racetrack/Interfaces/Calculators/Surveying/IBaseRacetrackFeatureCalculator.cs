﻿using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Surveying;
using JetBrains.Annotations;
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace Core2.Selkie.Racetrack.Interfaces.Calculators.Surveying
{
    public interface IBaseRacetrackFeatureCalculator : ICalculator
    {
        [NotNull]
        IPath[] Paths { get; }

        [NotNull]
        ISurveyFeature FromFeature { get; set; }

        [NotNull]
        ISurveyFeature[] ToFeatures { get; set; }

        bool IsPortTurnAllowed { get; set; }

        bool IsStarboardTurnAllowed { get; set; }

        [NotNull]
        IFeaturePairToRacetrackCalculator Calculator { get; }

        [NotNull]
        Distance TurnRadiusForPort { get; set; }

        [NotNull]
        Distance TurnRadiusForStarboard { get; set; }
    }
}