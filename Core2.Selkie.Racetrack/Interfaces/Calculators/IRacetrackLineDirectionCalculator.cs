using Core2.Selkie.Geometry;
using JetBrains.Annotations;

namespace Core2.Selkie.Racetrack.Interfaces.Calculators
{
    internal interface IRacetrackLineDirectionCalculator
    {
        [UsedImplicitly]
        Constants.TurnDirection TurnDirection { get; }
    }
}