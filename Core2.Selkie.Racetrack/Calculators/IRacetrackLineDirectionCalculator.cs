using Core2.Selkie.Geometry;

namespace Core2.Selkie.Racetrack.Calculators
{
    internal interface IRacetrackLineDirectionCalculator
    {
        Constants.TurnDirection TurnDirection { get; }
    }
}