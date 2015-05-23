using Selkie.Geometry;

namespace Selkie.Racetrack.Calculators
{
    internal interface IRacetrackLineDirectionCalculator
    {
        Constants.TurnDirection TurnDirection { get; }
    }
}