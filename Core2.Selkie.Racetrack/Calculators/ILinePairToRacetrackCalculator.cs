﻿using JetBrains.Annotations;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using Core2.Selkie.Racetrack.Interfaces;

namespace Core2.Selkie.Racetrack.Calculators
{
    public interface ILinePairToRacetrackCalculator : ICalculator
    {
        [NotNull]
        ILine FromLine { get; set; }

        [NotNull]
        ILine ToLine { get; set; }

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
    }
}